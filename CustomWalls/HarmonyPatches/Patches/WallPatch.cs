using BS_Utils.Utilities;
using CustomWalls.Data;
using CustomWalls.Settings;
using CustomWalls.Utilities;
using HarmonyLib;
using System;
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
        private static void Prefix(ref ObstacleController __instance, StretchableObstacle ____stretchableObstacle)
        {
            try
            {
                CustomMaterial customMaterial = MaterialAssetLoader.CustomMaterialObjects[MaterialAssetLoader.SelectedMaterial];
                if (customMaterial.FileName != "DefaultMaterials")
                {
                    Renderer mesh = __instance.gameObject.GetComponentInChildren<Renderer>();
                    MaterialUtils.ReplaceRenderer(mesh, customMaterial.MaterialRenderer);
                    MaterialUtils.SetMaterialsColor(mesh?.materials, MaterialUtils.CurrentColorManager.GetObstacleEffectColor());
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
