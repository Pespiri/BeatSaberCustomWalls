using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomWalls.Data;
using CustomWalls.Utilities;
using HMUI;
using System;
using System.Linq;
using UnityEngine;

namespace CustomWalls.Settings.UI
{
    internal class MaterialListView : BSMLResourceViewController
    {
        public override string ResourceName => "CustomWalls.Settings.UI.Views.materialList.bsml";

        private bool isGeneratingPreview = false;
        private GameObject preview;
        private ColorManager colorManager = null;

        // Objects
        private GameObject materialObject;

        public Action<CustomMaterial> customMaterialChanged;

        [UIComponent("materialList")]
        public CustomListTableData customListTableData;

        [UIAction("materialSelect")]
        public void Select(TableView _, int row)
        {
            MaterialAssetLoader.SelectedMaterial = row;
            Configuration.CurrentlySelectedMaterial = MaterialAssetLoader.CustomMaterialObjects[row].FileName;

            GenerateMaterialPreview(row);
        }

        [UIAction("#post-parse")]
        public void SetupList()
        {
            customListTableData.data.Clear();
            foreach (CustomMaterial material in MaterialAssetLoader.CustomMaterialObjects)
            {
                CustomListTableData.CustomCellInfo customCellInfo = new CustomListTableData.CustomCellInfo(material.Descriptor.MaterialName, material.Descriptor.AuthorName, material.Descriptor.Icon);
                customListTableData.data.Add(customCellInfo);
            }

            customListTableData.tableView.ReloadData();
            int selectedMaterial = MaterialAssetLoader.SelectedMaterial;

            customListTableData.tableView.ScrollToCellWithIdx(selectedMaterial, TableViewScroller.ScrollPositionType.Beginning, false);
            customListTableData.tableView.SelectCellWithIdx(selectedMaterial);
        }

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            base.DidActivate(firstActivation, type);

            if (!colorManager)
            {
                colorManager = Resources.FindObjectsOfTypeAll<ColorManager>().LastOrDefault();
            }

            if (!preview)
            {
                preview = new GameObject();
                preview.transform.position = new Vector3(2.15f, 1.45f, 1.45f); // new Vector3(2.25f, 1.45f, 1.55f)
                preview.transform.Rotate(0.0f, -30.0f, 0.0f);
            }

            Select(customListTableData.tableView, MaterialAssetLoader.SelectedMaterial);
        }

        protected override void DidDeactivate(DeactivationType deactivationType)
        {
            base.DidDeactivate(deactivationType);
            ClearPreview();
        }

        private void GenerateMaterialPreview(int selectedMaterial)
        {
            if (!isGeneratingPreview)
            {
                try
                {
                    isGeneratingPreview = true;
                    ClearObjects();

                    CustomMaterial currentMaterial = MaterialAssetLoader.CustomMaterialObjects[selectedMaterial];
                    if (currentMaterial != null)
                    {
                        customMaterialChanged?.Invoke(currentMaterial);
                        InitializePreviewObjects(currentMaterial, preview.transform);
                    }
                }
                catch (Exception ex)
                {
                    Logger.log.Error(ex);
                }
                finally
                {
                    isGeneratingPreview = false;
                }
            }
        }

        private void InitializePreviewObjects(CustomMaterial customMaterial, Transform transform)
        {
            if (customMaterial.GameObject)
            {
                materialObject = CreatePreviewMaterial(customMaterial.GameObject, transform);

                if (materialObject)
                {
                    materialObject.transform.localPosition = Vector3.zero;
                    materialObject.transform.localScale = new Vector3(10, 37.5f, 75); // new Vector3(5, 25, 50)

                    Renderer renderer = materialObject.gameObject?.GetComponentInChildren<Renderer>();
                    MaterialUtils.SetRendererColor(renderer, colorManager.GetObstacleEffectColor());
                }
            }
        }

        private GameObject CreatePreviewMaterial(GameObject material, Transform transform)
        {
            GameObject gameObject = InstantiateGameObject(material, transform);
            return gameObject;
        }

        private GameObject InstantiateGameObject(GameObject gameObject, Transform transform = null)
        {
            if (gameObject)
            {
                return transform ? Instantiate(gameObject, transform) : Instantiate(gameObject);
            }

            return null;
        }

        private void ClearPreview()
        {
            DestroyGameObject(ref preview);
            ClearObjects();
        }

        private void ClearObjects()
        {
            DestroyGameObject(ref materialObject);
        }

        private void DestroyGameObject(ref GameObject gameObject)
        {
            if (gameObject)
            {
                Destroy(gameObject);
                gameObject = null;
            }
        }
    }
}
