using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

// Builds the Final scenes for lessons 004-007. Run through the Runner (Logs/run.txt).
public static class BuildLessons
{
    const string Template = "Assets/Scenes/001-Try-the-XR-Foundation.unity";

    // ---------- helpers ----------
    static void Ensure(string dir)
    {
        if (AssetDatabase.IsValidFolder(dir)) return;
        var p = Path.GetDirectoryName(dir).Replace("\\", "/");
        AssetDatabase.CreateFolder(p, Path.GetFileName(dir));
    }

    static Material Mat(string path, Color c, float smooth = 0.5f, float metal = 0f)
    {
        var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (m == null) { m = new Material(Shader.Find("Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(m, path); }
        m.SetColor("_BaseColor", c); m.SetFloat("_Smoothness", smooth); m.SetFloat("_Metallic", metal);
        EditorUtility.SetDirty(m); return m;
    }

    static void MakeInteractable(GameObject root)
    {
        var rb = root.GetComponent<Rigidbody>(); if (rb == null) rb = root.AddComponent<Rigidbody>();
        rb.isKinematic = true; rb.useGravity = false;
        if (root.GetComponent<XRGrabInteractable>() == null) root.AddComponent<XRGrabInteractable>();
        if (root.GetComponent<ARTransformer>() == null) root.AddComponent<ARTransformer>();
    }

    static Bounds LocalBounds(Transform root)
    {
        bool any = false; var b = new Bounds();
        foreach (var r in root.GetComponentsInChildren<Renderer>())
        {
            Mesh m = r is SkinnedMeshRenderer sm ? sm.sharedMesh : (r.TryGetComponent<MeshFilter>(out var mf) ? mf.sharedMesh : null);
            if (m == null) continue;
            var mb = m.bounds;
            for (int i = 0; i < 8; i++)
            {
                var c = mb.center + Vector3.Scale(mb.extents, new Vector3((i & 1) == 0 ? -1 : 1, (i & 2) == 0 ? -1 : 1, (i & 4) == 0 ? -1 : 1));
                var p = root.InverseTransformPoint(r.transform.TransformPoint(c));
                if (!any) { b = new Bounds(p, Vector3.zero); any = true; } else b.Encapsulate(p);
            }
        }
        return b;
    }

    static void LoopClips(string fbx)
    {
        var imp = (ModelImporter)AssetImporter.GetAtPath(fbx);
        var clips = imp.defaultClipAnimations;
        foreach (var c in clips) c.loopTime = true;
        imp.clipAnimations = clips; imp.SaveAndReimport();
    }

    static void AnimateModel(GameObject inst, string fbx, string controllerPath)
    {
        var clip = AssetDatabase.LoadAllAssetsAtPath(fbx).OfType<AnimationClip>().FirstOrDefault(c => !c.name.StartsWith("__preview"));
        if (clip == null) return;
        var ctrl = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
        if (ctrl == null)
        {
            ctrl = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            var st = ctrl.layers[0].stateMachine.AddState(clip.name); st.motion = clip;
            ctrl.layers[0].stateMachine.defaultState = st;
        }
        var an = inst.GetComponent<Animator>(); if (an == null) an = inst.AddComponent<Animator>();
        an.runtimeAnimatorController = ctrl;
    }

    static ObjectSpawner OpenNewScene(string sceneName, GameObject[] prefabs)
    {
        EditorSceneManager.SaveOpenScenes();
        var dst = "Assets/Scenes/" + sceneName + ".unity";
        AssetDatabase.DeleteAsset(dst);
        AssetDatabase.CopyAsset(Template, dst);
        EditorSceneManager.OpenScene(dst);
        var ui = GameObject.Find("UI"); if (ui != null) ui.SetActive(false);
        var sp = Object.FindFirstObjectByType<ObjectSpawner>(FindObjectsInactive.Include);
        var so = new SerializedObject(sp);
        var list = so.FindProperty("m_ObjectPrefabs"); list.arraySize = prefabs.Length;
        for (int i = 0; i < prefabs.Length; i++) list.GetArrayElementAtIndex(i).objectReferenceValue = prefabs[i];
        so.FindProperty("m_SpawnOptionIndex").intValue = 0;
        so.FindProperty("m_ApplyRandomAngleAtSpawn").boolValue = false;
        so.FindProperty("m_OnlySpawnInView").boolValue = false;
        so.ApplyModifiedProperties();
        return sp;
    }

    static Transform MakeCanvas()
    {
        var go = new GameObject("Course UI", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var cv = go.GetComponent<Canvas>(); cv.renderMode = RenderMode.ScreenSpaceOverlay;
        var cs = go.GetComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; cs.referenceResolution = new Vector2(1080, 1920); cs.matchWidthOrHeight = 0.5f;
        var panel = new GameObject("Button Bar", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        panel.transform.SetParent(go.transform, false);
        var rt = (RectTransform)panel.transform;
        rt.anchorMin = new Vector2(0, 0); rt.anchorMax = new Vector2(1, 0); rt.pivot = new Vector2(0.5f, 0);
        rt.sizeDelta = new Vector2(0, 220); rt.anchoredPosition = new Vector2(0, 40);
        var h = panel.GetComponent<HorizontalLayoutGroup>();
        h.spacing = 24; h.padding = new RectOffset(24, 24, 24, 24);
        h.childControlWidth = true; h.childControlHeight = true; h.childForceExpandWidth = true; h.childForceExpandHeight = true;
        return panel.transform;
    }

    static Button MakeButton(Transform bar, string label, Color color)
    {
        var go = new GameObject(label + " Button", typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(bar, false);
        go.GetComponent<Image>().color = color;
        var t = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
        t.transform.SetParent(go.transform, false);
        var rt = (RectTransform)t.transform;
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.offsetMin = rt.offsetMax = Vector2.zero;
        var tm = t.GetComponent<TextMeshProUGUI>();
        tm.text = label; tm.fontSize = 56; tm.alignment = TextAlignmentOptions.Center; tm.color = Color.white;
        return go.GetComponent<Button>();
    }

    // ---------- 004 Build a Molecule ----------
    public static string Build004()
    {
        Ensure("Assets/004-Build-a-Molecule");
        var specs = new[] { ("Hydrogen", Color.white, 0.03f), ("Oxygen", new Color(0.85f, 0.1f, 0.1f), 0.05f), ("Carbon", new Color(0.2f, 0.2f, 0.22f), 0.045f) };
        var prefabs = specs.Select(s =>
        {
            var m = Mat("Assets/004-Build-a-Molecule/" + s.Item1 + ".mat", s.Item2, 0.7f);
            var root = new GameObject(s.Item1);
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere); sphere.name = "Atom";
            Object.DestroyImmediate(sphere.GetComponent<Collider>());
            sphere.transform.SetParent(root.transform, false); sphere.transform.localScale = Vector3.one * s.Item3;
            sphere.GetComponent<Renderer>().sharedMaterial = m;
            var col = root.AddComponent<SphereCollider>(); col.radius = s.Item3 / 2f;
            MakeInteractable(root);
            var p = PrefabUtility.SaveAsPrefabAsset(root, "Assets/Shared/Prefabs/" + s.Item1 + ".prefab");
            Object.DestroyImmediate(root); return p;
        }).ToArray();

        var sp = OpenNewScene("004-Build-a-Molecule", prefabs);
        var picker = new GameObject("Atom Picker").AddComponent<AtomPicker>(); picker.spawner = sp;
        var bar = MakeCanvas();
        for (int i = 0; i < specs.Length; i++)
        {
            var btn = MakeButton(bar, specs[i].Item1, new Color(0.15f, 0.25f, 0.4f, 0.9f));
            UnityEventTools.AddIntPersistentListener(btn.onClick, picker.Pick, i);
        }
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        return "004 ok";
    }

    // ---------- 005 Label the Parts ----------
    public static string Build005()
    {
        Ensure("Assets/005-Label-the-Parts");
        var heart = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Shared/Prefabs/Heart.prefab");
        var root = (GameObject)PrefabUtility.InstantiatePrefab(heart); root.name = "Heart Labeled";
        PrefabUtility.UnpackPrefabInstance(root, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        var labels = new GameObject("Labels"); labels.transform.SetParent(root.transform, false);
        var items = new[] {
            ("Aorta", new Vector3(0.02f, 0.09f, 0f)), ("Pulmonary artery", new Vector3(-0.05f, 0.08f, 0.02f)),
            ("Left ventricle", new Vector3(0.07f, -0.02f, 0f)), ("Right atrium", new Vector3(-0.08f, 0.01f, 0f)) };
        foreach (var it in items)
        {
            var l = new GameObject(it.Item1, typeof(TextMeshPro), typeof(LabelBillboard));
            l.transform.SetParent(labels.transform, false); l.transform.localPosition = it.Item2;
            var tm = l.GetComponent<TextMeshPro>();
            tm.text = it.Item1; tm.fontSize = 0.25f; tm.alignment = TextAlignmentOptions.Center; tm.color = Color.white;
            tm.textWrappingMode = TextWrappingModes.NoWrap;
            ((RectTransform)l.transform).sizeDelta = new Vector2(0.2f, 0.05f);
        }
        var prefab = PrefabUtility.SaveAsPrefabAsset(root, "Assets/Shared/Prefabs/HeartLabeled.prefab");
        Object.DestroyImmediate(root);

        OpenNewScene("005-Label-the-Parts", new[] { prefab });
        var tog = new GameObject("Label Toggle").AddComponent<LabelToggle>();
        var bar = MakeCanvas();
        var btn = MakeButton(bar, "Labels on/off", new Color(0.15f, 0.25f, 0.4f, 0.9f));
        UnityEventTools.AddVoidPersistentListener(btn.onClick, tog.Toggle);
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        return "005 ok";
    }

    // ---------- 006 Solar System ----------
    public static string Build006()
    {
        Ensure("Assets/006-Solar-System");
        const string fbx = "Assets/Shared/Models/solar-system.fbx";
        LoopClips(fbx);
        var model = AssetDatabase.LoadAssetAtPath<GameObject>(fbx);
        var root = new GameObject("Solar System");
        var inst = (GameObject)PrefabUtility.InstantiatePrefab(model); inst.name = "Model"; inst.transform.SetParent(root.transform, false);
        // each planet (the child with the same name as its pivot) spins on its own axis
        foreach (var pivot in inst.transform.Cast<Transform>().ToArray())
            foreach (var planet in pivot.Cast<Transform>().Where(t => t.name == pivot.name).ToArray())
                planet.gameObject.AddComponent<Spin>().degreesPerSecond = 40f;
        AnimateModel(inst, fbx, "Assets/006-Solar-System/SolarSystem.controller");
        var b = LocalBounds(root.transform);
        var col = root.AddComponent<BoxCollider>(); col.center = b.center; col.size = b.size;
        MakeInteractable(root);
        var prefab = PrefabUtility.SaveAsPrefabAsset(root, "Assets/Shared/Prefabs/SolarSystem.prefab");
        Object.DestroyImmediate(root);
        OpenNewScene("006-Solar-System-on-the-Table", new[] { prefab });
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        return "006 ok bounds " + b.size;
    }

    // ---------- 007 VR ----------
    public static string Build007()
    {
        EditorSceneManager.SaveOpenScenes();
        var src = "Assets/Samples/XR Interaction Toolkit/3.5.1/Starter Assets/DemoScene.unity";
        var dst = "Assets/Scenes/007-Virtual-Lab-Visit.unity";
        AssetDatabase.DeleteAsset(dst); AssetDatabase.CopyAsset(src, dst);
        EditorSceneManager.OpenScene(dst);
        var tex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Shared/Textures/Heritage/panorama-fortress.jpg");
        Ensure("Assets/007-Virtual-Lab-Visit");
        var sky = AssetDatabase.LoadAssetAtPath<Material>("Assets/007-Virtual-Lab-Visit/FortressSky.mat");
        if (sky == null) { sky = new Material(Shader.Find("Skybox/Panoramic")); AssetDatabase.CreateAsset(sky, "Assets/007-Virtual-Lab-Visit/FortressSky.mat"); }
        sky.SetTexture("_MainTex", tex); sky.SetFloat("_Mapping", 1f); sky.SetFloat("_ImageType", 0f);
        EditorUtility.SetDirty(sky);
        RenderSettings.skybox = sky;
        DynamicGI.UpdateEnvironment();
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        return "007 ok";
    }

    public static string FixOld()
    {
        LoopClips("Assets/Shared/Models/v8-engine.fbx");
        EditorSceneManager.SaveOpenScenes();
        var s = EditorSceneManager.OpenScene("Assets/Scenes/003-Textbook-Pop-up.unity");
        var ui = GameObject.Find("UI"); if (ui != null) ui.SetActive(false);
        EditorSceneManager.SaveScene(s);
        return "fixed";
    }
}
