using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomWalls.Data;
using CustomWalls.Utilities;
using TMPro;

namespace CustomWalls.Settings.UI
{
    internal class MaterialDetailsViewController : BSMLResourceViewController
    {
        public override string ResourceName => "CustomWalls.Settings.UI.Views.materialDetails.bsml";

        private readonly string scoreDisabledByMaterial = "This CustomWall disables Score Submission";
        private readonly string scoreDisabledByUser = "Score Submission has been manually disabled";

        [UIComponent("material-description")]
        public TextPageScrollView materialDescription;

        [UIValue("enable-obstacle-frame")]
        public bool EnableObstacleFrame
        {
            get => Configuration.EnableObstacleFrame;
            set => Configuration.EnableObstacleFrame = value;
        }

        [UIValue("disable-score-submission")]
        public bool ManuallyDisableScoreSubmission
        {
            get => Configuration.UserDisabledScores;
            set => Configuration.UserDisabledScores = value;
        }

        [UIComponent("score-submission-info")]
        public TextMeshProUGUI scoreSubmissionInfo;

        public void OnMaterialWasChanged(CustomMaterial customMaterial)
        {
            materialDescription.SetText($"{customMaterial.Descriptor.MaterialName}:\n\n{Utils.SafeUnescape(customMaterial.Descriptor.Description)}");

            if (!Configuration.UserDisabledScores)
            {
                scoreSubmissionInfo.text = customMaterial.Descriptor.DisablesScore
                    ? scoreDisabledByMaterial
                    : string.Empty;
            }
        }

        [UIAction("score-submission-manual-change")]
        private void OnManualScoreSubmissionChange(bool state)
        {
            if (state)
            {
                scoreSubmissionInfo.text = scoreDisabledByUser;
            }
            else
            {
                CustomMaterial customMaterial = MaterialAssetLoader.CustomMaterialObjects[MaterialAssetLoader.SelectedMaterial];
                scoreSubmissionInfo.text = customMaterial.Descriptor.DisablesScore
                    ? scoreDisabledByMaterial
                    : string.Empty;
            }
        }
    }
}
