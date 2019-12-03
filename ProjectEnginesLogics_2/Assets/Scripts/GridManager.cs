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
        foreach(TileSO singleTileSO in bombTileSOs)
        {
            singleTileSO.spriteHolder.color = Color.red;
            singleTileSO.spriteHolder.gameObject.SetActive(true);
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

            safeTileSOs[randomTileSO].type = Type.Bomb;
            bombTileSOs.Add(safeTileSOs[randomTileSO]);
            safeTileSOs.Remove(safeTileSOs[randomTileSO]);
            // FLOOD FILL CHECK HERE
            // if returns true
            //      continue
            // else if false
            //      remove from bombTileSOs
            //      readd to safeTileSOs
            //      i--;
            //      chose new randomX & randomZ
        }
    }
    //public bool FloodCheck()
    //{
    //    int randomTileSO = Random.Range(0, safeTileSOs.Count);

    //    if (safeTileSOs[randomTileSO].coordX == 0
    //        || safeTileSOs[randomTileSO].coordZ == 0
    //        || safeTileSOs[randomTileSO].coordX == (gridSizeX - 1)
    //        || safeTileSOs[randomTileSO].coordZ == (gridSizeZ - 1))
    //    {

    //    }
    //    else
    //    {
            
    //    }

    //}
    //public bool BombCheck(TileSO tile)
    //{
    //    if(tile.type == Type.Bomb)
    //    {
    //        check
    //    }
    //}
}
