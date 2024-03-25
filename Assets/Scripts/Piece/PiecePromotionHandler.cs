using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePromotionHandler : NetworkBehaviour
{
    [SerializeField] protected PieceInfo piece;
    [SerializeField] protected Transform modelTransform;

    protected virtual bool TryPromotePiece(PiecePromotionHandler promotedPiece, int x, int z)
    {
        if (promotedPiece != this) return false;
        PromotePiece();
        return true;
    }

    protected void PromotePiece()
    {
        piece.MyType = PieceType.King;
        modelTransform.rotation = Quaternion.Euler(180, transform.rotation.eulerAngles.y, 0);
    }
}
