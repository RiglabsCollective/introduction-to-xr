using System.Text;
using UnityEditor;
using UnityEngine;

public static class Dump
{
    [MenuItem("Tools/Course/Dump models")]
    public static void Run()
    {
        var sb = new StringBuilder();
        foreach (var path in new[] { "Assets/Shared/Models/solar-system.fbx", "Assets/Shared/Models/heart.fbx" })
        {
            var m = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            sb.AppendLine("== " + path);
            Walk(m.transform, 0, sb);
            foreach (var o in AssetDatabase.LoadAllAssetsAtPath(path)) if (o is AnimationClip c) sb.AppendLine("clip " + c.name + " len " + c.length);
        }
        System.IO.File.WriteAllText("Logs/dump.txt", sb.ToString());
        Debug.Log("dump written");
    }
    static void Walk(Transform t, int d, StringBuilder sb)
    {
        var r = t.GetComponent<Renderer>();
        sb.AppendLine(new string(' ', d * 2) + t.name + " pos" + t.localPosition.ToString("F2") + " scl" + t.localScale.ToString("F2") + (r ? " bounds" + r.bounds.size.ToString("F2") : ""));
        foreach (Transform c in t) Walk(c, d + 1, sb);
    }
}
