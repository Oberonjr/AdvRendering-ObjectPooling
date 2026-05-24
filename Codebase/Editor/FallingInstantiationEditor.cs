using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InstantiateFallingObjects))]
public class FallingInstantiationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InstantiateFallingObjects instantiatedObject = (InstantiateFallingObjects)target;
        EditorGUILayout.Space();
        if (GUILayout.Button("Spawn objects"))
        {
            instantiatedObject.Spawn(null);
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Run Bechmark"))
        {
            instantiatedObject.BenchmarkAllStrategies();
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Destroy Prefabs"))
        {
            instantiatedObject.DestroyPrefabs();
        }
        if (GUILayout.Button("Hard Destroy Prefabs"))
        {
            try
            {
                instantiatedObject.HardDestroyPrefabs();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.Log("Hard destroy error: probably no parent found");
            }
        }
    }
}
