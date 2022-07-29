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
                SetInRange();
                break;
            case Piece.Knight:
                SetInRange();
                break;
            case Piece.Bishop:
                SetInRange();
                break;
            case Piece.Tower:
                SetInRange();
                break;
            case Piece.Queen:
                SetInRange();
                break;
            case Piece.King:
                SetInRange();
                break;
            default:
                break;
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
                Debug.Log($"There is no tile in SetZTiles(), iteration number: {newPos} by piece: {gameObject.name}");
                break;
            }
            if (isPieceBlocking) break;
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
                Debug.Log($"There is no tile in SetXTiles(), iteration number: {newPos} by piece: {gameObject.name}");
                break;
            }
            if (isPieceBlocking) break;
        }
    }

    protected override void SetLeftHorizontalTiles(int gridMovement, bool isOffset)
    {
        bool isPieceBlocking = false;
        for (int newPos = 0; newPos < gridMovement; newPos += GridManager.TileDistance)
        {
            Tile tile = GetLeftHorizontalTile(newPos, isOffset);
            if (tile)
            {
                isPieceBlocking = PiecePlacementCheck(tile);
                GridManager.Instance.HighlightTile(tile);
            }
            else
            {
                Debug.Log($"There is no tile in SetZTiles(), iteration number: {newPos} by piece: {gameObject.name}");
                break;
            }
            if (isPieceBlocking) break;
        }
        }

    protected override void SetRightHorizontalTiles(int gridMovement, bool isOffset)
    {
        bool isPieceBlocking = false;
        for (int newPos = 0; newPos < gridMovement; newPos += GridManager.TileDistance)
        {
            Tile tile = GetRightHorizontalTile(newPos, isOffset);
            if (tile)
            {
                isPieceBlocking = PiecePlacementCheck(tile);
                GridManager.Instance.HighlightTile(tile);
            }
            else
            {
                Debug.Log($"There is no tile in SetZTiles(), iteration number: {newPos} by piece: {gameObject.name}");
                break;
            }
            if (isPieceBlocking) break;
        }
    }
}
