using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceLocal : MonoBehaviour
{
    void Start()
    {
        Board.Instance.OnPieceCaptured += HandlePieceCaptured;
    }

    void OnDestroy()
    {
        Board.Instance.OnPieceCaptured -= HandlePieceCaptured;
    }

    void HandlePieceCaptured(Vector3 capturedPiecePosition)
    {
        if (capturedPiecePosition != transform.position) return;
        Destroy(gameObject);
    }
}
