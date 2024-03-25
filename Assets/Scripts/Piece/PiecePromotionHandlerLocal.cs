using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePromotionHandlerLocal : PiecePromotionHandler
{
    void Start()
    {
        PieceMovementHandlerLocal.OnPieceReachedBackline += TryPromotePiece;
    }

    void OnDestroy()
    {
        PieceMovementHandlerLocal.OnPieceReachedBackline -= TryPromotePiece;
    }
}
