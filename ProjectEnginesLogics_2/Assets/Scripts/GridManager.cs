using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    /*public GameManager gameManager;
    [SerializeField]
    private int gridSizeX;
    [SerializeField]
    private int gridSizeZ;
    public int[,] grid;

    private void Start()
    {
        gridSizeX = gameManager.gridSizeX;
        gridSizeZ = gameManager.gridSizeZ;
    }

    public GameObject prefabTile;
    public List<GameObject> spawnedTiles;

    // Flood Fill Algorithm needed
    */

        // STARTS RAHEEL'S PART
    public static GridManager instance;
    public List<GameObject> prefabTiles;

    public int[,] Grid;

    public int mapSizeX = 10;
    public int mapSizeZ = 10;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Grid = new int[mapSizeX, mapSizeZ];
        for (int i = 0; i < mapSizeX; i++)
        {
            for (int y = 0; y < mapSizeZ; y++)
            {
                if (IsEdge(i, y))
                {
                    Grid[i, y] = 0;
                }
                else
                {
                    Grid[i, y] = Random.Range(1, prefabTiles.Count);
                }
                SpawnTile(i, y);
            }
        }
    }

    public void SpawnTile(int x, int z)
    {
        GameObject singleTile = Instantiate(prefabTiles[Grid[x, z]]);
        singleTile.transform.position = new Vector3(x, 0, z);
        singleTile.GetComponent<TileLocation>().SetLocation(x, z);
    }

    public bool IsEdge(int x, int z)
    {
        if (z == 0 || x == 0 || z == mapSizeZ - 1 || x == mapSizeX - 1)
        {
            return true;
        }
        return false;
    }

    public void TellMyLocation(Vector3 tileLocation)
    {
        print(" ------ >" + tileLocation);
    }

}
