using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodCheck : MonoBehaviour
{
    public GameManager gameManager;
    public GridManager gridManager;

    public int gridSizeX;
    public int gridSizeZ;

    public List<TileSO> safeTileSOs;
    public List<TileSO> bombTileSOs;

    public void Initialize()
    {
        gameManager = FindObjectOfType<GameManager>();
        gridManager = FindObjectOfType<GridManager>();

        gridSizeX = gameManager.gridSizeX;
        gridSizeZ = gameManager.gridSizeZ;
    }

    public void NeighbourAssigner()
    {
        safeTileSOs = gridManager.safeTileSOs;
        TileSO tempSO = new TileSO();
        int tempX;
        int tempZ;
        foreach (TileSO singleTile in safeTileSOs)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    tempX = singleTile.coordX + x;
                    tempZ = singleTile.coordZ + z;
                    if (tempX >= 0 && tempX < gridSizeX && tempZ >= 0 && tempZ < gridSizeZ)
                    {
                        foreach (TileSO adjacentTile in safeTileSOs)
                        {
                            if (adjacentTile.coordX == tempX && adjacentTile.coordZ == tempZ && adjacentTile != singleTile)
                            {
                                singleTile.neighbours.Add(adjacentTile);
                            }
                        }
                    }
                }
            }
        }
    }
    public bool FloodChecker()
    {
        safeTileSOs = gridManager.safeTileSOs;
        bombTileSOs = gridManager.bombTileSOs;
        List<TileSO> tileQueue = new List<TileSO>();
        List<TileSO> checkedTiles = new List<TileSO>();
        TileSO tempSO = new TileSO();
        int randomTileSO = Random.Range(0, safeTileSOs.Count);
        tempSO = safeTileSOs[randomTileSO];
        checkedTiles.Add(tempSO);
        tileQueue.Add(tempSO);
        while (tileQueue.Count > 0)
        {
            tempSO = tileQueue[0];
            foreach (TileSO singleTile in tempSO.neighbours)
            {
                bool isChecked = new bool();
                isChecked = false;
                foreach (TileSO checkedTile in checkedTiles)
                {
                    if (checkedTile == singleTile)
                    {
                        isChecked = true;
                    }
                }
                if (!isChecked && singleTile.type == Type.Safe)
                {
                    tileQueue.Add(singleTile);
                    checkedTiles.Add(singleTile);
                }
                else if (!isChecked && singleTile.type == Type.Bomb)
                {
                    checkedTiles.Add(singleTile);
                }
            }
            tileQueue.Remove(tempSO);
        }
        if (checkedTiles.Count != (gridSizeX * gridSizeZ))
        {
            Debug.Log(checkedTiles.Count);
            return false;
        }
        else
        {
            Debug.Log(checkedTiles.Count);
            return true;
        }
    }
}
