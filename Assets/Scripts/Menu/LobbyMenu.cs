using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] Button startGameButton;
    [SerializeField] Text[] playerNameTexts = new Text[2];

    public void StartGame()
    {
        NetworkManager.singleton.ServerChangeScene("Game Scene");
    }
    private void Start()
    {
        PlayerNetwork.ClientOnInfoUpdated += ClientHandleInfoUpdated;
        PlayerNetwork.OnHostConnect += HandleOnHostConnect;
    }
    private void OnDestroy()
    {
        PlayerNetwork.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
        PlayerNetwork.OnHostConnect -= HandleOnHostConnect;
    }

    private void HandleOnHostConnect(bool newLobbyOwner)
    {
        startGameButton.gameObject.SetActive(newLobbyOwner);

    }

    void ClientHandleInfoUpdated()
    {

        List<PlayerNetwork> player = ((CheckersNetworkManager) NetworkManager.singleton).NetworkPlayers;
        for (int i = 0; i < player.Count; i++)
        {
            playerNameTexts[i].text = player[i].DisplayName;
        }
        for(int i = player.Count; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player";
        }
        startGameButton.interactable = player.Count > 1;
    }
}
