﻿using CustomWalls.Data;
using CustomWalls.HarmonyPatches;
using CustomWalls.Settings;
using CustomWalls.Settings.UI;
using CustomWalls.Utilities;
using Hive.Versioning;
using IPA;
using IPA.Config;
using IPA.Loader;
using IPA.Utilities;
using System.IO;
using IPALogger = IPA.Logging.Logger;

namespace CustomWalls
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        public static string PluginName => "CustomWalls";
        public static Version PluginVersion { get; private set; } = new Version("0.0.0");
        public static string PluginAssetPath => Path.Combine(UnityGame.InstallPath, "CustomWalls");

        [Init]
        public void Init(IPALogger logger, Config config, PluginMetadata metadata)
        {
            Logger.log = logger;
            Configuration.Init(config);

            if (metadata?.HVersion != null)
            {
                PluginVersion = metadata.HVersion;
            }
        }

        [OnEnable]
        public void OnEnable() => Load();
        [OnDisable]
        public void OnDisable() => Unload();

        private void OnGameSceneLoaded()
        {
            CustomMaterial customMaterial = MaterialAssetLoader.CustomMaterialObjects[MaterialAssetLoader.SelectedMaterial];
            if (customMaterial.Descriptor.DisablesScore
                || Configuration.UserDisabledScores)
            {
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission(PluginName);
                Logger.log.Info("ScoreSubmission has been disabled.");
            }
        }

        private void Load()
        {
            Configuration.Load();
            MaterialAssetLoader.Load();
            CustomMaterialsPatches.ApplyHarmonyPatches();
            SettingsUI.CreateMenu();
            AddEvents();

            Logger.log.Info($"{PluginName} v.{PluginVersion} has started.");
        }

        private void Unload()
        {
            RemoveEvents();
            CustomMaterialsPatches.RemoveHarmonyPatches();
            Configuration.Save();
            MaterialAssetLoader.Clear();
            SettingsUI.RemoveMenu();
        }

        private void AddEvents()
        {
            RemoveEvents();
            BS_Utils.Utilities.BSEvents.gameSceneLoaded += OnGameSceneLoaded;
        }

        private void RemoveEvents()
        {
            BS_Utils.Utilities.BSEvents.gameSceneLoaded -= OnGameSceneLoaded;
        }
    }
}
