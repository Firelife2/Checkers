using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiecesHandler : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] protected Player player;
    [SerializeField] protected Transform piecesParent;
    public Transform PiecesParent { get { return piecesParent; } }

    [Header("Pieces prefabs")]
    [SerializeField] protected GameObject whitePiecePrefab;
    [SerializeField] protected GameObject blackPiecePrefab;
    protected GameObject instanceToSpawn;

    public static event Action OnPiecesSpawned;

    protected void HandleGameStarted()
    {
        if (player.IsWhite)
        {
            for (int z = 0; z < 3; z++)
                for (int x = (z == 1 ? 1 : 0); x < 8; x += 2)
                    Spawn(whitePiecePrefab, x, z);
            OnPiecesSpawned?.Invoke();
        }
        else
            for (int z = 0; z < 3; z++)
                for (int x = (z == 1 ? 0 : 1); x < 8; x += 2)
                    Spawn(blackPiecePrefab, x, z + 5);
    }

    protected virtual void Spawn(GameObject prefab, int xPos, int zPos)
    {
        var position = new Vector3(xPos, 0, zPos);
        instanceToSpawn = Instantiate(prefab, position,
            prefab.transform.rotation, piecesParent);
        var pieceInfo = instanceToSpawn.GetComponent<PieceInfo>();
        Board.Instance.BoardList[xPos][zPos] =
            (int)pieceInfo.MyColor + (int)PieceType.Man;
    }
}
