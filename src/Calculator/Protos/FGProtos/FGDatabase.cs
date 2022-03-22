﻿using Calculator.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Linq;
using Calculator.Extensions;

namespace Calculator.Protos.FGProtos
{
    public class FGDatabase
    {
        public List<FGItem> Items = new List<FGItem>();
        public List<FGRecipe> Recipes = new List<FGRecipe>();
        public List<FGFactory> Factories = new List<FGFactory>();
        public List<FGGenerator> Generators = new List<FGGenerator>();
        public Dictionary<string, List<string>> ExtractorMap = new Dictionary<string, List<string>>();
        private static string GetFilename()
        {
            string root = Properties.Settings.Default.GameFolder;
            return Path.Combine(root, "CommunityResources\\Docs\\Docs.json");
        }
        public static string Filename => GetFilename();

        public static FGDatabase Intance => GetInstance();

        private static FGDatabase instance;
        private static FGDatabase GetInstance()
        {
            if (instance == null) Load();
            return instance;
        }
        public static void Load()
        {
            FGDatabase.instance = new FGDatabase();
            var documentOptions = new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip
            };

            string jsonString = File.ReadAllText(Filename);

            using (JsonDocument document = JsonDocument.Parse(jsonString, documentOptions))
            {
                JsonElement root = document.RootElement;
                foreach (JsonElement node in root.EnumerateArray())
                {
                    JsonElement nativeClass = node.GetProperty("NativeClass");
                    JsonElement classes = node.GetProperty("Classes");
                    foreach (JsonElement classe in classes.EnumerateArray())
                    {
                        switch (nativeClass.GetString())
                        {
                            case "Class'/Script/FactoryGame.FGItemDescriptor'":
                                FGDatabase.instance.AddItem(classe, ItemType.Item);
                                break;
                            case "Class'/Script/FactoryGame.FGConsumableDescriptor'":
                                FGDatabase.instance.AddItem(classe, ItemType.Consumable);
                                break;
                            case "Class'/Script/FactoryGame.FGEquipmentDescriptor'":
                                FGDatabase.instance.AddItem(classe, ItemType.Equipment);
                                break;
                            case "Class'/Script/FactoryGame.FGItemDescriptorBiomass'":
                                FGDatabase.instance.AddItem(classe, ItemType.Item);
                                break;
                            case "Class'/Script/FactoryGame.FGItemDescAmmoTypeProjectile'":
                                FGDatabase.instance.AddItem(classe, ItemType.Ammo);
                                break;
                            case "Class'/Script/FactoryGame.FGItemDescAmmoTypeInstantHit'":
                                FGDatabase.instance.AddItem(classe, ItemType.Ammo);
                                break;
                            case "Class'/Script/FactoryGame.FGItemDescAmmoTypeColorCartridge'":
                                FGDatabase.instance.AddItem(classe, ItemType.Ammo);
                                break;
                            case "Class'/Script/FactoryGame.FGItemDescriptorNuclearFuel'":
                                FGDatabase.instance.AddItem(classe, ItemType.Item);
                                break;
                            case "Class'/Script/FactoryGame.FGResourceDescriptor'":
                                FGDatabase.instance.AddItem(classe, ItemType.Resource);
                                break;
                            case "Class'/Script/FactoryGame.FGBuildingDescriptor'":
                                FGDatabase.instance.AddItem(classe, ItemType.Building);
                                break;
                            case "Class'/Script/FactoryGame.FGPoleDescriptor'":
                                FGDatabase.instance.AddItem(classe, ItemType.Building);
                                break;
                            case "Class'/Script/FactoryGame.FGVehicleDescriptor'":
                                FGDatabase.instance.AddItem(classe, ItemType.Vehicule);
                                break;
                            case "Class'/Script/FactoryGame.FGRecipe'":
                                FGDatabase.instance.AddRecipe(classe);
                                break;
                            case "Class'/Script/FactoryGame.FGConsumableEquipment'":
                                break;
                            case "Class'/Script/FactoryGame.FGSchematic'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableWall'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableDoor'":
                                break;
                            case "Class'/Script/FactoryGame.FGCustomizationRecipe'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableCornerWall'":
                                break;
                            case "Class'/Script/FactoryGame.FGChainsaw'":
                                break;
                            case "Class'/Script/FactoryGame.FGGolfCartDispenser'":
                                break;
                            case "Class'/Script/FactoryGame.FGSuitBase'":
                                break;
                            case "Class'/Script/FactoryGame.FGJetPack'":
                                break;
                            case "Class'/Script/FactoryGame.FGJumpingStilts'":
                                break;
                            case "Class'/Script/FactoryGame.FGWeapon'":
                                break;
                            case "Class'/Script/FactoryGame.FGEquipmentStunSpear'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildablePole'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableConveyorBelt'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildablePowerPole'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableWire'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableTradingPost'":
                                break;
                            case "Class'/Script/FactoryGame.FGGasMask'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableManufacturer'":
                                FGDatabase.instance.AddManufacturer(classe);
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableResourceExtractor'":
                                FGDatabase.instance.AddManufacturer(classe);
                                break;
                            case "Class'/Script/FactoryGame.FGPortableMinerDispenser'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableStorage'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildable'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableGeneratorFuel'":
                                FGDatabase.instance.AddGenerator(classe);
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableSpaceElevator'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableMAM'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableBeam'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildablePillar'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableFactory'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableWalkway'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildablePipelinePump'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildablePipelineSupport'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildablePipeline'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildablePipelineJunction'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildablePipeReservoir'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableWaterPump'":
                                FGDatabase.instance.AddManufacturer(classe);
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableResourceSink'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableResourceSinkShop'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableDroneStation'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableFrackingExtractor'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableFrackingActivator'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableManufacturerVariablePower'":
                                FGDatabase.instance.AddManufacturer(classe);
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableGeneratorNuclear'":
                                FGDatabase.instance.AddGenerator(classe);
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableConveyorLift'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableFoundation'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableRamp'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableSplitterSmart'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableAttachmentMerger'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableAttachmentSplitter'":
                                break;
                            case "Class'/Script/FactoryGame.FGObjectScanner'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableJumppad'":
                                break;
                            case "Class'/Script/FactoryGame.FGConveyorPoleStackable'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableDockingStation'":
                                break;
                            case "Class'/Script/FactoryGame.FGPipeHyperStart'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildablePipeHyper'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildablePowerStorage'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableRailroadSignal'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableTrainPlatformCargo'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableTrainPlatformEmpty'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableRailroadStation'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableRailroadTrack'":
                                break;
                            case "Class'/Script/FactoryGame.FGHoverPack'":
                                break;
                            case "Class'/Script/FactoryGame.FGEquipmentZipline'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableCircuitSwitch'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableGeneratorGeoThermal'":
                                FGDatabase.instance.AddGenerator(classe);
                                break;
                            case "Class'/Script/FactoryGame.FGParachute'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableRadarTower'":
                                break;
                            case "Class'/Script/FactoryGame.FGChargedWeapon'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableFactorySimpleProducer'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableSnowDispenser'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableFactoryBuilding'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableWidgetSign'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableLightSource'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableLadder'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableStair'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableFloodlight'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableLightsControlPanel'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildablePassthrough'":
                                break;
                        }
                    }
                }
            }
            instance.FinalizeData();
        }

        private void FinalizeData()
        {

        }

        private void AddItem(JsonElement classe, ItemType itemType)
        {
            var element = new FGItem(classe);
            element.ItemType = itemType;
            instance.Items.Add(element);
        }
        private void AddManufacturer(JsonElement classe)
        {
            string classname = classe.GetStringValue("ClassName");
            string itemName = classname.Replace("Build", "Desc");
            var item = Items.FirstOrDefault(x => x.ClassName == itemName);
            if (item != null)
            {
                item.ItemType = ItemType.Factory;
                item.DisplayName = classe.GetStringValue("mDisplayName");
                item.Description = classe.GetStringValue("mDescription");
                var element = item.Clone<FGFactory>();
                element.ClassName = classname;
                element.Speed = classe.GetDoubleValue("mManufacturingSpeed");
                element.PowerConsumption = classe.GetDoubleValue("mPowerConsumption");
                element.PowerConsumptionExponent = classe.GetDoubleValue("mPowerConsumptionExponent");
                element.PowerProduction = classe.GetDoubleValue("mPowerProduction");
                element.PowerProductionExponent = classe.GetDoubleValue("mPowerProductionExponent");
                element.AllowedResourceForms = classe.GetArrayValue("mAllowedResourceForms");
                element.AllowedResources = classe.GetArrayValue("mAllowedResources");
                instance.Factories.Add(element);
            }

        }
        private void AddGenerator(JsonElement classe)
        {
            string classname = classe.GetStringValue("ClassName");
            string itemName = classname.Replace("Build", "Desc");
            var item = Items.FirstOrDefault(x => x.ClassName == itemName);
            if (item != null)
            {
                item.ItemType = ItemType.Generator;
                item.DisplayName = classe.GetStringValue("mDisplayName");
                item.Description = classe.GetStringValue("mDescription");
                var element = item.Clone<FGGenerator>();
                element.Power = classe.GetDoubleValue("mPowerProduction");
                element.PowerExponent = classe.GetDoubleValue("mPowerProductionExponent");
                instance.Generators.Add(element);
            }

        }
        private void AddRecipe(JsonElement classe)
        {
            var element = new FGRecipe(classe);
            instance.Recipes.Add(element);
        }
        
    }
}
