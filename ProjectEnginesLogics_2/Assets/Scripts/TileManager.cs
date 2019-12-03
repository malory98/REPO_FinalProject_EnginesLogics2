using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileManager : MonoBehaviour
{
    public GameManager gameManager;
    public List<GameObject> tilePrefabs;
    public int[,] grid;
    public TileSO tileSOs;
    public List<GameObject> safeTiles;
    public List<GameObject> bombTiles;

    public void CreateTiles()
    {
        for (int x = 0; x < gameManager.gridSizeX; x++)
        {
            for (int z = 0; z < gameManager.gridSizeZ; z++)
            {

            }// Instanciate tilePrefab with tileSO
        }
    }
    public void SpawnTile(int x, int z)
    {
        //GameObject singleTile = new GameObject("x: " + x + " z: " + z);
        //If edge location on map, use differnt tile
        //GameObject tempObj = myTileCenter;
        //if(IsEdge(x,z))
        //{
        //    tempObj = myTileEdge;
        //}
        GameObject singleTile = Instantiate(tilePrefabs[grid[x, z]]);
        singleTile.transform.position = new Vector3(x, 0, z);
        singleTile.GetComponent<TileLocation>().SetLocation(x, z);
        safeTiles.Add(singleTile);
    }

    public int CalculateTileAmount()
    {
        return gameManager.gridSizeX * gameManager.gridSizeZ;

    }
    public int CaculateBombAmount()
    {
        return (gameManager.gridSizeX * gameManager.gridSizeZ) / 4;
    }

    // Will create tiles and assign an SO to them then place them so they can be placed on the grid.
    public void LoadTileSO(TileSO tileSO)
    {
        tileSO.tilePrefab = gameObject;
        tileSO.adjacentTMP = FindObjectOfType<TextMeshProUGUI>();
        tileSO.spriteHolder = FindObjectOfType<Image>();
        tileSO.tileSprites = gameManager.tileSprites;
    }

}
