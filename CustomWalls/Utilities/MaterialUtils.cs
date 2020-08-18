using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomWalls.Utilities
{
    internal class MaterialUtils
    {
        public static ColorManager CurrentColorManager { get; internal set; }

        /// <summary>
        /// Find a specific renderer within a GameObject
        /// </summary>
        /// <param name="gameObject">GameObject</param>
        /// <param name="rendererName">Renderer name</param>
        public static Renderer GetGameObjectRenderer(GameObject gameObject, string rendererName)
        {
            IEnumerable<Renderer> renderers = GetGameObjectRenderer(gameObject);
            foreach (Renderer renderer in renderers)
            {
                if (string.Equals(renderer.name, rendererName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return renderer;
                }
            }

            return null;
        }

        /// <summary>
        /// Find all renderers within a GameObject
        /// </summary>
        /// <param name="gameObject">GameObject</param>
        /// <param name="includeInactive">Include inactive renderers</param>
        public static IEnumerable<Renderer> GetGameObjectRenderer(GameObject gameObject, bool includeInactive = false)
        {
            IEnumerable<Renderer> renderers = gameObject?.GetComponentsInChildren<Renderer>(includeInactive);
            return renderers ?? Enumerable.Empty<Renderer>();
        }

        /// <summary>
        /// Copy over the essential parts of the donor over to the target renderer
        /// </summary>
        /// <param name="target"></param>
        /// <param name="donor"></param>
        public static void ReplaceRenderer(Renderer target, Renderer donor)
        {
            target.material = donor.material;
            //int materialsLength = donor.materials.Length;
            //if (materialsLength > target.materials.Length)
            //{
            //    materialsLength = target.materials.Length;
            //}

            //for (int i = 0; i < materialsLength; i++)
            //{
            //    target.materials[i] = donor.materials[i];
            //}

            //target.sharedMaterial = donor.sharedMaterial;

            //int sharedMaterialsLength = donor.sharedMaterials.Length;
            //if (sharedMaterialsLength > target.sharedMaterials.Length)
            //{
            //    sharedMaterialsLength = target.sharedMaterials.Length;
            //}

            //for (int i = 0; i < sharedMaterialsLength; i++)
            //{
            //    target.sharedMaterials[i] = donor.sharedMaterials[i];
            //}

            #region Renderer testing
            // #1
            //target.material = donor.material;
            //target.materials = donor.materials;
            //target.sharedMaterial = donor.sharedMaterial;
            //target.sharedMaterials = donor.sharedMaterials;
            //target.lightmapIndex = donor.lightmapIndex;
            //target.lightmapScaleOffset = donor.lightmapScaleOffset;
            //target.lightProbeProxyVolumeOverride = donor.lightProbeProxyVolumeOverride;
            //target.lightProbeUsage = donor.lightProbeUsage;
            //target.motionVectorGenerationMode = donor.motionVectorGenerationMode;
            //target.probeAnchor = donor.probeAnchor;
            //target.realtimeLightmapIndex = donor.realtimeLightmapIndex;
            //target.realtimeLightmapScaleOffset = donor.realtimeLightmapScaleOffset;
            //target.receiveShadows = donor.receiveShadows;
            //target.reflectionProbeUsage = donor.reflectionProbeUsage;
            //target.rendererPriority = donor.rendererPriority;
            //target.renderingLayerMask = donor.renderingLayerMask;
            //target.shadowCastingMode = donor.shadowCastingMode;
            //target.sortingLayerID = donor.sortingLayerID;
            //target.sortingLayerName = donor.sortingLayerName;
            //target.sortingOrder = donor.sortingOrder;
            //target.tag = donor.tag;

            // #2
            //target.material = donor.material ?? target.material;
            //target.materials = donor.materials ?? target.materials;
            //target.sharedMaterial = donor.sharedMaterial ?? target.sharedMaterial;
            //target.sharedMaterials = donor.sharedMaterials ?? target.sharedMaterials;
            //target.lightmapIndex = donor.lightmapIndex;
            //target.lightmapScaleOffset = donor.lightmapScaleOffset;
            //target.lightProbeProxyVolumeOverride = donor.lightProbeProxyVolumeOverride ?? target.lightProbeProxyVolumeOverride;
            //target.lightProbeUsage = donor.lightProbeUsage;
            //target.motionVectorGenerationMode = donor.motionVectorGenerationMode;
            //target.probeAnchor = donor.probeAnchor ?? target.probeAnchor;
            //target.realtimeLightmapIndex = donor.realtimeLightmapIndex;
            //target.realtimeLightmapScaleOffset = donor.realtimeLightmapScaleOffset;
            //target.receiveShadows = donor.receiveShadows;
            //target.reflectionProbeUsage = donor.reflectionProbeUsage;
            //target.rendererPriority = donor.rendererPriority;
            //target.renderingLayerMask = donor.renderingLayerMask;
            //target.shadowCastingMode = donor.shadowCastingMode;
            //target.sortingLayerID = donor.sortingLayerID;
            //target.sortingLayerName = !string.IsNullOrWhiteSpace(donor.sortingLayerName) ? donor.sortingLayerName : target.sortingLayerName;
            //target.sortingOrder = donor.sortingOrder;
            //target.tag = !string.IsNullOrWhiteSpace(donor.tag) ? donor.tag : target.tag;

            // Other
            //target.gameObject.hideFlags = donor.gameObject.hideFlags;
            //target.gameObject.isStatic = donor.gameObject.isStatic;
            //target.gameObject.layer = donor.gameObject.layer;
            //target.gameObject.tag = donor.gameObject.tag;
            #endregion
        }

        /// <summary>
        /// Set the _Color field in every material if it has it
        /// </summary>
        /// <param name="materials">Materials</param>
        /// <param name="color">Color</param>
        public static void SetMaterialsColor(IEnumerable<Material> materials, Color color)
        {
            if (materials != null)
            {
                foreach (Material material in materials)
                {
                    if (material != null
                        && material.HasProperty("_Color"))
                    {
                        material.SetColor("_Color", color);
                    }
                }
            }
        }
    }
}
