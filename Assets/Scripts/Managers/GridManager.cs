using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int width, lenght;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Tile upgradeTilePrefab;
    [SerializeField] private PawnUpgrade pawnUpgrade;

    private Dictionary<Vector3, Tile> tiles;
    private List<Tile> highlightedTiles;

    private const float yPos = -4.5f;
    private const float zOffset = 3.5f;
    public const int TileDistance = 1;

    // Singleton and initialize
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Tried to duplicate singleton!");
        }
        highlightedTiles = new List<Tile>();
    }

    public int GetGridWidth()
    {
        return width;
    }

    public int GetGridLenght()
    {
        return lenght;
    }

    public Transform GetGridCenter()
    {
        GameObject emtpyGO = new GameObject();
        emtpyGO.transform.position = new Vector3(width / 2 - 0.5f, -4.5f, 1);
        emtpyGO.name = "GridCenter";
        return emtpyGO.transform;
    }

    #region GridGen

    // Creates the board and pieces, then center the camera and update state.
    public void GenerateGrid()
    {
        UnitManager unitManager = UnitManager.Instance;
        tiles = new Dictionary<Vector3, Tile>();
        int tileNumber = 0;

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < lenght; z++)
            {
                Tile currentTile = GenerateTile(x, z);
                if(z <= 1 && currentTile)
                {
                    tileNumber++;
                    Debug.Log($"After going up number: {tileNumber}, iteration X: {x}");
                    unitManager.SpawnPieces(tileNumber, currentTile, Faction.White);
                }
                else if (z == 6 && currentTile)
                {
                    unitManager.SpawnPieces(tileNumber, currentTile, Faction.Black);
                } 
                else if(z == 7 && currentTile)
                {
                    unitManager.SpawnPieces(tileNumber - 1, currentTile, Faction.Black);
                }
            }
        }

        unitManager.SetCastlingPieces();

        GameManager.Instance.UpdateGameState(GameState.WhiteTurn);
    }

    // Create the tile using the position given with the parameters, set it, return the tile.
    private Tile GenerateTile(int x, int z)
    {
        Tile spawnedTile;
        Vector3 tilePosition = new Vector3(x, yPos, z - zOffset);
        if(z == 0 || z == lenght - 1)
        {
            spawnedTile = Instantiate(upgradeTilePrefab, tilePosition, Quaternion.identity);
            UpgradeTile setUpgrade = (UpgradeTile)spawnedTile;
            setUpgrade.SetUpgrade(pawnUpgrade);
        }
        else
        {
            spawnedTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity); 
        }
        spawnedTile.name = $"Tile {x} {z}";

        SetTileColour(spawnedTile, x, z);

        tiles.Add(tilePosition, spawnedTile);
        return spawnedTile;
    }

    // Give it a tile, depending on its position, give it a colour. 
    private void SetTileColour(Tile tile, float xPos, float zPos)
    {
        var offset = (xPos + zPos) % 2 == 1;
        tile.SetColor(offset);
    }

    // Resets the board without generating the tiles again.
    public void ResetGrid()
    {
        UnitManager unitManager = UnitManager.Instance;
        unitManager.ClearPieces();
        int tileNumber = 0;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < lenght; z++)
            {
                Tile currentTile = GetTileAtPosition(new Vector3(x, yPos, z - zOffset));
                if (z <= 1 && currentTile)
                {
                    tileNumber++;
                    Debug.Log($"After going up number: {tileNumber}, iteration X: {x}");
                    unitManager.SpawnPieces(tileNumber, currentTile, Faction.White);
                }
                else if (z == 6 && currentTile)
                {
                    unitManager.SpawnPieces(tileNumber, currentTile, Faction.Black);
                }
                else if (z == 7 && currentTile)
                {
                    unitManager.SpawnPieces(tileNumber - 1, currentTile, Faction.Black);
                }
            }
        }
        GameManager.Instance.UpdateGameState(GameState.WhiteTurn);
    }

    #endregion

    #region Tile Highlight & Setting

    // Using the given position return the tile from the dictonary
    public Tile GetTileAtPosition(Vector3 pos)
    {
        if (tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    // Highlight given tile and add it to the highlited list
    public void HighlightTile(Tile tile)
    {
        highlightedTiles.Add(tile);
        tile.Highlight();
    }

    // Stop highliting all tiles in the highlight list, reset list
    public void UnhighlightMoveTiles()
    {
        foreach (var tile in highlightedTiles)
        {
            tile.StopHighlight();
            tile.isInRange = false;
        }
        highlightedTiles.Clear();
    }

    #endregion

    // Check if the tile given is inside the highlight list, used to not stop highlighting tiles once unhovered
    public bool MoveTileCheck(Tile tile)
    {
        if (highlightedTiles.Contains(tile)) return true;
        return false;
    }

    // Check if the list containing the tiles has been created, if not, generate the board
    public bool BoardGenCheck()
    {
        if(tiles != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
