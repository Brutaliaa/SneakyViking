using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ServerSync;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;



namespace SneakyViking
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
    [BepInIncompatibility("org.bepinex.plugins.valheim_plus")]
    public class SneakyViking : BaseUnityPlugin
    {
        private const string pluginGUID = "Brutaliaa.SneakyViking";
        private const string pluginName = "SneakyViking";
        private const string pluginVersion = "1.0.0";
        private static string ConfigFileName = "Brutaliaa.SneakyViking.cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
        internal static string ConnectionError = "";
        private readonly Harmony _harmony = new Harmony(pluginGUID);
        private static readonly ConfigSync ConfigSync = new(pluginGUID)
        { DisplayName = pluginName, CurrentVersion = pluginVersion, MinimumRequiredVersion = pluginVersion };
        public enum Toggle
        {
            On = 1,
            Off = 0
        }
        public static float CurrentNoise;
        public static float LastSneak;
        public static string CurrentNoiseSource;
        private static readonly Dictionary<float, string> MovementNoiseByRange = new Dictionary<float, string>
        {
            { 5f, "Dodge" },
            { 15f, "Walk / Swim" },
            { 30f, "Run / Jump" }
        };
        private static ConfigEntry<float> _maximumReductionPercent;
        private static ConfigEntry<bool> _reduceDodgeNoise;
        private static ConfigEntry<bool> _reduceWalkSwimNoise;
        private static ConfigEntry<bool> _reduceRunJumpNoise;
        private static ConfigEntry<Toggle> _serverConfigLocked;

        public void Awake()
        {
            bool saveOnSet = Config.SaveOnConfigSet;
            Config.SaveOnConfigSet = false; // This and the variable above are used to prevent the config from saving on startup for each config entry. This is speeds up the startup process.

            _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On, "If on, the configuration is locked and can be changed by server admins only.");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

            _maximumReductionPercent = config("2 - Noise", "MaximumReductionPercent", 80f, "Maximum reduction at Sneak skill level 100. 0 Disables it.");
            _reduceDodgeNoise = config("2 - Noise", "ReduceDodgeNoise", true, "Apply the reduction to dodge noise.");
            _reduceWalkSwimNoise = config("2 - Noise", "ReduceWalkSwimNoise", true, "Apply the reduction to walking and swimming noise.");
            _reduceRunJumpNoise = config("2 - Noise", "ReduceRunJumpNoise", true, "Apply the reduction to running and jumping noise.");

            Assembly assembly = Assembly.GetExecutingAssembly();
            Harmony harmony = new(pluginGUID);
            harmony.PatchAll(assembly);
            SetupWatcher();

            if (saveOnSet)
            {
                Config.SaveOnConfigSet = saveOnSet;
                Config.Save();
            }            
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                Config.Reload();
            }
            catch
            { }
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true)
        {
            ConfigDescription configDescription = new ConfigDescription(
                description + (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"));
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, configDescription);
            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;
            return configEntry;
        }

        [HarmonyPatch(typeof(Character), nameof(Character.RPC_AddNoise))]
        private class NoisePatch
        {
            [HarmonyPrefix]
            private static void Prefix(Character __instance, ref float range)
            {
                if (!(__instance is Player player))
                    return;

                float sneak = player.GetSkills().GetSkillFactor(Skills.SkillType.Sneak);
                float reduction = Math.Min(Math.Max(sneak, 0f), 1f) * Math.Min(Math.Max(_maximumReductionPercent.Value / 100f, 0f), 1f);
                range *= 1f - reduction;
            }
        }
    }
}