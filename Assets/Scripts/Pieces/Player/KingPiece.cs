using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPiece : PlayerPiece
{
    private int Movement;

    private void Awake()
    {
        Movement = 1;
        UnitManager.FirstMove += OnFirstMove;
    }

    private void OnDestroy()
    {
        UnitManager.FirstMove -= OnFirstMove;
    }

    private void OnFirstMove()
    {
        if (UnitManager.Instance.SelectedPiece == this)
        {
            Debug.Log("Towers cant castle anymore");
            UnitManager.FirstMove -= OnFirstMove;
        }
    }

    protected override void SetInRange()
    {
        for (int side = 0; side < 2; side++)
        {
            bool isOffset = DirectionCheck(side);
            SetLeftHorizontalTiles(Movement, isOffset);
            SetRightHorizontalTiles(Movement, isOffset);
            SetXTiles(Movement, isOffset);
            SetZTiles(Movement, isOffset);
        }
    }
}
