using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomWalls.Data.CustomMaterialExtensions.MaterialDescriptor))]
public class MyScriptEditor : Editor
{
    SerializedProperty MaterialName;
    SerializedProperty AuthorName;
    SerializedProperty Description;
    SerializedProperty Icon;
    SerializedProperty ReplaceMesh;
    SerializedProperty MeshScaleMultiplier;
    SerializedProperty ReplaceOnlyOverlayMesh;
    SerializedProperty Overlay;
    SerializedProperty OverlayOffset;
    SerializedProperty DisablesScore;

    void OnEnable()
    {
        MaterialName = serializedObject.FindProperty("MaterialName");
        AuthorName = serializedObject.FindProperty("AuthorName");
        Description = serializedObject.FindProperty("Description");
        Icon = serializedObject.FindProperty("Icon");
        ReplaceMesh = serializedObject.FindProperty("ReplaceMesh");
        MeshScaleMultiplier = serializedObject.FindProperty("MeshScaleMultiplier");
        ReplaceOnlyOverlayMesh = serializedObject.FindProperty("ReplaceOnlyOverlayMesh");
        Overlay = serializedObject.FindProperty("Overlay");
        OverlayOffset = serializedObject.FindProperty("OverlayOffset");
        DisablesScore = serializedObject.FindProperty("DisablesScore");
    }
    override public void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(MaterialName);
        EditorGUILayout.PropertyField(AuthorName);
        EditorGUILayout.PropertyField(Description);
        EditorGUILayout.PropertyField(Icon);
        EditorGUILayout.PropertyField(ReplaceMesh);

        EditorGUI.BeginDisabledGroup(!ReplaceMesh.boolValue);
            EditorGUILayout.PropertyField(MeshScaleMultiplier);
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(!(Overlay.boolValue && ReplaceMesh.boolValue));
            EditorGUILayout.PropertyField(ReplaceOnlyOverlayMesh);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(Overlay);
        EditorGUI.BeginDisabledGroup(!Overlay.boolValue);
            EditorGUILayout.PropertyField(OverlayOffset);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(DisablesScore);

        serializedObject.ApplyModifiedProperties();
    }
}
