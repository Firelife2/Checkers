using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementHandlerLocal : PieceMovementHandler
{
    public static event Func<PiecePromotionHandler, int, int, bool> OnPieceReachedBackline;

    void Start()
    {
        TilesSelectionHandler.OnTileSelected += HandleTileSelected;
    }

    void OnDestroy()
    {
        TilesSelectionHandler.OnTileSelected -= HandleTileSelected;
    }

    protected override void ReachedBackline(Vector2Int newPosition)
    {
        OnPieceReachedBackline?.Invoke(promotionHandler, newPosition.x, newPosition.y);
    }
}
