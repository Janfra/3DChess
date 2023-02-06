using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] 
    private GameObject infoUI;
    [SerializeField] 
    private int textLimit;
    [SerializeField]
    private Text text;
    [SerializeField]
    private LayerMask piecesOnly;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, piecesOnly))
        {
            if (hit.collider.TryGetComponent(out Tile tile))
            {
                BasePiece pieceInfo = tile.OccupiedPiece;
                if (pieceInfo)
                {
                    DisplayUI("Hovered on " + pieceInfo.GetPieceName());
                }
                else
                {
                    TryDisplaySelected();
                }
            }
        }
        else
        {
            TryDisplaySelected();
        }
    }

    private void TryDisplaySelected()
    {
        if (UnitManager.Instance.SelectedPiece != null)
        {
            DisplayUI("Selected " + UnitManager.Instance.SelectedPiece.GetPieceName());
        }
        else
        {
            DisableUI();
        }
    }

    private void DisplayName(string name)
    {
        if(name.Length > textLimit)
        {
            text.text = name.Substring(0, textLimit);
        }
        else
        {
            text.text = name;
        }
    }

    public void DisplayUI(string name)
    {
        infoUI.SetActive(true);
        DisplayName(name);
    }

    public void DisableUI()
    {
        infoUI.SetActive(false);
        DisplayName("");
    }
}
