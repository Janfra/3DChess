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
    Tower,
    Knight,
    Bishop,
    Queen,
    King,
}

public enum Faction
{
    White,
    Black,
}
