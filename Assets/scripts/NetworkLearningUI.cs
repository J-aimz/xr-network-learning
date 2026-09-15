using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NetworkLearningUI : MonoBehaviour
{
    private Canvas canvas;

    private GameObject infoPanel;
    private TMP_Text titleText;
    private TMP_Text typeText;
    private TMP_Text descriptionText;

    private DeviceInfo selectedDevice;

    private Camera mainCamera;

    private Color panelColor = new Color(0.015f, 0.035f, 0.075f, 0.96f);
    private Color accentColor = new Color(0.05f, 0.55f, 1f);
    private Color textColor = new Color(0.9f, 0.96f, 1f);

    void Start()
    {
        mainCamera = Camera.main;

        CreateEventSystem();
        CreateCanvas();
        CreateUI();

        HideInformationPanel();
    }

    void Update()
    {
        HandleDeviceSelection();
    }

    // =========================================================
    // CANVAS
    // =========================================================

    void CreateCanvas()
    {
        GameObject canvasObject = new GameObject("Learning UI");

        canvas = canvasObject.AddComponent<Canvas>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();
    }

   
    void CreateUI()
    {
        // =====================================================
        // HEADER
        // =====================================================

        TMP_Text title = CreateAnchoredText(
            "NETWORK LEARNING",
            new Vector2(40, -30),
            new Vector2(500, 55),
            30
        );

        title.fontStyle = FontStyles.Bold;
        title.color = textColor;

        TMP_Text subtitle = CreateAnchoredText(
            "Interactive Computer Network Environment",
            new Vector2(40, -72),
            new Vector2(600, 35),
            15
        );

        subtitle.color =
            new Color(0.55f, 0.75f, 0.95f);

        // =====================================================
        // INSTRUCTION
        // =====================================================

        GameObject instructionPanel =
            CreateAnchoredPanel(
                "Instruction Panel",
                new Vector2(40, 35),
                new Vector2(320, 70),
                new Vector2(0, 0),
                new Vector2(0, 0)
            );

        TMP_Text instruction = CreateAnchoredText(
            "SELECT A NETWORK COMPONENT\nTO LEARN MORE",
            Vector2.zero,
            new Vector2(290, 60),
            14,
            instructionPanel.transform
        );

        instruction.alignment =
            TextAlignmentOptions.Center;

        instruction.color = textColor;

        // =====================================================
        // INFORMATION PANEL
        // =====================================================

        infoPanel = CreateAnchoredPanel(
            "Information Panel",
            new Vector2(-35, 0),
            new Vector2(370, 380),
            new Vector2(1, 0.5f),
            new Vector2(1, 0.5f)
        );

        titleText = CreateAnchoredText(
            "DEVICE",
            new Vector2(25, -30),
            new Vector2(320, 45),
            27,
            infoPanel.transform
        );

        titleText.fontStyle =
            FontStyles.Bold;

        titleText.color =
            accentColor;

        typeText = CreateAnchoredText(
            "NETWORK DEVICE",
            new Vector2(25, -75),
            new Vector2(320, 30),
            14,
            infoPanel.transform
        );

        typeText.color =
            new Color(0.5f, 0.75f, 0.95f);

        descriptionText = CreateAnchoredText(
            "",
            new Vector2(25, -115),
            new Vector2(320, 180),
            16,
            infoPanel.transform
        );

        descriptionText.color = textColor;

        descriptionText.enableWordWrapping = true;

        descriptionText.alignment =
            TextAlignmentOptions.TopLeft;

        // =====================================================
        // CLOSE BUTTON
        // =====================================================

        GameObject closeButton =
            CreateAnchoredButton(
                "CLOSE",
                new Vector2(25, 25),
                new Vector2(140, 42),
                infoPanel.transform
            );

        closeButton
            .GetComponent<Button>()
            .onClick.AddListener(
                HideInformationPanel
            );

        // =====================================================
        // DATA FLOW BUTTON
        // =====================================================

        GameObject dataFlowButton =
            CreateAnchoredButton(
                "▶  SHOW DATA FLOW",
                new Vector2(-35, 35),
                new Vector2(220, 50)
            );

        RectTransform flowRect =
            dataFlowButton.GetComponent<RectTransform>();

        flowRect.anchorMin =
            new Vector2(1, 0);

        flowRect.anchorMax =
            new Vector2(1, 0);

        flowRect.pivot =
            new Vector2(1, 0);

        dataFlowButton
            .GetComponent<Button>()
            .onClick.AddListener(
                ShowDataFlow
            );
    }

    TMP_Text CreateAnchoredText(
    string text,
    Vector2 position,
    Vector2 size,
    float fontSize,
    Transform parent = null)
    {
        GameObject textObject =
            new GameObject("Text");

        textObject.transform.SetParent(
            parent != null
                ? parent
                : canvas.transform,
            false
        );

        RectTransform rect =
            textObject.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0, 1);

        rect.anchorMax =
            new Vector2(0, 1);

        rect.pivot =
            new Vector2(0, 1);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            size;

        TMP_Text tmp =
            textObject.AddComponent<TextMeshProUGUI>();

        tmp.text = text;
        tmp.fontSize = fontSize;

        return tmp;
    }

    GameObject CreateAnchoredPanel(
    string objectName,
    Vector2 position,
    Vector2 size,
    Vector2 anchorMin,
    Vector2 anchorMax)
    {
        GameObject panel =
            new GameObject(objectName);

        panel.transform.SetParent(
            canvas.transform,
            false
        );

        RectTransform rect =
            panel.AddComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;

        if (anchorMin == new Vector2(1, 0.5f))
        {
            rect.pivot =
                new Vector2(1, 0.5f);
        }
        else
        {
            rect.pivot =
                new Vector2(0, 0);
        }

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            size;

        Image image =
            panel.AddComponent<Image>();

        image.color =
            panelColor;

        return panel;
    }


    GameObject CreateAnchoredButton(
    string buttonText,
    Vector2 position,
    Vector2 size,
    Transform parent = null)
    {
        GameObject buttonObject =
            new GameObject(buttonText);

        buttonObject.transform.SetParent(
            parent != null
                ? parent
                : canvas.transform,
            false
        );

        RectTransform rect =
            buttonObject.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0, 0);

        rect.anchorMax =
            new Vector2(0, 0);

        rect.pivot =
            new Vector2(0, 0);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            size;

        Image image =
            buttonObject.AddComponent<Image>();

        image.color =
            accentColor;

        Button button =
            buttonObject.AddComponent<Button>();

        GameObject textObject =
            new GameObject("Text");

        textObject.transform.SetParent(
            buttonObject.transform,
            false
        );

        RectTransform textRect =
            textObject.AddComponent<RectTransform>();

        textRect.anchorMin =
            Vector2.zero;

        textRect.anchorMax =
            Vector2.one;

        textRect.offsetMin =
            Vector2.zero;

        textRect.offsetMax =
            Vector2.zero;

        TMP_Text text =
            textObject.AddComponent<TextMeshProUGUI>();

        text.text = buttonText;
        text.fontSize = 14;
        text.fontStyle =
            FontStyles.Bold;

        text.alignment =
            TextAlignmentOptions.Center;

        text.color =
            Color.white;

        return buttonObject;
    }

    // =========================================================
    // DEVICE SELECTION
    // =========================================================

    void HandleDeviceSelection()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (mainCamera == null)
            mainCamera = Camera.main;

        Ray ray =
            mainCamera.ScreenPointToRay(
                Input.mousePosition
            );

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            DeviceInfo device =
                hit.collider.GetComponentInParent<DeviceInfo>();

            if (device != null)
            {
                ShowInformation(device);
            }
        }
    }

    void ShowInformation(DeviceInfo device)
    {
        selectedDevice = device;

        infoPanel.SetActive(true);

        titleText.text =
            device.deviceName.ToUpper();

        typeText.text =
            device.deviceType.ToUpper();

        descriptionText.text =
            device.description;
    }

    void HideInformationPanel()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);

        selectedDevice = null;
    }

    // =========================================================
    // DATA FLOW
    // =========================================================

    void ShowDataFlow()
    {
        DataFlowController flowController =
            FindFirstObjectByType<DataFlowController>();

        if (flowController == null)
        {
            Debug.LogError(
                "DataFlowController was not found in the scene."
            );
            return;
        }

        flowController.StartFlow();
    }



    // =========================================================
    // EVENT SYSTEM
    // =========================================================

    void CreateEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null)
            return;

        GameObject eventSystem =
            new GameObject("EventSystem");

        eventSystem.AddComponent<EventSystem>();

        eventSystem.AddComponent<
            StandaloneInputModule
        >();
    }
}