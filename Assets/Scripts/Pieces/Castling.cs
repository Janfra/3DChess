using System;
using System.Collections.Generic;
using UnityEngine;

public class Castling : MonoBehaviour
{
    public static event Action Castled;
    public static event Action<Faction> IsKingSafe;
    private List<TowerPiece> towerList = new List<TowerPiece>();
    private List<KingPiece> kingList = new List<KingPiece>();
    private List<Tile> castleTilesList = new List<Tile>();

    public bool CastlingCheck()
    {
        Tile kingPosition = GetKingPosition();
        kingPosition.isInRange = false;
        IsKingSafe?.Invoke(GameManager.Instance.FactionTurn);
        if(kingPosition.isInRange == true)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetTowers(TowerPiece tower)
    {
        towerList.Add(tower);
    }

    public void SetKings(KingPiece king)
    {
        kingList.Add(king);
    }

    public void SetCastlingTiles(Tile tile)
    {
        castleTilesList.Add(tile);
    }

    public bool CheckCastlingTiles(Tile checkTile)
    {
        foreach (var tile in castleTilesList)
        {
            if (tile.name == checkTile.name) return true;
        }
        return false;
    }

    public void CastleMove(KingMoveDirection direction)
    {
        Tile towerTile = GetTowerCastling(direction);
        Tile kingTile = GetKingPosition();
        TowerCastleMove(towerTile, direction);
        KingCastleMove(kingTile, direction);
        Castled?.Invoke();
    }

    public void DisableCastling(Faction faction)
    {
        foreach (var piece in towerList)
        {
            if (piece.GetFaction() == faction)
            {
                piece.NoCastling();
            }
        }
    }

    public Tile GetKingPosition()
    {
        foreach (var king in kingList)
        {
            if (king.GetFaction() == GameManager.Instance.FactionTurn)
            {
                return king.OcuppiedTile;
            }
        }
        return null;
    }

    public Tile GetTowerCastling(KingMoveDirection direction)
    {
        foreach (var tower in towerList)
        {
            if (tower.GetFaction() == GameManager.Instance.FactionTurn && tower.GetCastlingDirection() == direction)
            {
                return tower.OcuppiedTile;
            }
        }
        return null;
    }

    public KingMoveDirection GetTileDirection(Tile tile)
    {
        if (tile.transform.position.x < GridManager.Instance.GetGridWidth() / 2) return KingMoveDirection.Left; else return KingMoveDirection.Right;
    }

    private void TowerCastleMove(Tile tile, KingMoveDirection direction)
    {
        Tile newPos = GetTowerMoveTile(tile, direction);
        newPos.SetPiece(tile.OccupiedPiece);
    }

    private void KingCastleMove(Tile tile, KingMoveDirection direction)
    {
        Tile newPos = GetKingMoveTile(tile, direction);
        KingPiece king = (KingPiece)tile.OccupiedPiece;
        king.KingMoved();
        newPos.SetPiece(tile.OccupiedPiece);
    }

    private Tile GetKingMoveTile(Tile tile, KingMoveDirection direction)
    {
        if (direction == KingMoveDirection.Left)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(tile.transform.position.x - GridManager.TileDistance * 2, tile.transform.position.y, tile.transform.position.z));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(tile.transform.position.x + GridManager.TileDistance * 2, tile.transform.position.y, tile.transform.position.z));
        }
    }

    private Tile GetTowerMoveTile(Tile tile, KingMoveDirection direction)
    {
        if(direction == KingMoveDirection.Left)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(tile.transform.position.x + GridManager.TileDistance * 3, tile.transform.position.y, tile.transform.position.z));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(tile.transform.position.x - GridManager.TileDistance * 2, tile.transform.position.y, tile.transform.position.z));
        }
    }

    public enum KingMoveDirection
    {
        Left,
        Right,
    }
}
