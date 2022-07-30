using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPiece : PlayerPiece
{
    [SerializeField] private int movement;
    private bool isMoveUp => pieceInfo.faction == Faction.White;


    private void Awake()
    {
        UnitManager.FirstMove += OnFirstMove;
        movement = 2;
    }

    private void OnDestroy()
    {
        UnitManager.FirstMove -= OnFirstMove;
    }

    private void OnFirstMove()
    {
        if(UnitManager.Instance.SelectedPiece == this)
        {
            movement = 1;
            UnitManager.FirstMove -= OnFirstMove;
        }
    }

    protected override void SetInRange()
    {
        if (isMoveUp) SetZTiles(movement, false); else SetZTiles(movement, true);
    }

    //public override void TestInheritance()
    //{
    //    Debug.Log("PawnPiece");
    //}
}
