using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RRGridManager : MonoBehaviour   // RAHEEL'S CODE
{
    public GameManager gameManager;
    public static RRGridManager instance;
    public TileSO tileSO;
    public List<TileSO> safeTileSOs;
    public List<GameObject> prefabTiles;

    public int[,] Grid;

    public int gridSizeX;
    public int gridSizeZ;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gridSizeX = gameManager.gridSizeX;
        gridSizeZ = gameManager.gridSizeZ;

        Grid = new int[gridSizeX, gridSizeZ];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                //if (IsEdge(x, z))
                //{
                //    Grid[x, z] = 0;
                //}
                //else
                //{
                //    Grid[x, z] = Random.Range(1, prefabTiles.Count);
                //}
                SpawnTile(x, z);
            }
        }
        foreach(TileSO tileSO in safeTileSOs)
        {
            tileSO.adjacentTMP.gameObject.SetActive(true);
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
        newSO.spriteHolder = newTile.GetComponent<Image>();
        newSO.adjacentTMP = newTile.GetComponent<TextMeshProUGUI>();
        newSO.tileSprites = gameManager.tileSprites;
        // Adds the new SO to a list of tile SOs
        safeTileSOs.Add(newSO);
    }

    public bool IsEdge(int x, int z)
    {
        if (z == 0 || x == 0 || z == gridSizeZ - 1 || x == gridSizeX - 1)
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
