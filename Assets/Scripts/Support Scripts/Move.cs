using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public Vector2Int InitialPosition { get; private set; }
    public Vector2Int TargetPosition { get; private set; }
    public GameObject MovingPiece { get; private set; }

    public Move(Vector2Int initialPosition, Vector2Int targetPosition, GameObject movingPiece)
    {
        InitialPosition = initialPosition;
        TargetPosition = targetPosition;
        MovingPiece = movingPiece;
    }

    public Vector2Int PieceToCapture { get; set; } = new Vector2Int(-1, -1);

    public bool IsForcedMove
    {
        get { return PieceToCapture.x != -1; }
    }

    public List<Move> MovesAfterCapturing { get; } = new List<Move>();
}
