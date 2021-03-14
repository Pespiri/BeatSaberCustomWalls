using CustomWalls.Data;
using CustomWalls.Settings;
using CustomWalls.Utilities;
using HarmonyLib;
using IPA.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CustomWalls.HarmonyPatches.Patches
{
    /// <summary>
    /// Walls
    /// </summary>
    [HarmonyPatch(typeof(ObstacleController))]
    [HarmonyPatch("Init", MethodType.Normal)]
    internal class WallPatch
    {
        private static Renderer original = null;

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Harmony calls this")]
        private static void Postfix(ref ObstacleController __instance, StretchableObstacle ____stretchableObstacle, ref ColorManager ____colorManager)
        {
            try
            {
                CustomMaterial customMaterial = MaterialAssetLoader.CustomMaterialObjects[MaterialAssetLoader.SelectedMaterial];
                if (customMaterial.FileName != "DefaultMaterials")
                {
                    Renderer mesh = __instance.gameObject.GetComponentInChildren<Renderer>();

                    if (customMaterial.Descriptor.Overlay)
                    {
                        GameObject overlay = MeshUtils.CreateOverlay(mesh, customMaterial.MaterialRenderer, customMaterial.Descriptor.OverlayOffset);
                        MaterialUtils.SetMaterialsColor(overlay?.GetComponent<Renderer>().materials, ____colorManager.obstaclesColor);
                        if (customMaterial.Descriptor.ReplaceMesh)
                        {
                            MeshUtils.ReplaceMesh(overlay.GetComponent<MeshFilter>(), customMaterial.MaterialMeshFilter, customMaterial.Descriptor.MeshScaleMultiplier);
                            if (!customMaterial.Descriptor.ReplaceOnlyOverlayMesh)
                            {
                                MeshUtils.ReplaceMesh(__instance.gameObject.GetComponentInChildren<MeshFilter>(), customMaterial.MaterialMeshFilter, customMaterial.Descriptor.MeshScaleMultiplier);
                            }
                        }
                    }
                    else
                    {
                        // Create a backup object as a fall-back
                        if (original == null)
                        {
                            original = UnityEngine.Object.Instantiate(mesh);
                            original.gameObject.SetActive(false);
                        }

                        try
                        {
                            MaterialUtils.ReplaceRenderer(mesh, MaterialUtils.MixRenderers(original, customMaterial.MaterialRenderer));
                            MaterialUtils.SetMaterialsColor(mesh?.materials, ____colorManager.obstaclesColor);
                            if (customMaterial.Descriptor.ReplaceMesh)
                            {
                                MeshUtils.ReplaceMesh(__instance.gameObject.GetComponentInChildren<MeshFilter>(), customMaterial.MaterialMeshFilter, customMaterial.Descriptor.MeshScaleMultiplier);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.log.Error("Unable to apply wall");
                            Logger.log.Error(ex);

                            if (mesh && original)
                            {
                                MaterialUtils.ReplaceRenderer(mesh, original);
                                MaterialUtils.SetMaterialsColor(mesh?.materials, ____colorManager.obstaclesColor);
                            }
                        }
                    }
                }

                if (!Configuration.EnableObstacleFrame)
                {
                    ParametricBoxFrameController frame = ____stretchableObstacle.GetField<ParametricBoxFrameController, StretchableObstacle>("_obstacleFrame");
                    frame.enabled = false;
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }
    }
}
