using CustomWalls.Settings.Utilities;
using IPA.Config;
using IPA.Utilities;

namespace CustomWalls.Settings
{
    public class Configuration
    {
        private static Ref<PluginConfig> config;
        private static IConfigProvider configProvider;

        public static string CurrentlySelectedMaterial { get; internal set; }
        public static bool EnableObstacleFrame { get; internal set; }

        public static bool UserDisabledScores { get; internal set; }

        internal static void Init(IConfigProvider cfgProvider)
        {
            configProvider = cfgProvider;
            config = cfgProvider.MakeLink<PluginConfig>((p, v) =>
            {
                if (v.Value == null || v.Value.RegenerateConfig)
                {
                    p.Store(v.Value = new PluginConfig() { RegenerateConfig = false });
                }
                config = v;
            });
        }

        internal static void Load()
        {
            CurrentlySelectedMaterial = config.Value.SelectedWallMaterial;
            EnableObstacleFrame = config.Value.EnableObstacleFrame;
        }

        internal static void Save()
        {
            config.Value.SelectedWallMaterial = CurrentlySelectedMaterial;
            config.Value.EnableObstacleFrame = EnableObstacleFrame;

            configProvider.Store(config.Value);
        }
    }
}
