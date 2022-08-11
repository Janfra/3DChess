using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePiece : MonoBehaviour
{
    [SerializeField] private Renderer pieceColour;
    [SerializeField] protected ScriptablePiece pieceInfo;
    [SerializeField] private ObjectLerper unitMovement;
    public Tile OcuppiedTile;


    private void Awake()
    {
        if(pieceColour == null)
        {
            pieceColour = GetComponent<Renderer>();
        }
        Debug.Assert(pieceInfo != null, $"{gameObject.name} has no piece info!");
        if(unitMovement == null)
        {
            unitMovement = GetComponent<ObjectLerper>();
        }
    }

    #region Highlight

    // Changes opacity to be able to see throught the piece hovered as well as give player feedback.
    public void PieceHovered()
    {
        Color colour = pieceColour.material.color;
        colour.a = 0.2f;
        pieceColour.material.color = colour;
    }

    // Brings the opacity back to normal.
    public void PieceUnhovered()
    {
        Color colour = pieceColour.material.color;
        colour.a = 1f;
        pieceColour.material.color = colour;
    }

    #endregion

    #region Movement

    // NOTE: 'gridMovement' is actually just used by 2 pieces, should probably change it to be specific to them...
    
    // Starts movement animation
    public void MovePieceTo(Vector3 targetPosition)
    {
        unitMovement.SetMovePosition(targetPosition);
    }

    // Depending on the current piece, sets the tiles as walkable. 
    abstract protected void SetInRange();

    // Sets the offset for selecting the side currently being changed in SetInRange().
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

    /// <param name="gridMovement">Set how many tiles are gonna be affected.</param>
    /// <param name="isOffset">Sets in which direction (top, bottom).</param>
    /// <summary> Sets the tiles on the Z position to walkable. </summary>
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

    /// <param name="gridMovement">Set how many tiles are gonna be affected.</param>
    /// <param name="isOffset">Sets in which direction (right, left).</param>
    /// <summary> Sets the tiles on the X position to walkable. </summary>
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

    /// <param name="gridMovement">Set how many tiles are gonna be affected.</param>
    /// <param name="isOffset">Sets in which direction (top, bottom).</param>
    /// <summary> Sets the left tiles horizontally to walkable. </summary>
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

    /// <summary> Sets the right tiles horizontally to walkable.</summary> 
    /// <param name="gridMovement"> Set how many tiles are gonna be affected.</param>
    /// <param name="isOffset"> Sets in which direction (top, bottom).</param>
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

    // Sets the given tile to be in range and then check if a piece is placed in it, return the result.
    protected bool PiecePlacementCheck(Tile tile)
    {
        tile.isInRange = true;
        if (tile.OccupiedPiece != null) return true; else return false;
    }

    /// <summary>
    /// Returns the tile in Z with the given direction.
    /// </summary>
    /// <param name="newPos">Sets the distance of the tile to return</param>
    /// <param name="isOffset">Sets the direction to check</param>
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

    /// <summary>
    /// Returns the tile in X with the given direction.
    /// </summary>
    /// <param name="newPos">Sets the distance of the tile to return</param>
    /// <param name="isOffset">Sets the direction to check</param>
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

    /// <summary>
    /// Returns the tile on the left horizontally with the given direction.
    /// </summary>
    /// <param name="newPos">Sets the distance of the tile to return</param>
    /// <param name="isOffset">Sets the direction to check</param>
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

    /// <summary>
    /// Returns the tile on the right horizontally with the given direction.
    /// </summary>
    /// <param name="newPos">Sets the distance of the tile to return</param>
    /// <param name="isOffset">Sets the direction to check</param>
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

    // Return the piece faction.
    public Faction GetFaction()
    {
        return pieceInfo.faction;
    }

    // Return the piece name.
    public string GetPieceName()
    {
       return pieceInfo.PieceName.ToString();
    }

    virtual public void PieceEaten()
    {
        gameObject.SetActive(false);
    }
}
