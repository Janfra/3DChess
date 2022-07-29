using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static event OnBeforeFirstMove FirstMove;
    public static event OnAfterFirstMove NormalMove;
    public delegate void OnBeforeFirstMove();
    public delegate void OnAfterFirstMove();
    
    public static UnitManager Instance;
    private List<ScriptablePiece> pieces;
    public PlayerPiece SelectedPiece;

    public const float pieceYOffset = 0.8f;
    public const float pieceZOffset = 0.15f;
    // Max move distance is also the same amount of max directions a piece can be moved, so they are shared.
    public const int MaxMoveDistance = 8;

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
                    Debug.LogWarning("Pawn in SpawnPieces() is null!");
                }
                break;
            case 5:
            case 11:
                piecePrefab = GetKnight();
                if (piecePrefab)
                {
                    piecePrefab = Instantiate(piecePrefab);
                    tile.SetPiece(piecePrefab);
                }
                else
                {
                    Debug.LogWarning("Knight in SpawnPieces() is null!");
                }
                break;
            case 3:
            case 13:
                piecePrefab = GetBishop();
                if (piecePrefab)
                {
                    piecePrefab = Instantiate(piecePrefab);
                    tile.SetPiece(piecePrefab);
                }
                else
                {
                    Debug.LogWarning("Bishop in SpawnPieces() is null!");
                }
                break;
            case 1:
            case 15:
                piecePrefab = GetTower();
                if (piecePrefab)
                {
                    piecePrefab = Instantiate(piecePrefab);
                    tile.SetPiece(piecePrefab);
                }
                else
                {
                    Debug.LogWarning("Tower in SpawnPieces() is null!");
                }
                break;
            case 9:
                piecePrefab = GetQueen();
                if (piecePrefab)
                {
                    piecePrefab = Instantiate(piecePrefab);
                    tile.SetPiece(piecePrefab);
                }
                else
                {
                    Debug.LogWarning("Queen in SpawnPieces() is null!");
                }
                break;
            case 7:
                piecePrefab = GetKing();
                if (piecePrefab)
                {
                    piecePrefab = Instantiate(piecePrefab);
                    tile.SetPiece(piecePrefab);
                }
                else
                {
                    Debug.LogWarning("King in SpawnPieces() is null!");
                }
                break;
            default:
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

    public void CheckFirstMove()
    {
        UnitManager.FirstMove?.Invoke();
    }
}
