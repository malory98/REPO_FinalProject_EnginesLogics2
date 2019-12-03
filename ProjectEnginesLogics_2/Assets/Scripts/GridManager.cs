using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridManager : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField]
    private int gridSizeX;
    [SerializeField]
    private int gridSizeZ;
    public int[,] grid;

    public TileSO tileSO;
    public List<TileSO> safeTileSOs;
    public GameObject prefabTile;
    public List<GameObject> spawnedTiles;

    // Flood Fill Algorithm needed


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
        gameManager = FindObjectOfType<GameManager>();
        gridSizeX = gameManager.gridSizeX;
        gridSizeZ = gameManager.gridSizeZ;
        Grid = new int[mapSizeX, mapSizeZ];
        for (int i = 0; i < mapSizeX; i++)
        {
            for (int y = 0; y < mapSizeZ; y++)
            {
                SpawnTile(i, y);
            }
        }
    }

    public void SpawnTile(int x, int z)
    {
        // Instantiate Tile prefab
        GameObject singleTile = Instantiate(prefabTiles[Grid[x, z]]);
        CreateTileSO(x, z, singleTile);
        singleTile.transform.position = new Vector3(x, 0, z);
        singleTile.GetComponent<TileLocation>().SetLocation(x, z);
    }

    // Instantiate TileSOs with x and z coordinates and a tile GameObject
    public void CreateTileSO(int x, int z, GameObject newTile)
    {
        var newSO = Instantiate(tileSO);
        newSO.coordX = x;
        newSO.coordX = z;
        newSO.tilePrefab = newTile;
        newSO.spriteHolder = newTile.gameObject.GetComponent<Image>();
        newSO.adjacentTMP = newTile.gameObject.GetComponent<TextMeshPro>();
        newSO.tileSprites = gameManager.tileSprites;
        // Adds the new SO to a list of tile SOs
        safeTileSOs.Add(newSO);
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
