using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameManager gameManager;
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

}
