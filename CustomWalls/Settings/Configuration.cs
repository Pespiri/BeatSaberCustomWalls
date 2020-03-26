using CustomWalls.Settings.Utilities;
using IPA.Config;
using IPA.Config.Stores;

namespace CustomWalls.Settings
{
    public class Configuration
    {
        public static string CurrentlySelectedMaterial { get; internal set; }
        public static bool EnableObstacleFrame { get; internal set; }

        public static bool UserDisabledScores { get; internal set; }

        internal static void Init(Config config)
        {
            PluginConfig.Instance = config.Generated<PluginConfig>();
        }

        internal static void Load()
        {
            CurrentlySelectedMaterial = PluginConfig.Instance.SelectedWallMaterial;
            EnableObstacleFrame = PluginConfig.Instance.EnableObstacleFrame;
        }

        internal static void Save()
        {
            PluginConfig.Instance.SelectedWallMaterial = CurrentlySelectedMaterial;
            PluginConfig.Instance.EnableObstacleFrame = EnableObstacleFrame;
        }
    }
}
