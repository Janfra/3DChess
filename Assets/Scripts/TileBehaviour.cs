using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tile))]
public class TileBehaviour : MonoBehaviour
{
    [SerializeField] Tile tile;
    GameManager gameManager;
    UnitManager unitManager;

    private void Awake()
    {
        unitManager = UnitManager.Instance;
        gameManager = GameManager.Instance;
        if(tile == null)
        {
            tile = GetComponent<Tile>();
        }
    }

    #region OnMouseEvents

    // Clicking logic, enables selecting a piece, eating enemy pieces and movement.
    private void OnMouseDown()
    {
        if (gameManager.State != GameState.WhiteTurn && gameManager.State != GameState.BlackTurn) return;

        if (tile.OccupiedPiece)
        {
            // If it is the piece faction turn select it, otherwise eat enemy piece with the selected piece (if possible).
            if (tile.OccupiedPiece.GetFaction() == gameManager.FactionTurn)
            {
                if(TowerCastlingCheck())
                {
                    return;
                }
                else
                {
                    SelectPiece();
                }
            }
            else
            {
                if (unitManager.SelectedPiece != null && tile.isInRange)
                {
                    EatEnemyPiece();
                }
            }
        }
        else
        {
            if (unitManager.SelectedPiece != null)
            {
                // Check if the clicked tile is not taken and within movement range, then move selected piece.
                if (tile.isWalkable && tile.isInRange && !unitManager.CastlingTileCheck(tile))
                {
                    PieceMove();
                } 
                else if(tile.isWalkable && unitManager.CastlingTileCheck(tile))
                {
                    unitManager.CastlingAttempt(unitManager.GetTileDirection(tile));
                }
            }
        }
    }

    #endregion

    private void SetSelectedPiece(PlayerPiece playerPiece)
    {
        unitManager.SetSelectedPiece(playerPiece);
    }
    private void PieceReplace(BasePiece newPiece)
    {
        tile.OccupiedPiece.PieceEaten();
        tile.SetPiece(newPiece);
    }
    private void PieceMove()
    {
        tile.SetPiece(unitManager.SelectedPiece);
        if (gameManager.State != GameState.PawnUpgrade) gameManager.NextTurn();
    }
    private void EatEnemyPiece()
    {
        PieceReplace(unitManager.SelectedPiece);
        if (gameManager.State != GameState.PawnUpgrade) gameManager.NextTurn();
    }
    private void SelectPiece()
    {
        if (unitManager.SelectedPiece != null) UnhighlitghtMoveTile();
        SetSelectedPiece((PlayerPiece)tile.OccupiedPiece);
        unitManager.SelectedPiece.PieceMoveHighlight();
    }

    // Stop highlight of movement all tiles.
    private void UnhighlitghtMoveTile()
    {
        GridManager.Instance.UnhighlightMoveTiles();
    }

    private bool TowerCastlingCheck()
    {
        if(unitManager.SelectedPiece != null)
        {
            bool rv;
            if(rv = (unitManager.SelectedPiece.GetPieceName() == Piece.Tower.ToString()) && (tile.OccupiedPiece.GetPieceName() == Piece.King.ToString()))
            {
                TowerPiece tower = (TowerPiece)unitManager.SelectedPiece;
                unitManager.CastlingAttempt(tower.GetCastlingDirection());
            }
            return rv; 
        }
        return false;
    }
}
