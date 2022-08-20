using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPiece : PlayerPiece
{
    private bool isCastling;
    public Castling.KingMoveDirection castlingDirection;

    private void OnEnable()
    {
        UnitManager.FirstMove += OnFirstMove;
        Castling.Castled += Castled;
        isCastling = true;
    }

    private void OnDestroy()
    {
        UnitManager.FirstMove -= OnFirstMove;
        Castling.Castled -= Castled;
    }

    public Castling.KingMoveDirection GetCastlingDirection()
    {
        return castlingDirection;
    }

    private void OnFirstMove()
    {
        if (UnitManager.Instance.SelectedPiece == this)
        {
            Debug.Log("No more castling... Will do later");
            NoCastling();
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
    private void Castled()
    {
        
    }

    public void NoCastling()
    {
        isCastling = false;
        UnitManager.FirstMove -= OnFirstMove;
    }

    public void GeneratedTower()
    {
        isCastling = false;
        UnitManager.FirstMove -= OnFirstMove;
    }

    protected override void SetXTiles(int gridMovement, bool isOffset)
    {
        if (isCastling)
        {
            SetCastlingTiles(gridMovement, isOffset);
        }
        else
        {
            base.SetXTiles(gridMovement, isOffset);
        }
    }

    private bool IsKing(Tile tile)
    {
        if(tile.OccupiedPiece != null)
        {
            if (tile.OccupiedPiece.GetPieceName() == Piece.King.ToString() && tile.OccupiedPiece.GetFaction() == GetFaction())
            {
                return true;
            }
        }
        return false;
    }

    private void SetCastlingTiles(int gridMovement, bool isOffset)
    {
        bool isKingBlocking = false;
        bool isPieceBlocking = true;
        for (int newPos = 0; newPos < gridMovement; newPos += GridManager.TileDistance)
        {
            Tile tile = GetXTile(newPos, isOffset);
            if (tile)
            {
                isPieceBlocking = PiecePlacementCheck(tile);
                isKingBlocking = IsKing(tile);
                GridManager.Instance.HighlightTile(tile);
            }
            else
            {
                DebugHelper.loopNullWarn("Tile", "SetLeftHorizontalTiles()", newPos, gameObject.name);
                break;
            }
            if (isPieceBlocking && !isKingBlocking)
            {
                break;
            }
            else if (isKingBlocking)
            {
                KingExtraMove(newPos, isOffset);
                break;
            }
        }
    }

    private void KingExtraMove(int currentPos, bool isOffset)
    {
        Tile tile = GetXTile(currentPos + GridManager.TileDistance, isOffset);
        GridManager.Instance.HighlightTile(tile);
    }

    public bool GetIsCastling()
    {
        return isCastling;
    }
}
