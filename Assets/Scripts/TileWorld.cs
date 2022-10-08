using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class TileWorld : MonoBehaviour
{
    public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    public int worldWidth = 22;
    public int worldHeight = 22;
    public int gridCellZ = 0;

    public GameObject gridCell;

    public List<Vector2Int> walkableTiles = new List<Vector2Int>();

    private void Start()
    {
        tiles = new Dictionary<Vector2Int, Tile>();

        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldHeight; y++)
            {
                tiles.Add(new Vector2Int(x,y), new Tile(x,y, walkableTiles.Contains(new Vector2Int(x, y)) ? TileType.Walkable : TileType.Wall));
                
                //debug
                /*
                if(walkableTiles.Contains(new Vector2Int(x, y)))
                {
                    tiles[new Vector2Int(x, y)].ToggleDisplay(true);
                }*/
            }
        }
    }
    private void Update()
    {
        //DEBUG
        /*if (Input.GetButtonDown("Fire1"))
        {
            Tile tile = GetCursorTile();
            if(tile != null)
            {
                tile.type = TileType.Walkable;
                tile.ToggleDisplay(true);
                walkableTiles.Add(tile.Vector2Int);
            }
        }*/
    }

    public void ToggleCellDisplay(int x, int y, bool display)
    {
        tiles[new Vector2Int(x, y)].ToggleDisplay(display);
    }

    private void OnGUI()
    {
        Tile CursorTile = GetCursorTile();
        Tile PlayerTile = GetPlayerTile();
        GUILayout.Box($"Cursor Tile: ({(CursorTile != null ? CursorTile.x : "")},{(CursorTile != null ? CursorTile.y : "")})\nPlayer Tile : ({PlayerTile.x},{PlayerTile.y})");
        GUILayout.Box($"horizontal: ({Input.GetAxisRaw("Horizontal")* Input.GetAxis("Horizontal")>=0})");
    }
}
