using UnityEngine;

public class LabelToggle : MonoBehaviour
{
    bool show = true;
    public void Toggle()
    {
        show = !show;
        foreach (var l in FindObjectsByType<LabelBillboard>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            l.gameObject.SetActive(show);
    }
}
