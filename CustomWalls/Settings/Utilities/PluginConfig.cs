namespace CustomWalls.Settings.Utilities
{
    public class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        public string SelectedWallMaterial;
        public bool EnableObstacleFrame = true;
    }
}
