using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNetwork : Board
{
    readonly SyncList<int[]> boardList = new SyncList<int[]>();
    public override IList<int[]> BoardList { get { return boardList; } }
    public override event Action<Vector3> OnPieceCaptured;
    public override void OnStartServer()
    {
        FillBoardList(boardList);

    }

    [Server]
    public override void MoveOnBoard(Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        base.MoveOnBoard(oldPosition, newPosition, nextTurn);
        RpcMoveOnBoard(oldPosition, newPosition, nextTurn);
    }

    [ClientRpc]
    void RpcMoveOnBoard(Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        if (NetworkServer.active)
        {
            return;
        }
        else
        {
            MoveOnBoard(oldPosition, newPosition, nextTurn);
            if (nextTurn)
            {
                NetworkClient.connection.identity.GetComponent<PlayerNetwork>().CmdNextTurn();
            }
        }
    }
    [Server]public override void CaptureOnBoard(Vector2Int piecePosition)
    {
        Capture(boardList, piecePosition);
        RpcCaptureOnBoard(piecePosition);
        OnPieceCaptured?.Invoke(new Vector3(piecePosition.x, 0, piecePosition.y));
    }
    [ClientRpc]
    void RpcCaptureOnBoard(Vector2Int piecePosition)
    {
        Capture(boardList, piecePosition);
    }
}
