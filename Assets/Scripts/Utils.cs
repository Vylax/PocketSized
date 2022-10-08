using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static TileWorld TW => GameObject.FindGameObjectWithTag("GameManager").GetComponent<TileWorld>();

    public static Tile GetTile(Vector3 v)
    {
        Vector2Int temp = new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
        return TileExists(temp) ? TW.tiles[temp] : null;
    }

    public static Tile GetCursorTile()
    {
        return GetTile(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public static Tile GetPlayerTile()
    {
        return GetTile(GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    public static bool IsWithin(float a, float x, float b, bool inclusive = true)
    {
        return (a < x && x < b) || inclusive && (a == x || x == b); 
    }

    public static bool TileExists(Vector3 v)
    {
        return IsWithin(0, v.x, TW.worldWidth - 1) && IsWithin(0, v.y, TW.worldHeight - 1);
    }
    public static bool TileExists(Vector2Int v)
    {
        return IsWithin(0, v.x, TW.worldWidth - 1) && IsWithin(0, v.y, TW.worldHeight - 1);
    }

    public static List<Tile> GetNeighborTiles(Tile tile)
    {
        List<Tile> list = new List<Tile>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if((i != 0 || j != 0) && Mathf.Abs(i)+Mathf.Abs(j)<=1 && TileExists(new Vector2Int(tile.x + i, tile.y + j)))
                {
                    list.Add(TW.tiles[new Vector2Int(tile.x + i, tile.y + j)]);
                }
            }
        }
        return list;
    }

    public static List<Tile> GetUsefullNeighborTiles(Tile tile)
    {
        List<Tile> list = GetNeighborTiles(tile);
        List<Tile> templist = new List<Tile>();
        foreach (Tile t in list)
        {
            if (t.type != TileType.Wall)
            {
                templist.Add(t);
            }
        }
        return templist;
    }

    public enum TileType
    {
        Wall,
        Walkable,
        Locked,
        Interactible
    }

    private static Dictionary<TileType, Color> TileColor = new Dictionary<TileType, Color>()
    {
        { TileType.Walkable, Color.white.WithAlpha(.5f)},
        { TileType.Locked, Color.red.WithAlpha(.5f)},
        { TileType.Interactible, Color.green.WithAlpha(.5f)},
    };

    [Serializable]
    public class Tile
    {
        public int x;
        public int y;

        public TileType type;

        public GameObject obj;
        private bool isDisplayed = false;

        public Tile(int x, int y, TileType type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
            SpawnTile(x,y);
        }

        public Tile(Vector2Int v, TileType type)
        {
            new Tile(v.x, v.y, type);
        }

        public Vector2Int Vector2Int { get { return new Vector2Int(this.x, this.y); } }

        private void SpawnTile(int x, int y)
        {
            this.obj = Instantiate(Resources.Load<GameObject>("Prefabs/GridCell"), new Vector3(x, y, 0), Quaternion.identity);
            this.obj.transform.name = $"Grid Cell {x}|{y}";
            this.obj.transform.parent = GameObject.FindGameObjectWithTag("Grid").transform;
            this.obj.GetComponent<SpriteRenderer>().enabled = isDisplayed;
        }

        public override string ToString()
        {
            return "(" + this.x + "," + this.y + ")";
        }

        public void ToggleDisplay(bool display)
        {
            if(display == isDisplayed || this.type == TileType.Wall && !isDisplayed)
            {
                return;
            }

            isDisplayed = display;
            if (display)
            {
                this.obj.GetComponent<SpriteRenderer>().color = TileColor[this.type];
            }
            this.obj.GetComponent<SpriteRenderer>().enabled = display; //IDEA: maybe add smooth transition animation later on
        }
    }
}
