using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;

namespace CustomWalls.Settings.UI
{
    internal class SettingsUI
    {
        public static MaterialsFlowCoordinator materialsFlowCoordinator;
        public static bool created = false;

        public static void CreateMenu()
        {
            if (!created)
            {
                MenuButton menuButton = new MenuButton("Custom Walls", "Change Custom Walls Here!", MaterialsMenuButtonPressed, true);
                MenuButtons.instance.RegisterButton(menuButton);

                created = true;
            }
        }

        public static void ShowMaterialsFlow()
        {
            if (materialsFlowCoordinator == null)
            {
                materialsFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<MaterialsFlowCoordinator>();
            }

            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(materialsFlowCoordinator, null, false, false);
        }

        private static void MaterialsMenuButtonPressed() => ShowMaterialsFlow();
    }
}
