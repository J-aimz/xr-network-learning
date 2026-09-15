using UnityEngine;

public class NetworkPrototype : MonoBehaviour
{
    public Material networkDeviceMaterial;
    private Material floorMaterial;
    private Material deviceMaterial;
    private Material darkMaterial;
    private Material screenMaterial;
    private Material cableMaterial;
    private Material accentMaterial;

    private readonly Color blue = new Color(0.05f, 0.35f, 1f);
    private readonly Color cyan = new Color(0.05f, 0.8f, 1f);
    private readonly Color darkBlue = new Color(0.015f, 0.04f, 0.09f);
    private readonly Color grey = new Color(0.12f, 0.15f, 0.19f);

    void RegisterDevice(
    GameObject device,
    string deviceName,
    string deviceType,
    string description)
    {
        DeviceInfo info = device.AddComponent<DeviceInfo>();

        info.deviceName = deviceName;
        info.deviceType = deviceType;
        info.description = description;
    }

    void Start()
    {
        CreateMaterials();
        BuildEnvironment();
        BuildNetwork();
        SetupLighting();
        SetupCamera();
    }

    // ---------------------------------------------------------
    // MATERIALS
    // ---------------------------------------------------------

    void CreateMaterials()
    {
        floorMaterial = CreateMaterial(
            "Floor",
            new Color(0.025f, 0.045f, 0.08f)
        );

        deviceMaterial = CreateMaterial(
            "Device",
            new Color(0.035f, 0.06f, 0.11f)
        );

        darkMaterial = CreateMaterial(
            "Dark",
            new Color(0.008f, 0.012f, 0.02f)
        );

        screenMaterial = CreateEmissionMaterial(
            "Screen",
            new Color(0.01f, 0.2f, 0.8f)
        );

        cableMaterial = CreateEmissionMaterial(
            "Network Cable",
            cyan
        );

        accentMaterial = CreateEmissionMaterial(
            "Accent",
            blue
        );
    }

    Material CreateMaterial(string materialName, Color color)
    {
        if (networkDeviceMaterial == null)
        {
            Debug.LogError(
                "Network Device Material has not been assigned."
            );

            return null;
        }

        Material material =
            new Material(networkDeviceMaterial);

        material.name = materialName;
        material.color = color;

        return material;
    }

    Material CreateEmissionMaterial(string materialName, Color color)
    {
        Material material = CreateMaterial(materialName, color);

        if (material.HasProperty("_EmissionColor"))
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color * 3f);
        }

        return material;
    }

    // ---------------------------------------------------------
    // ENVIRONMENT
    // ---------------------------------------------------------

    void BuildEnvironment()
    {
        GameObject floor = CreateCube(
            "Learning Floor",
            new Vector3(0, -0.5f, 0),
            new Vector3(18, 0.5f, 13),
            floorMaterial
        );

        // Raised platform
        CreateCube(
            "Network Platform",
            new Vector3(0, -0.05f, 0),
            new Vector3(15, 0.15f, 10),
            darkMaterial
        );

        // Accent strips
        CreateCube(
            "Front Accent",
            new Vector3(0, 0.05f, -5),
            new Vector3(14, 0.04f, 0.08f),
            accentMaterial
        );

        CreateCube(
            "Back Accent",
            new Vector3(0, 0.05f, 5),
            new Vector3(14, 0.04f, 0.08f),
            accentMaterial
        );
    }

    // ---------------------------------------------------------
    // NETWORK
    // ---------------------------------------------------------

    void BuildNetwork()
    {
        GameObject server = CreateServer(
            new Vector3(0, 3.4f, 4)
        );

        RegisterDevice(
    server,
    "Server",
    "Network Server",
    "A server provides services and resources to computers and other devices on a network. In this learning environment, the server represents the destination for network requests."
);

        GameObject router = CreateRouter(
            new Vector3(0, 2.25f, 1)
        );

        RegisterDevice(
    router,
    "Router",
    "Network Router",
    "A router connects different networks and determines where network traffic should be forwarded."
);

        GameObject switchObj = CreateSwitch(
            new Vector3(0, 1.25f, -2)
        );

        RegisterDevice(
    switchObj,
    "Switch",
    "Network Switch",
    "A switch connects devices within a local network and forwards data to the appropriate connected device."
);

        GameObject pc1 = CreateComputer(
            "PC 1",
            new Vector3(-4.2f, 1.2f, -5)
        );

        RegisterDevice(
    pc1,
    "PC 1",
    "Client Computer",
    "A client computer is an end-user device that can send and receive information through the network."
);

        GameObject pc2 = CreateComputer(
            "PC 2",
            new Vector3(0, 1.2f, -5)
        );

        RegisterDevice(
    pc2,
    "PC 2",
    "Client Computer",
    "A client computer is an end-user device that can send and receive information through the network."
);

        GameObject pc3 = CreateComputer(
            "PC 3",
            new Vector3(4.2f, 1.2f, -5)
        );

        RegisterDevice(
    pc3,
    "PC 3",
    "Client Computer",
    "A client computer is an end-user device that can send and receive information through the network."
);

        // Network connections
        CreateConnection(server, router);
        CreateConnection(router, switchObj);

        CreateConnection(switchObj, pc1);
        CreateConnection(switchObj, pc2);
        CreateConnection(switchObj, pc3);
    }

    // ---------------------------------------------------------
    // SERVER
    // ---------------------------------------------------------

    GameObject CreateServer(Vector3 position)
    {
        GameObject server = CreateCube(
            "Server",
            position,
            new Vector3(2.1f, 2.8f, 1.4f),
            darkMaterial
        );

        // Server front panel
        CreateCube(
            "Server Panel",
            position + new Vector3(0, 0, -0.72f),
            new Vector3(1.5f, 2.1f, 0.04f),
            deviceMaterial
        );

        // Server lights
        for (int i = 0; i < 5; i++)
        {
            CreateCube(
                "Server LED " + i,
                position + new Vector3(
                    -0.55f,
                    0.75f - (i * 0.35f),
                    -0.76f
                ),
                new Vector3(0.08f, 0.08f, 0.03f),
                screenMaterial
            );
        }

        CreateLabel("SERVER", position + new Vector3(0, 1.8f, 0));

        return server;
    }

    // ---------------------------------------------------------
    // ROUTER
    // ---------------------------------------------------------

    GameObject CreateRouter(Vector3 position)
    {
        GameObject router = CreateCube(
            "Router",
            position,
            new Vector3(2.5f, 0.65f, 1.35f),
            deviceMaterial
        );

        // Top surface
        CreateCube(
            "Router Top",
            position + new Vector3(0, 0.35f, 0),
            new Vector3(2.2f, 0.08f, 1.1f),
            darkMaterial
        );

        // Antennas
        for (int i = -1; i <= 1; i++)
        {
            GameObject antenna = CreateCylinder(
                "Router Antenna",
                position + new Vector3(i * 0.7f, 0.8f, 0),
                new Vector3(0.06f, 0.5f, 0.06f),
                accentMaterial
            );

            antenna.transform.rotation =
                Quaternion.Euler(0, 0, i * -8f);
        }

        // Router status lights
        for (int i = 0; i < 5; i++)
        {
            CreateCube(
                "Router LED " + i,
                position + new Vector3(
                    -0.65f + (i * 0.3f),
                    -0.05f,
                    -0.7f
                ),
                new Vector3(0.1f, 0.1f, 0.03f),
                screenMaterial
            );
        }

        CreateLabel("ROUTER", position + new Vector3(0, 0.85f, 0));

        return router;
    }

    // ---------------------------------------------------------
    // SWITCH
    // ---------------------------------------------------------

    GameObject CreateSwitch(Vector3 position)
    {
        GameObject switchObj = CreateCube(
            "Switch",
            position,
            new Vector3(4.0f, 0.65f, 1.15f),
            darkMaterial
        );

        // Front panel
        CreateCube(
            "Switch Front Panel",
            position + new Vector3(0, 0, -0.6f),
            new Vector3(3.6f, 0.42f, 0.04f),
            deviceMaterial
        );

        // Network ports
        for (int i = 0; i < 12; i++)
        {
            CreateCube(
                "Switch Port " + i,
                position + new Vector3(
                    -1.55f + (i * 0.28f),
                    0,
                    -0.65f
                ),
                new Vector3(0.16f, 0.16f, 0.03f),
                accentMaterial
            );
        }

        CreateLabel("SWITCH", position + new Vector3(0, 0.7f, 0));

        return switchObj;
    }

    // ---------------------------------------------------------
    // COMPUTER
    // ---------------------------------------------------------

    GameObject CreateComputer(string computerName, Vector3 position)
    {
        GameObject parent = new GameObject(computerName);
        parent.transform.position = position;

        // Monitor
        GameObject monitor = CreateCube(
            computerName + " Monitor",
            position + new Vector3(0, 1.25f, 0),
            new Vector3(2.1f, 1.35f, 0.18f),
            darkMaterial
        );

        // Screen
        CreateCube(
            computerName + " Screen",
            position + new Vector3(0, 1.25f, -0.11f),
            new Vector3(1.75f, 1.05f, 0.03f),
            screenMaterial
        );

        // Stand
        CreateCube(
            computerName + " Stand",
            position + new Vector3(0, 0.4f, 0),
            new Vector3(0.25f, 0.55f, 0.25f),
            darkMaterial
        );

        // Base
        CreateCube(
            computerName + " Base",
            position + new Vector3(0, 0.08f, 0),
            new Vector3(1.1f, 0.12f, 0.65f),
            deviceMaterial
        );

        // CPU
        CreateCube(
            computerName + " CPU",
            position + new Vector3(1.35f, 0.65f, 0),
            new Vector3(0.6f, 1.25f, 0.8f),
            darkMaterial
        );

        // Keyboard
        CreateCube(
            computerName + " Keyboard",
            position + new Vector3(0, 0.2f, -0.8f),
            new Vector3(1.4f, 0.08f, 0.5f),
            deviceMaterial
        );

        CreateLabel(
            computerName,
            position + new Vector3(0, -0.1f, 0)
        );

        return parent;
    }

    // ---------------------------------------------------------
    // CONNECTIONS
    // ---------------------------------------------------------

    void CreateConnection(GameObject from, GameObject to)
    {
        GameObject cable = new GameObject(
            from.name + " -> " + to.name
        );

        LineRenderer line = cable.AddComponent<LineRenderer>();

        line.positionCount = 2;

        line.SetPosition(
            0,
            from.transform.position
        );

        line.SetPosition(
            1,
            to.transform.position
        );

        line.startWidth = 0.08f;
        line.endWidth = 0.08f;

        line.material = cableMaterial;

        // Make the connection visible from both directions.
        line.numCapVertices = 4;

        // Add a small light at the connection midpoint.
        Vector3 midpoint =
            (from.transform.position + to.transform.position) / 2f;

        CreatePointLight(
            midpoint,
            cyan,
            3f,
            3f
        );
    }

    // ---------------------------------------------------------
    // LIGHTING
    // ---------------------------------------------------------

    void SetupLighting()
    {
        RenderSettings.ambientIntensity = 0.25f;

        GameObject mainLight = GameObject.Find("Directional Light");

        if (mainLight != null)
        {
            Light light = mainLight.GetComponent<Light>();

            light.intensity = 0.7f;
            light.color = new Color(0.65f, 0.8f, 1f);

            mainLight.transform.rotation =
                Quaternion.Euler(50f, -30f, 0);
        }

        CreatePointLight(
            new Vector3(0, 5, 1),
            blue,
            7f,
            8f
        );

        CreatePointLight(
            new Vector3(-5, 2, -4),
            blue,
            4f,
            6f
        );

        CreatePointLight(
            new Vector3(5, 2, -4),
            blue,
            4f,
            6f
        );
    }

    GameObject CreatePointLight(
        Vector3 position,
        Color color,
        float intensity,
        float range
    )
    {
        GameObject lightObject = new GameObject("Network Light");

        lightObject.transform.position = position;

        Light light = lightObject.AddComponent<Light>();

        light.type = LightType.Point;
        light.color = color;
        light.intensity = intensity;
        light.range = range;

        return lightObject;
    }

    // ---------------------------------------------------------
    // CAMERA
    // ---------------------------------------------------------

    void SetupCamera()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
            return;

        mainCamera.transform.position =
            new Vector3(11, 8, -17);

        mainCamera.transform.LookAt(
            new Vector3(0, 1.8f, -0.5f)
        );

        mainCamera.fieldOfView = 48f;
    }

    // ---------------------------------------------------------
    // HELPERS
    // ---------------------------------------------------------

    GameObject CreateCube(
        string objectName,
        Vector3 position,
        Vector3 scale,
        Material material
    )
    {
        GameObject obj =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        obj.name = objectName;
        obj.transform.position = position;
        obj.transform.localScale = scale;

        if (material != null)
        {
            obj.GetComponent<Renderer>().material = material;
        }

        return obj;
    }

    GameObject CreateCylinder(
        string objectName,
        Vector3 position,
        Vector3 scale,
        Material material
    )
    {
        GameObject obj =
            GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        obj.name = objectName;
        obj.transform.position = position;
        obj.transform.localScale = scale;

        if (material != null)
        {
            obj.GetComponent<Renderer>().material = material;
        }

        return obj;
    }

    void CreateLabel(string text, Vector3 position)
    {
        GameObject label =
            new GameObject(text + " Label");

        label.transform.position = position;

        TextMesh textMesh =
            label.AddComponent<TextMesh>();

        textMesh.text = text;
        textMesh.fontSize = 48;
        textMesh.characterSize = 0.08f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = Color.white;

        // Face the main camera.
        if (Camera.main != null)
        {
            label.transform.LookAt(Camera.main.transform);
            label.transform.Rotate(0, 180, 0);
        }
    }
}