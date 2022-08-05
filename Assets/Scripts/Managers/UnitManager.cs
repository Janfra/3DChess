using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static event OnBeforeFirstMove FirstMove;
    public delegate void OnBeforeFirstMove();
    
    public static UnitManager Instance;
    private List<ScriptablePiece> pieceSO;
    private List<GameObject> pieceList;
    public PlayerPiece SelectedPiece;

    public const float pieceYOffset = 0.8f;
    public const float pieceZOffset = 0.15f;
    // Max move distance is also the same amount of max directions a piece can be moved, so they are shared.
    public const int MaxMoveDistance = 8;

    private void Awake()
    {
        Instance = this;
        pieceSO = Resources.LoadAll<ScriptablePiece>("Pieces").ToList();
        pieceList = new List<GameObject>();
    }

    // Given the tile number and faction, the function will spawn the required piece at the given tile.
    public void SpawnPieces(int currentTile, Tile tile, Faction faction)
    {
        // Unit information
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
                    GeneratePiece(piecePrefab, tile);
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
                    GeneratePiece(piecePrefab, tile);
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
                    GeneratePiece(piecePrefab, tile);
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
                    GeneratePiece(piecePrefab, tile);
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
                    GeneratePiece(piecePrefab, tile);
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
                    GeneratePiece(piecePrefab, tile);
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

    // Return the piece information and give the parameter to define the side (faction).
    private BasePiece GetPawn(Faction faction) 
    {
        if(faction == Faction.White)
        {
            foreach (var piece in pieceSO)
            {
                if(piece.PieceName == Piece.Pawn && piece.faction == faction)
                {
                    return piece.typePrefab;
                }
            }
        } 
        else
        {
            foreach (var piece in pieceSO)
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
        if (faction == Faction.White) return pieceSO.Where(piece => piece.PieceName == Piece.Knight && piece.faction == faction).First().typePrefab;
        else return pieceSO.Where(piece => piece.PieceName == Piece.Knight && piece.faction == faction).First().typePrefab;
    }
    private BasePiece GetBishop(Faction faction)
    {
        BasePiece rv;
        if (faction == Faction.White) rv = pieceSO.Where(piece => piece.PieceName == Piece.Bishop && piece.faction == Faction.White).First().typePrefab;
        else rv = pieceSO.Where(piece => piece.PieceName == Piece.Bishop && piece.faction == faction).First().typePrefab;
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
        if (faction == Faction.White) return pieceSO.Where(piece => piece.PieceName == Piece.Tower && piece.faction == faction).First().typePrefab;
        else return pieceSO.Where(piece => piece.PieceName == Piece.Tower && piece.faction == faction).First().typePrefab;
    }
    private BasePiece GetQueen(Faction faction)
    {
        if (faction == Faction.White) return pieceSO.Where(piece => piece.PieceName == Piece.Queen && piece.faction == faction).First().typePrefab;
        else return pieceSO.Where(piece => piece.PieceName == Piece.Queen && piece.faction == faction).First().typePrefab;
    }
    private BasePiece GetKing(Faction faction)
    {
        if (faction == Faction.White) return pieceSO.Where(piece => piece.PieceName == Piece.King && piece.faction == faction).First().typePrefab;
        else return pieceSO.Where(piece => piece.PieceName == Piece.King && piece.faction == faction).First().typePrefab;
    }

    // Spawns piece with the given information at the given tile and adds it to the 'pieceList' for clearing.
    private void GeneratePiece(BasePiece piecePrefab, Tile tile)
    {
        piecePrefab = Instantiate(piecePrefab);
        piecePrefab.name = piecePrefab.GetPieceName();
        tile.SetPiece(piecePrefab);
        pieceList.Add(piecePrefab.gameObject);
    }

    // Clear (destroy) all pieces in game. Could eventually change it to save them in a dictionary and move them to their new position and reuse them instead of destroying and instantiating again.(Object pooling)
    public void ClearPieces()
    {
        foreach (var piece in pieceList)
        {
            Destroy(piece);
        }
        pieceList.Clear();
    }

    // Sets the clicked piece as the selected piece
    public void SetSelectedPiece(PlayerPiece piece)
    {
        SelectedPiece = piece;
    }

    // Check if its the first move of the selected piece and if there are any left
    public void CheckFirstMove()
    {
        UnitManager.FirstMove?.Invoke();
    }
}
