using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReset : MonoBehaviour
{
    
    public GridManager gridManager;

    public void Initialize()
    {
        gridManager = GetComponent<GridManager>();
    }

    public void Reset()
    {
        gridManager.allTileSOs.Clear();
        gridManager.bombTileSOs.Clear();
        gridManager.safeTileSOs.Clear();
    }
}
