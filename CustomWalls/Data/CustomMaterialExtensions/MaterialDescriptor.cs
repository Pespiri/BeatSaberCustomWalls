using UnityEngine;

namespace CustomWalls.Data.CustomMaterialExtensions
{
    public class MaterialDescriptor : MonoBehaviour
    {
        public string AuthorName = "Wall Author";
        public string MaterialName = "Wall Name";
        public string Description = string.Empty;
        public bool ReplaceMesh = false;
        public float MeshScaleMultiplier = 1;
        public bool Overlay = false;
        public float OverlayOffset = .001f;
        public bool ReplaceOnlyOverlayMesh = true;
        public bool DisablesScore = false;
        public Texture2D Icon;
    }
}
