using System;
using System.Collections.Generic;
using UnityEngine;

public class Castling : MonoBehaviour
{
    public static event Action Castled;
    public static event Action<Faction> IsKingSafe;
    private List<TowerPiece> towerList = new List<TowerPiece>();
    private List<KingPiece> kingList = new List<KingPiece>();
    private List<CastlingTile> castleTilesList = new List<CastlingTile>();
    private GameManager gameManager;

    [System.Serializable]
    private class CastlingTile
    {
        public CastlingTile(Tile tile, KingMoveDirection direction, Faction faction)
        {
            Tile = tile;
            MoveDirection = direction;
            Faction = faction;
        }
        public Tile Tile;
        public KingMoveDirection MoveDirection;
        public Faction Faction;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    #region Setters & Getters

    public void SetTowers(TowerPiece tower)
    {
        towerList.Add(tower);
    }

    public void SetKings(KingPiece king)
    {
        kingList.Add(king);
    }

    /// <summary>
    /// Sets the tiles for castling while checking for no duplicates.
    /// </summary>
    /// <param name="tile">The tile added to the list</param>
    /// <param name="direction">In which direction is the tile for the king movement</param>
    /// <param name="faction">Which faction would use this tile for castling</param>
    public void SetCastlingTiles(Tile tile, KingMoveDirection direction, Faction faction)
    {
        bool inList = false;
        foreach (var castleTile in castleTilesList)
        {
            if (castleTile.Tile == tile) inList = true;
        }
        if (!inList) castleTilesList.Add(new CastlingTile(tile, direction, faction));
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

    private Tile GetKingNewTile(Tile tile, KingMoveDirection direction)
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

    private Tile GetTowerNewTile(Tile tile, KingMoveDirection direction)
    {
        if (direction == KingMoveDirection.Left)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(tile.transform.position.x + GridManager.TileDistance * 3, tile.transform.position.y, tile.transform.position.z));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(tile.transform.position.x - GridManager.TileDistance * 2, tile.transform.position.y, tile.transform.position.z));
        }
    }

    #endregion

    public bool CheckKingCastling()
    {
        foreach (var king in kingList)
        {
            if (king.GetFaction() == gameManager.FactionTurn && king.GetIsCastling()) return true;
        }
        return false;
    }

    public bool CastlingCheck(KingMoveDirection direction)
    {
        Tile kingPosition = GetKingPosition();
        StartInRangeCheck(kingPosition);
        if(IsKingMoveSafe(kingPosition, direction))
        {
            return true;
        }
        else
        {
            GridManager.Instance.UnhighlightMoveTiles();
            return false;
        }
    }

    /// <summary>
    /// Sets in motion all the components required to check if the castling is safe for the king and therefore, possible.
    /// </summary>
    /// <param name="kingPosition">The tile ocuppied by the king to run the check</param>
    private void StartInRangeCheck(Tile kingPosition)
    {
        kingPosition.isInRange = false;
        GridManager.Instance.UnhighlightMoveTiles();
        IsKingSafe?.Invoke(GameManager.Instance.FactionTurn);
    }

    /// <summary>
    /// Check if the tower is able to castle
    /// </summary>
    /// <param name="direction">Sets the direction in which the tower for castling is</param>
    /// <returns>Whether the tower is still able to castle</returns>
    public bool TowerCheck(KingMoveDirection direction)
    {
        TowerPiece towerMoving = null;
        foreach (var tower in towerList)
        {
            if (tower.GetFaction() == gameManager.FactionTurn && tower.GetCastlingDirection() == direction)
            {
                towerMoving = tower;
            }
        }

        if (towerMoving)
        {
            return towerMoving.GetIsCastling();
        }
        return false;
    }

    public bool CheckCastlingTiles(Tile checkTile)
    {
        foreach (var tile in castleTilesList)
        {
            if (tile.Tile == checkTile && !tile.Tile.isInRange) return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if the king's tile or the tiles it will be moving to are in range for the enemy.
    /// </summary>
    /// <param name="kingTile">The kings tile position</param>
    /// <param name="moveDirection">The direction the king is attempting to move</param>
    /// <returns>Is the king in enemy range at any point while castling</returns>
    private bool IsKingMoveSafe(Tile kingTile, KingMoveDirection moveDirection)
    {
        if (kingTile.isInRange) return false;
        if(moveDirection == KingMoveDirection.Left)
        {
            foreach (var tile in castleTilesList)
            {
                if(tile.MoveDirection == KingMoveDirection.Left && tile.Faction == gameManager.FactionTurn && tile.Tile.isInRange)
                {
                    return false;
                }
            }
        }
        else
        {
            foreach (var tile in castleTilesList)
            {
                if (tile.MoveDirection == KingMoveDirection.Right && tile.Faction == gameManager.FactionTurn && tile.Tile.isInRange)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Start castling, move the king and the tower.
    /// </summary>
    /// <param name="direction">Sets the direction the castling will be done</param>
    public void StartCastling(KingMoveDirection direction)
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

    /// <summary>
    /// Moves the tower to its new position.
    /// </summary>
    /// <param name="tile">The initial position of the tower, used to calculate new position</param>
    /// <param name="direction">The direction the king will be moving to select the tower movement</param>
    private void TowerCastleMove(Tile tile, KingMoveDirection direction)
    {
        Tile newPos = GetTowerNewTile(tile, direction);
        newPos.SetPiece(tile.OccupiedPiece);
    }

    /// <summary>
    /// Moves the king to its new position and disables castling.
    /// </summary>
    /// <param name="tile">The king old position to chech for the new one</param>
    /// <param name="direction">The direction the movement will be done</param>
    private void KingCastleMove(Tile tile, KingMoveDirection direction)
    {
        Tile newPos = GetKingNewTile(tile, direction);
        KingPiece king = (KingPiece)tile.OccupiedPiece;
        king.KingMoved();
        newPos.SetPiece(tile.OccupiedPiece);
    }

    public bool CheckTowersLeft()
    {
        foreach (var tower in towerList)
        {
            if(tower.GetFaction() == gameManager.FactionTurn && tower.GetIsCastling())
            {
                return true;
            }
        }
        return false;
    }

    public enum KingMoveDirection
    {
        Left,
        Right,
    }
}
