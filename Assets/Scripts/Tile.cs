using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Renderer tileColour;
    [SerializeField] private SO_Colours colour;
    private Color defaultColour;
    private Color highlightColour;
    public bool isInRange;
    public BasePiece OccupiedPiece;
    private UnitManager unitManager;

    private void Awake()
    {
        unitManager = UnitManager.Instance;
    }

    public bool isWalkable => OccupiedPiece == null;

    #region OnMouseEvents

    private void OnMouseEnter()
    {
        Highlight();
        if (OccupiedPiece)
        {
            OccupiedPiece.PieceHovered();
        }
    }
    private void OnMouseExit()
    {
        HighlightUnhovered();
        if (OccupiedPiece)
        {
            OccupiedPiece.PieceUnhovered();
        }
    }
    private void OnMouseDown()
    {
        if (GameManager.Instance.State != GameState.PlayerTurn) return;

        if (OccupiedPiece)
        {
            if (OccupiedPiece.GetFaction() == Faction.Player)
            {
                if (unitManager.SelectedPiece != null) GridManager.Instance.UnhighlightMoveTiles();
                unitManager.SetSelectedPiece((PlayerPiece)OccupiedPiece);
                unitManager.SelectedPiece.PieceMoveHighlight();
            }
            else
            {
                if (unitManager.SelectedPiece != null)
                {
                    unitManager.SetSelectedPiece(null);
                    GridManager.Instance.UnhighlightMoveTiles();
                }
            }
        }
        else
        {
            if (unitManager.SelectedPiece != null)
            {
                if (isWalkable && isInRange)
                {
                    SetPiece(unitManager.SelectedPiece);
                    unitManager.SetSelectedPiece(null);
                    GridManager.Instance.UnhighlightMoveTiles();
                }
            }
        }
    }

    #endregion

    #region Highlight & Colour

    public void SetColor(bool offSet)
    {
        defaultColour = offSet ? colour.colourOne : colour.colourTwo;
        highlightColour = offSet ? colour.colourHighlightOne : colour.colourHighlightTwo;
        ChangeColour(defaultColour);
    }
    private void ChangeColour(Color newColour)
    {
        tileColour.material.color = newColour;
    }
    public void Highlight()
    {
        ChangeColour(highlightColour);
    }
    public void HighlightUnhovered()
    {
        if (!GridManager.Instance.MoveTile(this)) ChangeColour(defaultColour);
    }
    public void StopHighlight()
    {
        ChangeColour(defaultColour);
    }

    #endregion

    public void SetPiece(BasePiece piece)
    {
        if(piece.OcuppiedTile != null)
        {
            piece.OcuppiedTile.OccupiedPiece = null;
        }
        piece.transform.position = new Vector3(transform.position.x, transform.position.y + UnitManager.pieceYOffset, transform.position.z - UnitManager.pieceZOffset);
        OccupiedPiece = piece;
        piece.OcuppiedTile = this;
    }
}
