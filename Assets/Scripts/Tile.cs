using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region Variables

    [SerializeField] private Renderer tileColour;
    [SerializeField] private SO_Colours colour;
    private Color defaultColour, highlightColour;
    public bool isInRange;
    public BasePiece OccupiedPiece;
    private UnitManager unitManager;
    public bool isWalkable => OccupiedPiece == null;

    #endregion

    private void Awake()
    {
        unitManager = UnitManager.Instance;
    }

    #region OnMouseEvents

    // Highlight tile and make piece transparent on hover
    private void OnMouseEnter()
    {
        Highlight();
        if (OccupiedPiece)
        {
            OccupiedPiece.PieceHovered();
        }
    }

    // Stop hovered highlight and set piece back to normal colour.
    private void OnMouseExit()
    {
        TileUnhover();
        if (OccupiedPiece)
        {
            OccupiedPiece.PieceUnhovered();
        }
    }

    // Clicking logic, enables selecting a piece, eating enemy pieces and movement.
    private void OnMouseDown()
    {
        if (GameManager.Instance.State != GameState.WhiteTurn && GameManager.Instance.State != GameState.BlackTurn) return;

        if (OccupiedPiece)
        {
            // If it is the piece faction turn select it, otherwise eat enemy piece with the selected piece (if possible).
            if (OccupiedPiece.GetFaction() == GameManager.Instance.FactionTurn)
            {
                SelectPiece();
            }
            else
            {
                if (unitManager.SelectedPiece != null && isInRange)
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
                if (isWalkable && isInRange)
                {
                    PieceMove();
                }
            }
        }
    }

    #endregion

    #region Highlight & Colour

    // Changes the tile colour to given colour.
    private void ChangeColour(Color newColour)
    {
        tileColour.material.color = newColour;
    }

    // With the parameter given, it sets the tile's colours. 
    public void SetColor(bool offSet)
    {
        defaultColour = offSet ? colour.colourTwo : colour.colourOne;
        highlightColour = offSet ? colour.colourHighlightTwo : colour.colourHighlightOne;
        ChangeColour(defaultColour);
    }

    // Start highlighting tile.
    public void Highlight()
    {
        ChangeColour(highlightColour);
    }

    // Stop highlighting tile.
    public void StopHighlight()
    {
        ChangeColour(defaultColour);
    }

    // Stop hovered highlight. NOTE: This one is specifically to stop highlighting only tiles highlighted by the hover effect, won't affect move highlight.
    public void TileUnhover()
    {
        if (!GridManager.Instance.MoveTileCheck(this)) ChangeColour(defaultColour);
    }

    // Stop highlight of movement all tiles.
    private void UnhighlitghtMoveTile()
    {
        GridManager.Instance.UnhighlightMoveTiles();
    }

    #endregion

    #region Piece Handling

    public void InitializePiece(BasePiece piece)
    {
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y + UnitManager.pieceYOffset, transform.position.z - UnitManager.pieceZOffset);
        piece.transform.position = newPos;
        piece.MovePieceTo(newPos);
        OccupiedPiece = piece;
        piece.OcuppiedTile = this;
        Debug.Log($"Piece name: {piece.name}, piece current position: {piece.transform.position}, piece target new position: {newPos}");
    }
    public void SetPiece(BasePiece piece)
    {
        if (piece.OcuppiedTile != null)
        {
            piece.OcuppiedTile.OccupiedPiece = null;
        }
        piece.MovePieceTo(new Vector3(transform.position.x, transform.position.y + UnitManager.pieceYOffset, transform.position.z - UnitManager.pieceZOffset));
        OccupiedPiece = piece;
        piece.OcuppiedTile = this;
    }
    private void SetSelectedPiece(PlayerPiece playerPiece)
    {
        unitManager.SetSelectedPiece(playerPiece);
    }
    private void PieceReplace(BasePiece newPiece)
    {
        OccupiedPiece.PieceEaten();
        SetPiece(newPiece);
    }
    private void PieceMove()
    {
        SetPiece(unitManager.SelectedPiece);
        unitManager.CheckFirstMove();
        GameManager.Instance.NextTurn();
    }
    private void EatEnemyPiece()
    {
        PieceReplace(unitManager.SelectedPiece);
        GameManager.Instance.NextTurn();
    }
    private void SelectPiece()
    {
        if (unitManager.SelectedPiece != null) UnhighlitghtMoveTile();
        SetSelectedPiece((PlayerPiece)OccupiedPiece);
        unitManager.SelectedPiece.PieceMoveHighlight();
    }

    #endregion
}