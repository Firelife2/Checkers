using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckersNetworkManager : NetworkManager
{
    [SerializeField] GameObject gameOverHandlerPrefab, boardPrefab, 
        turnsHandlerPrefab;
    public List<PlayerNetwork> NetworkPlayers { get; } = new List<PlayerNetwork>();
    public static event Action ClientConnect;
    public static event Action ServerGameStarted;
    public List<Player> Players { get; } = new List<Player>();

    public override void OnStartServer()
    {
        GameObject board = Instantiate(boardPrefab);
        GameObject turnsHandler = Instantiate(turnsHandlerPrefab);
        
        NetworkServer.Spawn(board);
        NetworkServer.Spawn(turnsHandler);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(sceneName == "Game Scene")
        {
            GameObject gameOverHandler = Instantiate(gameOverHandlerPrefab);
            ServerGameStarted?.Invoke();
            NetworkServer.Spawn(gameOverHandler);
        }
    }
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientConnect?.Invoke();
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject playerInstanse = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, playerInstanse);
        PlayerNetwork player = playerInstanse.GetComponent<PlayerNetwork>();
        NetworkPlayers.Add(player);
        Players.Add(player);
        player.IsWhite = numPlayers == 1;
        player.LobbyOwner = player.IsWhite;
        player.DisplayName = player.IsWhite ? "White" : "Black"; //TODO: Improve
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        PlayerNetwork player = conn.identity.GetComponent<PlayerNetwork>();
        NetworkPlayers.Remove(player);
        Players.Remove(player);
        base.OnServerDisconnect(conn);
    }
    public override void OnStopServer()
    {
        NetworkPlayers.Clear();
        Players.Clear();
    }
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        SceneManager.LoadScene("Lobby Scene");
        Destroy(gameObject);
    }
}
