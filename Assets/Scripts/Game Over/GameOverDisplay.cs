using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text gameResultText;

    void Start()
    {
        GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
    }

    void OnDestroy()
    {
        GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;
    }

    void ClientHandleGameOver(string result)
    {
        gameOverPanel.SetActive(true);
        gameResultText.text = result;
    }
}
