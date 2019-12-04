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

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gridManager = FindObjectOfType<GridManager>();

        gridSizeX = gameManager.gridSizeX;
        gridSizeZ = gameManager.gridSizeZ;
    }
    public bool FloodChecker()
    {
        safeTileSOs = gridManager.safeTileSOs;
        bombTileSOs = gridManager.bombTileSOs;
        List<TileSO> tileQueue = new List<TileSO>();
        List<TileSO> checkedTiles = new List<TileSO>();
        TileSO tempSO = new TileSO();
        int tempX;
        int tempZ;
        int randomTileSO = Random.Range(0, safeTileSOs.Count);
        tempSO = safeTileSOs[randomTileSO];
        checkedTiles.Add(tempSO);
        tileQueue.Add(tempSO);
        Debug.Log("<color=green>New SO added, TileSOs in queue:</color><color=cyan> " + tileQueue.Count
            + "</color>");
        while (tileQueue.Count > 0)
        {
            //foreach(TileSO checkedTile in checkedTiles)
            //{
            //    int timesAppeared = new int();
            //    foreach (TileSO recheckedTiles in checkedTiles)
            //    {
            //        if (checkedTile == recheckedTiles)
            //        {
            //            timesAppeared++;
            //        }
            //    }
            //    if (timesAppeared > 1)
            //    {
            //        checkedTiles.Remove(checkedTile);
            //    }
            //}
            tempSO = tileQueue[0];
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    tempX = tempSO.coordX + x;
                    tempZ = tempSO.coordZ + z;
                    if (tempX >= 0 && tempX < gridSizeX && tempZ >= 0 && tempZ < gridSizeZ)
                    {
                        foreach (TileSO singleTile in safeTileSOs)
                        {
                            bool isChecked = new bool();
                            isChecked = false;
                            if (singleTile.coordX == tempX && singleTile.coordZ == tempZ)
                            {
                                foreach (TileSO checkedTile in checkedTiles)
                                {
                                    if (checkedTile == singleTile)
                                    {
                                        isChecked = true;
                                    }
                                }
                                if (!isChecked)
                                {
                                    tileQueue.Add(singleTile);
                                    Debug.Log("<color=green>New SO added, TileSOs in queue:</color><color=cyan> " + tileQueue.Count
                                           + "</color>");
                                    checkedTiles.Add(singleTile);
                                }
                            }
                        }
                        foreach (TileSO singleTile in bombTileSOs)
                        {
                            bool isChecked = new bool();
                            isChecked = false;
                            if (singleTile.coordX == tempX && singleTile.coordZ == tempZ)
                            {
                                foreach (TileSO checkedTile in checkedTiles)
                                {
                                    if (checkedTile == singleTile)
                                    {
                                        isChecked = true;
                                    }
                                }
                                if (!isChecked)
                                {
                                    checkedTiles.Add(singleTile);
                                }
                            }
                        }
                    }
                }
            }
            tileQueue.Remove(tempSO);
            Debug.Log("<color=red>SO removed, TileSOs in queue:</color><color=magenta> " + tileQueue.Count
            + "</color>");
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
