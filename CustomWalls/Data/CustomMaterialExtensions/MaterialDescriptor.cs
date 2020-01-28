﻿using UnityEngine;

namespace CustomWalls.Data.CustomMaterialExtensions
{
    public class MaterialDescriptor : MonoBehaviour
    {
        public string AuthorName = "Wall Author";
        public string MaterialName = "Wall Name";
        public string Description = string.Empty;
        public bool DisablesScore = false;
        public Texture2D Icon;
    }
}