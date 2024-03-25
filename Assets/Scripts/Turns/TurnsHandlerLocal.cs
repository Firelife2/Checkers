using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsHandlerLocal : TurnsHandler
{
    void Start()
    {
        PlayerPiecesHandler.OnPiecesSpawned += NextTurn;
        Players = LocalGameManager.Instance.Players;
    }

    void OnDestroy()
    {
        PlayerPiecesHandler.OnPiecesSpawned -= NextTurn;
    }
}
