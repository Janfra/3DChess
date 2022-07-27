using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int width, lenght;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;

    private Dictionary<Vector3, Tile> tiles;
    private List<Tile> highlightedTiles;

    private const float yPos = -4.5f;
    private const float zOffset = 3.5f;
    public const int TileDistance = 1;
    // Max move distance is also the same amount of max directions a piece can be moved, so they are shared.
    public const int MaxMoveDistance = 8;


    private void Awake()
    {
        Instance = this;
        highlightedTiles = new List<Tile>();
    }
    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector3, Tile>();
        int tileNumber = 0;

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < lenght; z++)
            {
                Tile currentTile = GenerateTile(x, z);
                if(z <= 1 && currentTile != null)
                {
                    tileNumber++;
                    UnitManager.Instance.SpawnPieces(tileNumber, currentTile);
                }
            }
        }

        cam.transform.position = new Vector3((float)width / 2 - 0.5f, 5, -10);

        GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
    }
    public Tile GetTileInfo()
    {
        return null;
    }

    private void SetTileColour(Tile tile, float xPos, float zPos)
    {
        var offset = (xPos + zPos) % 2 == 1;
        tile.SetColor(offset);
    }
    private Tile GenerateTile(int x, int z)
    {
        Vector3 tilePosition = new Vector3(x, yPos, z - zOffset);

        var spawnedTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
        spawnedTile.name = $"Tile {x} {z}";

        SetTileColour(spawnedTile, x, z);

        tiles.Add(tilePosition, spawnedTile);
        return spawnedTile;
    }
    public Tile GetTileAtPosition(Vector3 pos)
    {
        if (tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public void HighlightTile(Tile tile)
    {
        highlightedTiles.Add(tile);
        tile.Highlight();
    }
    public void UnhighlightMoveTiles()
    {
        foreach (var tile in highlightedTiles)
        {
            tile.StopHighlight();
        }
        highlightedTiles.Clear();
    }
    public bool MoveTile(Tile tile)
    {
        if (highlightedTiles.Contains(tile)) return true;
        return false;
    }
}
