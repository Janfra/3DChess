using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static event Action FirstMove;

    [SerializeField] private Castling castling;
    [SerializeField] private PawnUpgrade pawnUpgrade;
    private List<PieceInformation> pieceList;
    private List<ScriptablePiece> pieceSOList;
    public static UnitManager Instance;
    public PlayerPiece SelectedPiece;

    #region Constants

    public const float pieceYOffset = 0.8f;
    public const float pieceZOffset = 0.15f;
    // Max move distance is also the same amount of max directions a piece can be moved, so they are shared.
    public const int MaxMoveDistance = 8;

    #endregion

    [Serializable]
    private class PieceInformation
    {
        public PieceInformation(GameObject newObj, BasePiece newPiece)
        {
            PieceObj = newObj;
            TypeInfo = newPiece;
            Faction = TypeInfo.GetFaction();
            TypeName = TypeInfo.GetPieceName();
        }

        public string TypeName;
        public GameObject PieceObj;
        public BasePiece TypeInfo;
        public Faction Faction;
    }

    private void Awake()
    {
        Instance = this;
        pieceSOList = Resources.LoadAll<ScriptablePiece>("Pieces").ToList();
        pieceList = new List<PieceInformation>();
    }



    #region Generation

    // Given the tile number and faction, the function will spawn the required piece at the given tile.
    public void SpawnPieces(int currentTile, Tile tile, Faction faction)
    {
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
                GeneratePawn(tile, faction);
                break;
            case 5:
            case 11:
                GenerateBishop(tile, faction);
                break;
            case 3:
            case 13:
                GenerateKnight(tile, faction);
                break;
            case 1:
            case 15:
                GenerateTower(tile, faction, currentTile);
                break;
            case 7:
                GenerateQueen(tile, faction);
                break;
            case 9:
                GenerateKing(tile, faction);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Generation of specific type of piece. 
    /// </summary>
    /// <param name="tile">Sets the position it will be spawned on</param>
    /// <param name="faction">Sets the side/colour the piece will be part of</param>
    private void GeneratePawn(Tile tile, Faction faction)
    {
        // Unit information
        BasePiece piecePrefab = GetPawn(faction);
        if (piecePrefab)
        {
            GeneratePiece(piecePrefab, tile);
        }
        else
        {
            DebugHelper.NullWarn("Pawn", "SpawnPieces()");
        }
    }
    private void GenerateBishop(Tile tile, Faction faction)
    {
        BasePiece piecePrefab = GetBishop(faction);
        if (piecePrefab)
        {
            GeneratePiece(piecePrefab, tile);
        }
        else
        {
            DebugHelper.NullWarn("Bishop", "SpawnPieces()");
        }
    }
    private void GenerateKnight(Tile tile, Faction faction)
    {
        BasePiece piecePrefab = GetKnight(faction);
        if (piecePrefab)
        {
            GeneratePiece(piecePrefab, tile);
        }
        else
        {
            DebugHelper.NullWarn("Knight", "SpawnPieces()");
        }
    }
    private void GenerateTower(Tile tile, Faction faction, int side)
    {
        BasePiece piecePrefab = GetTower(faction);
        if (piecePrefab)
        {
            GeneratePiece(piecePrefab, tile, side);
        }
        else
        {
            DebugHelper.NullWarn("Tower", "SpawnPieces()");
        }
    }
    private void GenerateQueen(Tile tile, Faction faction)
    {
        BasePiece piecePrefab = GetQueen(faction);
        if (piecePrefab)
        {
            GeneratePiece(piecePrefab, tile);
        }
        else
        {
            DebugHelper.NullWarn("Queen", "SpawnPieces()");
        }
    }
    private void GenerateKing(Tile tile, Faction faction)
    {
        BasePiece piecePrefab = GetKing(faction);
        if (piecePrefab)
        {
            GeneratePiece(piecePrefab, tile);
        }
        else
        {
            DebugHelper.NullWarn("King", "SpawnPieces()");
        }
    }

    /// <summary>
    /// Spawns any piece and adds it to the 'pieceList' for clearing.
    /// </summary>
    /// <param name="piecePrefab">Decides the type of piece it will be</param>
    /// <param name="tile">Sets the position it will be spawned on</param>
    private void GeneratePiece(BasePiece piecePrefab, Tile tile)
    {
        BasePiece tempPiece = Instantiate(piecePrefab);
        tempPiece.name = tempPiece.GetPieceName();
        tile.InitializePiece(tempPiece);
        pieceList.Add(new PieceInformation(gameObject, tempPiece));
    }

    /// <summary>
    /// Overloaded version for setting the Tower direction for castling
    /// </summary>
    /// <param name="piecePrefab">Decides the type of piece it will be</param>
    /// <param name="tile">Sets the position it will be spawned on</param>
    /// <param name="currentTile">Sets the direction of the castling</param>
    private void GeneratePiece(BasePiece piecePrefab, Tile tile, int currentTile)
    {
        BasePiece tempPiece = Instantiate(piecePrefab);
        tempPiece.name = tempPiece.GetPieceName();
        tile.InitializePiece(tempPiece);
        pieceList.Add(new PieceInformation(gameObject, tempPiece));
        TowerPiece tower = (TowerPiece)tempPiece;
        tower.SetCastlingDirection(currentTile);
    }

    /// <summary>
    /// Clears the board for restarting
    /// </summary>
    public void ClearPieces()
    {
        foreach (var piece in pieceList)
        {
            Destroy(piece.PieceObj);
        }
        pieceList.Clear();
    }

    #endregion

    #region Piece Type Getters

    public ScriptablePiece GetQueenSO(Faction faction)
    {
        return pieceSOList.Where(piece => piece.PieceName == Piece.Queen && piece.faction == faction).First();
    }

    // Return the piece information and give the parameter to define the side (faction).
    private BasePiece GetPawn(Faction faction) 
    {
        if(faction == Faction.White)
        {
            foreach (var piece in pieceSOList)
            {
                if(piece.PieceName == Piece.Pawn && piece.faction == faction)
                {
                    return piece.typePrefab;
                }
            }
        } 
        else
        {
            foreach (var piece in pieceSOList)
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
        if (faction == Faction.White) return pieceSOList.Where(piece => piece.PieceName == Piece.Knight && piece.faction == faction).First().typePrefab;
        else return pieceSOList.Where(piece => piece.PieceName == Piece.Knight && piece.faction == faction).First().typePrefab;
    }
    private BasePiece GetBishop(Faction faction)
    {
        BasePiece rv;
        if (faction == Faction.White) rv = pieceSOList.Where(piece => piece.PieceName == Piece.Bishop && piece.faction == Faction.White).First().typePrefab;
        else rv = pieceSOList.Where(piece => piece.PieceName == Piece.Bishop && piece.faction == faction).First().typePrefab;
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
        if (faction == Faction.White) return pieceSOList.Where(piece => piece.PieceName == Piece.Tower && piece.faction == faction).First().typePrefab;
        else return pieceSOList.Where(piece => piece.PieceName == Piece.Tower && piece.faction == faction).First().typePrefab;
    }
    private BasePiece GetQueen(Faction faction)
    {
        if (faction == Faction.White) return pieceSOList.Where(piece => piece.PieceName == Piece.Queen && piece.faction == faction).First().typePrefab;
        else return pieceSOList.Where(piece => piece.PieceName == Piece.Queen && piece.faction == faction).First().typePrefab;
    }
    private BasePiece GetKing(Faction faction)
    {
        if (faction == Faction.White) return pieceSOList.Where(piece => piece.PieceName == Piece.King && piece.faction == faction).First().typePrefab;
        else return pieceSOList.Where(piece => piece.PieceName == Piece.King && piece.faction == faction).First().typePrefab;
    }

    #endregion

    // Sets the clicked piece as the selected piece
    public void SetSelectedPiece(PlayerPiece piece)
    {
        SelectedPiece = piece;
    }

    // Check if its the first move of the selected piece and if there are any left
    public void CheckFirstMove()
    {
        FirstMove?.Invoke();
    }

    #region Castling Methods
    public void SetCastlingPieces()
    {
        SetTowersForCastling();
        SetKingsForCastling();
    }

    private void SetTowersForCastling()
    {
        foreach (var piece in pieceList)
        {
            if (piece.TypeName == Piece.Tower.ToString())
            {
                castling.SetTowers((TowerPiece)piece.TypeInfo);
            }
        }
    }

    private void SetKingsForCastling()
    {
        foreach (var piece in pieceList)
        {
            if (piece.TypeName == Piece.King.ToString())
            {
                castling.SetKings((KingPiece)piece.TypeInfo);
            }
        }
    }

    public void SetCastlingTiles(Tile tile)
    {
        castling.SetCastlingTiles(tile);
    }

    public bool CastlingTileCheck(Tile tile)
    {
        return castling.CheckCastlingTiles(tile);
    }

    public void CastlingAttempt(Castling.KingMoveDirection direction)
    {
        if (castling.CastlingCheck())
        {
            castling.CastleMove(direction);
        }
        else
        {

        }
    }

    public void KingMoved(Faction faction)
    {
        castling.DisableCastling(faction);
    }

    public Tile GetTowerCastling(Castling.KingMoveDirection direction)
    {
        return castling.GetTowerCastling(direction);
    }

    public Castling.KingMoveDirection GetTileDirection(Tile tile)
    {
        return castling.GetTileDirection(tile);
    }

    #endregion

    #region PawnUpgrade Methods

    public void CheckUpgrade(Tile tile)
    {
        pawnUpgrade.CheckUpgrade(tile);
    }

    public void SetUpgradeTiles(Tile tile, int zPos)
    {
        pawnUpgrade.SetUpgradeTiles(tile, zPos);
    }

    #endregion
}
