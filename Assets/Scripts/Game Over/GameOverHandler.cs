using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHandler : NetworkBehaviour
{
    public static event Action<string> ClientOnGameOver;

    protected void CallGameOver(string result)
    {
        ClientOnGameOver?.Invoke(result);
    }
}
