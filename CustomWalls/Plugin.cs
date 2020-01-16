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
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace CustomWalls
{
    public class Plugin : IBeatSaberPlugin, IDisablablePlugin
    {
        public static string PluginName => "CustomWalls";
        public static SemVer.Version PluginVersion { get; private set; } = new SemVer.Version("0.0.0");
        public static string PluginAssetPath => Path.Combine(BeatSaber.InstallPath, "CustomWalls");

        public void Init(IPALogger logger, [Config.Prefer("json")] IConfigProvider cfgProvider, PluginLoader.PluginMetadata metadata)
        {
            Logger.log = logger;
            Configuration.Init(cfgProvider);

            if (metadata?.Version != null)
            {
                PluginVersion = metadata.Version;
            }
        }

        public void OnEnable() => Load();
        public void OnDisable() => Unload();
        public void OnApplicationQuit() => Unload();

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "GameCore")
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
        }

        public void OnApplicationStart() { }
        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) { }
        public void OnSceneUnloaded(Scene scene) { }
        public void OnUpdate() { }
        public void OnFixedUpdate() { }

        private void Load()
        {
            Configuration.Load();
            MaterialAssetLoader.LoadCustomMaterials();
            CustomMaterialsPatches.ApplyHarmonyPatches();
            SettingsUI.CreateMenu();

            Logger.log.Info($"{PluginName} v.{PluginVersion} has started.");
        }

        private void Unload()
        {
            CustomMaterialsPatches.RemoveHarmonyPatches();
            ScoreUtility.Cleanup();
            Configuration.Save();
        }
    }
}
