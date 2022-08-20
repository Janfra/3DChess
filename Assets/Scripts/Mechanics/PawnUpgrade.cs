using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnUpgrade : MonoBehaviour
{
    GameManager gameManager;
    ObjectPooler objectPooler;
    UpgradeTile currentUpgradeTile;

    private void Start()
    {
        gameManager = GameManager.Instance;
        objectPooler = ObjectPooler.Instance;
    }

    public void SetUpgradeTile(UpgradeTile tile)
    {
        currentUpgradeTile = tile;
    }

    public void TowerUpgrade()
    {
        BasePiece newPiece = objectPooler.SpawnPiece(GetPieceToSpawn(Piece.Tower));
        UpgradeChosen(newPiece);
    }

    public void KnightUpgrade()
    {
        BasePiece newPiece = objectPooler.SpawnPiece(GetPieceToSpawn(Piece.Knight));
        UpgradeChosen(newPiece);
    }

    public void BishopUpgrade()
    {
        BasePiece newPiece = objectPooler.SpawnPiece(GetPieceToSpawn(Piece.Bishop));
        UpgradeChosen(newPiece);
    }

    public void QueenUpgrade()
    {
        BasePiece newPiece = objectPooler.SpawnPiece(GetPieceToSpawn(Piece.Queen));
        UpgradeChosen(newPiece);
    }

    private void UpgradeChosen(BasePiece piece)
    {
        currentUpgradeTile.SetPiece(piece);
        UnitManager.Instance.AddToPieceList(piece);
        gameManager.NextTurn();
    }

    /// <summary>
    /// Returns the key to access the pool for the piece type given depending on the current turn
    /// </summary>
    /// <param name="pieceType">Piece type name that is trying to be accessed</param>
    /// <returns>Key to pool type requested from current faction</returns>
    private ObjectPooler.poolObjName GetPieceToSpawn(Piece pieceType)
    {
        int factionSet = 0;
        int rv = 0;
        if (gameManager.FactionTurn == Faction.White) factionSet = 1;
        switch (pieceType)
        {
            case Piece.Tower:
                rv = ((int)Piece.Tower * 2) - factionSet;
                break;
            case Piece.Knight:
                rv = ((int)Piece.Knight * 2) - factionSet;
                break;
            case Piece.Bishop:
                rv = ((int)Piece.Bishop * 2) - factionSet;
                break;
            case Piece.Queen:
                rv = ((int)Piece.Queen * 2) - factionSet;
                break;
            case Piece.Pawn:
            case Piece.King:
            default:
                throw new ArgumentException($"Tried to upgrade to a {pieceType}, which is not valid");
        }
        return (ObjectPooler.poolObjName)rv;
    }
}
