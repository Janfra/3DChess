using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static event Action FirstMove;

    [SerializeField] private Castling castling;
    private List<PieceInformation> pieceList;
    public static UnitManager Instance;
    private ObjectPooler objectPooler;
    public PlayerPiece SelectedPiece;

    #region Constants

    public const float pieceYOffset = 0.8f;
    // Max move distance is also the same amount of max directions a piece can be moved, so they are shared.
    public const int MaxMoveDistance = 8;

    #endregion

    [Serializable]
    private class PieceInformation
    {
        public PieceInformation(BasePiece newPiece)
        {
            TypeInfo = newPiece;
            Faction = TypeInfo.GetFaction();
            TypeName = TypeInfo.GetPieceName();
        }

        public string TypeName;
        public BasePiece TypeInfo;
        public Faction Faction;
    }

    private void Awake()
    {
        Instance = this;
        pieceList = new List<PieceInformation>();
    }
    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
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
                GenerateBishop(tile, faction, currentTile);
                break;
            case 3:
            case 13:
                GenerateKnight(tile, faction, currentTile);
                break;
            case 1:
            case 15:
                GenerateTower(tile, faction, currentTile);
                break;
            case 7:
                GenerateQueen(tile, faction, currentTile);
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
        BasePiece piecePrefab;
        if (faction == Faction.Black)
        {
            piecePrefab = objectPooler.SpawnPiece(ObjectPooler.poolObjName.BlackPawn);
        }
        else
        {
            piecePrefab = objectPooler.SpawnPiece(ObjectPooler.poolObjName.WhitePawn);
        }
        if (piecePrefab)
        {
            PieceGenerated(tile, piecePrefab);
        }
        else
        {
            DebugHelper.NullWarn("Pawn", "SpawnPieces()");
        }
    }
    private void GenerateBishop(Tile tile, Faction faction, int side)
    {
        BasePiece piecePrefab;
        if (faction == Faction.Black)
        {
            piecePrefab = objectPooler.SpawnPiece(ObjectPooler.poolObjName.BlackBishop);
        }
        else
        {
            piecePrefab = objectPooler.SpawnPiece(ObjectPooler.poolObjName.WhiteBishop);
        }
        if (piecePrefab)
        {
            PieceGenerated(tile, piecePrefab);
            castling.SetCastlingTiles(tile, GetCastlingDirection(side), faction);
        }
        else
        {
            DebugHelper.NullWarn("Bishop", "SpawnPieces()");
        }
    }
    private void GenerateKnight(Tile tile, Faction faction, int side)
    {
        BasePiece piecePrefab;
        if (faction == Faction.Black)
        {
            piecePrefab = objectPooler.SpawnPiece(ObjectPooler.poolObjName.BlackKnight);
        }
        else
        {
            piecePrefab = objectPooler.SpawnPiece(ObjectPooler.poolObjName.WhiteKnight);
        }
        if (piecePrefab)
        {
            PieceGenerated(tile, piecePrefab);
            castling.SetCastlingTiles(tile, GetCastlingDirection(side), faction);
        }
        else
        {
            DebugHelper.NullWarn("Knight", "SpawnPieces()");
        }
    }
    private void GenerateTower(Tile tile, Faction faction, int side)
    {
        BasePiece piecePrefab;
        if (faction == Faction.Black)
        {
            piecePrefab = objectPooler.SpawnPiece(ObjectPooler.poolObjName.BlackTower);
        }
        else
        {
            piecePrefab = objectPooler.SpawnPiece(ObjectPooler.poolObjName.WhiteTower);
        }
        if (piecePrefab)
        {
            PieceGenerated(tile, piecePrefab);
            TowerPiece tower = (TowerPiece)piecePrefab;
            tower.castlingDirection = GetCastlingDirection(side);
        }
        else
        {
            DebugHelper.NullWarn("Tower", "SpawnPieces()");
        }
    }
    private void GenerateQueen(Tile tile, Faction faction, int side)
    {
        BasePiece piecePrefab;
        if (faction == Faction.Black)
        {
            piecePrefab = objectPooler.SpawnPiece(ObjectPooler.poolObjName.BlackQueen);
        }
        else
        {
            piecePrefab = objectPooler.SpawnPiece(ObjectPooler.poolObjName.WhiteQueen);
        }
        if (piecePrefab)
        {
            PieceGenerated(tile, piecePrefab);
            castling.SetCastlingTiles(tile, GetCastlingDirection(side), faction);
        }
        else
        {
            DebugHelper.NullWarn("Queen", "SpawnPieces()");
        }
    }
    private void GenerateKing(Tile tile, Faction faction)
    {
        BasePiece piecePrefab;
        if (faction == Faction.Black)
        {
            piecePrefab = objectPooler.SpawnPiece(ObjectPooler.poolObjName.BlackKing);
        }
        else
        {
            piecePrefab = objectPooler.SpawnPiece(ObjectPooler.poolObjName.WhiteKing);
        }
        if (piecePrefab)
        {
            PieceGenerated(tile, piecePrefab);
        }
        else
        {
            DebugHelper.NullWarn("King", "SpawnPieces()");
        }
    }
    private void PieceGenerated(Tile tile, BasePiece piece)
    {
        tile.InitializePiece(piece);
        pieceList.Add(new PieceInformation(piece));
    }

    /// <summary>
    /// Clears the board for restarting
    /// </summary>
    public void ClearPieces()
    {
        objectPooler.DespawnAll();
        foreach (var piece in pieceList)
        {
            piece.TypeInfo.OcuppiedTile.OccupiedPiece = null;
            piece.TypeInfo.OcuppiedTile = null;
        }
        pieceList.Clear();
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

    public bool KingCastlingCheck(Tile tile)
    {
        return (castling.CheckCastlingTiles(tile) && castling.TowerCheck(GetTileDirection(tile)));
    }

    public void CastlingAttempt(Castling.KingMoveDirection direction)
    {
        if (castling.CastlingCheck(direction))
        {
            castling.StartCastling(direction);
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

    // If the tile of generation is lower than half of the grids width, is going to castle to the right, otherwise left.
    public Castling.KingMoveDirection GetCastlingDirection(int side)
    {
        if (side < (GridManager.Instance.GetGridWidth() / 2))
        {
            return Castling.KingMoveDirection.Left;
        }
        else
        {
            return Castling.KingMoveDirection.Right;
        }
    }

    public bool CheckTowersLeft()
    {
        return castling.CheckTowersLeft();
    }

    public bool CheckKingCastling()
    {
        return castling.CheckKingCastling();
    }

    #endregion

    public void AddToPieceList(BasePiece piece)
    {
        pieceList.Add(new PieceInformation(piece));
    }
}
