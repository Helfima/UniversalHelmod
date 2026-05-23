using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using UniversalHelmod.Extensions;
using UniversalHelmod.Extractors.StarRupture.Models;

namespace UniversalHelmod.Extractors.StarRupture
{
    internal class ProcessStarRupture
    {
        public void LoadFiles(StarRuptureSaveModel model)
        {
            var path = model.Path;
            var fileSaves = new List<FileSave>();
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path, "*.met", SearchOption.AllDirectories))
                {
                    var json = File.ReadAllText(file);
                    var fileSave = JsonSerializer.Deserialize<FileSave>(json);
                    if (fileSave != null)
                    {
                        fileSave.Filename = file;
                        fileSave.SaveFilename = file.Replace(".met", ".sav");
                        fileSaves.Add(fileSave);
                    }
                }
            }
            fileSaves.Sort((a, b) => string.Compare(b.Timestamp, a.Timestamp, StringComparison.Ordinal));
            model.FileSaves = fileSaves.ToObservableCollection();
        }
        public void ExtractSave(string path)
        {
            var compressedData = File.ReadAllBytes(path);
            var data = HelperSave.Decompress(compressedData);
            var dataString = Encoding.UTF8.GetString(data);
            var tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "StarRuptureSave.json");
            ParseJson(dataString);
            var dataSave = JsonSerializer.Deserialize<DataSave>(dataString);
            File.WriteAllText(tempFile, dataString);
            System.Diagnostics.Debug.WriteLine($"Fichier temporaire: {tempFile}");
        }
        private void ParseJson(string json)
        {
            var options = new JsonSerializerOptions 
            { 
                IncludeFields = true,
                PropertyNameCaseInsensitive = true
            };

            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            // === REQUEST ===
            var requestData = root.GetProperty("itemData")
                               .GetProperty("Mass")
                               .GetProperty("logisticsRequestSubsystemState")
                               .GetProperty("requestData");
            var requestMap = new Dictionary<string, object>();
            foreach (var entity in requestData.EnumerateObject())
            {
                var entityDataObj = JsonSerializer.Deserialize<object>(entity.Value.GetRawText(), options);
                if (entityDataObj != null)
                {
                    requestMap[entity.Name] = entityDataObj;
                }
            }
            // === ENTITIES ===
            var entities = root.GetProperty("itemData")
                               .GetProperty("Mass")
                               .GetProperty("entities");

            var entityMap = new Dictionary<string, SREntity>();
            foreach (var entity in entities.EnumerateObject())
            {
                var entityDataObj = JsonSerializer.Deserialize<SREntity>(entity.Value.GetRawText(), options);
                var path = entityDataObj.SpawnData?.EntityConfigDataPath ?? "";
                if (path.Contains("Drones")) continue;
                if (entityDataObj != null)
                {
                    entityMap[entity.Name] = entityDataObj;
                    AnalyseItem(entityDataObj);
                }
            }

            var mapView = new MapView(entityMap);
            mapView.Show();
        }

        private void AnalyseItem(SREntity entity)
        {
            var path = entity.SpawnData?.EntityConfigDataPath ?? "";
            if (path.Contains("Extractor", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Extractor;
            else if (path.Contains("Drill", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Extractor;
            else if (path.Contains("BaseCore", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.BaseCore;
            else if (path.Contains("Exporter", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Exporter;
            else if (path.Contains("Sender", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Sender;
            else if (path.Contains("Receiver", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Receiver;
            else if (path.Contains("Power", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Generator;
            else if (path.Contains("Turret", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Turret;
            else if (path.Contains("Zipline", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Zipline;
            else if (path.Contains("Crafter", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Fabricator;
            else if (path.Contains("Furnace", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Fabricator;
            else if (path.Contains("Hammer", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Fabricator;
            else if (path.Contains("Smelter", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Fabricator;
            else if (path.Contains("Compounder", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Fabricator;
            else if (path.Contains("Assembler", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Fabricator;
            else if (path.Contains("Constructorizer", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Fabricator;
            else if (path.Contains("Refinery", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Fabricator;
            else if (path.Contains("Synthetizer", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Fabricator;
            else if (path.Contains("Forge", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Fabricator;
            else if (path.Contains("Buildings", StringComparison.OrdinalIgnoreCase)) entity.Type = EntityType.Building;

            var database = Workspaces.Models.WorkspacesModel.Intance.Current.Database;
            var recipePattern = @"/Script/Chimera\.CrItemRecipeData'([^']*)'";
            var itemPattern = @"ItemDataBase=""([^""]*)"",Count=([0-9]*)";
            var linkPattern = @"Position=\(X=([^,]*),Y=([^,]*),Z=([^)]*)\)";
            foreach (var fragmentValue in entity.FragmentValues)
            {
                if (fragmentValue.StartsWith("/Script/Chimera.CrCraftingFragment")
                    && fragmentValue.Contains("/Script/Chimera.CrItemRecipeData"))
                {
                    entity.IsProducer = true;
                    var match = Regex.Match(fragmentValue, recipePattern);
                    if (match.Success)
                    {
                        entity.RecipePath = match.Groups[1].Value;
                        entity.Recipe = database.SelectRecipeByTag(entity.RecipePath);
                    }
                }
                if (fragmentValue.StartsWith("/Script/Chimera.CrInventoryFragment")
                    && fragmentValue.Contains("ItemDataBase"))
                {
                    var matches = Regex.Matches(fragmentValue, itemPattern);
                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            var itemFullPath = match.Groups[1].Value;
                            var itemSplitted = itemFullPath.Split('\'');
                            var count = System.Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
                            var slot = new SRSlot()
                            {
                                ItemPath = itemSplitted[0],
                                Count = count
                            };
                            entity.Inventory.Add(slot);
                        }
                    }
                }
                if (fragmentValue.StartsWith("/Script/AuActorPlacement.AuSplineConnectionFragment")
                    && fragmentValue.Contains("SplineData"))
                {
                    var matches = Regex.Matches(fragmentValue, linkPattern);
                    if (matches.Count > 0)
                    {
                        foreach(Match match in matches)
                        {
                            var x = System.Convert.ToDouble(match.Groups[1].Value, CultureInfo.InvariantCulture);
                            var y = System.Convert.ToDouble(match.Groups[2].Value, CultureInfo.InvariantCulture);
                            var z = System.Convert.ToDouble(match.Groups[3].Value, CultureInfo.InvariantCulture);
                            var translation = new SRTranslation()
                            {
                                X = x,
                                Y = y,
                                Z = z,
                            };
                            entity.Link.Add(translation);
                        }
                    }
                }
            }
        }
    }
}
