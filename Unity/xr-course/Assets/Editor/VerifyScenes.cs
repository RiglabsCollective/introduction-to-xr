using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

// Checks every lesson scene for missing scripts / references and prints what each one is set up with.
public static class VerifyScenes
{
    public static string Run()
    {
        var sb = new StringBuilder();
        EditorSceneManager.SaveOpenScenes();
        foreach (var guid in AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes", "Assets/Starter" }))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var scene = EditorSceneManager.OpenScene(path);
            sb.AppendLine("== " + path);
            int missing = 0, total = 0;
            foreach (var go in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None).Select(t => t.gameObject))
            {
                total++;
                missing += GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
                if (PrefabUtility.IsPrefabAssetMissing(go)) { missing++; sb.AppendLine("  missing prefab asset: " + go.name); }
            }
            sb.AppendLine("  objects " + total + ", missing scripts/prefabs " + missing);
            foreach (var sp in Object.FindObjectsByType<ObjectSpawner>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                sb.AppendLine("  spawner active=" + sp.gameObject.activeInHierarchy + " prefabs=[" + string.Join(", ", sp.objectPrefabs.Select(p => p == null ? "NULL" : p.name)) + "] index=" + sp.spawnOptionIndex);
            foreach (var m in Object.FindObjectsByType<ARTrackedImageManager>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                sb.AppendLine("  trackedImageManager library=" + (m.referenceLibrary != null) + " prefab=" + (m.trackedImagePrefab != null ? m.trackedImagePrefab.name : "NULL"));
            foreach (var b in Object.FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                var cv = b.GetComponentInParent<Canvas>(true);
                if (cv == null || cv.name != "Course UI") continue;
                var n = b.onClick.GetPersistentEventCount();
                sb.AppendLine("  button '" + b.name + "' listeners=" + n + (n > 0 ? " -> " + b.onClick.GetPersistentMethodName(0) + " on " + (b.onClick.GetPersistentTarget(0) != null) : ""));
            }
            var ui = GameObject.Find("UI");
            sb.AppendLine("  template UI active=" + (ui != null));
            sb.AppendLine("  skybox=" + (RenderSettings.skybox != null ? RenderSettings.skybox.name : "none"));
        }
        foreach (var p in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Shared/Prefabs" }).Select(AssetDatabase.GUIDToAssetPath))
        {
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(p);
            sb.AppendLine("prefab " + p + " components: " + string.Join(",", go.GetComponents<Component>().Select(c => c == null ? "MISSING" : c.GetType().Name)));
        }
        System.IO.File.WriteAllText("Logs/verify.txt", sb.ToString());
        return "verify written";
    }
}
