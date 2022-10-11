using UniversalHelmod.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Linq;
using UniversalHelmod.Extensions;

namespace UniversalHelmod.Extractors.Satisfactory.Models
{
    public class FGDatabase
    {
        public List<FGItem> Items = new List<FGItem>();
        public List<FGRecipe> Recipes = new List<FGRecipe>();
        public List<FGFactory> Factories = new List<FGFactory>();
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
                                FGDatabase.instance.AddItem(classe, "Item");
                                break;
                            case "Class'/Script/FactoryGame.FGConsumableDescriptor'":
                                FGDatabase.instance.AddItem(classe, "Consumable");
                                break;
                            case "Class'/Script/FactoryGame.FGEquipmentDescriptor'":
                                FGDatabase.instance.AddItem(classe, "Equipment");
                                break;
                            case "Class'/Script/FactoryGame.FGItemDescriptorBiomass'":
                                FGDatabase.instance.AddItem(classe, "Item");
                                break;
                            case "Class'/Script/FactoryGame.FGItemDescAmmoTypeProjectile'":
                                FGDatabase.instance.AddItem(classe, "Ammo");
                                break;
                            case "Class'/Script/FactoryGame.FGItemDescAmmoTypeInstantHit'":
                                FGDatabase.instance.AddItem(classe, "Ammo");
                                break;
                            case "Class'/Script/FactoryGame.FGItemDescAmmoTypeColorCartridge'":
                                FGDatabase.instance.AddItem(classe, "Ammo");
                                break;
                            case "Class'/Script/FactoryGame.FGItemDescriptorNuclearFuel'":
                                FGDatabase.instance.AddItem(classe, "Item");
                                break;
                            case "Class'/Script/FactoryGame.FGResourceDescriptor'":
                                FGDatabase.instance.AddItem(classe, "Resource");
                                break;
                            case "Class'/Script/FactoryGame.FGBuildingDescriptor'":
                                FGDatabase.instance.AddItem(classe, "Building");
                                break;
                            case "Class'/Script/FactoryGame.FGPoleDescriptor'":
                                FGDatabase.instance.AddItem(classe, "Building");
                                break;
                            case "Class'/Script/FactoryGame.FGVehicleDescriptor'":
                                FGDatabase.instance.AddItem(classe, "Vehicle");
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
                                FGDatabase.instance.AddFactory(classe, "Factory");
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableResourceExtractor'":
                                FGDatabase.instance.AddFactory(classe, "Extractor");
                                break;
                            case "Class'/Script/FactoryGame.FGPortableMinerDispenser'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableStorage'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildable'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableGeneratorFuel'":
                                FGDatabase.instance.AddFactory(classe, "Generator");
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
                                FGDatabase.instance.AddFactory(classe, "Extractor");
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableResourceSink'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableResourceSinkShop'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableDroneStation'":
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableFrackingExtractor'":
                                FGDatabase.instance.AddFactory(classe, "Extractor");
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableFrackingActivator'":
                                FGDatabase.instance.AddFactory(classe, "Extractor");
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableManufacturerVariablePower'":
                                FGDatabase.instance.AddFactory(classe, "Factory");
                                break;
                            case "Class'/Script/FactoryGame.FGBuildableGeneratorNuclear'":
                                FGDatabase.instance.AddFactory(classe, "Generator");
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
                                FGDatabase.instance.AddFactory(classe, "Generator");
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
            AddWaterRecipe();
            AddNitrogenGasRecipe();
        }
        private void AddWaterRecipe()
        {
            string classwater = "{";
            classwater += "\"ClassName\": \"Recipe_Water_C\",";
            classwater += "\"FullName\": \"BlueprintGeneratedClass /Game/FactoryGame/Recipes/RawResources/Recipe_CrudeOil.Recipe_Water_C\",";
            classwater += "\"mDisplayName\": \"Water\",";
            classwater += "\"mIngredients\": \"((ItemClass=BlueprintGeneratedClass'\\\"/Game/FactoryGame/Resource/RawResources/Water/Desc_Water.Desc_Water_C\\\"',Amount=1))\",";
            classwater += "\"mProduct\": \"((ItemClass=BlueprintGeneratedClass'\\\"/Game/FactoryGame/Resource/RawResources/Water/Desc_Water.Desc_Water_C\\\"',Amount=1))\",";
            classwater += "\"mManufacturingMenuPriority\": \"0.000000\",";
            classwater += "\"mManufactoringDuration\": \"2.000000\",";
            classwater += "\"mManualManufacturingMultiplier\": \"1.000000\",";
            classwater += "\"mProducedIn\": \"(/Game/FactoryGame/Buildable/Factory/Converter/Build_Converter.Build_Converter_C)\",";
            classwater += "\"mRelevantEvents\": \"\",";
            classwater += "\"mVariablePowerConsumptionConstant\": \"0.000000\",";
            classwater += "\"mVariablePowerConsumptionFactor\": \"1.000000\"";
            classwater += "}";
            JsonDocument document = JsonDocument.Parse(classwater);
            AddRecipe(document.RootElement);
        }
        private void AddNitrogenGasRecipe()
        {
            string classwater = "{";
            classwater += "\"ClassName\": \"Recipe_NitrogenGas_C\",";
            classwater += "\"FullName\": \"BlueprintGeneratedClass /Game/FactoryGame/Recipes/RawResources/Recipe_CrudeOil.Recipe_NitrogenGas_C\",";
            classwater += "\"mDisplayName\": \"Nitrogen Gas\",";
            classwater += "\"mIngredients\": \"((ItemClass=BlueprintGeneratedClass'\\\"/Game/FactoryGame/Resource/RawResources/NitrogenGas/Desc_NitrogenGas.Desc_NitrogenGas_C\\\"',Amount=1))\",";
            classwater += "\"mProduct\": \"((ItemClass=BlueprintGeneratedClass'\\\"/Game/FactoryGame/Resource/RawResources/NitrogenGas/Desc_NitrogenGas.Desc_NitrogenGas_C\\\"',Amount=1))\",";
            classwater += "\"mManufacturingMenuPriority\": \"0.000000\",";
            classwater += "\"mManufactoringDuration\": \"2.000000\",";
            classwater += "\"mManualManufacturingMultiplier\": \"1.000000\",";
            classwater += "\"mProducedIn\": \"(/Game/FactoryGame/Buildable/Factory/Converter/Build_Converter.Build_Converter_C)\",";
            classwater += "\"mRelevantEvents\": \"\",";
            classwater += "\"mVariablePowerConsumptionConstant\": \"0.000000\",";
            classwater += "\"mVariablePowerConsumptionFactor\": \"1.000000\"";
            classwater += "}";
            JsonDocument document = JsonDocument.Parse(classwater);
            AddRecipe(document.RootElement);
        }

        private void AddItem(JsonElement classe, string itemType)
        {
            var element = new FGItem(classe);
            element.Type = itemType;
            instance.Items.Add(element);
        }
        private void AddFactory(JsonElement classe, string type)
        {
            string classname = classe.GetStringValue("ClassName");
            string itemName = classname.Replace("Build", "Desc");
            var item = Items.FirstOrDefault(x => x.ClassName == itemName);
            if (item != null)
            {
                var element = new FGFactory(item, type, classe);
                instance.Factories.Add(element);
            }

        }
        
        private void AddRecipe(JsonElement classe)
        {
            var element = new FGRecipe(classe);
            instance.Recipes.Add(element);
        }
        
    }
}
