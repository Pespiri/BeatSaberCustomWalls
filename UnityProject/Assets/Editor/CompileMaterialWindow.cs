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
        GUILayout.Label("Walls", EditorStyles.boldLabel);
        GUILayout.Space(20);

        foreach (MaterialDescriptor material in materials)
        {
            GUILayout.Label("GameObject: " + material.MaterialName, EditorStyles.boldLabel);
            material.AuthorName = EditorGUILayout.TextField("Author name", material.AuthorName);
            material.MaterialName = EditorGUILayout.TextField("Wall name", material.MaterialName);
            material.Description = EditorGUILayout.TextField("Wall description", material.Description);
            material.DisablesScore = EditorGUILayout.Toggle("Disables Score", material.DisablesScore);
            material.Icon = (Texture2D)EditorGUILayout.ObjectField("Cover Image", material.Icon, typeof(Texture2D), false);

            bool disableExportButton = false;
            if (material.transform.Find("Pixie") == null
                || string.IsNullOrWhiteSpace(material.AuthorName)
                || string.IsNullOrWhiteSpace(material.MaterialName))
            {
                disableExportButton = true;
            }

            EditorGUI.BeginDisabledGroup(disableExportButton);

            if (GUILayout.Button("Export " + material.MaterialName))
            {
                GameObject materialObject = material.gameObject;
                if (materialObject != null && material != null)
                {
                    string path = EditorUtility.SaveFilePanel("Save wall file", "", material.MaterialName + ".wall", "wall");

                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        string guid = $"{{{GUID.Generate()}}}";
                        string fileName = $"{Path.GetFileName(path)}_{guid}";
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

            if (string.IsNullOrWhiteSpace(material.AuthorName))
            {
                GUILayout.Label("Author name is empty", EditorStyles.boldLabel);
            }

            if (string.IsNullOrWhiteSpace(material.MaterialName))
            {
                GUILayout.Label("Wall name is empty", EditorStyles.boldLabel);
            }

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
}
