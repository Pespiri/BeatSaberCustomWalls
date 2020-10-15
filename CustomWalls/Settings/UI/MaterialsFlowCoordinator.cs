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

                if (materialsDescriptionView)
                {
                    materialsListView.customMaterialChanged += materialsDescriptionView.OnMaterialWasChanged;
                }

                if (materialsPreviewView)
                {
                    materialsListView.customMaterialChanged += materialsPreviewView.OnMaterialWasChanged;
                }
            }
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            try
            {
                if (firstActivation)
                {
                    SetTitle("Custom Walls", ViewController.AnimationType.In);
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
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}
