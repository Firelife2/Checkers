using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class GameOverHandlerNetworked : GameOverHandler
{
    [ClientRpc]
    void RpcGameOver(string win)
    {
        CallGameOver(win);
    }
    [ServerCallback]
    void HandleGameOver(string win)
    {
        RpcGameOver(win);
    }
    public override void OnStartServer()
    {
        TurnsHandler.Instance.OnGameOver += HandleGameOver;
    }
    public override void OnStopServer()
    {
        TurnsHandler.Instance.OnGameOver -= HandleGameOver;
    }
}
