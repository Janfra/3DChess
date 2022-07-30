using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : BasePiece
{
    public void PieceMoveHighlight()
    {
        SetInRange();
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
                DebugHelper.loopNullWarn("Tile", "SetLeftHorizontalTiles()", newPos, gameObject.name);
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
                DebugHelper.loopNullWarn("Tile", "SetLeftHorizontalTiles()", newPos, gameObject.name);
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
                DebugHelper.loopNullWarn("Tile", "SetLeftHorizontalTiles()", newPos, gameObject.name);
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
                DebugHelper.loopNullWarn("Tile", "SetLeftHorizontalTiles()", newPos, gameObject.name);
                break;
            }
            if (isPieceBlocking) break;
        }
    }
}
