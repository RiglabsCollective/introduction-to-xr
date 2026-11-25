using UnityEngine;

public class LabelBillboard : MonoBehaviour
{
    void LateUpdate() { transform.forward = transform.position - Camera.main.transform.position; }
}
