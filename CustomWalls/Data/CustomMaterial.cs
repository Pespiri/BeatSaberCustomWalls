using CustomWalls.Data.CustomMaterialExtensions;
using CustomWalls.Utilities;
using System;
using System.IO;
using UnityEngine;

namespace CustomWalls.Data
{
    public class CustomMaterial
    {
        public string FileName { get; }
        public AssetBundle AssetBundle { get; }
        public MaterialDescriptor Descriptor { get; }
        public GameObject GameObject { get; }
        public Renderer MaterialRenderer { get; }

        public CustomMaterial(string fileName)
        {
            FileName = fileName;

            if (fileName != "DefaultMaterials")
            {
                try
                {
                    AssetBundle = AssetBundle.LoadFromFile(Path.Combine(Plugin.PluginAssetPath, fileName));

                    GameObject = AssetBundle.LoadAsset<GameObject>("Assets/_CustomMaterial.prefab");
                    Descriptor = GameObject.GetComponent<MaterialDescriptor>();
                    MaterialRenderer = MaterialUtils.GetGameObjectRenderer(GameObject, "pixie");
                }
                catch (Exception ex)
                {
                    Logger.log.Warn($"Something went wrong getting the AssetBundle for '{fileName}'!");
                    Logger.log.Warn(ex);

                    Descriptor = new MaterialDescriptor()
                    {
                        MaterialName = "Invalid Wall (Delete it!)",
                        AuthorName = fileName,
                        Description = $"File: '{fileName}'" +
                                    "\n\nThis file failed to load." +
                                    "\n\nThis may have been caused by having duplicated files," +
                                    " another wall with the same name already exists or that the custom wall is simply just broken." +
                                    "\n\nThe best thing is probably just to delete it!",
                        Icon = Utils.GetErrorIcon()
                    };

                    FileName = "DefaultMaterials";
                }
            }
            else
            {
                Descriptor = new MaterialDescriptor
                {
                    MaterialName = "Default",
                    AuthorName = "Beat Saber",
                    Description = "This is the default walls. (No preview available)",
                    Icon = Utils.GetDefaultIcon()
                };
            }
        }

        public CustomMaterial(byte[] materialObject, string name)
        {
            if (materialObject != null)
            {
                try
                {
                    AssetBundle = AssetBundle.LoadFromMemory(materialObject);
                    GameObject = AssetBundle.LoadAsset<GameObject>("Assets/_CustomMaterial.prefab");

                    FileName = $@"internalResource\{name}";
                    Descriptor = GameObject.GetComponent<MaterialDescriptor>();
                    MaterialRenderer = MaterialUtils.GetGameObjectRenderer(GameObject, "pixie");
                }
                catch (Exception ex)
                {
                    Logger.log.Warn($"Something went wrong getting the AssetBundle from resource!");
                    Logger.log.Warn(ex);

                    Descriptor = new MaterialDescriptor
                    {
                        MaterialName = "Internal Error (Report it!)",
                        AuthorName = $@"internalResource\{name}",
                        Description = $@"File: 'internalResource\\{name}'" +
                                    "\n\nAn internal asset has failed to load." +
                                    "\n\nThis shouldn't have happened and should be reported!" +
                                    " Remember to include the log related to this incident." +
                                    "\n\nDiscord: Pespiri#5919",
                        Icon = Utils.GetErrorIcon()
                    };

                    FileName = "DefaultMaterials";
                }
            }
            else
            {
                throw new ArgumentNullException("materialObject cannot be null for the constructor!");
            }
        }

        public void Destroy()
        {
            if (AssetBundle != null)
            {
                AssetBundle.Unload(true);
            }
            else
            {
                UnityEngine.Object.Destroy(Descriptor);
            }
        }
    }
}
