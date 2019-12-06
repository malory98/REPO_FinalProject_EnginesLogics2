using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridManager : MonoBehaviour
{
    public AdjacentBombChecker adjacentBombChecker;
    public FloodCheck floodCheck;
    public GameManager gameManager;
    [SerializeField]
    private int gridSizeX;
    [SerializeField]
    private int gridSizeZ;
    // The number which will determine the number of bombs
    // (gridSizeX*gridSizeZ)/bombDevide=totalBombs
    [SerializeField]
    private int bombDevide;
    private int numberOfBombs;

    public TileSO tileSO;
    public List<TileSO> safeTileSOs;
    public List<TileSO> bombTileSOs;
    public GameObject prefabTile;
    public List<GameObject> spawnedTiles;


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
        adjacentBombChecker = FindObjectOfType<AdjacentBombChecker>();
        gameManager = FindObjectOfType<GameManager>();
        floodCheck = FindObjectOfType<FloodCheck>();
        gridSizeX = gameManager.gridSizeX;
        gridSizeZ = gameManager.gridSizeZ;
        bombDevide = gameManager.bombDevide;
        Grid = new int[gridSizeX, gridSizeZ];
        numberOfBombs = ((gridSizeX * gridSizeZ) / bombDevide);
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                SpawnTile(x, z);
            }
        }
        floodCheck.Initialize();
        adjacentBombChecker.Initialize();
        floodCheck.NeighbourAssigner();
        PlaceBombs();
        adjacentBombChecker.CheckAdjacentBombs();
        foreach(TileSO singleTile in safeTileSOs)
        {
            singleTile.adjacentTMP.gameObject.SetActive(true);
        }
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

    // Changes a set number of safe tiles into bomb tiles, using a flood fill algorithm
    public void PlaceBombs()
    {
        int strikes = new int();
        bool floodResult = new bool();
        for (int i = 0; i < numberOfBombs; i++)
        {
            int randomTileSO = Random.Range(0, safeTileSOs.Count);
            TileSO tempSO = new TileSO();
            tempSO = safeTileSOs[randomTileSO];
            tempSO.type = Type.Bomb;
            bombTileSOs.Add(tempSO);
            safeTileSOs.Remove(tempSO);
            Debug.Log("<color=blue>Flood Check beginning...</color>");
            floodResult = floodCheck.FloodChecker();
            Debug.Log("<color=orange>FloodCheck findished and returned</color> " + floodResult);
            if (!floodResult)
            {
                bombTileSOs.Remove(tempSO);
                safeTileSOs.Add(tempSO);
                i--;
                strikes++;
                if (strikes > 5)
                {
                    numberOfBombs--;
                }
            }
            if(floodResult)
            {
                strikes = 0;
            }
        }
    }

    // Flood fill algorithm that makes sure every tile on the game map will be accessable
    // from one to the other
    public bool FloodCheck()
    {
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
                    if ((tempX >= 0 && tempX < gridSizeX) && (tempZ >= 0 && tempZ < gridSizeZ) &&
                        (x != 0 && z != 0))
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
        // If there are less tileSOs in the checkedTiles then there are total tiles in the grid
        // return false
        if (checkedTiles.Count < (gridSizeX * gridSizeZ))
        {
            Debug.Log(checkedTiles.Count);
            return false;
        }
        else
        {
            Debug.Log(checkedTiles.Count);
            return true;
        }
        // Pre-Optimized attempt at algorithm
        //tempX = tempSO.coordX + -1;
        //tempZ = tempSO.coordZ + -1;
        //if (tempX > -1 || tempZ > -1)
        //{
        //    foreach (TileSO singleTile in safeTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //            Flood(singleTile);
        //        }
        //    }
        //    foreach (TileSO singleTile in bombTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //        }
        //    }
        //}
        //tempX = tempSO.coordX + 0;
        //tempZ = tempSO.coordZ + -1;
        //if (tempZ > -1)
        //{
        //    foreach (TileSO singleTile in safeTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //            Flood(singleTile);
        //        }
        //    }
        //    foreach (TileSO singleTile in bombTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //        }
        //    }
        //}
        //tempX = tempSO.coordX + 1;
        //tempZ = tempSO.coordZ + -1;
        //if (tempX < gridSizeX && tempZ > -1)
        //{
        //    foreach (TileSO singleTile in safeTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //            Flood(singleTile);
        //        }
        //    }
        //    foreach (TileSO singleTile in bombTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //        }
        //    }
        //}
        //tempX = tempSO.coordX + -1;
        //tempZ = tempSO.coordZ + 0;
        //if (tempX > -1)
        //{
        //    foreach (TileSO singleTile in safeTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //            Flood(singleTile);
        //        }
        //    }
        //    foreach (TileSO singleTile in bombTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //        }
        //    }
        //}
        //tempX = tempSO.coordX + 1;
        //tempZ = tempSO.coordZ + 0;
        //if (tempX < gridSizeX)
        //{
        //    foreach (TileSO singleTile in safeTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //            Flood(singleTile);
        //        }
        //    }
        //    foreach (TileSO singleTile in bombTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //        }
        //    }
        //}
        //tempX = tempSO.coordX + -1;
        //tempZ = tempSO.coordZ + 1;
        //if (tempX > -1 && tempZ < gridSizeZ)
        //{
        //    foreach (TileSO singleTile in safeTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //            Flood(singleTile);
        //        }
        //    }
        //    foreach (TileSO singleTile in bombTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //        }
        //    }
        //}
        //tempX = tempSO.coordX + 0;
        //tempZ = tempSO.coordZ + 1;
        //if (tempZ < gridSizeZ)
        //{
        //    foreach (TileSO singleTile in safeTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //            Flood(singleTile);
        //        }
        //    }
        //    foreach (TileSO singleTile in bombTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //        }
        //    }
        //}
        //tempX = tempSO.coordX + 1;
        //tempZ = tempSO.coordZ + 1;
        //if (tempX < gridSizeX && tempZ < gridSizeZ)
        //{
        //    foreach (TileSO singleTile in safeTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //            Flood(singleTile);
        //        }
        //    }
        //    foreach (TileSO singleTile in bombTileSOs)
        //    {
        //        if ((singleTile.coordX == tempX && singleTile.coordZ == tempZ)
        //            && singleTile.isChecked == false)
        //        {
        //            singleTile.isChecked = true;
        //        }
        //    }
        //}
    }
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
