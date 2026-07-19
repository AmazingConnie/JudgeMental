using UnityEngine;
using Unity.XR.CoreUtils; // Required for XR Origin

public class SpawnPlayer : MonoBehaviour
{
    public Transform spawnPoint; // Assign the spawn point in the inspector

    void Start()
    {
        XROrigin xrOrigin = FindObjectOfType<XROrigin>(); // Find the XR Rig
        if (xrOrigin != null)
        {
            xrOrigin.transform.position = spawnPoint.position;
            xrOrigin.transform.rotation = spawnPoint.rotation;
        }
    }
}