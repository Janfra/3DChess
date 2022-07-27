using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    private List<ScriptablePiece> pieces;
    public PlayerPiece SelectedPiece;

    public const float pieceYOffset = 0.8f;
    public const float pieceZOffset = 0.15f;

    private void Awake()
    {
        Instance = this;
        pieces = Resources.LoadAll<ScriptablePiece>("Pieces").ToList();
    }

    public void SpawnPieces(int currentTile, Tile tile)
    {
        BasePiece piecePrefab;
        switch (currentTile)
        {
            case 2:
            case 4:
            case 6:
            case 8:
            case 10:
            case 12:
            case 14:
            case 16:
                piecePrefab = GetPawn();
                if (piecePrefab)
                {
                    piecePrefab = Instantiate(piecePrefab);
                    tile.SetPiece(piecePrefab);
                }
                else
                {
                    Debug.LogWarning("Pawn is null!");
                }
                break;
            default:
                //Debug.LogError("Unknown Piece Type!");
                break;
        }
    }

    private BasePiece GetPawn() 
    {
        foreach (var piece in pieces)
        {
            if(piece.PieceName == Piece.Pawn)
            {
                return piece.typePrefab;
            }
        }
        return null;
    }
    private BasePiece GetKnight()
    {
        return pieces.Where(piece => piece.PieceName == Piece.Knight).First().typePrefab;
    }
    private BasePiece GetBishop()
    {
        return pieces.Where(piece => piece.PieceName == Piece.Bishop).First().typePrefab;
    }
    private BasePiece GetTower()
    {
        return pieces.Where(piece => piece.PieceName == Piece.Tower).First().typePrefab;
    }
    private BasePiece GetQueen()
    {
        return pieces.Where(piece => piece.PieceName == Piece.Queen).First().typePrefab;
    }
    private BasePiece GetKing()
    {
        return pieces.Where(piece => piece.PieceName == Piece.King).First().typePrefab;
    }

    public void SetSelectedPiece(PlayerPiece piece)
    {
        SelectedPiece = piece;
    }
}
