using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePromotionHandlerNetwork : PiecePromotionHandler
{

    public override void OnStartServer()
    {
        PieceMovementHandlerNetwork.ServerOnPieceReachedBackline += TryPromotePiece;
    }

    public override void OnStopServer()
    {
        PieceMovementHandlerNetwork.ServerOnPieceReachedBackline -= TryPromotePiece;
    }
    protected override bool TryPromotePiece(PiecePromotionHandler promotedPiece, int x, int z)
    {
        if(!base.TryPromotePiece(promotedPiece, x, z)) return false;
        PromotePiece();
        RpcPromotePiece();
        return true;
    }
    [ClientRpc]
    void RpcPromotePiece() {
        if (NetworkServer.active) {
            return;
        }
        else
        {
            PromotePiece();
        }
    }
}
