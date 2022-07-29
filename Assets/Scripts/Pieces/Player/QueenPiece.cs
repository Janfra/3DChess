using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenPiece : PlayerPiece
{
    protected override void SetInRange()
    {
        for (int side = 0; side < 2; side++)
        {
            bool isOffset = DirectionCheck(side);
            SetLeftHorizontalTiles(UnitManager.MaxMoveDistance, isOffset);
            SetRightHorizontalTiles(UnitManager.MaxMoveDistance, isOffset);
            SetXTiles(UnitManager.MaxMoveDistance, isOffset);
            SetZTiles(UnitManager.MaxMoveDistance, isOffset);
        }
    }
}
