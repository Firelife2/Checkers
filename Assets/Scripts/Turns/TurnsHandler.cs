using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsHandler : NetworkBehaviour
{
    protected const string DRAW = "Stalemate!";
    protected const string WHITE_WIN = "White Wins!";
    protected const string BLACK_WIN = "Black Wins!";

    public bool WhiteTurn { get; protected set; }
    public List<Move> Moves { get; protected set; } = new List<Move>();
    protected List<Player> Players { get; set; } = new List<Player>();
    protected PlayerPiecesHandler piecesHandler = null;

    public event Action<string> OnGameOver;
    public event Action OnMovesGenerated;

    #region Singleton
    public static TurnsHandler Instance { get; private set; }
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

    public void Surrender()
    {
        OnGameOver?.Invoke(WhiteTurn ? BLACK_WIN : WHITE_WIN);
    }

    public void Surrender(string result)
    {
        OnGameOver?.Invoke(result);
    }

    public void NextTurn()
    {
        FillMovesList();
        CheckVictory();
    }

    protected virtual void FillMovesList()
    {
        WhiteTurn = !WhiteTurn;
        foreach (var player in Players)
            if (player.IsWhite == WhiteTurn)
                piecesHandler = player.GetComponent<PlayerPiecesHandler>();
        GenerateMoves(piecesHandler.PiecesParent);
    }

    void CheckVictory()
    {
        if (Moves.Count != 0) return;
        FillMovesList();
        if (Moves.Count == 0)
        {
            OnGameOver?.Invoke(DRAW);
        }
        else if (this is TurnsHandlerLocal)
        {
            OnGameOver?.Invoke(WhiteTurn ? WHITE_WIN : BLACK_WIN);
        }
    }

    protected void GenerateMoves(Transform playerPiecesParent)
    {
        List<Move> moves = new List<Move>();
        List<Move> forcedMoves = new List<Move>();
        for (int i = 0; i < playerPiecesParent.childCount; i++)
        {
            var piece = playerPiecesParent.GetChild(i).gameObject;
            var pieceMovesGenerator = piece.GetComponent<PieceMovesGenerator>();
            var pieceMoves = pieceMovesGenerator.GenerateMoves(new Vector2Int(
                Mathf.RoundToInt(piece.transform.position.x),
                Mathf.RoundToInt(piece.transform.position.z)),
                new List<Vector2Int>());
            if (pieceMoves.Count > 0)
            {
                List<Move> listToFill = moves;
                if (pieceMoves[0].IsForcedMove)
                    listToFill = forcedMoves;
                foreach (var move in pieceMoves)
                    listToFill.Add(move);
            }
        }
        if (forcedMoves.Count > 0)
            Moves = forcedMoves;
        else
            Moves = moves;
        OnMovesGenerated?.Invoke();
    }
}
