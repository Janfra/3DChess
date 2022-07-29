using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePiece : MonoBehaviour
{
    [SerializeField] private Renderer pieceColour;
    [SerializeField] protected ScriptablePiece pieceInfo;
    public Tile OcuppiedTile;

    private void Awake()
    {
        if(pieceColour == null)
        {
            pieceColour = GetComponent<Renderer>();
        }
        Debug.Assert(pieceInfo != null, $"{gameObject.name} has no piece info!");
    }

    #region Highlight
    public void PieceHovered()
    {
        Color colour = pieceColour.material.color;
        colour.a = 0.2f;
        pieceColour.material.color = colour;
    }
    public void PieceUnhovered()
    {
        Color colour = pieceColour.material.color;
        colour.a = 1f;
        pieceColour.material.color = colour;
    }
    #endregion

    #region Movement
    virtual protected void SetInRange()
    {
            
    }
    protected bool DirectionCheck(int direction)
    {
        if (direction % 2 == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    virtual protected void SetZTiles(int gridMovement, bool isOffset)
    {
        bool isPieceBlocking = false;
        for (int newPos = 0; newPos < gridMovement; newPos += GridManager.TileDistance)
        {
            Tile tile = GetZTile(newPos, isOffset);
            if (tile)
            {
                isPieceBlocking = PiecePlacementCheck(tile);
            }
            else
            {
                Debug.Log($"There is no tile in SetZTiles(), iteration number: {newPos}");
                break;
            }
            if (isPieceBlocking) break;
        }
    }
    virtual protected void SetXTiles(int gridMovement, bool isOffset)
    {
        bool isPieceBlocking = false;
        for (int newPos = 0; newPos < gridMovement; newPos += GridManager.TileDistance)
        {
            Tile tile = GetXTile(newPos, isOffset);
            if (tile)
            {
                isPieceBlocking = PiecePlacementCheck(tile);
            }
            else
            {
                Debug.Log($"There is no tile in SetXTiles(), iteration number: {newPos}");
                break;
            }
            if (isPieceBlocking) break;
        }
    }
    virtual protected void SetLeftHorizontalTiles(int gridMovement, bool isOffset)
    {
        bool isPieceBlocking = false;
        for (int newPos = 0; newPos < gridMovement; newPos += GridManager.TileDistance)
        {
            Tile tile = GetLeftHorizontalTile(newPos, isOffset);
            if (tile)
            {
                isPieceBlocking = PiecePlacementCheck(tile);
            }
            else
            {
                Debug.Log($"There is no tile in SetZTiles(), iteration number: {newPos}");
                break;
            }
            if (isPieceBlocking) break;
        }
    }
    virtual protected void SetRightHorizontalTiles(int gridMovement, bool isOffset)
    {
        bool isPieceBlocking = false;
        for (int newPos = 0; newPos < gridMovement; newPos += GridManager.TileDistance)
        {
            Tile tile = GetRightHorizontalTile(newPos, isOffset);
            if (tile)
            {
                isPieceBlocking = PiecePlacementCheck(tile);
            }
            else
            {
                Debug.Log($"There is no tile in SetZTiles(), iteration number: {newPos}");
                break;
            }
            if (isPieceBlocking) break;
        }
    }
    protected bool PiecePlacementCheck(Tile tile)
    {
        tile.isInRange = true;
        if (tile.OccupiedPiece != null) return true; else return false;
    }
    protected Tile GetZTile(int newPos, bool isOffset)
    {
        if (isOffset)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z - newPos - GridManager.TileDistance));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z + newPos + GridManager.TileDistance));
        }
    }
    protected Tile GetXTile(int newPos, bool isOffset)
    {
        if (isOffset)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x - newPos - GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x + newPos + GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z));
        }
    }
    protected Tile GetLeftHorizontalTile(int newPos, bool isOffset)
    {
        if (isOffset)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x + newPos + GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z - newPos - GridManager.TileDistance));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x + newPos + GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z + newPos + GridManager.TileDistance));
        }
    }
    protected Tile GetRightHorizontalTile(int newPos, bool isOffset)
    {
        if (isOffset)
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x - newPos - GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z - newPos - GridManager.TileDistance));
        }
        else
        {
            return GridManager.Instance.GetTileAtPosition(new Vector3(OcuppiedTile.transform.position.x - newPos - GridManager.TileDistance, OcuppiedTile.transform.position.y, OcuppiedTile.transform.position.z + newPos + GridManager.TileDistance));
        }
    }

    #endregion

    //virtual public void TestInheritance()
    //{
    //    Debug.Log("BasePiece");
    //}

    public Faction GetFaction()
    {
        return pieceInfo.faction;
    }
    public void PawnUpgrade(BasePiece newType, Piece newName)
    {
        pieceInfo.typePrefab = newType;
        pieceInfo.PieceName = newName;
    }
}
