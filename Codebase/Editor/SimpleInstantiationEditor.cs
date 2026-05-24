using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InstantiateWallObjects))]
public class SimpleInstantiationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InstantiateWallObjects instantiatedWallObject = (InstantiateWallObjects)target;
        EditorGUILayout.Space();
        if (GUILayout.Button("Spawn"))
        {
            instantiatedWallObject.Spawn(null);
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Run Bechmark"))
        {
            instantiatedWallObject.BenchmarkAllStrategies();
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Despawn"))
        {
            instantiatedWallObject.DestroyPrefabs();
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Hard Destroy Prefabs"))
        {
            try
            {
                instantiatedWallObject.HardDestroyPrefabs();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.Log("Hard destroy error: probably no parent found");
            }
        }
    }
}
