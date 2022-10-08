using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class PlayerController : MonoBehaviour
{
    public List<Tile> neighbors = new List<Tile>();

    private float moveCooldown = .2f;
    private float nextMoveTime = -10f;

    private void Start()
    {
        neighbors = GetUsefullNeighborTiles(GetPlayerTile());
    }

    private void Update()
    {
        TryMove();
    }

    private void TryMove()
    {
        Tile playerTile = GetPlayerTile();

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D) && Time.time >= nextMoveTime) && TileExists(new Vector2Int(playerTile.x + 1, playerTile.y)) && TW.tiles[new Vector2Int(playerTile.x + 1, playerTile.y)].type != TileType.Wall)
        {
            Move(TW.tiles[new Vector2Int(playerTile.x + 1, playerTile.y)]);
        }
        else if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKey(KeyCode.Q) && Time.time >= nextMoveTime) && TileExists(new Vector2Int(playerTile.x - 1, playerTile.y)) && TW.tiles[new Vector2Int(playerTile.x - 1, playerTile.y)].type != TileType.Wall)
        {
            Move(TW.tiles[new Vector2Int(playerTile.x - 1, playerTile.y)]);
        }
        else if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKey(KeyCode.Z) && Time.time >= nextMoveTime) && TileExists(new Vector2Int(playerTile.x, playerTile.y + 1)) && TW.tiles[new Vector2Int(playerTile.x, playerTile.y + 1)].type != TileType.Wall)
        {
            Move(TW.tiles[new Vector2Int(playerTile.x, playerTile.y + 1)]);
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S) && Time.time >= nextMoveTime) && TileExists(new Vector2Int(playerTile.x, playerTile.y - 1)) && TW.tiles[new Vector2Int(playerTile.x, playerTile.y - 1)].type != TileType.Wall)
        {
            Move(TW.tiles[new Vector2Int(playerTile.x, playerTile.y - 1)]);
        }
    }

    private void Move(Tile tile)
    {
        this.transform.position = new Vector3(tile.x, tile.y, this.transform.position.z);

        List<Tile> newNeighbors = GetUsefullNeighborTiles(tile);

        foreach(Tile t in neighbors)
        {
            if (!newNeighbors.Contains(t)) //t was a neighbor tile but it isn't anymore
            {
                t.ToggleDisplay(false);
            }
        }

        foreach(Tile t in newNeighbors)
        {
            if (!neighbors.Contains(t)) //t wasn't a neighbor tile but it is now
            {
                t.ToggleDisplay(true);
            }
        }
        neighbors = newNeighbors;
        nextMoveTime = Time.time + moveCooldown;
    }
}
