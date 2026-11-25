using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

// Dev helper: simulation rooms per lesson, slower solar system, scenes without the template menus.
public static class RebuildClean
{
    static Type Find(string n) => AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => { try { return a.GetTypes(); } catch { return new Type[0]; } }).FirstOrDefault(t => t.Name == n);

    static void Strip(string path, params string[] names)
    {
        var root = PrefabUtility.LoadPrefabContents(path);
        foreach (var t in root.GetComponentsInChildren<Transform>(true).Where(t => names.Contains(t.name)).ToArray())
            if (t != null && t.gameObject != root) UnityEngine.Object.DestroyImmediate(t.gameObject);
        PrefabUtility.SaveAsPrefabAsset(root, path);
        PrefabUtility.UnloadPrefabContents(root);
    }

    public static string Envs()
    {
        const string src = "Packages/com.unity.xr.arfoundation/Assets/Prefabs/DefaultSimulationEnvironment.prefab";
        const string dir = "Assets/Shared/SimulationEnvironments/";
        AssetDatabase.Refresh();
        if (!AssetDatabase.IsValidFolder("Assets/Shared/SimulationEnvironments")) AssetDatabase.CreateFolder("Assets/Shared", "SimulationEnvironments");
        AssetDatabase.DeleteAsset(dir + "Table.prefab"); AssetDatabase.DeleteAsset(dir + "Table-With-Engine-Page.prefab");
        AssetDatabase.CopyAsset(src, dir + "Table.prefab");
        Strip(dir + "Table.prefab", "Wall", "Simulated Tracked Image");
        AssetDatabase.CopyAsset(src, dir + "Table-With-Engine-Page.prefab");
        Strip(dir + "Table-With-Engine-Page.prefab", "Wall");
        var p = PrefabUtility.LoadPrefabContents(dir + "Table-With-Engine-Page.prefab");
        var sti = p.GetComponentInChildren(Find("SimulatedTrackedImage"), true);
        var so = new SerializedObject(sti);
        so.FindProperty("m_Image").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Shared/Textures/engine-marker.png");
        so.FindProperty("m_ImagePhysicalSizeMeters").vector2Value = new Vector2(0.5f, 0.5f);
        so.ApplyModifiedProperties();
        PrefabUtility.SaveAsPrefabAsset(p, dir + "Table-With-Engine-Page.prefab");
        PrefabUtility.UnloadPrefabContents(p);
        AssetDatabase.DeleteAsset("Assets/003-Textbook-Popup/SimulationEnvironment.prefab");
        var mgr = AssetDatabase.LoadMainAssetAtPath("Assets/XR/UserSimulationSettings/SimulationEnvironmentAssetsManager.asset");
        var mso = new SerializedObject(mgr);
        var list = mso.FindProperty("m_EnvironmentPrefabPaths");
        list.ClearArray();
        foreach (var s in new[] { dir + "Table.prefab", dir + "Table-With-Engine-Page.prefab", src }) { list.arraySize++; list.GetArrayElementAtIndex(list.arraySize - 1).stringValue = s; }
        mso.ApplyModifiedProperties(); EditorUtility.SetDirty(mgr);
        AssetDatabase.SaveAssets();
        return "envs ok";
    }

    public static string Solar()
    {
        var ctrl = AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/006-Solar-System/SolarSystem.controller");
        foreach (var s in ctrl.layers[0].stateMachine.states) s.state.speed = 0.1f;
        EditorUtility.SetDirty(ctrl);
        var path = "Assets/Shared/Prefabs/SolarSystem.prefab";
        var root = PrefabUtility.LoadPrefabContents(path);
        foreach (var sp in root.GetComponentsInChildren<Spin>(true)) sp.degreesPerSecond = 4f;
        PrefabUtility.SaveAsPrefabAsset(root, path);
        PrefabUtility.UnloadPrefabContents(root);
        AssetDatabase.SaveAssets();
        return "solar slowed";
    }

    public static string Scenes()
    {
        EditorSceneManager.SaveOpenScenes();
        foreach (var n in new[] { "002-Place-the-Heart", "003-Textbook-Pop-up", "004-Build-a-Molecule", "005-Label-the-Parts", "006-Solar-System-on-the-Table" })
        {
            var s = EditorSceneManager.OpenScene("Assets/Scenes/" + n + ".unity", OpenSceneMode.Single);
            foreach (var r in s.GetRootGameObjects().Where(g => g.name == "UI").ToArray()) UnityEngine.Object.DestroyImmediate(r);
            if (n.StartsWith("003"))
                foreach (var sp in UnityEngine.Object.FindObjectsByType(Find("ObjectSpawner"), FindObjectsInactive.Include, FindObjectsSortMode.None).Cast<Component>().ToArray()) UnityEngine.Object.DestroyImmediate(sp.gameObject);
            EditorSceneManager.SaveScene(s);
        }
        return "scenes cleaned";
    }
}
