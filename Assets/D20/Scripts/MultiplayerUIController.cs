using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerUIController : MonoBehaviour
{
    public Button HostButton;
    public Button JoinButton;

    void Awake()
    {
        HostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        JoinButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
    }

    void Update()
    {
        
    }
}
