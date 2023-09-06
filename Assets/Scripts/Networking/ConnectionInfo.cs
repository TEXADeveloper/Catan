using UnityEngine;
using TMPro;

public class ConnectionInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text gameVersion;
    [SerializeField] private TMP_Text connectionState;

    void Awake()
    {
        gameVersion.text = "Version: " + Application.version;
        ConnectionManager.ConnectionState += changeConnectionState;
    }

    void changeConnectionState(string value, Color color)
    {
        connectionState.color = color;
        connectionState.text = value;
    }

    void OnDestroy()
    {
        ConnectionManager.ConnectionState -= changeConnectionState;
    }
}
