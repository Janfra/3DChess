using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region Variables

    [SerializeField] private Renderer tileColour;
    [SerializeField] private SO_Colours colour;
    private Color defaultColour;
    private Color highlightColour;
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
        StopHighlightUnhover();
        if (OccupiedPiece)
        {
            OccupiedPiece.PieceUnhovered();
        }
    }
    private void OnMouseDown()
    {
        if (GameManager.Instance.State != GameState.WhiteTurn && GameManager.Instance.State != GameState.BlackTurn) return;

        if (OccupiedPiece)
        {
            if (OccupiedPiece.GetFaction() == GameManager.Instance.FactionTurn)
            {
                SelectPiece();
            }
            else
            {
                if (unitManager.SelectedPiece != null)
                {
                    EatEnemyPiece();
                }
            }
        }
        else
        {
            if (unitManager.SelectedPiece != null)
            {
                if (isWalkable && isInRange)
                {
                    PieceMove();
                }
            }
        }
    }

    #endregion

    #region Highlight & Colour

    private void ChangeColour(Color newColour)
    {
        tileColour.material.color = newColour;
    }
    public void SetColor(bool offSet)
    {
        defaultColour = offSet ? colour.colourOne : colour.colourTwo;
        highlightColour = offSet ? colour.colourHighlightOne : colour.colourHighlightTwo;
        ChangeColour(defaultColour);
    }
    public void Highlight()
    {
        ChangeColour(highlightColour);
    }
    public void StopHighlight()
    {
        ChangeColour(defaultColour);
    }
    public void StopHighlightUnhover()
    {
        if (!GridManager.Instance.MoveTile(this)) ChangeColour(defaultColour);
    }
    private void UnhighlitghtMoveTile()
    {
        GridManager.Instance.UnhighlightMoveTiles();
    }

    #endregion

    #region Piece Movement

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
    private void SetSelectedPiece(PlayerPiece playerPiece)
    {
        unitManager.SetSelectedPiece(playerPiece);
    }
    private void PieceReplace(BasePiece newPiece)
    {
        OccupiedPiece.enabled = false;
        SetPiece(newPiece);
    }
    private void NextTurn()
    {
        GameManager.Instance.UpdateGameState(GameManager.Instance.TurnUpdate());
    }
    private void PieceMove()
    {
        SetPiece(unitManager.SelectedPiece);
        unitManager.CheckFirstMove();
        SetSelectedPiece(null);
        UnhighlitghtMoveTile();
        NextTurn();
    }
    private void EatEnemyPiece()
    {
        PieceReplace(unitManager.SelectedPiece);
        SetSelectedPiece(null);
        UnhighlitghtMoveTile();
        NextTurn();
    }
    private void SelectPiece()
    {
        if (unitManager.SelectedPiece != null) UnhighlitghtMoveTile();
        SetSelectedPiece((PlayerPiece)OccupiedPiece);
        unitManager.SelectedPiece.PieceMoveHighlight();
    }

    #endregion
}