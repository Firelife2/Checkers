using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PieceSelectionHandler : MonoBehaviour
{
    [SerializeField] UnityEvent onSelected, onDeselected;
    [SerializeField] PieceNetwork pieceNet;
    public PieceNetwork PieceNet { get { return pieceNet; } }

    public bool IsSelected { get; private set; }

    public static event Action<int, int, Move> OnTileHighlight;
    public static event Action<int, int, Move> OnTileUnhighlight;

    void Start()
    {
        PiecesSelectionHandler.OnPieceSelected += Select;
        PiecesSelectionHandler.OnPieceDeselected += Deselect;
    }

    void OnDestroy()
    {
        PiecesSelectionHandler.OnPieceSelected -= Select;
        PiecesSelectionHandler.OnPieceDeselected -= Deselect;
    }

    void Select(PieceSelectionHandler selectedPiece)
    {
        if (selectedPiece != this) return;
        IsSelected = true;
        onSelected.Invoke();
        HighlightOrUnhighlightTiles(HighlightTile);
    }

    void Deselect(PieceSelectionHandler deselectedPiece)
    {
        if (deselectedPiece != this) return;
        IsSelected = false;
        onDeselected?.Invoke();
        HighlightOrUnhighlightTiles(UnhighlightTile);
    }

    void HighlightOrUnhighlightTiles(Action<int, int, Move> action)
    {
        List<Move> moves = GetMoves();
        foreach (Move move in moves)
            action(move.TargetPosition[0], move.TargetPosition[1], move);
    }

    List<Move> GetMoves()
    {
        var moves = new List<Move>();
        foreach (var move in TurnsHandler.Instance.Moves)
            if (move.MovingPiece == gameObject)
                moves.Add(move);
        return moves;
    }

    void HighlightTile(int x, int y, Move move)
    {
        OnTileHighlight?.Invoke(x, y, move);
    }

    public void HighlightTiles(List<Move> moves)
    {
        foreach (Move move in moves)
            HighlightTile(move.TargetPosition[0], move.TargetPosition[1], move);
    }

    void UnhighlightTile(int x, int y, Move move)
    {
        OnTileUnhighlight?.Invoke(x, y, move);
    }
}
