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

    public void SpawnPieces(int currentTile, Tile tile, Faction faction)
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
                piecePrefab = GetPawn(faction);
                if (piecePrefab)
                {
                    piecePrefab = Instantiate(piecePrefab);
                    tile.SetPiece(piecePrefab);
                }
                else
                {
                    DebugHelper.NullWarn("Pawn", "SpawnPieces()");
                }
                break;
            case 5:
            case 11:
                piecePrefab = GetKnight(faction);
                if (piecePrefab)
                {
                    piecePrefab = Instantiate(piecePrefab);
                    tile.SetPiece(piecePrefab);
                }
                else
                {
                    DebugHelper.NullWarn("Knight", "SpawnPieces()");
                }
                break;
            case 3:
            case 13:
                piecePrefab = GetBishop(faction);
                if (piecePrefab)
                {
                    piecePrefab = Instantiate(piecePrefab);
                    tile.SetPiece(piecePrefab);
                }
                else
                {
                    DebugHelper.NullWarn("Bishop", "SpawnPieces()");
                }
                break;
            case 1:
            case 15:
                piecePrefab = GetTower(faction);
                if (piecePrefab)
                {
                    piecePrefab = Instantiate(piecePrefab);
                    tile.SetPiece(piecePrefab);
                }
                else
                {
                    DebugHelper.NullWarn("Tower", "SpawnPieces()");
                }
                break;
            case 9:
                piecePrefab = GetQueen(faction);
                if (piecePrefab)
                {
                    piecePrefab = Instantiate(piecePrefab);
                    tile.SetPiece(piecePrefab);
                }
                else
                {
                    DebugHelper.NullWarn("Queen", "SpawnPieces()");
                }
                break;
            case 7:
                piecePrefab = GetKing(faction);
                if (piecePrefab)
                {
                    piecePrefab = Instantiate(piecePrefab);
                    tile.SetPiece(piecePrefab);
                }
                else
                {
                    DebugHelper.NullWarn("King", "SpawnPieces()");
                }
                break;
            default:
                break;
        }
    }

    private BasePiece GetPawn(Faction faction) 
    {
        if(faction == Faction.White)
        {
            foreach (var piece in pieces)
            {
                if(piece.PieceName == Piece.Pawn && piece.faction == faction)
                {
                    return piece.typePrefab;
                }
            }
        } 
        else
        {
            foreach (var piece in pieces)
            {
                if(piece.PieceName == Piece.Pawn && piece.faction == faction)
                {
                    return piece.typePrefab;
                }
            }
        }
        return null;
    }
    private BasePiece GetKnight(Faction faction)
    {
        if (faction == Faction.White) return pieces.Where(piece => piece.PieceName == Piece.Knight && piece.faction == faction).First().typePrefab;
        else return pieces.Where(piece => piece.PieceName == Piece.Knight && piece.faction == faction).First().typePrefab;
    }
    private BasePiece GetBishop(Faction faction)
    {
        BasePiece rv;
        if (faction == Faction.White) rv = pieces.Where(piece => piece.PieceName == Piece.Bishop && piece.faction == Faction.White).First().typePrefab;
        else rv = pieces.Where(piece => piece.PieceName == Piece.Bishop && piece.faction == faction).First().typePrefab;
        if (rv)
        {
            return rv;
        }
        else
        {
            return null;
        }
    }
    private BasePiece GetTower(Faction faction)
    {
        if (faction == Faction.White) return pieces.Where(piece => piece.PieceName == Piece.Tower && piece.faction == faction).First().typePrefab;
        else return pieces.Where(piece => piece.PieceName == Piece.Tower && piece.faction == faction).First().typePrefab;
    }
    private BasePiece GetQueen(Faction faction)
    {
        if (faction == Faction.White) return pieces.Where(piece => piece.PieceName == Piece.Queen && piece.faction == faction).First().typePrefab;
        else return pieces.Where(piece => piece.PieceName == Piece.Queen && piece.faction == faction).First().typePrefab;
    }
    private BasePiece GetKing(Faction faction)
    {
        if (faction == Faction.White) return pieces.Where(piece => piece.PieceName == Piece.King && piece.faction == faction).First().typePrefab;
        else return pieces.Where(piece => piece.PieceName == Piece.King && piece.faction == faction).First().typePrefab;
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
