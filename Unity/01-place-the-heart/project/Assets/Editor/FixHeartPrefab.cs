using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

// Tools > Heart > Rebuild prefab for AR: builds Assets/Prefabs/Heart.prefab from heart.fbx with
// a Box Collider, Rigidbody (kinematic), XR Grab Interactable and AR Transformer.
public static class FixHeartPrefab
{
    [MenuItem("Tools/Heart/Rebuild prefab for AR")]
    public static void Run()
    {
        var model = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Models/heart.fbx");
        var root = new GameObject("Heart");
        var inst = (GameObject)PrefabUtility.InstantiatePrefab(model);
        inst.name = "Model";
        inst.transform.SetParent(root.transform, false);

        // Measure at scale 1, then scale the model so the longest side is 0.15 m.
        var b = MeshBounds(root.transform, inst);
        float s = 0.15f / Mathf.Max(b.size.x, b.size.y, b.size.z);
        inst.transform.localScale = Vector3.one * s;
        b = new Bounds(b.center * s, b.size * s);

        var bc = root.AddComponent<BoxCollider>();
        bc.center = b.center; bc.size = b.size;
        var rb = root.AddComponent<Rigidbody>();
        rb.isKinematic = true; rb.useGravity = false;
        root.AddComponent<XRGrabInteractable>();
        root.AddComponent<ARTransformer>();

        PrefabUtility.SaveAsPrefabAsset(root, "Assets/Prefabs/Heart.prefab");
        Object.DestroyImmediate(root);
        AssetDatabase.SaveAssets();
        Debug.Log("Heart rebuilt. model scale " + s + ", collider size " + b.size + ", center " + b.center);
    }

    static Bounds MeshBounds(Transform root, GameObject inst)
    {
        bool any = false; var b = new Bounds();
        foreach (var r in inst.GetComponentsInChildren<Renderer>())
        {
            Mesh m = null;
            if (r is SkinnedMeshRenderer sm) m = sm.sharedMesh;
            else if (r.TryGetComponent<MeshFilter>(out var mf)) m = mf.sharedMesh;
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
