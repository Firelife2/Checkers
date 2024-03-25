using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelectionHandler : MonoBehaviour
{
    public bool IsHighlighted { get; set; }

    public Move MoveInfo { get; set; }

    public bool HaveToCapture
    {
        get { return MoveInfo.PieceToCapture.x != -1; }
    }
}
