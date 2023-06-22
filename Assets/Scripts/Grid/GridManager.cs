using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;

    private GameController _game;
    private Vector2 _levelSize;
    private Dictionary<Vector2, Tile> _tiles;

    void Awake()
    {
        _game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _levelSize = _game.GetLevelSize();
    }

    void Start()
    {
        GenerateGrid();
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out Tile tile))
        {
            return tile;
        }
        return null;
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        int width = (int) _levelSize.x;
        int height = (int) _levelSize.y;
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                Tile spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x}, {y}";
                bool isOffset = (x + y) % 2 == 1;
                spawnedTile.Init(isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
    }


}
