using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] GameObject onlinePage;
    [SerializeField] InputField addressInput;

    private void Start()
    {
        CheckersNetworkManager.ClientConnect += ConnectClient;
    }
    private void OnDestroy()
    {
        CheckersNetworkManager.ClientConnect -= ConnectClient;
    }
    public void Join()
    {
        NetworkManager.singleton.networkAddress = addressInput.text;
        NetworkManager.singleton.StartClient();
    }
    void ConnectClient()
    {
        onlinePage.SetActive(false);
        gameObject.SetActive(false);
    }
}
