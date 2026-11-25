using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Opens each lesson scene, enters Play mode for a few seconds, and records console errors to Logs/playtest.txt.
[InitializeOnLoad]
public static class PlayTester
{
    static readonly string[] Scenes = {
        "Assets/Scenes/001-Try-the-XR-Foundation.unity", "Assets/Scenes/002-Place-the-Heart.unity",
        "Assets/Scenes/003-Textbook-Pop-up.unity", "Assets/Scenes/004-Build-a-Molecule.unity",
        "Assets/Scenes/005-Label-the-Parts.unity", "Assets/Scenes/006-Solar-System-on-the-Table.unity",
        "Assets/Scenes/007-Virtual-Lab-Visit.unity" };

    static PlayTester()
    {
        Application.logMessageReceived += OnLog;
        EditorApplication.update += Tick;
    }

    public static string Start()
    {
        EditorSceneManager.SaveOpenScenes();
        SessionState.SetInt("pt_i", 0); SessionState.SetInt("pt_phase", 0); SessionState.SetString("pt_report", "");
        return "playtest started";
    }

    static void OnLog(string msg, string stack, LogType type)
    {
        int i = SessionState.GetInt("pt_i", -1);
        if (i < 0 || i >= Scenes.Length) return;
        if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
            SessionState.SetString("pt_report", SessionState.GetString("pt_report", "") + "[" + Path.GetFileNameWithoutExtension(Scenes[i]) + "] " + type + ": " + msg.Split('\n')[0] + "\n");
    }

    static void AddInfo(string text, int i)
    {
        SessionState.SetString("pt_report", SessionState.GetString("pt_report", "") + "[" + Path.GetFileNameWithoutExtension(Scenes[i]) + "] PROBE: " + text + "\n");
    }

    static string CountClones() => string.Join(",", Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
        .Where(t => t.name.EndsWith("(Clone)")).Select(t => t.name.Replace("(Clone)", "")));

    static string Probe(string scene)
    {
        var sb = new StringBuilder();
        var cam = Camera.main;
        var pos = cam != null ? cam.transform.position + cam.transform.forward * 0.6f + Vector3.down * 0.3f : Vector3.forward;
        var sp = Object.FindFirstObjectByType<ObjectSpawner>();
        if (scene.Contains("003"))
        {
            var imgs = Object.FindObjectsByType<ARTrackedImage>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            sb.Append("trackedImages=" + imgs.Length);
            foreach (var im in imgs) sb.Append(" [" + im.referenceImage.name + " children=" + im.transform.childCount + " animators=" + im.GetComponentsInChildren<Animator>().Length + "]");
            return sb.ToString();
        }
        if (sp == null || !sp.gameObject.activeInHierarchy) return "no active spawner (scene is not a spawner scene)";
        if (scene.Contains("004"))
        {
            var picker = Object.FindFirstObjectByType<AtomPicker>();
            for (int k = 0; k < 3; k++) { picker.Pick(k); sb.Append(sp.TrySpawnObject(pos + Vector3.right * 0.1f * k, Vector3.up) ? "spawned " : "FAILED "); }
            sb.Append("-> " + CountClones());
            return sb.ToString();
        }
        sb.Append(sp.TrySpawnObject(pos, Vector3.up) ? "spawned " : "spawn FAILED ");
        sb.Append("-> " + CountClones());
        if (scene.Contains("005"))
        {
            int on = Object.FindObjectsByType<LabelBillboard>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
            Object.FindFirstObjectByType<LabelToggle>().Toggle();
            int off = Object.FindObjectsByType<LabelBillboard>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
            Object.FindFirstObjectByType<LabelToggle>().Toggle();
            int back = Object.FindObjectsByType<LabelBillboard>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
            sb.Append(" labels visible " + on + " -> toggle -> " + off + " -> toggle -> " + back);
        }
        if (scene.Contains("006"))
        {
            var an = Object.FindObjectsByType<Animator>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).FirstOrDefault(a => a.GetComponentInParent<Transform>() != null && a.name == "Model");
            sb.Append(" spinners=" + Object.FindObjectsByType<Spin>(FindObjectsSortMode.None).Length);
            if (an != null) sb.Append(" animatorTime=" + an.GetCurrentAnimatorStateInfo(0).normalizedTime.ToString("F2") + " speed=" + an.speed);
        }
        return sb.ToString();
    }

    static void Tick()
    {
        int i = SessionState.GetInt("pt_i", -1);
        if (i < 0 || EditorApplication.isCompiling || EditorApplication.isUpdating) return;
        int phase = SessionState.GetInt("pt_phase", 0);
        double now = EditorApplication.timeSinceStartup;
        if (phase == 0)
        {
            if (EditorApplication.isPlaying) return;
            EditorSceneManager.OpenScene(Scenes[i]);
            SessionState.SetFloat("pt_t0", (float)now); SessionState.SetInt("pt_phase", 1);
            EditorApplication.isPlaying = true;
        }
        else if (phase == 1)
        {
            if (!EditorApplication.isPlaying) return;
            if (now - SessionState.GetFloat("pt_t0", 0) < 14) return;
            AddInfo(Probe(Scenes[i]), i);
            EditorApplication.isPlaying = false; SessionState.SetInt("pt_phase", 2);
        }
        else if (phase == 2)
        {
            if (EditorApplication.isPlaying) return;
            i++; SessionState.SetInt("pt_i", i); SessionState.SetInt("pt_phase", 0);
            if (i >= Scenes.Length)
            {
                SessionState.SetInt("pt_i", -1);
                var r = SessionState.GetString("pt_report", "");
                File.WriteAllText("Logs/playtest.txt", r.Length == 0 ? "NO ERRORS in " + Scenes.Length + " scenes" : r);
            }
        }
    }
}
