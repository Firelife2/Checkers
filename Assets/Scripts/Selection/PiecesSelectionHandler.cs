using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesSelectionHandler : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    public PieceSelectionHandler SelectedPiece { get; private set; }
    bool selectionLocked = false;
    bool isLocal = true;

    public static event Action<PieceSelectionHandler> OnPieceSelected;
    public static event Action<PieceSelectionHandler> OnPieceDeselected;

    void Start()
    {
        PieceMovementHandler.OnPieceMoved += DeselectPiece;
        PieceMovementHandler.OnLockSelectedPiece += LockPiece;
        TurnsHandler.Instance.OnMovesGenerated += ReselectPiece;
        if (!LocalGameManager.Instance)
            isLocal = false;
    }

    void OnDestroy()
    {
        PieceMovementHandler.OnPieceMoved -= DeselectPiece;
        PieceMovementHandler.OnLockSelectedPiece -= LockPiece;
        TurnsHandler.Instance.OnMovesGenerated -= ReselectPiece;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ProcessClick();
    }

    void ProcessClick()
    {
        if (selectionLocked) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)
            || !hit.collider.TryGetComponent(out PieceSelectionHandler piece)) return;
        
        if (SelectedPiece) OnPieceDeselected?.Invoke(SelectedPiece);
        SelectedPiece = piece;
        OnPieceSelected?.Invoke(SelectedPiece);
    }

    void DeselectPiece(bool deselect)
    {
        if (!deselect) return;
        var selectedPieceCopy = SelectedPiece;
        SelectedPiece = null;
        OnPieceDeselected?.Invoke(selectedPieceCopy);
    }

    void LockPiece(bool locked)
    {
        selectionLocked = locked;
    }

    void ReselectPiece()
    {
        if (SelectedPiece)
            OnPieceSelected?.Invoke(SelectedPiece);
    }
}
