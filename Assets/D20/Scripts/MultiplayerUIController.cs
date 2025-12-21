using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MultiplayerUIController : MonoBehaviour
{
    public Button HostButton;
    public Button JoinButton;

    public TMP_InputField PortInput;
    public TMP_InputField IPInput;

    public List<UnityAction> HostButtonCallbackList = new();
    public List<UnityAction> JoinButtonCallbackList = new();

    public void HostButton_Callback ()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", (ushort)int.Parse(PortInput.text), "0.0.0.0");
        NetworkManager.Singleton.StartHost();

        HostButtonCallbackList.ForEach(m => m());
    }

    public void JoinButton_Callback()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(IPInput.text, (ushort)int.Parse(PortInput.text));
        NetworkManager.Singleton.StartClient();

        JoinButtonCallbackList.ForEach(m => m());
    }
}
