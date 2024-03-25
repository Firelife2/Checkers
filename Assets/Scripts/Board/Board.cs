using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : NetworkBehaviour
{
    public virtual IList<int[]> BoardList { get; }

    #pragma warning disable 0067
    public virtual event Action<Vector3> OnPieceCaptured;
    #pragma warning restore 0067
    // Singleton - Single Object(We created it once and it's always in the Game.)
    #region Singleton 
    public static Board Instance { get; private set; }
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

    protected void FillBoardList(IList<int[]> lst)
    {
        for (int i = 0; i < 8; i++)
            lst.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0 });
    }

    protected void PromotePieceOnBoard(IList<int[]> lst, int x, int z)
    {
        lst[x][z] += (int)PieceType.King - (int)PieceType.Man;
    }

    protected void MoveOnBoard(IList<int[]> lst, Vector2Int oldPosition, Vector2Int newPosition)
    {
        var piece = lst[oldPosition.x][oldPosition.y];
        lst[newPosition.x][newPosition.y] = piece;
        lst[oldPosition.x][oldPosition.y] = 0;
    }

    protected void Capture(IList<int[]> lst, Vector2Int piecePosition)
    {
        lst[piecePosition.x][piecePosition.y] = 0;
    }

    public virtual void CaptureOnBoard(Vector2Int piecePosition) { }
    public virtual void MoveOnBoard(Vector2Int oldPosition, 
        Vector2Int newPosition, bool nextTurn) { }
}
