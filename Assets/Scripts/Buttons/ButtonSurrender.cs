using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ButtonSurrender : MonoBehaviour
{
    public void Surrender()
    {
        if (LocalGameManager.Instance)
        {
            TurnsHandler.Instance.Surrender();
        }
        else
        {
            
        }
    }
}
