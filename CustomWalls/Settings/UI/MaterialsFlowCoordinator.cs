using BeatSaberMarkupLanguage;
using HMUI;
using System;

namespace CustomWalls.Settings.UI
{
    internal class MaterialsFlowCoordinator : FlowCoordinator
    {
        private MaterialListViewController materialsListView;
        private MaterialPreviewViewController materialsPreviewView;
        private MaterialDetailsViewController materialsDescriptionView;

        public void Awake()
        {
            if (!materialsPreviewView)
            {
                materialsPreviewView = BeatSaberUI.CreateViewController<MaterialPreviewViewController>();
            }

            if (!materialsDescriptionView)
            {
                materialsDescriptionView = BeatSaberUI.CreateViewController<MaterialDetailsViewController>();
            }

            if (!materialsListView)
            {
                materialsListView = BeatSaberUI.CreateViewController<MaterialListViewController>();
                materialsListView.customMaterialChanged += materialsDescriptionView.OnMaterialWasChanged;
            }
        }

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            try
            {
                if (firstActivation)
                {
                    title = "Custom Walls";
                    showBackButton = true;
                    ProvideInitialViewControllers(materialsListView, materialsDescriptionView, materialsPreviewView);
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            // Dismiss ourselves
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, false);
        }
    }
}
