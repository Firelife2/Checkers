using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerNetwork - Class Of Our Player
public class PlayerNetwork : Player
{
    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))] string displayName; //Hook - When Displayed Name Changed, Hook Functions Runs
    [SyncVar(hook = nameof(HandleLobbyOwnerChange))] bool lobbyOwner;
    public static event Action ClientOnInfoUpdated;
    public static event Action<bool> OnHostConnect;
    public string DisplayName
    {
        get
        {
            return displayName;
        }
        [Server] //Server - Only Works On Server(Able To Change)
        set
        {
            displayName = value;
        }

    }
    public bool LobbyOwner
    {
        get
        {
            return lobbyOwner;
        }
        [Server]
        set
        {
            lobbyOwner = value;
        }
    }
    void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName) //Hook Function - We Invoke The Event
    {
        ClientOnInfoUpdated?.Invoke();
    }
    void HandleLobbyOwnerChange(bool oldLobbyOwner, bool newLobbyOwner)
    {
        if (!hasAuthority) //If Has Authority(Will Be True For Host)
        {
            return;
        }
        else
        {
            OnHostConnect?.Invoke(newLobbyOwner);
        }
    }
    public override void OnStartClient() //OnStartClient - Runs When Client Joined(Also Applys To Host)
    {
        if (!isClientOnly) //If Host Joined Do Nothing
        {
            return;
        }
        else
        {
            ((CheckersNetworkManager)NetworkManager.singleton).NetworkPlayers.Add(this); // Adds Client To Network Players
        }
    }
    public override void OnStopClient()
    {
        if (!isClientOnly)
        {
            ((CheckersNetworkManager)NetworkManager.singleton).NetworkPlayers.Remove(this); // OnStopClient, It Checks If It's Host And Removes Him When he disconnects.
        }
        ClientOnInfoUpdated?.Invoke();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    [Command]
    public void CmdNextTurn()
    {
        TurnsHandler.Instance.NextTurn();
    }
}
