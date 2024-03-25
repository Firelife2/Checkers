using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovesGenerator : MonoBehaviour
{
    [SerializeField] PieceInfo piece;

    public List<Move> GenerateMoves(Vector2Int startPosition,
        List<Vector2Int> visited, bool freeMovement = true)
    {
        if (!OnBoard(startPosition)) return new List<Move>();
        List<Move> moves = new List<Move>();

        TryDirection(startPosition, Direction.upLeft, visited, freeMovement, moves);
        TryDirection(startPosition, Direction.upRight, visited, freeMovement, moves);
        TryDirection(startPosition, Direction.downLeft, visited,
            piece.MyType == PieceType.Man ? false : freeMovement, moves);
        TryDirection(startPosition, Direction.downRight, visited,
            piece.MyType == PieceType.Man ? false : freeMovement, moves);

        List<Move> forcedMoves = new List<Move>();
        foreach (Move move in moves)
            if (move.IsForcedMove) forcedMoves.Add(move);
        if (forcedMoves.Count > 0)
        {
            return forcedMoves;
        }
        return moves;
    }


    void TryDirection(Vector2Int startPosition, Vector2Int direction,
        List<Vector2Int> visited, bool freeMovement, List<Move> moves)
    {
        var target = GetTargetPosition(startPosition, direction);
        if (!visited.Contains(target) && OnBoard(target))
            if (TryMove(startPosition, direction, target, visited, freeMovement, moves)
                && piece.MyType == PieceType.King)
                TryDirection(target, direction, visited, freeMovement, moves);
    }

    Vector2Int GetTargetPosition(Vector2Int startPosition, Vector2Int direction, int step = 1)
    {
        return new Vector2Int
            (startPosition.x + direction.x * step,
            startPosition.y + (piece.MyColor == PieceColor.White ? 1 : -1) * direction.y * step);
    }

    bool TryMove(Vector2Int startPos, Vector2Int direction, Vector2Int targetPos,
        List<Vector2Int> visited, bool freeMovement, List<Move> moves)
    {
        var move = new Move(startPos, targetPos, gameObject);
        if (IsOccupiedBy(targetPos) == PieceColor.None)
        {
            if (freeMovement)
            {
                moves.Add(move);
                return true;
            }
        }
        else if (IsOccupiedBy(targetPos) != piece.MyColor)
        {
            Vector2Int jumpPos = GetTargetPosition(targetPos, direction);
            if (!OnBoard(jumpPos)) return false;
            if (IsOccupiedBy(jumpPos) == PieceColor.None)
            {
                Move killingMove = new Move(startPos, jumpPos, gameObject);
                killingMove.PieceToCapture = targetPos;
                moves.Add(killingMove);
                visited.Add(targetPos);
                foreach (var nextMove in GenerateMoves(jumpPos, visited, false))
                    killingMove.MovesAfterCapturing.Add(nextMove);
            }
        }
        return false;
    }

    PieceColor IsOccupiedBy(Vector2Int position)
    {
        switch (Board.Instance.BoardList[position.x][position.y])
        {
            case (int)PieceColor.White + (int)PieceType.Man:
            case (int)PieceColor.White + (int)PieceType.King:
                return PieceColor.White;
            case (int)PieceColor.Black + (int)PieceType.Man:
            case (int)PieceColor.Black + (int)PieceType.King:
                return PieceColor.Black;
            default:
                return PieceColor.None;
        }
    }

    bool OnBoard(Vector2Int position)
    {
        return !(position.x < 0 || position.y < 0
            || position.x > 7 || position.y > 7);
    }
}
