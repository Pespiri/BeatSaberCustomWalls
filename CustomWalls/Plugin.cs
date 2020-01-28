using CustomWalls.Data;
using CustomWalls.HarmonyPatches;
using CustomWalls.Settings;
using CustomWalls.Settings.UI;
using CustomWalls.Utilities;
using IPA;
using IPA.Config;
using IPA.Loader;
using IPA.Utilities;
using System.IO;
using System.Linq;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;

namespace CustomWalls
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        public static string PluginName => "CustomWalls";
        public static SemVer.Version PluginVersion { get; private set; } = new SemVer.Version("0.0.0");
        public static string PluginAssetPath => Path.Combine(UnityGame.InstallPath, "CustomWalls");

        [Init]
        public void Init(IPALogger logger, Config config, PluginMetadata metadata)
        {
            Logger.log = logger;
            Configuration.Init(config);

            if (metadata?.Version != null)
            {
                PluginVersion = metadata.Version;
            }
        }

        [OnEnable]
        public void OnEnable() => Load();
        [OnDisable]
        public void OnDisable() => Unload();
        [OnExit]
        public void OnApplicationQuit() => Unload();

        private void OnGameSceneLoaded()
        {
            MaterialUtils.CurrentColorManager = Resources.FindObjectsOfTypeAll<ColorManager>().LastOrDefault();

            CustomMaterial customMaterial = MaterialAssetLoader.CustomMaterialObjects[MaterialAssetLoader.SelectedMaterial];
            if (customMaterial.Descriptor.DisablesScore
                || Configuration.UserDisabledScores)
            {
                ScoreUtility.DisableScoreSubmission("Material");
            }
            else if (ScoreUtility.ScoreIsBlocked)
            {
                ScoreUtility.EnableScoreSubmission("Material");
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
            ScoreUtility.Cleanup();
            CustomMaterialsPatches.RemoveHarmonyPatches();
            Configuration.Save();
            MaterialAssetLoader.Clear();
            RemoveEvents();
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
