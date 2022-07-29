using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopPiece : PlayerPiece
{
    protected override void SetInRange()
    {
        for (int side = 0; side < 2; side++)
        {
            bool isOffset = DirectionCheck(side);
            SetLeftHorizontalTiles(UnitManager.MaxMoveDistance, isOffset);
            SetRightHorizontalTiles(UnitManager.MaxMoveDistance, isOffset);
        }
    }
}
