using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class AtomPicker : MonoBehaviour
{
    public ObjectSpawner spawner;
    public void Pick(int index) { spawner.spawnOptionIndex = index; }
}
