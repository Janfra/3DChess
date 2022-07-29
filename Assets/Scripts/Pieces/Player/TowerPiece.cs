using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPiece : PlayerPiece
{
    private bool isCastling;

    private void Awake()
    {
        UnitManager.FirstMove += OnFirstMove;
        isCastling = true;
    }

    private void OnDestroy()
    {
        UnitManager.FirstMove -= OnFirstMove;
    }

    private void OnFirstMove()
    {
        if (UnitManager.Instance.SelectedPiece == this)
        {
            Debug.Log("No more castling... Will do later");
            isCastling = false;
            UnitManager.FirstMove -= OnFirstMove;
        }
    }

    protected override void SetInRange()
    {
        for (int side = 0; side < 2; side++)
        {
            bool isOffset = DirectionCheck(side);
            SetXTiles(UnitManager.MaxMoveDistance, isOffset);
            SetZTiles(UnitManager.MaxMoveDistance, isOffset);
        }
    }
}
