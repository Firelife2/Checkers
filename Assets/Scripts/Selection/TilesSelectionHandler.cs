using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesSelectionHandler : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    public static event Action<TileSelectionHandler> OnTileSelected;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ProcessClick();
    }

    void ProcessClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)
            || !hit.collider.TryGetComponent(out TileSelectionHandler tile)
            || !tile.IsHighlighted) return;
        OnTileSelected?.Invoke(tile);
    }
}
