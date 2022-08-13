using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPiece : PlayerPiece
{
    private bool isCastling;
    private Castling.KingMoveDirection castlingDirection;

    private void Awake()
    {
        UnitManager.FirstMove += OnFirstMove;
        Castling.Castled += Castled;
        isCastling = true;
    }

    private void OnDestroy()
    {
        UnitManager.FirstMove -= OnFirstMove;
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
            else
            {
                return false;
            }
        }
        return false;
    }

    private void SetCastlingTiles(int gridMovement, bool isOffset)
    {
        bool isKingBlocking = false;
        bool isPieceBlocking = false;
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

    // If the tile of generation is lower than half of the grids width, is going to castle to the right, otherwise left.
    public void SetCastlingDirection(int side)
    {
        if(side < (GridManager.Instance.GetGridWidth() / 2))
        {
            castlingDirection = Castling.KingMoveDirection.Left;
        } 
        else
        {
            castlingDirection = Castling.KingMoveDirection.Right;
        }
    }
}
