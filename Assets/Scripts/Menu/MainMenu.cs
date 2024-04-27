using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject landingPagePanel, onlinePage, lobbyParent;
    public static bool UsingSteamworks { get; private set; } = true;
    public static CSteamID LobbyId { get; private set; }
    Callback<LobbyCreated_t> LobbyCreatedCallback;
    Callback<GameLobbyJoinRequested_t> GameLobbyJoinRequestedCallback;
    Callback<LobbyEnter_t> LobbyEnterCallback;
    public void HostLobby()
    {
        if (UsingSteamworks)
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);
        }
        else
        {
            NetworkManager.singleton.StartHost();
        }
        
    }
    void OnLobbyCreated(LobbyCreated_t lobbyCreated)
    {
        if(!UsingSteamworks)
        {
            return;
        }
        if(lobbyCreated.m_eResult != EResult.k_EResultOK)
        {
            landingPagePanel.SetActive(true);
            return ;
        }
        LobbyId = new CSteamID(lobbyCreated.m_ulSteamIDLobby);
        SteamMatchmaking.SetLobbyData(LobbyId, "HostAddress", SteamUser.GetSteamID().ToString());
        NetworkManager.singleton.StartHost();
    }
    private void Start()
    {
        if (!UsingSteamworks)
        {
            return ;
        }
        LobbyCreatedCallback = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        GameLobbyJoinRequestedCallback = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        LobbyEnterCallback = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
    }
    private void OnDisable()
    {
        if (!UsingSteamworks) 
        {
            return;        
        }
        if(LobbyCreatedCallback == null)
        {
            return;
        }
        LobbyCreatedCallback.Dispose();
        GameLobbyJoinRequestedCallback.Dispose();
        LobbyEnterCallback.Dispose();
    }
    void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t gameLobbyJoinRequest)
    {
        if (!UsingSteamworks)
        {
            return;
        }
        SteamMatchmaking.JoinLobby(gameLobbyJoinRequest.m_steamIDLobby);
    }
    void OnLobbyEnter(LobbyEnter_t lobbyEnter) 
    {
        if (!UsingSteamworks)
        {
            return;
        }
        if(NetworkServer.active) { 
            return;
        }
        LobbyId = new CSteamID(lobbyEnter.m_ulSteamIDLobby);
        string hostAddress = SteamMatchmaking.GetLobbyData(LobbyId, "HostAddress");
        NetworkManager.singleton.networkAddress = hostAddress;
        NetworkManager.singleton.StartClient();
        landingPagePanel.SetActive(false);
        onlinePage.SetActive(false);
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            Application.Quit();
        #endif
    }
}
