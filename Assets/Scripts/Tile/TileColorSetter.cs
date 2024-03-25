using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TileColorSetter : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material white, black, highlightWhite, highlightBlack;
    [SerializeField] TileSelectionHandler tileSelectionHandler;

    void Start()
    {
        PieceSelectionHandler.OnTileHighlight += ClientHandleTileHighlight;
        PieceSelectionHandler.OnTileUnhighlight += ClientHandleTileUnhighlight;
        PieceMovementHandler.OnPieceMoved += ClientHandlePieceMoved;
    }

    void OnDestroy()
    {
        PieceSelectionHandler.OnTileHighlight -= ClientHandleTileHighlight;
        PieceSelectionHandler.OnTileUnhighlight -= ClientHandleTileUnhighlight;
        PieceMovementHandler.OnPieceMoved -= ClientHandlePieceMoved;
    }

    void Update()
    {
        if (Application.isPlaying) return;
        Paint(white, black);
    }

    void ClientHandleTileHighlight(int x, int z, Move move)
    {
        if (Mathf.RoundToInt(transform.position.x) != x
            || Mathf.RoundToInt(transform.position.z) != z) return;
        Paint(highlightWhite, highlightBlack);
        tileSelectionHandler.IsHighlighted = true;
        tileSelectionHandler.MoveInfo = move;
    }

    void ClientHandleTileUnhighlight(int x, int z, Move move)
    {
        if (Mathf.RoundToInt(transform.position.x) != x
            || Mathf.RoundToInt(transform.position.z) != z) return;
        Paint(white, black);
        tileSelectionHandler.IsHighlighted = false;
        tileSelectionHandler.MoveInfo = move;
    }

    void ClientHandlePieceMoved(bool moveFinished)
    {
        if (!tileSelectionHandler.IsHighlighted) return;
        Paint(white, black);
        tileSelectionHandler.IsHighlighted = false;
    }

    void Paint(Material white, Material black)
    {
        if (IsWhite()) meshRenderer.material = white;
        else meshRenderer.material = black;
    }

    bool IsWhite()
    {
        if ((transform.position.x + transform.position.z) % 2 == 0)
            return false;
        return true;
    }
}
