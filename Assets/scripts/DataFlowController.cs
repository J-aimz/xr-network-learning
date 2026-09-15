using System.Collections;
using UnityEngine;

public class DataFlowController : MonoBehaviour
{
    [Header("Animation")]
    public float travelTime = 1.2f;
    public float packetSize = 0.18f;

    private Transform pc1;
    private Transform networkSwitch;
    private Transform router;
    private Transform server;

    private GameObject packet;
    private bool isRunning;

    public void StartFlow()
    {
        if (isRunning)
            return;

        if (!FindDevices())
            return;

        StartCoroutine(AnimateDataFlow());
    }

    bool FindDevices()
    {
        GameObject pc =
            GameObject.Find("PC 1");

        GameObject sw =
            GameObject.Find("Switch");

        GameObject rt =
            GameObject.Find("Router");

        GameObject srv =
            GameObject.Find("Server");

        if (pc == null ||
            sw == null ||
            rt == null ||
            srv == null)
        {
            Debug.LogError(
                "Data Flow: Could not find all network devices."
            );

            return false;
        }

        pc1 = pc.transform;
        networkSwitch = sw.transform;
        router = rt.transform;
        server = srv.transform;

        return true;
    }

    IEnumerator AnimateDataFlow()
    {
        isRunning = true;

        CreatePacket();

        yield return MovePacket(
            pc1.position,
            networkSwitch.position
        );

        yield return MovePacket(
            networkSwitch.position,
            router.position
        );

        yield return MovePacket(
            router.position,
            server.position
        );

        Destroy(packet);

        isRunning = false;

        Debug.Log("Data flow completed.");
    }

    IEnumerator MovePacket(
        Vector3 start,
        Vector3 destination)
    {
        start += Vector3.up * 0.35f;
        destination += Vector3.up * 0.35f;

        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;

            float progress =
                Mathf.Clamp01(
                    elapsed / travelTime
                );

            progress =
                Mathf.SmoothStep(
                    0f,
                    1f,
                    progress
                );

            packet.transform.position =
                Vector3.Lerp(
                    start,
                    destination,
                    progress
                );

            yield return null;
        }

        packet.transform.position =
            destination;
    }

    void CreatePacket()
    {
        packet =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere
            );

        packet.name =
            "Data Packet";

        packet.transform.localScale =
            Vector3.one * packetSize;

        packet.transform.position =
            pc1.position +
            Vector3.up * 0.35f;

        Renderer renderer =
            packet.GetComponent<Renderer>();

        Material material =
            new Material(
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                )
            );

        Color packetColor =
            new Color(
                0.05f,
                0.8f,
                1f
            );

        material.color =
            packetColor;

        material.EnableKeyword(
            "_EMISSION"
        );

        material.SetColor(
            "_EmissionColor",
            packetColor * 5f
        );

        renderer.material =
            material;

        Collider collider =
            packet.GetComponent<Collider>();

        if (collider != null)
            Destroy(collider);
    }
}