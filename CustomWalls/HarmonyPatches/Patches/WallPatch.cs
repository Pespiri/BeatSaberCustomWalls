using BS_Utils.Utilities;
using CustomWalls.Data;
using CustomWalls.Settings;
using CustomWalls.Utilities;
using HarmonyLib;
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
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Harmony calls this")]
        private static void Prefix(ref ObstacleController __instance, StretchableObstacle ____stretchableObstacle)
        {
            try
            {
                CustomMaterial customMaterial = MaterialAssetLoader.CustomMaterialObjects[MaterialAssetLoader.SelectedMaterial];
                if (customMaterial.FileName != "DefaultMaterials")
                {
                    Renderer mesh = __instance.gameObject.GetComponentInChildren<Renderer>();
                    Color color = MaterialUtils.CurrentColorManager.GetObstacleEffectColor();
                    if (customMaterial.Descriptor.Overlay)
                    {
                        GameObject overlay = MeshUtils.CreateOverlay(mesh, customMaterial.MaterialRenderer, customMaterial.Descriptor.OverlayOffset);
                        MaterialUtils.SetMaterialsColor(overlay?.GetComponent<Renderer>().materials, color);
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
                        MaterialUtils.ReplaceRenderer(mesh, customMaterial.MaterialRenderer);
                        MaterialUtils.SetMaterialsColor(mesh?.materials, color);
                        if (customMaterial.Descriptor.ReplaceMesh)
                        {
                            MeshUtils.ReplaceMesh(__instance.gameObject.GetComponentInChildren<MeshFilter>(), customMaterial.MaterialMeshFilter, customMaterial.Descriptor.MeshScaleMultiplier);
                        }
                    }
                }

                if (!Configuration.EnableObstacleFrame)
                {
                    ParametricBoxFrameController frame = ____stretchableObstacle.GetPrivateField<ParametricBoxFrameController>("_obstacleFrame");
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
