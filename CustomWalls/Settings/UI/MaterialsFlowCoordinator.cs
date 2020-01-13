using BeatSaberMarkupLanguage;
using HMUI;
using IPA.Utilities;
using System;

namespace CustomWalls.Settings.UI
{
    internal class MaterialsFlowCoordinator : FlowCoordinator
    {
        private MaterialListView materialsListView;
        private MaterialPreviewViewController materialsPreviewView;
        private MaterialDetailsViewController materialsDescriptionView;

        public void Awake()
        {
            if (materialsListView == null)
            {
                materialsListView = BeatSaberUI.CreateViewController<MaterialListView>();
                materialsPreviewView = BeatSaberUI.CreateViewController<MaterialPreviewViewController>();
                materialsDescriptionView = BeatSaberUI.CreateViewController<MaterialDetailsViewController>();

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
            MainFlowCoordinator mainFlow = BeatSaberUI.MainFlowCoordinator;
            mainFlow.InvokePrivateMethod("DismissFlowCoordinator", this, null, false);
        }
    }
}
