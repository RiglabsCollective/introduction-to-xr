using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEditor.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// Tools > Course > Build 003 scene: image library + V8 engine prefab + scene 003.
public static class Build003
{
    [MenuItem("Tools/Course/Build 003 scene")]
    public static void Run()
    {
        const string tex = "Assets/Shared/Textures/";
        // texture import settings
        var norm = (TextureImporter)AssetImporter.GetAtPath(tex + "V8Engine/initialShadingGroup_normal.png");
        if (norm != null && norm.textureType != TextureImporterType.NormalMap) { norm.textureType = TextureImporterType.NormalMap; norm.SaveAndReimport(); }

        // material
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(tex + "V8Engine/initialShadingGroup_albedo.jpg"));
        mat.SetTexture("_BumpMap", AssetDatabase.LoadAssetAtPath<Texture2D>(tex + "V8Engine/initialShadingGroup_normal.png"));
        mat.EnableKeyword("_NORMALMAP");
        mat.SetFloat("_Metallic", 0.5f); mat.SetFloat("_Smoothness", 0.5f);
        Directory.CreateDirectory("Assets/003-Textbook-Popup");
        AssetDatabase.CreateAsset(mat, "Assets/003-Textbook-Popup/V8Engine.mat");

        // reference image library
        var lib = ScriptableObject.CreateInstance<XRReferenceImageLibrary>();
        AssetDatabase.CreateAsset(lib, "Assets/003-Textbook-Popup/TextbookImages.asset");
        lib.Add();
        lib.SetTexture(0, AssetDatabase.LoadAssetAtPath<Texture2D>(tex + "engine-marker.png"), true);
        lib.SetName(0, "engine-page");
        lib.SetSpecifySize(0, true);
        lib.SetSize(0, new Vector2(0.2f, 0.2f));
        EditorUtility.SetDirty(lib);

        // prefab: V8 engine, animated, ~15 cm
        var model = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Shared/Models/v8-engine.fbx");
        var root = new GameObject("V8 Engine");
        var inst = (GameObject)PrefabUtility.InstantiatePrefab(model);
        inst.name = "Model"; inst.transform.SetParent(root.transform, false);
        var b = Measure(root.transform, inst);
        float s = 0.15f / Mathf.Max(b.size.x, b.size.y, b.size.z);
        inst.transform.localScale = Vector3.one * s;
        foreach (var r in inst.GetComponentsInChildren<Renderer>()) r.sharedMaterial = mat;
        var clip = AssetDatabase.LoadAllAssetsAtPath("Assets/Shared/Models/v8-engine.fbx").OfType<AnimationClip>().FirstOrDefault(c => !c.name.StartsWith("__preview"));
        if (clip != null)
        {
            var ctrl = AnimatorController.CreateAnimatorControllerAtPath("Assets/003-Textbook-Popup/V8Engine.controller");
            var st = ctrl.layers[0].stateMachine.AddState(clip.name); st.motion = clip;
            ctrl.layers[0].stateMachine.defaultState = st;
            var an = inst.GetComponent<Animator>(); if (an == null) an = inst.AddComponent<Animator>();
            an.runtimeAnimatorController = ctrl;
        }
        PrefabUtility.SaveAsPrefabAsset(root, "Assets/Shared/Prefabs/V8Engine.prefab");
        Object.DestroyImmediate(root);

        // scene 003 from the template scene 001
        AssetDatabase.CopyAsset("Assets/Scenes/001-Try-the-XR-Foundation.unity", "Assets/Scenes/003-Textbook-Pop-up.unity");
        var scene = EditorSceneManager.OpenScene("Assets/Scenes/003-Textbook-Pop-up.unity");
        var spawner = Object.FindFirstObjectByType<UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets.ObjectSpawner>(FindObjectsInactive.Include);
        if (spawner != null) spawner.gameObject.SetActive(false);
        var origin = Object.FindFirstObjectByType<Unity.XR.CoreUtils.XROrigin>();
        var mgr = origin.gameObject.AddComponent<ARTrackedImageManager>();
        var so = new SerializedObject(mgr);
        so.FindProperty("m_SerializedLibrary").objectReferenceValue = lib;
        so.FindProperty("m_MaxNumberOfMovingImages").intValue = 1;
        so.FindProperty("m_TrackedImagePrefab").objectReferenceValue = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Shared/Prefabs/V8Engine.prefab");
        so.ApplyModifiedProperties();
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Debug.Log("Build003 done. clip=" + (clip ? clip.name : "none") + " scale=" + s);
    }

    static Bounds Measure(Transform root, GameObject inst)
    {
        bool any = false; var b = new Bounds();
        foreach (var r in inst.GetComponentsInChildren<Renderer>())
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
}
