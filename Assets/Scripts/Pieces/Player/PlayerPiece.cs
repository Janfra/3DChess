using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : BasePiece
{
    public void PieceMoveHighlight()
    {
        switch (pieceInfo.PieceName)
        {
            case Piece.Pawn:
                PawnMovementHighlight();
                break;
            case Piece.Knight:
                break;
            case Piece.Bishop:
                break;
            case Piece.Tower:
                HighlightLine();
                break;
            case Piece.Queen:
                break;
            case Piece.King:
                break;
            default:
                break;
        }
    }

    private void PawnMovementHighlight()
    {
        int gridMovement = 2;
        HightlightTop(gridMovement);
    }

    private void HighlightLine()
    {
        int gridMovement = 8;
        HightlightTop(gridMovement);
        HightlightRight(gridMovement);
        HightlightBottom(gridMovement);
        HightlightLeft(gridMovement);
    }

    private void HighlightHorizontal()
    {

    }

    private void HightlightTop(int gridMovement)
    {
        Tile highlightTile;

        for (int i = 0; i < gridMovement; i += GridManager.TileDistance)
        {
            highlightTile = GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z + i + GridManager.TileDistance));
            if (highlightTile)
            {
                GridManager.Instance.HighlightTile(highlightTile);
                if (highlightTile.OccupiedPiece != null) break;
            }
        }
    }
    private void HightlightLeft(int gridMovement)
    {
        Tile highlightTile;

        for (int i = 0; i < gridMovement; i += GridManager.TileDistance)
        {
            highlightTile = GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x - i - GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z));
            if (highlightTile)
            {
                GridManager.Instance.HighlightTile(highlightTile);
                if (highlightTile.OccupiedPiece != null) break;
            }
        }
    }
    private void HightlightRight(int gridMovement)
    {
        Tile highlightTile;

        for (int i = 0; i < gridMovement; i += GridManager.TileDistance)
        {
            highlightTile = GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x + i + GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z));
            if (highlightTile)
            {
                GridManager.Instance.HighlightTile(highlightTile);
                if (highlightTile.OccupiedPiece != null) break;
            }
        }
    }
    private void HightlightBottom(int gridMovement)
    {
        Tile highlightTile;

        for (int i = 0; i < gridMovement; i += GridManager.TileDistance)
        {
            highlightTile = GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z - i - GridManager.TileDistance));
            if (highlightTile)
            {
                GridManager.Instance.HighlightTile(highlightTile);
                if (highlightTile.OccupiedPiece != null) break;
            }
        }
    }
}
