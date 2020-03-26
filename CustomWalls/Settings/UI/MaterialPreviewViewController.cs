using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomWalls.Data;
using CustomWalls.Utilities;
using HMUI;

namespace CustomWalls.Settings.UI
{
    internal class MaterialPreviewViewController : BSMLResourceViewController
    {
        public override string ResourceName => "CustomWalls.Settings.UI.Views.materialPreview.bsml";

        [UIComponent("error-description")]
        public TextPageScrollView errorDescription;

        public void OnMaterialWasChanged(CustomMaterial customMaterial)
        {
            if (!string.IsNullOrWhiteSpace(customMaterial?.ErrorMessage))
            {
                errorDescription.gameObject.SetActive(true);
                errorDescription.SetText($"{customMaterial.Descriptor?.MaterialName}:\n\n{Utils.SafeUnescape(customMaterial.ErrorMessage)}");
            }
            else
            {
                errorDescription.gameObject.SetActive(false);
            }
        }
    }
}
