using UnityEngine;

public class Spin : MonoBehaviour
{
    public float degreesPerSecond = 30f;
    void Update() { transform.Rotate(0, degreesPerSecond * Time.deltaTime, 0); }
}
