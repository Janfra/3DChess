using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPiece : PlayerPiece
{
    [SerializeField] private int movement;
    // Sets the direction the pawn will be moving.
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
        if (isMoveUp)
        {
            SetZTiles(movement, false);
            SetZDiagonalTopTiles();
        }
        else
        {
            SetZTiles(movement, true);
            SetZDiagonalBotTiles();
        }
    }

    protected override void SetXTiles(int gridMovement, bool isOffset)
    {
        bool isPieceBlocking = false;
        for (int newPos = 0; newPos < gridMovement; newPos += GridManager.TileDistance)
        {
            Tile tile = GetXTile(newPos, isOffset);
            if (tile)
            {
                isPieceBlocking = PiecePlacementCheck(tile);
                GridManager.Instance.HighlightTile(tile);
            }
            else
            {
                Debug.Log($"There is no tile in SetXTiles(), iteration number: {newPos}");
                break;
            }
            if (isPieceBlocking)
            {
                tile.isInRange = false;
                break;
            }
        }
    }

    protected override void SetZTiles(int gridMovement, bool isOffset)
    {
        bool isPieceBlocking = false;
        for (int newPos = 0; newPos < gridMovement; newPos += GridManager.TileDistance)
        {
            Tile tile = GetZTile(newPos, isOffset);
            if (tile)
            {
                isPieceBlocking = PiecePlacementCheck(tile);
                GridManager.Instance.HighlightTile(tile);
            }
            else
            {
                Debug.Log($"There is no tile in SetZTiles(), iteration number: {newPos}");
                break;
            }
            if (isPieceBlocking)
            {
                tile.isInRange = false;
                break;
            }
        }
    }

    private void SetZDiagonalTopTiles()
    {
        bool isOffset = false;
        bool isPieceBlocking = false;
        for (int side = 0; side < 2; side++)
        {
            Tile tile = GetZTDiagonalopTile(isOffset);
            if (tile)
            {
                isPieceBlocking = (PieceFactionCheck(tile));
                if (isPieceBlocking)
                {
                    tile.isInRange = true;
                    GridManager.Instance.HighlightTile(tile);
                }

            }
            else
            {
                Debug.Log($"There is no tile in SetZDiagonalTopTile(), piece: {gameObject.name}");
            }
            isOffset = true;
        }
    }
    private void SetZDiagonalBotTiles()
    {
        bool isOffset = false;
        bool isPieceBlocking = false;
        for (int side = 0; side < 2; side++)
        {
            Tile tile = GetZDiagonalBotTile(isOffset);
            if (tile)
            {
                isPieceBlocking = (PieceFactionCheck(tile));
                if (isPieceBlocking)
                {
                    tile.isInRange = true;
                    GridManager.Instance.HighlightTile(tile);
                }
            }
            else
            {
                Debug.Log($"There is no tile in SetZDiagonalBotTile(), piece: {gameObject.name}");
            }
            isOffset = true;
        }
    }

    private Tile GetZTDiagonalopTile(bool isOffset)
    {
        if (isOffset)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x - GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z + GridManager.TileDistance));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x + GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z + GridManager.TileDistance));
        }
    }
    private Tile GetZDiagonalBotTile(bool isOffset)
    {
        if (isOffset)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x - GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z - GridManager.TileDistance));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x + GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z - GridManager.TileDistance));
        }
    }

    private bool PieceFactionCheck(Tile tile)
    {
        if (tile.OccupiedPiece != null && tile.OccupiedPiece.GetFaction() != pieceInfo.faction) return true; else return false;
    }


    //public override void TestInheritance()
    //{
    //    Debug.Log("PawnPiece");
    //}
}
