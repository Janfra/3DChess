using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightPiece : PlayerPiece
{
    private int lineMovement;

    private void Awake()
    {
        lineMovement = 2;
    }

    protected override void SetInRange()
    {
        SetKnightTile();
    }

    private void SetKnightTile()
    {
        for (int side = 0; side < 2; side++)
        {
            bool isOffset = DirectionCheck(side);
            SetXLeftKnightTiles(isOffset);
            SetXRightKnightTiles(isOffset);
            SetZTopKnightTiles(isOffset);
            SetZBottomKnightTiles(isOffset);
        }
    }

    private void SetXLeftKnightTiles(bool isOffset)
    {
        Tile tile = GetXLeftKnightTile(isOffset);
        if (tile)
        {
            PiecePlacementCheck(tile);
            GridManager.Instance.HighlightTile(tile);
        }
        else
        {
            Debug.Log($"There is no tile in SetXLeftKnightTile(), piece: {gameObject.name}");
        }
    }
    private void SetXRightKnightTiles(bool isOffset)
    {
        Tile tile = GetXRightKnightTile(isOffset);
        if (tile)
        {
            PiecePlacementCheck(tile);
            GridManager.Instance.HighlightTile(tile);
        }
        else
        {
            Debug.Log($"There is no tile in SetXLeftKnightTile(), piece: {gameObject.name}");
        }
    }
    private void SetZTopKnightTiles(bool isOffset)
    {
        Tile tile = GetZTopKnightTile(isOffset);
        if (tile)
        {
            PiecePlacementCheck(tile);
            GridManager.Instance.HighlightTile(tile);
        }
        else
        {
            Debug.Log($"There is no tile in SetXLeftKnightTile(), piece: {gameObject.name}");
        }
    }
    private void SetZBottomKnightTiles(bool isOffset)
    {
        Tile tile = GetZBottomKnightTile(isOffset);
        if (tile)
        {
            PiecePlacementCheck(tile);
            GridManager.Instance.HighlightTile(tile);
        }
        else
        {
            Debug.Log($"There is no tile in SetXLeftKnightTile(), piece: {gameObject.name}");
        }
    }
    private Tile GetXLeftKnightTile(bool isOffset)
    {
        if (isOffset)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x - GridManager.TileDistance * lineMovement, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z - GridManager.TileDistance));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x - GridManager.TileDistance * lineMovement, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z + GridManager.TileDistance));
        }
    }
    private Tile GetXRightKnightTile(bool isOffset)
    {
        if (isOffset)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x + GridManager.TileDistance * lineMovement, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z - GridManager.TileDistance));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x + GridManager.TileDistance * lineMovement, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z + GridManager.TileDistance));
        }
    }
    private Tile GetZTopKnightTile(bool isOffset)
    {
        if (isOffset)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x - GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z + GridManager.TileDistance * lineMovement));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x + GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z + GridManager.TileDistance * lineMovement));
        }
    }
    private Tile GetZBottomKnightTile(bool isOffset)
    {
        if (isOffset)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x - GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z - GridManager.TileDistance * lineMovement));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x + GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z - GridManager.TileDistance * lineMovement));
        }
    }
}
