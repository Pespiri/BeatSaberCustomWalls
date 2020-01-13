using CustomWalls.Data.CustomMaterialExtensions;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CompileMaterialWindow : EditorWindow
{
    private IEnumerable<MaterialDescriptor> materials;

    [MenuItem("Window/Wall Exporter")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CompileMaterialWindow), false, "Wall Exporter");
    }

    private void OnGUI()
    {
        //string[] targets = TargetObjectEnumToStringArray();

        GUILayout.Label("Walls", EditorStyles.boldLabel);
        GUILayout.Space(20);

        foreach (MaterialDescriptor material in materials)
        {

            GUILayout.Label("GameObject: " + material.MaterialName, EditorStyles.boldLabel);
            material.AuthorName = EditorGUILayout.TextField("Author name", material.AuthorName);
            material.MaterialName = EditorGUILayout.TextField("Wall name", material.MaterialName);
            material.Description = EditorGUILayout.TextField("Wall description", material.Description);
            //material.Description = EditorGUILayout.TextArea("Material description", GUILayout.Width(position.width - 7), GUILayout.Height(30));

            //material.ApplicableTo[0] = (TargetObject)EditorGUILayout.MaskField("Supported materials", (int)material.ApplicableTo[0], targets);
            //GUILayout.Label($"{(int)material.ApplicableTo[0]}", EditorStyles.boldLabel);

            material.Icon = (Texture2D)EditorGUILayout.ObjectField("Cover Image", material.Icon, typeof(Texture2D), false);

            EditorGUI.BeginDisabledGroup(material.transform.Find("Pixie") == null);

            if (GUILayout.Button("Export " + material.MaterialName))
            {
                GameObject materialObject = material.gameObject;
                if (materialObject != null && material != null)// && material.ApplicableTo[0] != 0)
                {
                    string path = EditorUtility.SaveFilePanel("Save wall (pixie) file", "", material.MaterialName + ".pixie", "pixie");

                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        string fileName = Path.GetFileName(path);
                        string folderPath = Path.GetDirectoryName(path);

                        Selection.activeObject = materialObject;
                        EditorUtility.SetDirty(material);
                        EditorSceneManager.MarkSceneDirty(materialObject.scene);
                        EditorSceneManager.SaveScene(materialObject.scene);
                        PrefabUtility.CreatePrefab("Assets/_CustomMaterial.prefab", Selection.activeObject as GameObject);
                        AssetBundleBuild assetBundleBuild = default(AssetBundleBuild);
                        assetBundleBuild.assetNames = new string[] {
                            "Assets/_CustomMaterial.prefab"
                        };

                        assetBundleBuild.assetBundleName = fileName;

                        BuildTargetGroup selectedBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
                        BuildTarget activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;

                        BuildPipeline.BuildAssetBundles(Application.temporaryCachePath, new AssetBundleBuild[] { assetBundleBuild }, 0, EditorUserBuildSettings.activeBuildTarget);
                        EditorPrefs.SetString("currentBuildingAssetBundlePath", folderPath);
                        EditorUserBuildSettings.SwitchActiveBuildTarget(selectedBuildTargetGroup, activeBuildTarget);
                        AssetDatabase.DeleteAsset("Assets/_CustomMaterial.prefab");

                        if (File.Exists(path))
                        {
                            bool isDirectory = (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
                            if (!isDirectory)
                            {
                                File.Delete(path);
                            }
                        }

                        File.Move(Path.Combine(Application.temporaryCachePath, fileName), path);
                        AssetDatabase.Refresh();
                        EditorUtility.DisplayDialog("Export Successful!", "Export Successful!", "OK");
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Export Failed!", "Path is invalid.", "OK");
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Export Failed!", "GameObject is missing.", "OK");
                }
            }

            EditorGUI.EndDisabledGroup();

            if (material.transform.Find("Pixie") == null)
            {
                GUILayout.Label("Wall gameObject is missing", EditorStyles.boldLabel);
            }

            //if (material.ApplicableTo[0] == 0)
            //{
            //    GUILayout.Label("Wall has to be applicable to something!", EditorStyles.boldLabel);
            //}

            GUILayout.Space(20);
        }
    }

    private void OnFocus()
    {
        materials = GameObject.FindObjectsOfType<MaterialDescriptor>();
    }

    private Bounds GetObjectBounds(GameObject gameObject)
    {
        Bounds bounds = new Bounds(gameObject.transform.position, Vector3.zero);
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }

    //private string[] TargetObjectEnumToStringArray()
    //{
    //    IList<string> array = new List<string>();
    //    foreach (object singleEnum in Enum.GetValues(typeof(TargetObject)))
    //    {
    //        array.Add(singleEnum.ToString());
    //    }

    //    return array.ToArray();
    //}
}
