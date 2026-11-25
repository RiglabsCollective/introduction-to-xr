using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// 003: a simulation environment whose poster is the engine page, so image tracking works in the Editor.
public static class FixSim
{
    static Type Find(string n) => AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => { try { return a.GetTypes(); } catch { return new Type[0]; } }).FirstOrDefault(t => t.Name == n);

    public static string SimEnv()
    {
        const string src = "Packages/com.unity.xr.arfoundation/Assets/Prefabs/DefaultSimulationEnvironment.prefab";
        const string dst = "Assets/003-Textbook-Popup/SimulationEnvironment.prefab";
        AssetDatabase.DeleteAsset(dst);
        if (!AssetDatabase.CopyAsset(src, dst)) return "copy failed";
        var root = PrefabUtility.LoadPrefabContents(dst);
        var t = Find("SimulatedTrackedImage");
        var sti = root.GetComponentInChildren(t, true);
        var so = new SerializedObject(sti);
        so.FindProperty("m_Image").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Shared/Textures/engine-marker.png");
        so.FindProperty("m_ImagePhysicalSizeMeters").vector2Value = new Vector2(0.5f, 0.5f);
        so.ApplyModifiedProperties();
        PrefabUtility.SaveAsPrefabAsset(root, dst);
        PrefabUtility.UnloadPrefabContents(root);

        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(dst);
        var prefs = AssetDatabase.LoadMainAssetAtPath("Assets/XR/UserSimulationSettings/Resources/XRSimulationPreferences.asset");
        var pso = new SerializedObject(prefs);
        pso.FindProperty("m_EnvironmentPrefab").objectReferenceValue = prefab;
        pso.ApplyModifiedProperties();
        EditorUtility.SetDirty(prefs);

        var mgr = AssetDatabase.LoadMainAssetAtPath("Assets/XR/UserSimulationSettings/SimulationEnvironmentAssetsManager.asset");
        var mso = new SerializedObject(mgr);
        var list = mso.FindProperty("m_EnvironmentPrefabPaths");
        bool has = false; for (int i = 0; i < list.arraySize; i++) if (list.GetArrayElementAtIndex(i).stringValue == dst) has = true;
        if (!has) { list.arraySize++; list.GetArrayElementAtIndex(list.arraySize - 1).stringValue = dst; }
        mso.ApplyModifiedProperties(); EditorUtility.SetDirty(mgr);
        AssetDatabase.SaveAssets();
        return "sim env set";
    }

    public static string Disable002Ui()
    {
        EditorSceneManager.SaveOpenScenes();
        var s = EditorSceneManager.OpenScene("Assets/Scenes/002-Place-the-Heart.unity");
        var ui = GameObject.Find("UI"); if (ui != null) ui.SetActive(false);
        EditorSceneManager.SaveScene(s);
        return "002 ui off";
    }
}
