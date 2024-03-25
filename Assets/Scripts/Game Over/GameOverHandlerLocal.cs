using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHandlerLocal : GameOverHandler
{
    void Start()
    {
        TurnsHandler.Instance.OnGameOver += CallGameOver;
    }

    void OnDestroy()
    {
        TurnsHandler.Instance.OnGameOver -= CallGameOver;
    }
}
