// These are your imports, mostly you'll be needing these 5 for every plugin. Some will need more.

using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
// using static Obeliskial_Essentials.Essentials;
using System;
using System.Collections.Generic;
using static Obeliskial_Essentials.Essentials;
using static Obeliskial_Essentials.CardDescriptionNew;
using BepInEx.Bootstrap;


// The Plugin csharp file is used to specify some general info about your plugin. and set up things for 


// Make sure all your files have the same namespace and this namespace matches the RootNamespace in the .csproj file
// All files that are in the same namespace are compiled together and can "see" each other more easily.

namespace OversizedPets
{
    // These are used to create the actual plugin. If you don't need Obeliskial Essentials for your mod, 
    // delete the BepInDependency and the associated code "RegisterMod()" below.

    // If you have other dependencies, such as obeliskial content, make sure to include them here.
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    // [BepInDependency("com.stiffmeds.obeliskialessentials")] // this is the name of the .dll in the !libs folder.
    [BepInProcess("AcrossTheObelisk.exe")] //Don't change this

    // If PluginInfo isn't working, you are either:
    // 1. Using BepInEx v6
    // 2. Have an issue with your csproj file (not loading the analyzer or BepInEx appropriately)
    // 3. You have an issue with your solution file (not referencing the correct csproj file)


    public class Plugin : BaseUnityPlugin
    {

        // If desired, you can create configs for users by creating a ConfigEntry object here, 
        // Configs allows users to specify certain things about the mod. 
        // The most common would be a flag to enable/disable portions of the mod or the entire mod.

        // You can use: config = Config.Bind() to set the title, default value, and description of the config.
        // It automatically creates the appropriate configs.
        public static bool EssentialsInstalled = false;
        public static ConfigEntry<bool> EnableMod { get; set; }
        public static ConfigEntry<bool> EnableDebugging { get; set; }
        public static ConfigEntry<bool> IncreasePetSize { get; set; }
        public static ConfigEntry<float> PetSizeScaleFactor { get; set; }
        public static ConfigEntry<bool> IncreaseHeroSize { get; set; }
        public static ConfigEntry<float> HeroSizeScaleFactor { get; set; }
        public static ConfigEntry<bool> IncreaseNPCSize { get; set; }
        public static ConfigEntry<float> NPCSizeScaleFactor { get; set; }

        internal int ModDate = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
        private readonly Harmony harmony = new(PluginInfo.PLUGIN_GUID);
        internal static ManualLogSource Log;

        public static string debugBase = $"{PluginInfo.PLUGIN_GUID} ";

        private void Awake()
        {

            // The Logger will allow you to print things to the LogOutput (found in the BepInEx directory)
            Log = Logger;
            Log.LogInfo($"{PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} has loaded!");

            // Sets the title, default values, and descriptions
            string modName = "OversizedPets";
            EnableMod = Config.Bind(new ConfigDefinition(modName, "EnableMod"), true, new ConfigDescription("Enables the mod. If false, the mod will not work then next time you load the game."));
            EnableDebugging = Config.Bind(new ConfigDefinition(modName, "EnableDebugging"), false, new ConfigDescription("Enables the debugging"));
            IncreasePetSize = Config.Bind(new ConfigDefinition(modName, "Increase Pet Size"), true, new ConfigDescription("If true, will increase all pets by the PetSizeScaleFactor. Updates every round, so restart combat to see changes."));
            PetSizeScaleFactor = Config.Bind(new ConfigDefinition(modName, "PetSizeScaleFactor"), 1.25f, new ConfigDescription("Scale factor to increase pets by. 1.0 = no change, 2.0 = double size, 0.5 = half size. Does not work if less than 0.05."));
            IncreaseHeroSize = Config.Bind(new ConfigDefinition(modName, "Increase Hero Size"), false, new ConfigDescription("If true, will increase all Heroes by the HeroSizeScaleFactor. Updates every round, so restart combat to see changes."));
            HeroSizeScaleFactor = Config.Bind(new ConfigDefinition(modName, "HeroSizeScaleFactor"), 1.25f, new ConfigDescription("Scale factor to increase Heroes by. 1.0 = no change, 2.0 = double size, 0.5 = half size. Does not work if less than 0.05."));
            IncreaseNPCSize = Config.Bind(new ConfigDefinition(modName, "Increase NPC Size"), false, new ConfigDescription("If true, will increase all NPCs by the NPCSizeScaleFactor. Updates every round, so restart combat to see changes."));
            NPCSizeScaleFactor = Config.Bind(new ConfigDefinition(modName, "NPCSizeScaleFactor"), 1.25f, new ConfigDescription("Scale factor to increase NPCs by. 1.0 = no change, 2.0 = double size, 0.5 = half size. Does not work if less than 0.05."));
            if (PetSizeScaleFactor.Value < 0.05f) { PetSizeScaleFactor.Value = 1.0f; }
            if (HeroSizeScaleFactor.Value < 0.05f) { HeroSizeScaleFactor.Value = 1.0f; }
            if (NPCSizeScaleFactor.Value < 0.05f) { NPCSizeScaleFactor.Value = 1.0f; }

            PetSizeScaleFactor.Value = 2.0f;
            // HeroSizeScaleFactor.Value = 1.5f;
            // NPCSizeScaleFactor.Value = 1.25f;

            EssentialsInstalled = Chainloader.PluginInfos.ContainsKey("com.stiffmeds.obeliskialessentials");

            // Register with Obeliskial Essentials
            if (EssentialsInstalled)
            {
                RegisterMod(
                    _name: PluginInfo.PLUGIN_NAME,
                    _author: "binbin",
                    _description: "Oversized Pets",
                    _version: PluginInfo.PLUGIN_VERSION,
                    _date: ModDate,
                    _link: @"https://github.com/binbinmods/SkilledNPCs"
                );

            }

            // apply patches, this functionally runs all the code for Harmony, running your mod
            if (EnableMod.Value) { harmony.PatchAll(); }
        }


        // These are some functions to make debugging a tiny bit easier.
        internal static void LogDebug(string msg)
        {
            if (EnableDebugging.Value)
            {
                Log.LogDebug(debugBase + msg);
            }

        }
        internal static void LogInfo(string msg)
        {
            Log.LogInfo(debugBase + msg);
        }
        internal static void LogError(string msg)
        {
            Log.LogError(debugBase + msg);
        }
    }
}