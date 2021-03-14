using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;

namespace CustomWalls.Settings.UI
{
    internal class SettingsUI
    {
        private static readonly MenuButton menuButton = new MenuButton("Custom Walls", "Change Custom Walls Here!", MaterialsMenuButtonPressed, true);

        public static MaterialsFlowCoordinator materialsFlowCoordinator;
        public static bool created = false;

        public static void CreateMenu()
        {
            if (!created)
            {
                MenuButtons.instance.RegisterButton(menuButton);
                created = true;
            }
        }

        public static void RemoveMenu()
        {
            if (created)
            {
                MenuButtons.instance.UnregisterButton(menuButton);
                created = false;
            }
        }

        public static void ShowMaterialsFlow()
        {
            if (materialsFlowCoordinator == null)
            {
                materialsFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<MaterialsFlowCoordinator>();
            }

            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(materialsFlowCoordinator);
        }

        private static void MaterialsMenuButtonPressed() => ShowMaterialsFlow();
    }
}
