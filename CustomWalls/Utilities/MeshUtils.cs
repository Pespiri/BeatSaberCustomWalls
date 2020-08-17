using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomWalls.Utilities
{
    internal class MeshUtils
    {
        /// <summary>
        /// Find a specific mesh filter within a GameObject
        /// </summary>
        /// <param name="gameObject">GameObject</param>
        /// <param name="rendererName">Filter name</param>
        public static MeshFilter GetGameObjectMeshFilter(GameObject gameObject, string filterName)
        {
            IEnumerable<MeshFilter> filters = GetGameObjectMeshFilter(gameObject);
            foreach (MeshFilter filter in filters)
            {
                if (string.Equals(filter.name, filterName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return filter;
                }
            }

            return null;
        }

        /// <summary>
        /// Find all mesh filters within a GameObject
        /// </summary>
        /// <param name="gameObject">GameObject</param>
        /// <param name="includeInactive">Include inactive filter</param>
        public static IEnumerable<MeshFilter> GetGameObjectMeshFilter(GameObject gameObject, bool includeInactive = false)
        {
            IEnumerable<MeshFilter> filters = gameObject?.GetComponentsInChildren<MeshFilter>(includeInactive);
            return filters ?? Enumerable.Empty<MeshFilter>();
        }

        /// <summary>
        /// Creates a brand new object that overlays on top of the normal wall
        /// </summary>
        /// <param name="target"></param>
        /// <param name="donor"></param>
        /// <param name="offset"></param>
        public static GameObject CreateOverlay(Renderer target, Renderer donor, float offset)
        {
            GameObject overlayObject = new GameObject("Overlay");
            overlayObject.transform.parent = target.transform;
            float wallScale = offset;
            overlayObject.transform.localScale = new Vector3(
                1 + wallScale * (1 / target.transform.lossyScale.x),
                1 + wallScale * (1 / target.transform.lossyScale.y),
                1 + wallScale * (1 / target.transform.lossyScale.z)
            );
            overlayObject.transform.localPosition = new Vector3(0, 0, 0);
            overlayObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            MeshRenderer overlayRenderer = overlayObject.AddComponent<MeshRenderer>();
            overlayRenderer.material = donor.material;

            MeshFilter overlayFilter = overlayObject.AddComponent<MeshFilter>();
            overlayFilter.mesh = target.gameObject.GetComponent<MeshFilter>().mesh;

            return overlayObject;
        }

        /// <summary>
        /// Copy over the essential parts of the donor over to the target meshfilter
        /// </summary>
        /// <param name="target"></param>
        /// <param name="donor"></param>
        public static void ReplaceMesh(MeshFilter target, MeshFilter donor, float scale)
        {
            target.mesh = donor.mesh;
            target.gameObject.transform.localScale *= scale;
        }
    }
}
