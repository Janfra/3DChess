using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Piece", menuName ="ScriptableObjects/NewPiece")]
public class ScriptablePiece : ScriptableObject
{
    public BasePiece typePrefab;
    public Piece PieceName;
    public Faction faction;
}
public enum Piece
{
    Pawn,
    Knight,
    Bishop,
    Tower,
    Queen,
    King,
}

public enum Faction
{
    Player,
    Enemy,
}
