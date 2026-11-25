using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Picks the simulated room for Play mode from the open scene's name, so each lesson shows only what it needs.
//   001 (the XR Foundation try-out): the full demo room
//   003: a table with the engine page lying on it
//   everything else: just a floor and a table
[InitializeOnLoad]
public static class SceneEnvironment
{
    const string Prefs = "Assets/XR/UserSimulationSettings/Resources/XRSimulationPreferences.asset";
    const string Dir = "Assets/Shared/SimulationEnvironments/";
    const string DefaultRoom = "Packages/com.unity.xr.arfoundation/Assets/Prefabs/DefaultSimulationEnvironment.prefab";

    static SceneEnvironment() { EditorApplication.playModeStateChanged += OnMode; }

    static void OnMode(PlayModeStateChange c)
    {
        if (c != PlayModeStateChange.ExitingEditMode) return;
        var name = EditorSceneManager.GetActiveScene().name;
        string path = name.StartsWith("001") ? DefaultRoom : name.StartsWith("003") ? Dir + "Table-With-Engine-Page.prefab" : Dir + "Table.prefab";
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        var prefs = AssetDatabase.LoadMainAssetAtPath(Prefs);
        if (prefab == null || prefs == null) return;
        var so = new SerializedObject(prefs);
        so.FindProperty("m_EnvironmentPrefab").objectReferenceValue = prefab;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
