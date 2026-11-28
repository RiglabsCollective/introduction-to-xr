using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Picks the simulated room for Play mode from the open scene's name.
//   a scene with "Marker" in its name: a table with the engine page lying on it
//   any other scene: just a floor and a table
[InitializeOnLoad]
public static class SceneEnvironment
{
    const string Prefs = "Assets/XR/UserSimulationSettings/Resources/XRSimulationPreferences.asset";
    const string Dir = "Assets/Shared/SimulationEnvironments/";

    static SceneEnvironment() { EditorApplication.playModeStateChanged += OnMode; }

    static void OnMode(PlayModeStateChange c)
    {
        if (c != PlayModeStateChange.ExitingEditMode) return;
        var name = EditorSceneManager.GetActiveScene().name;
        var room = name.Contains("Marker") ? "Table-With-Engine-Page" : "Table";
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(Dir + room + ".prefab");
        var prefs = AssetDatabase.LoadMainAssetAtPath(Prefs);
        if (prefab == null || prefs == null) return;
        var so = new SerializedObject(prefs);
        so.FindProperty("m_EnvironmentPrefab").objectReferenceValue = prefab;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
