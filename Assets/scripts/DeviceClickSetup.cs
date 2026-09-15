using UnityEngine;

public class DeviceClickSetup : MonoBehaviour
{
    void Start()
    {
        SetupDeviceColliders();
    }

    void SetupDeviceColliders()
    {
        DeviceInfo[] devices =
            FindObjectsByType<DeviceInfo>(
                FindObjectsSortMode.None
            );

        foreach (DeviceInfo device in devices)
        {
            Renderer[] renderers =
                device.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                // Don't add another collider if one already exists
                if (renderer.GetComponent<Collider>() != null)
                    continue;

                BoxCollider collider =
                    renderer.gameObject.AddComponent<BoxCollider>();

                collider.center =
                    renderer.localBounds.center;

                collider.size =
                    renderer.localBounds.size;
            }
        }

        Debug.Log(
            $"DeviceClickSetup: configured {devices.Length} devices."
        );
    }
}