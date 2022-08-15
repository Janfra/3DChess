using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnUpgrade : MonoBehaviour
{
    List<TileInformation> upgradeTiles = new List<TileInformation>();
    PawnPiece upgradePawn;

    [Serializable]
    private class TileInformation
    {
        public TileInformation(Tile newTile, Faction newFaction)
        {
            tile = newTile;
            faction = newFaction;
        }

        public Tile tile;
        public Faction faction;
    }

    public void SetUpgradeTiles(Tile tile, int zPos)
    {
        upgradeTiles.Add(new TileInformation(tile, GetTileFactionToUpgrade(zPos)));
    }

    public void CheckUpgrade(Tile tile)
    {
        TileInformation tileInformation = CheckTile(tile);
        if (tileInformation == null) return;
        if(tile.OccupiedPiece.GetFaction() == tileInformation.faction && tile.OccupiedPiece.GetPieceName() == Piece.Pawn.ToString())
        {
            upgradePawn = (PawnPiece)tile.OccupiedPiece;
            GameManager.Instance.UpdateGameState(GameState.PawnUpgrade);
        }
    }
    private TileInformation CheckTile(Tile checkTile)
    {
        foreach (var tile in upgradeTiles)
        {
            if (tile.tile == checkTile) return tile;
        }
        return null;
    }

    private Faction GetTileFactionToUpgrade(int zPos)
    {
        // Just in case a value that shouldnt be here get passed 
        if (zPos != 0 && zPos != 7)
        {
            throw new ArgumentException($"GetTileFactionToUpgrade is getting a value outside the expected amount. Value: {zPos}");
        }
        if(zPos == 0)
        {
            return Faction.Black;
        } 
        else
        {
            return Faction.White;
        }
    }

    public void QueenUpgrade()
    {
        //Doesnt work, the new scriptable doesnt change it, may go to the scriptable and make it do it on enable or something
        QueenPiece newQueen = upgradePawn.gameObject.AddComponent(typeof(QueenPiece)) as QueenPiece;
        newQueen.OcuppiedTile = upgradePawn.OcuppiedTile;
        newQueen.OcuppiedTile.OccupiedPiece = newQueen;
        newQueen.SetTypeInfo(UnitManager.Instance.GetQueenSO(upgradePawn.GetFaction()));
        upgradePawn.enabled = false;
        GameManager.Instance.NextTurn();
    }
}
