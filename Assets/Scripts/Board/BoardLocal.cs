using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLocal : Board
{
    public readonly List<int[]> boardList = new List<int[]>();
    public override IList<int[]> BoardList 
    { 
        get => boardList;
    }

    public override event Action<Vector3> OnPieceCaptured;

    void Start()
    {
        FillBoardList(boardList);
        PieceMovementHandlerLocal.OnPieceReachedBackline += TryPromotePieceOnBoard;
    }

    void OnDestroy()
    {
        PieceMovementHandlerLocal.OnPieceReachedBackline -= TryPromotePieceOnBoard;
    }

    bool TryPromotePieceOnBoard(PiecePromotionHandler promotedPiece, int x, int z)
    {
        PromotePieceOnBoard(boardList, x, z);
        return true;
    }

    public override void MoveOnBoard(Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        MoveOnBoard(boardList, oldPosition, newPosition);
        if (nextTurn)
            StartCoroutine(NextTurn());
    }

    public override void CaptureOnBoard(Vector2Int piecePosition)
    {
        Capture(boardList, piecePosition);
        OnPieceCaptured?.Invoke(new Vector3(piecePosition.x, 0, piecePosition.y));
    }

    IEnumerator NextTurn()
    {
        yield return null;
        TurnsHandler.Instance.NextTurn();
    }
}
