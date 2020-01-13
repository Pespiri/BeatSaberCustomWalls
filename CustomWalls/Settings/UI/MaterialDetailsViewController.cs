using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomWalls.Data;

namespace CustomWalls.Settings.UI
{
    internal class MaterialDetailsViewController : BSMLResourceViewController
    {
        public override string ResourceName => "CustomWalls.Settings.UI.Views.materialDetails.bsml";

        [UIComponent("material-description")]
        internal TextPageScrollView materialDescription;

        [UIValue("enable-obstacle-frame")]
        public bool EnableObstacleFrame
        {
            get => Configuration.EnableObstacleFrame;
            set => Configuration.EnableObstacleFrame = value;
        }

        public void OnMaterialWasChanged(CustomMaterial customMaterial)
        {
            materialDescription.SetText($"{customMaterial.Descriptor.MaterialName}:\n\n{customMaterial.Descriptor.Description}");
        }
    }
}
