using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerUIController : MonoBehaviour
{
    public Button HostButton;
    public Button JoinButton;

    public TMP_InputField PortInput;
    public TMP_InputField IPInput;

    void Awake()
    {
        HostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", (ushort)int.Parse(PortInput.text), "0.0.0.0");
            NetworkManager.Singleton.StartHost();
        });

        JoinButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(IPInput.text, (ushort)int.Parse(PortInput.text));
            NetworkManager.Singleton.StartClient();
        });
    }

    void Update()
    {
        
    }
}
