using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementHandler : NetworkBehaviour
{
    [SerializeField] protected PieceInfo pieceInfo;
    [SerializeField] protected PieceSelectionHandler selectionHandler;
    [SerializeField] protected PiecePromotionHandler promotionHandler;
    [SerializeField] protected PieceAudioHandler audioHandler;

    public static event Action<bool> OnPieceMoved;
    public static event Action<bool> OnLockSelectedPiece;

    protected void HandleTileSelected(TileSelectionHandler tile)
    {
        if (!selectionHandler.IsSelected) return;
        if (!tile.HaveToCapture)
        {
            OnPieceMoved?.Invoke(true);
            Move(tile.transform.position, true);
            return;
        }
        Capture(tile.MoveInfo.PieceToCapture);
        if (tile.MoveInfo.MovesAfterCapturing.Count == 0)
        {
            OnPieceMoved?.Invoke(true);
            OnLockSelectedPiece?.Invoke(false);
            Move(tile.transform.position, true);
            return;
        }

        OnPieceMoved?.Invoke(false);
        Move(tile.transform.position, false);
        OnLockSelectedPiece?.Invoke(true);
        selectionHandler.HighlightTiles(tile.MoveInfo.MovesAfterCapturing);
    }

    protected virtual void Move(Vector3 position, bool nextTurn)
    {
        Vector2Int oldPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z));
        Vector2Int newPosition = new Vector2Int(
            Mathf.RoundToInt(position.x),
            Mathf.RoundToInt(position.z));
        Board.Instance.MoveOnBoard(oldPosition, newPosition, nextTurn);
        transform.position = new Vector3(
            position.x,
            transform.position.y,
            position.z);
        if (((position.z == 7 && pieceInfo.MyColor == PieceColor.White)
            || (position.z == 0 && pieceInfo.MyColor == PieceColor.Black))
            && pieceInfo.MyType == PieceType.Man)
            ReachedBackline(newPosition);
        PlayAudio();
    }

    protected virtual void Capture(Vector2Int piecePosition)
    {
        Board.Instance.CaptureOnBoard(piecePosition);
    }

    protected virtual void PlayAudio()
    {
        audioHandler.PlayMoveSound();
    }

    protected virtual void ReachedBackline(Vector2Int newPosition) { }
}
