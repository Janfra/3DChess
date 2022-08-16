using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPiece : PlayerPiece
{
    private int Movement;
    private bool isCastling;

    private void OnEnable()
    {
        Movement = 1;
        isCastling = true;
        UnitManager.FirstMove += OnFirstMove;
        Castling.Castled += Castled;
    }


    private void OnDestroy()
    {
        UnitManager.FirstMove -= OnFirstMove;
    }

    public void KingEaten()
    {
        GameManager.Instance.UpdateGameState(GameState.Victory);
    }
    private void Castled()
    {
       
    }

    public void KingMoved()
    {
        Debug.Log("Towers cant castle anymore");
        isCastling = false;
        UnitManager.Instance.KingMoved(GetFaction());
        UnitManager.FirstMove -= OnFirstMove;
    }

    private void OnFirstMove()
    {
        if (UnitManager.Instance.SelectedPiece == this)
        {
            KingMoved();
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

    public override void PieceEaten()
    {
        base.PieceEaten();
        KingEaten();
    }

    protected override void SetXTiles(int gridMovement, bool isOffset)
    {
        if (isCastling)
        {
            byte castlingExtraMove = 1;
            bool isPieceBlocking = false;
            for (int newPos = 0; newPos < gridMovement + castlingExtraMove; newPos += GridManager.TileDistance)
            {
                Tile tile = GetXTile(newPos, isOffset);
                if (tile)
                {
                    isPieceBlocking = PiecePlacementCheck(tile);
                    GridManager.Instance.HighlightTile(tile);
                }
                else
                {
                    //DebugHelper.loopNullWarn("Tile", "SetLeftHorizontalTiles()", newPos, gameObject.name);
                    break;
                }
                if (isPieceBlocking) break;
                if(newPos == gridMovement)
                {
                    tile.isInRange = false;
                    UnitManager.Instance.SetCastlingTiles(tile);
                }
            }
        } 
        else
        {
            base.SetXTiles(gridMovement, isOffset);
        }
    }
}
