using System.Text;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public static class DumpScenes
{
    public static string Run()
    {
        var sb = new StringBuilder();
        foreach (var p in AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes", "Assets/Starter" }).Select(AssetDatabase.GUIDToAssetPath).OrderBy(x => x))
        {
            var s = EditorSceneManager.OpenScene(p, OpenSceneMode.Single);
            sb.AppendLine("== " + p);
            foreach (var r in s.GetRootGameObjects()) Walk(r.transform, 0, sb);
        }
        System.IO.File.WriteAllText("Logs/scenes.txt", sb.ToString());
        return "ok";
    }
    static void Walk(Transform t, int d, StringBuilder sb)
    {
        if (d > 2) return;
        sb.AppendLine(new string(' ', d * 2) + t.name + (t.gameObject.activeSelf ? "" : " (off)") + " [" + string.Join(",", t.GetComponents<Component>().Where(c => c != null).Select(c => c.GetType().Name).Where(n => n != "Transform")) + "]");
        foreach (Transform c in t) Walk(c, d + 1, sb);
    }
}
