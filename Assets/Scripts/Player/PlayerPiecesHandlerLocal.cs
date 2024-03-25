using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiecesHandlerLocal : PlayerPiecesHandler
{
    void Start()
    {
        LocalGameManager.OnGameStarted += HandleGameStarted;
    }

    void OnDestroy()
    {
        LocalGameManager.OnGameStarted -= HandleGameStarted;
    }
}
