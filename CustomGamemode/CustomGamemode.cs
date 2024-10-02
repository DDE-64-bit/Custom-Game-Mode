using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

namespace CustomGamemode
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class CustomGamemode : BaseUnityPlugin
    {
        // Unieke ID, naam en versie van de plugin
        private const string PluginGuid = "com.DDE64bit.CustomGamemode";
        private const string PluginName = "CustomGamemode";
        private const string PluginVersion = "1.0.0";

        public static ManualLogSource Log;

        // Harmony-instantie
        private static readonly Harmony harmonyInstance = new Harmony(PluginGuid);

        // Logging
        public static ManualLogSource PluginLog;

        // Configuratiebestand pad
        private const string ConfigFilePath = "BepInEx/config/CustomGamemodeConfig.ini";

        // Configuratie-instellingen
        private bool Disable;
        private float startFoodLevel;
        private float startWaterLevel;
        private float MaxHealth;
        private float GetMedkitHealth;
        private bool DisableWater;
        private bool DisableFood;
        private bool EnableAutoRegen;
        private float RegenThresholdFood;
        private float RegenThresholdWater;
        private float HealthRegenRate;
        private bool enableLeviathans;
        private bool DestroyAllAggressiveCreatures;
        private bool DestroyAllDefensiveCreatures;
        private bool DestroyAllPassiveCreatures;
        private bool DestroyAllCreatures;
        private bool DamageChallenge;
        private string DamagePerInterval;
        private bool NoBatteryUsage;
        private bool DisableOxygen;

        private bool CreatureChallenge;
        private bool UsePassiveCreatures;
        private bool UseAgressiveCreatures;
        private bool UseLeviathans;

        private bool DisableRadiation;

        private bool StartItems;
        private bool UseCustomItems;

        private bool RadiationSuit;
        private bool ReinforcedDiveSuit;
        private bool Stillsuit;
        private bool FirstAidKit;
        private bool FireExtinguisher;
        private bool Rebreather;
        private bool Compass;
        private bool Pipe;
        private bool PipeSurfaceFloater;
        private bool Scanner;
        private bool RepairTool;
        private bool Flashlight;
        private bool SurvivalKnife;
        private bool PathfinderTool;
        private bool AirBladder;
        private bool Flare;
        private bool HabitatBuilder;
        private bool LaserCutter;
        private bool StasisRifle;
        private bool PropulsionCannon;
        private bool LightStick;
        private bool Transfuser;
        private bool Seaglide;
        private bool MobileVehicleBay;
        private bool Beacon;
        private bool WaterproofLocker;
        private bool GravTrap;
        private bool CyclopsDepthModuleMk1;
        private bool Seamoth;
        private bool Cyclops;
        private bool PrawnSuit;
        private bool NeptuneEscapeRocket;
        private bool LithiumIonBattery;
        private bool Thermoblade;
        private bool LightweightHighCapacityTank;
        private bool UltraHighCapacityTank;
        private bool UltraGlideFins;
        private bool SwimChargeFins;
        private bool RepulsionCannon;
        private bool CyclopsDepthModuleMk2;

        private bool OnlyOneLife;

        // Singleton-instantie voor toegang vanuit andere klassen
        public static CustomGamemode Instance { get; private set; }

        private void Awake()
        {
            // Singleton-initialisatie
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            Logger.LogInfo(PluginName + " " + PluginVersion + " loaded.");
            PluginLog = Logger;

            // Laad configuratiewaarden
            LoadConfiguration();

            // Patches toepassen met Harmony
            if (!Disable)
            {
                harmonyInstance.PatchAll();
            }

            // Toon geladen waarden
            PluginLog.LogInfo($"Disable: {Disable}");
            PluginLog.LogInfo($"Starting food level: {startFoodLevel}");
            PluginLog.LogInfo($"Starting water level: {startWaterLevel}");
            PluginLog.LogInfo($"Max Health: {MaxHealth}");
            PluginLog.LogInfo($"Medkit Health: {GetMedkitHealth}");
            PluginLog.LogInfo($"Disable Food: {DisableFood}");
            PluginLog.LogInfo($"Disable Water: {DisableWater}");
            PluginLog.LogInfo($"Disable Oxygen: {DisableOxygen}");
            PluginLog.LogInfo($"Enable Auto Regen: {EnableAutoRegen}");
            PluginLog.LogInfo($"Regen Threshold Food: {RegenThresholdFood}");
            PluginLog.LogInfo($"Regen Threshold Water: {RegenThresholdWater}");
            PluginLog.LogInfo($"Health Regen Rate: {HealthRegenRate}");
            PluginLog.LogInfo($"Damage Challenge: {DamageChallenge}");
            PluginLog.LogInfo($"Damage Per Interval: {DamagePerInterval}");

            PluginLog.LogInfo($"Creature Challenge: {CreatureChallenge}");
            PluginLog.LogInfo($"Use Passive Creatures: {UsePassiveCreatures}");
            PluginLog.LogInfo($"Use Agressive Creatures: {UseAgressiveCreatures}");
            PluginLog.LogInfo($"Use Leviathans: {UseLeviathans}");

            PluginLog.LogInfo($"Disable Radiation: {DisableRadiation}");

            PluginLog.LogInfo($"No Battery Usage: {NoBatteryUsage}");

            PluginLog.LogInfo($"Start Items: {StartItems}");

            PluginLog.LogInfo($"Only One Life: {OnlyOneLife}");

            PluginLog.LogInfo($"Destroy Leviathans: {enableLeviathans}");
            PluginLog.LogInfo($"Destroy All Aggressive Creatures: {DestroyAllAggressiveCreatures}");
            PluginLog.LogInfo($"Destroy All Defensive Creatures: {DestroyAllDefensiveCreatures}");
            PluginLog.LogInfo($"Destroy All Passive Creatures: {DestroyAllPassiveCreatures}");
            PluginLog.LogInfo($"Destroy All Creatures: {DestroyAllCreatures}");
        }

        // Laad configuratiewaarden uit een .ini bestand
        private void LoadConfiguration()
        {
            if (!File.Exists(ConfigFilePath))
            {
                // Maak het .ini bestand met standaardwaarden als het niet bestaat
                PluginLog.LogInfo("Configuration file not found. Creating a new one with default values.");
                CreateDefaultConfigFile();
            }

            // Lees het .ini bestand
            var parser = new IniParser(ConfigFilePath);

            // Haal de waarden op uit het .ini bestand
            Disable = parser.GetValue("Settings", "Disable", false);

            startFoodLevel = parser.GetValue("Settings", "StartFoodLevel", 50.0f);
            startWaterLevel = parser.GetValue("Settings", "StartWaterLevel", 50.0f);

            MaxHealth = parser.GetValue("Settings", "MaxHealth", 100.0f);

            GetMedkitHealth = parser.GetValue("Settings", "MedkitHealth", 50.0f);

            DisableWater = parser.GetValue("Settings", "DisableWater", false);
            DisableFood = parser.GetValue("Settings", "DisableFood", false);

            DisableOxygen = parser.GetValue("Settings", "DisableOxygen", false);

            EnableAutoRegen = parser.GetValue("Settings", "EnableAutoRegen", false);
            RegenThresholdFood = parser.GetValue("Settings", "RegenThresholdFood", 90.0f);
            RegenThresholdWater = parser.GetValue("Settings", "RegenThresholdWater", 90.0f);
            HealthRegenRate = parser.GetValue("Settings", "HealthRegenRate", 1.0f);

            DamageChallenge = parser.GetValue("Settings", "DamageChallenge", false);
            DamagePerInterval = parser.GetValue("Settings", "Mode", "easy");

            CreatureChallenge = parser.GetValue("Settings", "CreatureChallenge", false);
            UsePassiveCreatures = parser.GetValue("Settings", "UsePassiveCreatures", false);
            UseAgressiveCreatures = parser.GetValue("Settings", "UseAgressiveCreatures", false);
            UseLeviathans = parser.GetValue("Settings", "UseLeviathans", false);

            NoBatteryUsage = parser.GetValue("Settings", "NoBatteryUsage", false);

            DisableRadiation = parser.GetValue("Settings", "DisableRadiation", false);

            StartItems = parser.GetValue("Settings", "StartItems", false);
            UseCustomItems = parser.GetValue("Settings", "UseCustomItems", false);

            enableLeviathans = parser.GetValue("Settings", "DestroyLeviathans", false);
            DestroyAllAggressiveCreatures = parser.GetValue("Settings", "DestroyAllAggressiveCreatures", false);
            DestroyAllDefensiveCreatures = parser.GetValue("Settings", "DestroyAllDefensiveCreatures", false);
            DestroyAllPassiveCreatures = parser.GetValue("Settings", "DestroyAllPassiveCreatures", false);
            DestroyAllCreatures = parser.GetValue("Settings", "DestroyAllCreatures", false);

            OnlyOneLife = parser.GetValue("Settings", "OnlyOneLife", false);

            // Haal waarden op voor elk nieuw item
            RadiationSuit = parser.GetValue("Settings", "RadiationSuit", false);
            ReinforcedDiveSuit = parser.GetValue("Settings", "ReinforcedDiveSuit", false);
            Stillsuit = parser.GetValue("Settings", "Stillsuit", false);
            FirstAidKit = parser.GetValue("Settings", "FirstAidKit", false);
            FireExtinguisher = parser.GetValue("Settings", "FireExtinguisher", false);
            Rebreather = parser.GetValue("Settings", "Rebreather", false);
            Compass = parser.GetValue("Settings", "Compass", false);
            Pipe = parser.GetValue("Settings", "Pipe", false);
            PipeSurfaceFloater = parser.GetValue("Settings", "PipeSurfaceFloater", false);
            Scanner = parser.GetValue("Settings", "Scanner", false);
            RepairTool = parser.GetValue("Settings", "RepairTool", false);
            Flashlight = parser.GetValue("Settings", "Flashlight", false);
            SurvivalKnife = parser.GetValue("Settings", "SurvivalKnife", false);
            PathfinderTool = parser.GetValue("Settings", "PathfinderTool", false);
            AirBladder = parser.GetValue("Settings", "AirBladder", false);
            Flare = parser.GetValue("Settings", "Flare", false);
            HabitatBuilder = parser.GetValue("Settings", "HabitatBuilder", false);
            LaserCutter = parser.GetValue("Settings", "LaserCutter", false);
            StasisRifle = parser.GetValue("Settings", "StasisRifle", false);
            PropulsionCannon = parser.GetValue("Settings", "PropulsionCannon", false);
            LightStick = parser.GetValue("Settings", "LightStick", false);
            Transfuser = parser.GetValue("Settings", "Transfuser", false);
            Seaglide = parser.GetValue("Settings", "Seaglide", false);
            MobileVehicleBay = parser.GetValue("Settings", "MobileVehicleBay", false);
            Beacon = parser.GetValue("Settings", "Beacon", false);
            WaterproofLocker = parser.GetValue("Settings", "WaterproofLocker", false);
            GravTrap = parser.GetValue("Settings", "GravTrap", false);
            CyclopsDepthModuleMk1 = parser.GetValue("Settings", "CyclopsDepthModuleMk1", false);
            Seamoth = parser.GetValue("Settings", "Seamoth", false);
            Cyclops = parser.GetValue("Settings", "Cyclops", false);
            PrawnSuit = parser.GetValue("Settings", "PrawnSuit", false);
            NeptuneEscapeRocket = parser.GetValue("Settings", "NeptuneEscapeRocket", false);
            LithiumIonBattery = parser.GetValue("Settings", "LithiumIonBattery", false);
            Thermoblade = parser.GetValue("Settings", "Thermoblade", false);
            LightweightHighCapacityTank = parser.GetValue("Settings", "LightweightHighCapacityTank", false);
            UltraHighCapacityTank = parser.GetValue("Settings", "UltraHighCapacityTank", false);
            UltraGlideFins = parser.GetValue("Settings", "UltraGlideFins", false);
            SwimChargeFins = parser.GetValue("Settings", "SwimChargeFins", false);
            RepulsionCannon = parser.GetValue("Settings", "RepulsionCannon", false);
            CyclopsDepthModuleMk2 = parser.GetValue("Settings", "CyclopsDepthModuleMk2", false);
        }

        // Maak het standaard .ini configuratiebestand
        private void CreateDefaultConfigFile()
        {
            using (StreamWriter writer = new StreamWriter(ConfigFilePath))
            {
                writer.WriteLine("[Settings]");
                writer.WriteLine();
                writer.WriteLine("# Warning this mod could interfere with other mods.");
                writer.WriteLine();
                writer.WriteLine("# Disables all the settings and allows you play without this mod ):");
                writer.WriteLine("Disable=false");
                writer.WriteLine();
                writer.WriteLine("# This lets you decide with how much food and water you start, value's between 0 and 100");
                writer.WriteLine("StartFoodLevel=50.0");
                writer.WriteLine("StartWaterLevel=50.0");
                writer.WriteLine();
                writer.WriteLine("DisableFood=false");
                writer.WriteLine("DisableWater=false");
                writer.WriteLine();
                writer.WriteLine("DisableOxygen=false");
                writer.WriteLine();
                //writer.WriteLine("MaxHealth=100.0");
                writer.WriteLine("# Gain health every 10 seconds, when food and water is above the threshold");
                writer.WriteLine("EnableAutoRegen=false");
                writer.WriteLine("RegenThresholdFood=90.0");
                writer.WriteLine("RegenThresholdWater=90.0");
                writer.WriteLine("# Amount of health that you regenerate per 10 seconds");
                writer.WriteLine("HealthRegenRate=1.0");
                writer.WriteLine();
                writer.WriteLine("# Get damage every 10 seconds");
                writer.WriteLine("DamageChallenge=false");
                writer.WriteLine("# Choose between: easy, medium, impossible");
                writer.WriteLine("Mode=easy");
                writer.WriteLine();
                writer.WriteLine("# Randomly spawns a creature on you");
                writer.WriteLine("CreatureChallenge=false");
                writer.WriteLine("UsePassiveCreatures=false");
                writer.WriteLine("UseAgressiveCreatures=false");
                writer.WriteLine("UseLeviathans=false");
                writer.WriteLine();
                writer.WriteLine("NoBatteryUsage=false");
                writer.WriteLine();
                writer.WriteLine("MedkitHealth=50.0");
                writer.WriteLine();
                writer.WriteLine("DisableRadiation=false");
                writer.WriteLine();
                writer.WriteLine("# If you want start items like in creative this is the setting.");
                writer.WriteLine("# Default is: 5 floaters, habitat builder, propulsion cannon, statis rifle, mobile vehicle bay, flashlight, seaglide, knife, fins.");
                writer.WriteLine("StartItems=false");
                writer.WriteLine("UseCustomItems=false");
                writer.WriteLine("-------------------------");
                writer.WriteLine("RadiationSuit=false");
                writer.WriteLine("ReinforcedDiveSuit=false");
                //writer.WriteLine("Stillsuit=false");
                writer.WriteLine("FirstAidKit=false");
                writer.WriteLine("FireExtinguisher=false");
                writer.WriteLine("Rebreather=false");
                writer.WriteLine("Compass=false");
                writer.WriteLine("Pipe=false");
                writer.WriteLine("PipeSurfaceFloater=false");
                writer.WriteLine("Scanner=false");
                writer.WriteLine("RepairTool=false");
                writer.WriteLine("Flashlight=false");
                writer.WriteLine("SurvivalKnife=false");
                writer.WriteLine("PathfinderTool=false");
                writer.WriteLine("AirBladder=false");
                writer.WriteLine("Flare=false");
                writer.WriteLine("HabitatBuilder=false");
                writer.WriteLine("LaserCutter=false");
                writer.WriteLine("StasisRifle=false");
                writer.WriteLine("PropulsionCannon=false");
                writer.WriteLine("LightStick=false");
                writer.WriteLine("Transfuser=false");
                writer.WriteLine("Seaglide=false");
                writer.WriteLine("MobileVehicleBay=false");
                writer.WriteLine("Beacon=false");
                writer.WriteLine("WaterproofLocker=false");
                writer.WriteLine("GravTrap=false");
                writer.WriteLine("CyclopsDepthModuleMk1=false");
                //writer.WriteLine("Seamoth=false");
                //writer.WriteLine("Cyclops=false");
                //writer.WriteLine("PrawnSuit=false");
                //writer.WriteLine("NeptuneEscapeRocket=false");
                writer.WriteLine("LithiumIonBattery=false");
                writer.WriteLine("Thermoblade=false");
                writer.WriteLine("LightweightHighCapacityTank=false");
                writer.WriteLine("UltraHighCapacityTank=false");
                writer.WriteLine("UltraGlideFins=false");
                writer.WriteLine("SwimChargeFins=false");
                writer.WriteLine("RepulsionCannon=false");
                writer.WriteLine("CyclopsDepthModuleMk2=false");
                writer.WriteLine();
                writer.WriteLine("# When you die the world is gone (just like hardcore)");
                writer.WriteLine("OnlyOneLife=false");
                writer.WriteLine();
                writer.WriteLine("# Enable or disenable creatures");
                writer.WriteLine("DestroyLeviathans=false");
                writer.WriteLine("DestroyAllAggressiveCreatures=false");
                writer.WriteLine("DestroyAllDefensiveCreatures=false");
                writer.WriteLine("DestroyAllPassiveCreatures=false");
                writer.WriteLine("DestroyAllCreatures=false");
            }
        }

        // Openbaar beschikbare methoden voor het verkrijgen van de configuratiewaarden
        public bool GetDisable() => Disable;
        public float GetStartFoodLevel() => startFoodLevel;
        public float GetStartWaterLevel() => startWaterLevel;
        public float GetMaxHealth() => MaxHealth;
        public float GetGetMedkitHealth() => GetMedkitHealth;
        public bool GetDisableFood() => DisableFood;
        public bool GetDisableWater() => DisableWater;
        public bool GetEnableAutoRegen() => EnableAutoRegen;
        public float GetRegenThresholdFood() => RegenThresholdFood;
        public float GetRegenThresholdWater() => RegenThresholdWater;
        public float GetHealthRegenRate() => HealthRegenRate;
        public bool GetDestroyLeviathans() => enableLeviathans;
        public bool GetDestroyAllAggressiveCreatures() => DestroyAllAggressiveCreatures;
        public bool GetDestroyAllDefensiveCreatures() => DestroyAllDefensiveCreatures;
        public bool GetDestroyAllPassiveCreatures() => DestroyAllPassiveCreatures;
        public bool GetDestroyAllCreatures() => DestroyAllCreatures;
        public bool GetDamageChallenge() => DamageChallenge;
        public string GetDamagePerInterval() => DamagePerInterval;
        public bool GetNoBatteryUsage() => NoBatteryUsage;
        public bool GetDisableOxygen() => DisableOxygen;
        public bool GetCreatureChallenge() => CreatureChallenge;
        public bool GetUsePassiveCreatures() => UsePassiveCreatures;
        public bool GetUseAgressiveCreatures() => UseAgressiveCreatures;
        public bool GetUseLeviathans() => UseLeviathans;
        public bool GetDisableRadiation() => DisableRadiation;
        public bool GetStartItems() => StartItems;
        public bool GetUseCustomItems() => UseCustomItems;

        // Methoden voor het verkrijgen van de configuratiewaarden voor elk nieuw item
        public bool GetRadiationSuit() => RadiationSuit;
        public bool GetReinforcedDiveSuit() => ReinforcedDiveSuit;
        public bool GetStillsuit() => Stillsuit;
        public bool GetFirstAidKit() => FirstAidKit;
        public bool GetFireExtinguisher() => FireExtinguisher;
        public bool GetRebreather() => Rebreather;
        public bool GetCompass() => Compass;
        public bool GetPipe() => Pipe;
        public bool GetPipeSurfaceFloater() => PipeSurfaceFloater;
        public bool GetScanner() => Scanner;
        public bool GetRepairTool() => RepairTool;
        public bool GetFlashlight() => Flashlight;
        public bool GetSurvivalKnife() => SurvivalKnife;
        public bool GetPathfinderTool() => PathfinderTool;
        public bool GetAirBladder() => AirBladder;
        public bool GetFlare() => Flare;
        public bool GetHabitatBuilder() => HabitatBuilder;
        public bool GetLaserCutter() => LaserCutter;
        public bool GetStasisRifle() => StasisRifle;
        public bool GetPropulsionCannon() => PropulsionCannon;
        public bool GetLightStick() => LightStick;
        public bool GetTransfuser() => Transfuser;
        public bool GetSeaglide() => Seaglide;
        public bool GetMobileVehicleBay() => MobileVehicleBay;
        public bool GetBeacon() => Beacon;
        public bool GetWaterproofLocker() => WaterproofLocker;
        public bool GetGravTrap() => GravTrap;
        public bool GetCyclopsDepthModuleMk1() => CyclopsDepthModuleMk1;
        public bool GetSeamoth() => Seamoth;
        public bool GetCyclops() => Cyclops;
        public bool GetPrawnSuit() => PrawnSuit;
        public bool GetNeptuneEscapeRocket() => NeptuneEscapeRocket;
        public bool GetLithiumIonBattery() => LithiumIonBattery;
        public bool GetThermoblade() => Thermoblade;
        public bool GetLightweightHighCapacityTank() => LightweightHighCapacityTank;
        public bool GetUltraHighCapacityTank() => UltraHighCapacityTank;
        public bool GetUltraGlideFins() => UltraGlideFins;
        public bool GetSwimChargeFins() => SwimChargeFins;
        public bool GetRepulsionCannon() => RepulsionCannon;
        public bool GetCyclopsDepthModuleMk2() => CyclopsDepthModuleMk2;

        public bool GetOnlyOneLife() => OnlyOneLife;
    }
}
