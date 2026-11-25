using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

// Tools > Course > Export Starter and Final packages
// Starter: the shared models and textures plus one clean Start scene. Drag it into a new Mobile AR project.
// Final:   everything finished (all lesson scenes, prefabs, scripts, image library) plus the Starter content.
public static class CoursePackager
{
    const string OutDir = "../packages";

    [MenuItem("Tools/Course/Export Starter and Final packages")]
    public static string Run()
    {
        Directory.CreateDirectory(OutDir);
        AssetDatabase.SaveAssets();
        Directory.CreateDirectory("Assets/Starter");
        // Clean start scene = the untouched template demo scene (001).
        AssetDatabase.DeleteAsset("Assets/Starter/Start.unity");
        AssetDatabase.CopyAsset("Assets/Scenes/001-Try-the-XR-Foundation.unity", "Assets/Starter/Start.unity");
        AssetDatabase.Refresh();

        var log = new StringBuilder();
        Export(new[] { "Assets/Shared/Models", "Assets/Shared/Textures", "Assets/Starter" }, "XR-Course-Starter", log);
        Export(new[] { "Assets/Shared", "Assets/Scenes", "Assets/003-Textbook-Popup", "Assets/004-Build-a-Molecule",
            "Assets/005-Label-the-Parts", "Assets/006-Solar-System", "Assets/007-Virtual-Lab-Visit", "Assets/Starter" }, "XR-Course-Final", log);
        File.WriteAllText("Logs/export-log.txt", log.ToString());
        return "exported";
    }

    static void Export(string[] roots, string name, StringBuilder log)
    {
        // List every asset under the roots explicitly, so nothing depends on folder recursion.
        var guids = AssetDatabase.FindAssets("", roots);
        var paths = guids.Select(AssetDatabase.GUIDToAssetPath).Where(p => !AssetDatabase.IsValidFolder(p)).Distinct().OrderBy(p => p).ToArray();
        var target = OutDir + "/" + name + ".unitypackage";
        if (File.Exists(target)) File.Delete(target);
        AssetDatabase.ExportPackage(paths, target, ExportPackageOptions.Default); // no dependencies: template assets already exist in a new Mobile AR project
        log.AppendLine("== " + name + " (" + paths.Length + " assets)");
        foreach (var p in paths) log.AppendLine(p);
    }
}
