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
    // The number which will determine the number of bombs
    // (gridSizeX*gridSizeZ)/bombDevide=totalBombs
    [SerializeField]
    private int bombDevide;

    public TileSO tileSO;
    public List<TileSO> safeTileSOs;
    public List<TileSO> bombTileSOs;
    public GameObject prefabTile;
    public List<GameObject> spawnedTiles;
    // Used for FloodCheck
    public List<TileSO> checkedTiles;

    // Flood Fill Algorithm needed


    // STARTS RAHEEL'S PART
    public static GridManager instance;
    public List<GameObject> prefabTiles;

    public int[,] Grid;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gridSizeX = gameManager.gridSizeX;
        gridSizeZ = gameManager.gridSizeZ;
        bombDevide = gameManager.bombDevide;
        Grid = new int[gridSizeX, gridSizeZ];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                SpawnTile(x, z);
            }
        }
        PlaceBombs();
        foreach(TileSO singleTile in bombTileSOs)
        {
            singleTile.spriteHolder.color = Color.red;
            singleTile.spriteHolder.gameObject.SetActive(true);
        }
        // This was for testing purposes
        //foreach (TileSO singleSO in safeTileSOs)
        //{
        //    singleSO.adjacentTMP.text = "[" + singleSO.coordX.ToString() +","
        //        + singleSO.coordZ.ToString() + "]";
        //    singleSO.adjacentTMP.gameObject.SetActive(true);
        //}
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
        newSO.coordZ = z;
        newSO.tilePrefab = newTile;
        newSO.spriteHolder = newTile.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        newSO.adjacentTMP = newTile.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        newSO.tileSprites = gameManager.tileSprites;
        // Adds the new SO to a list of tile SOs
        safeTileSOs.Add(newSO);
    }

    public void TellMyLocation(Vector3 tileLocation)
    {
        print(" ------ >" + tileLocation);
    }

    public void PlaceBombs()
    {
        for (int i = 0; i < ((gridSizeX * gridSizeZ)/bombDevide); i++)
        {
            int randomTileSO = Random.Range(0, safeTileSOs.Count);
            TileSO tempSO = new TileSO();
            tempSO = safeTileSOs[randomTileSO];
            tempSO.type = Type.Bomb;
            bombTileSOs.Add(tempSO);
            safeTileSOs.Remove(tempSO);
            //if(FloodCheck() ==  false)
            //{
            //    bombTileSOs.Remove(tempSO);
            //    safeTileSOs.Add(tempSO);
            //    i--;
            //}
        }
    }
    //public bool FloodCheck()
    //{
    //    TileSO tempSO = new TileSO();
    //    int randomTileSO = Random.Range(0, safeTileSOs.Count);
    //    int tempX;
    //    int tempZ;
    //    tempSO = safeTileSOs[randomTileSO];
    //    List<TileSO> tileQueue = new List<TileSO>();
    //    tileQueue.Add(tempSO);
    //    while(tileQueue.Count > 0)
    //    {
    //        tempSO = tileQueue[0];
    //        for (int x = -1; x <= 1; x++)
    //        {
    //            for (int z = -1; z <= 1; z++)
    //            {
    //                tempX = tempSO.coordX + x;
    //                tempZ = tempSO.coordZ + z;
    //                if (tempX > -1 && tempX < gridSizeX && tempZ > -1 && tempZ < gridSizeZ)
    //                {
    //                    foreach (TileSO singleTile in safeTileSOs)
    //                    {
    //                        bool isChecked = new bool();
    //                        isChecked = false;
    //                        if (singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //                        {
    //                            foreach (TileSO checkedTile in checkedTiles)
    //                            {
    //                                if (checkedTile == singleTile)
    //                                {
    //                                    isChecked = true;
    //                                }
    //                            }
    //                            if (!isChecked)
    //                            {
    //                                tileQueue.Add(singleTile);
    //                            }
    //                        }
    //                    }
    //                    foreach (TileSO singleTile in bombTileSOs)
    //                    {
    //                        bool isChecked = new bool();
    //                        isChecked = false;
    //                        if (singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //                        {
    //                            foreach (TileSO checkedTile in checkedTiles)
    //                            {
    //                                if (checkedTile == singleTile)
    //                                {
    //                                    isChecked = true;
    //                                }
    //                            }
    //                            if (!isChecked)
    //                            {
    //                                checkedTiles.Add(singleTile);
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        checkedTiles.Add(tempSO);
    //        tileQueue.Remove(tempSO);
    //    }
    //    if(checkedTiles.Count == (gridSizeX*gridSizeZ))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //    // Pre-Optimized
    //    //tempX = tempSO.coordX + -1;
    //    //tempZ = tempSO.coordZ + -1;
    //    //if (tempX > -1 || tempZ > -1)
    //    //{
    //    //    foreach (TileSO singleTile in safeTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //            Flood(singleTile);
    //    //        }
    //    //    }
    //    //    foreach (TileSO singleTile in bombTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //        }
    //    //    }
    //    //}
    //    //tempX = tempSO.coordX + 0;
    //    //tempZ = tempSO.coordZ + -1;
    //    //if (tempZ > -1)
    //    //{
    //    //    foreach (TileSO singleTile in safeTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //            Flood(singleTile);
    //    //        }
    //    //    }
    //    //    foreach (TileSO singleTile in bombTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //        }
    //    //    }
    //    //}
    //    //tempX = tempSO.coordX + 1;
    //    //tempZ = tempSO.coordZ + -1;
    //    //if (tempX < gridSizeX && tempZ > -1)
    //    //{
    //    //    foreach (TileSO singleTile in safeTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //            Flood(singleTile);
    //    //        }
    //    //    }
    //    //    foreach (TileSO singleTile in bombTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //        }
    //    //    }
    //    //}
    //    //tempX = tempSO.coordX + -1;
    //    //tempZ = tempSO.coordZ + 0;
    //    //if (tempX > -1)
    //    //{
    //    //    foreach (TileSO singleTile in safeTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //            Flood(singleTile);
    //    //        }
    //    //    }
    //    //    foreach (TileSO singleTile in bombTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //        }
    //    //    }
    //    //}
    //    //tempX = tempSO.coordX + 1;
    //    //tempZ = tempSO.coordZ + 0;
    //    //if (tempX < gridSizeX)
    //    //{
    //    //    foreach (TileSO singleTile in safeTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //            Flood(singleTile);
    //    //        }
    //    //    }
    //    //    foreach (TileSO singleTile in bombTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //        }
    //    //    }
    //    //}
    //    //tempX = tempSO.coordX + -1;
    //    //tempZ = tempSO.coordZ + 1;
    //    //if (tempX > -1 && tempZ < gridSizeZ)
    //    //{
    //    //    foreach (TileSO singleTile in safeTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //            Flood(singleTile);
    //    //        }
    //    //    }
    //    //    foreach (TileSO singleTile in bombTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //        }
    //    //    }
    //    //}
    //    //tempX = tempSO.coordX + 0;
    //    //tempZ = tempSO.coordZ + 1;
    //    //if (tempZ < gridSizeZ)
    //    //{
    //    //    foreach (TileSO singleTile in safeTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //            Flood(singleTile);
    //    //        }
    //    //    }
    //    //    foreach (TileSO singleTile in bombTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //        }
    //    //    }
    //    //}
    //    //tempX = tempSO.coordX + 1;
    //    //tempZ = tempSO.coordZ + 1;
    //    //if (tempX < gridSizeX && tempZ < gridSizeZ)
    //    //{
    //    //    foreach (TileSO singleTile in safeTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //            Flood(singleTile);
    //    //        }
    //    //    }
    //    //    foreach (TileSO singleTile in bombTileSOs)
    //    //    {
    //    //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
    //    //            && singleTile.isChecked == false)
    //    //        {
    //    //            singleTile.isChecked = true;
    //    //        }
    //    //    }
    //    //}
    //}
    //Might use but maybe not
    //public bool BombCheck(TileSO tile)
    //{
    //    if (tile.type == Type.Bomb)
    //    {
    //        checkedTiles.Add(tile);
    //        tile.isChecked = true;
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
}
