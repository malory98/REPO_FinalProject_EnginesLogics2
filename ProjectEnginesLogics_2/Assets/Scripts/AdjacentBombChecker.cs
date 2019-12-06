using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentBombChecker : MonoBehaviour
{
    public GridManager gridManager;

    public List<TileSO> safeTileSOs;
    public List<TileSO> bombTileSOs;

    public void Initialize()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    public void CheckAdjacentBombs()
    {
        safeTileSOs = gridManager.safeTileSOs;
        bombTileSOs = gridManager.bombTileSOs;
        foreach (TileSO safeTile in safeTileSOs)
        {
            int adjacentBombs = new int();

            foreach (TileSO adjacentTile in safeTile.neighbours)
            {
                if (adjacentTile.type == Type.Bomb)
                {
                    adjacentBombs++;
                }
            }
            safeTile.numOfAdjacent = adjacentBombs;
            safeTile.adjacentTMP.text = safeTile.numOfAdjacent.ToString();
        }
    }
}
