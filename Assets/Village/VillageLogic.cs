using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static UnityEngine.UI.Button;

public class VillageLogic : MonoBehaviour
{
    public MultiplayerUIController muic;
    public Camera LobbyCamera;
    public Camera PlayerCamera;

    private void Awake()
    {
        muic.HostButtonCallbackList.Add(SetUp);
        muic.JoinButtonCallbackList.Add(SetUp);
    }

    public void SetUp()
    {
        LobbyCamera.gameObject.SetActive(false);
        PlayerCamera.gameObject.SetActive(true);
        PlayerCamera.tag = "MainCamera";
    }
}
