using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalGameManager : MonoBehaviour
{
    #region Singleton
    public static LocalGameManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }
    #endregion

    [SerializeField] GameObject playerPrefab,
        gameOverHandlerPrefab, boardPrefab, turnsHandlerPrefab;
    public List<Player> Players { get; } = new List<Player>();

    public static event Action OnGameStarted;

    IEnumerator Start()
    {
        SceneManager.LoadScene("Game Scene");
        Instantiate(boardPrefab);
        Instantiate(turnsHandlerPrefab);
        while (SceneManager.GetActiveScene().buildIndex == 0)
            yield return null;
        Instantiate(gameOverHandlerPrefab);
        var whitePlayer = Instantiate(playerPrefab).GetComponent<Player>();
        whitePlayer.IsWhite = true;
        Players.Add(whitePlayer);
        var blackPlayer = Instantiate(playerPrefab).GetComponent<Player>();
        Players.Add(blackPlayer);
        yield return null;
        OnGameStarted?.Invoke();
    }
}
