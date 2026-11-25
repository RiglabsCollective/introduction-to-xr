using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// Dev helper: write "Class.Method" or "Class.Method:argument" into Logs/run.txt and the Editor runs that static method,
// then writes the outcome to Logs/run-result.txt. Lets scripts drive the open Editor without menu clicks.
[InitializeOnLoad]
public static class Runner
{
    const string Trigger = "Logs/run.txt", Result = "Logs/run-result.txt";
    static double waitStart; static bool refreshed;
    static Runner() { EditorApplication.update += Poll; }

    static void Poll()
    {
        if (EditorApplication.isCompiling || EditorApplication.isUpdating) return;
        if (!File.Exists(Trigger)) return;
        if (!refreshed) { refreshed = true; AssetDatabase.Refresh(); return; } // import and compile any new scripts first
        var full = File.ReadAllText(Trigger).Trim();
        var arg = full.Contains(":") ? full.Substring(full.IndexOf(':') + 1) : null;
        var name = full.Contains(":") ? full.Substring(0, full.IndexOf(':')) : full;
        var parts = name.Split('.');
        Type t = null;
        // GetAssemblies() can still list older, already replaced assemblies: the newest copy of the type is the last one.
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies()) { var found = a.GetType(parts[0]); if (found != null) t = found; }
        if (t == null)
        {
            // New script not compiled yet: wait up to two minutes for the import and compile to finish.
            if (waitStart == 0) waitStart = EditorApplication.timeSinceStartup;
            if (EditorApplication.timeSinceStartup - waitStart < 120) return;
        }
        waitStart = 0; refreshed = false;
        File.Delete(Trigger);
        try
        {
            var m = t.GetMethod(parts[1], BindingFlags.Public | BindingFlags.Static);
            var r = m.Invoke(null, m.GetParameters().Length == 1 ? new object[] { arg } : null);
            File.WriteAllText(Result, "OK " + name + " " + r);
        }
        catch (Exception e) { File.WriteAllText(Result, "ERR " + name + "\n" + e); }
    }
}
