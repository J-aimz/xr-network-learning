using UnityEngine;

public class DeviceInfo : MonoBehaviour
{
    public string deviceName;
    public string deviceType;
    [TextArea(3, 8)]
    public string description;

    public DeviceInfo(
        string name,
        string type,
        string info)
    {
        deviceName = name;
        deviceType = type;
        description = info;
    }
}