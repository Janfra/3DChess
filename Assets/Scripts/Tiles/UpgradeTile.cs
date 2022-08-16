using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTile : Tile
{
    [SerializeField] private PawnUpgrade upgrade;

    public void SetUpgrade(PawnUpgrade newUpgrade)
    {
        upgrade = newUpgrade;
    }

    public override void SetPiece(BasePiece piece)
    {
        base.SetPiece(piece);
        CheckUpgrade();
    }

    private void CheckUpgrade()
    {
        if(OccupiedPiece.GetPieceName() == Piece.Pawn.ToString())
        {
            OccupiedPiece.gameObject.SetActive(false);
            upgrade.SetUpgradeTile(this);
            GameManager.Instance.UpdateGameState(GameState.PawnUpgrade);
        }
    }
}
