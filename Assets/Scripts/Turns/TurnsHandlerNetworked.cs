using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsHandlerNetworked : TurnsHandler
{
    protected override void FillMovesList()
    {
        base.FillMovesList();
        RpcGenarateMoves(piecesHandler);
    }
    [ClientRpc]
    void RpcGenarateMoves(PlayerPiecesHandler piecesHandler)
    {
        if (NetworkServer.active)
        {
            return;
        }
        else
        {
            GenerateMoves(piecesHandler.PiecesParent);
        }
    }   
    public override void OnStartServer()
    {
        PlayerPiecesHandler.OnPiecesSpawned += NextTurn;
        Players = ((CheckersNetworkManager)NetworkManager.singleton).Players;
    }

    public override void OnStopServer()
    {
        PlayerPiecesHandler.OnPiecesSpawned -= NextTurn;
    }

}
