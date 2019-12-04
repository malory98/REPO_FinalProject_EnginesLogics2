using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentBombChecker : MonoBehaviour
{
    public GridManager gridManager;

    public List<TileSO> safeTileSOs;
    public List<TileSO> bombTileSOs;

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    public void CheckAdjacentBombs()
    {
        safeTileSOs = gridManager.safeTileSOs;
        bombTileSOs = gridManager.bombTileSOs;
        int tempX;
        int tempZ;
        foreach (TileSO safeTile in safeTileSOs)
        {
            int adjacentBombs = new int();
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    tempX = safeTile.coordX + x;
                    tempZ = safeTile.coordZ + z;
                    foreach(TileSO bombTile in bombTileSOs)
                    {
                        if(tempX == bombTile.coordX 
                            && tempZ == bombTile.coordZ)
                        {
                            adjacentBombs++;
                        }
                    }
                }
            }
            safeTile.numOfAdjacent = adjacentBombs;
            safeTile.adjacentTMP.text = safeTile.numOfAdjacent.ToString();
        }
    }
}
