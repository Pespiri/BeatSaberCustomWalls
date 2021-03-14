using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomWalls.Utilities
{
    internal class MaterialUtils
    {
        private static readonly System.Random mixStrength = new System.Random();

        public static Renderer MixRenderers(Renderer original, Renderer custom)
        {
            return DateTime.Now.Month == 4 && DateTime.Now.Day == 1 ? mixStrength.Next(100) == 0 ? original : custom : custom;
        }

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
