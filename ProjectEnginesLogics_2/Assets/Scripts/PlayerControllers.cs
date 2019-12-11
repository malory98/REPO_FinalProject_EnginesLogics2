using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControllers : MonoBehaviour   // Mouse clicks Left/Right
{
    TileLocation tileLoc;
    int thisTileLocationX;
    int thisTileLocationZ;

    GridManager gridMG;

    [SerializeField]
    private int bombsCount;

    // Used to check if win condition
    [SerializeField]
    private int bombsMarked;

    public bool isGameOver;

    public TextMeshProUGUI bombText;

    [SerializeField]
    GameManager gmMg;

    public GameObject inGamePanel;
    public GameObject victoryPanel;
    public GameObject losePanel;

    private void Start()
    {
        tileLoc = FindObjectOfType<TileLocation>();
        //gridMG = FindObjectOfType<GridManager>();
        //bombsCount = 10;  // place-holder number
        gmMg = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        RightClick();
        LeftClick();
        bombText.text = bombsCount.ToString();
    }

    public void Initialize()
    {
        isGameOver = false;
        //tileLoc = FindObjectOfType<TileLocation>();
        gridMG = FindObjectOfType<GridManager>();
        // Gets number of bombs from GameManager
        bombsCount = gridMG.bombTileSOs.Count;
    }

    public void RightClick()   // ONLY MARKING / No revealing the tiles
    {
        // I set it to initialize after the bombs render in the gridmanager
        //Initialize();

        if (Input.GetMouseButtonDown(1) && gmMg.isPaused == false && isGameOver == false)  // getting Mouse input AND CHECKING is the game is not paused
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000) && (!(hit.rigidbody == null)
                || !(hit.collider == null)))
            {
                if (hit.collider.gameObject.tag == "Tile")
                {
                    TileLocation tileLoc = hit.collider.gameObject.GetComponent<TileLocation>();
                    // in case there is a problem, try using GetComponentInParent<TileLocation>
                    print(tileLoc.locationX.ToString() + ", " + tileLoc.locationZ.ToString());
                    thisTileLocationX = tileLoc.locationX;
                    thisTileLocationZ = tileLoc.locationZ;
                }

                foreach (TileSO tile in gridMG.allTileSOs)
                {
                    if (tile.coordX == thisTileLocationX && tile.coordZ == thisTileLocationZ)
                    {
                        if (tile.isClicked == false && tile.isMarked == false && bombsCount > 0)
                        {
                            tile.isMarked = true; // Tile's isMarked state changes to prevent accidental missclicks
                            tile.spriteHolder.sprite = tile.tileSprites[1];  // Change Sprite to the Marked Sprite
                            bombsCount--;  // count down the number of Bombs
                            // If the tile clicked was a bomb
                            if(tile.type == Type.Bomb)
                            {
                                // Adds to the total bombs marked
                                bombsMarked++;
                                if(bombsMarked == gridMG.bombTileSOs.Count)
                                {
                                    // Player wins
                                    StartCoroutine(WinAnim());
                                    isGameOver = true;
                                    //StartCoroutine(PanelWaiting());
                                    // Moved into coroutine
                                    //inGamePanel.SetActive(false);
                                    //victoryPanel.SetActive(true);
                                }
                            }
                        }
                        else if (tile.isClicked == false && tile.isMarked == true)
                        {
                            tile.isMarked = false; // Tile's isMarked bool is back to default state
                            tile.spriteHolder.sprite = tile.tileSprites[0];  // Change Sprite to the UNmarked Sprite
                            bombsCount++; // count up the bombs;
                            if(tile.type == Type.Bomb)
                            {
                                bombsMarked--;
                            }
                        }
                    }
                }
            }
        }
    }

    public void LeftClick()   // NO MARKING / just revealing tiles
    {
        //Initialize();

        if (Input.GetMouseButtonDown(0) && gmMg.isPaused == false && isGameOver == false)  // CHECKING is the game is not paused
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000) && (!(hit.rigidbody == null)
                || !(hit.collider == null)))
            {
                if (hit.collider.gameObject.tag == "Tile")
                {
                    TileLocation tileLoc = hit.collider.gameObject.GetComponent<TileLocation>();
                    // in case there is a problem, try using GetComponentInParent<TileLocation>
                    print(tileLoc.locationX.ToString() + ", " + tileLoc.locationZ.ToString());
                    thisTileLocationX = tileLoc.locationX;
                    thisTileLocationZ = tileLoc.locationZ;
                }

                foreach (TileSO tile in gridMG.allTileSOs)
                {
                    if (tile.coordX == thisTileLocationX && tile.coordZ == thisTileLocationZ)
                    {
                        // If a safe tile is left clicked
                        if (tile.isMarked == false && tile.type == Type.Safe)
                        {
                            tile.spriteHolder.gameObject.SetActive(false); 
                            tile.adjacentTMP.gameObject.SetActive(true);
                            // Sets isClicked to true
                            tile.isClicked = true;
                            Debug.Log("Left click");
                            // If the tile isn't adjacent to any bombTiles burst revealing all adjacent tiles
                            if(tile.numOfAdjacent == 0)
                            {
                                StartCoroutine(BurstDelay(tile));
                            }
                        }
                        else if (tile.isMarked == false && tile.type == Type.Bomb)
                        {
                            StartCoroutine(GameOverAnim());  // Player loses
                            // GAME OVER
                            isGameOver = true;
                            // Moved into the coroutine
                            //StartCoroutine(PanelWaiting());
                            //inGamePanel.SetActive(false);
                            //losePanel.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    // Used for safe tiles that have 0 adjacent bombs
    public void ZeroBurst(TileSO zeroTile)
    {
        foreach (TileSO neighbour in zeroTile.neighbours)
        {
            if (neighbour.isClicked == false && neighbour.isMarked == false)
            {
                // Remove sprite and show number of adjacent bombs
                neighbour.isClicked = true;
                neighbour.spriteHolder.gameObject.SetActive(false);
                neighbour.adjacentTMP.text = neighbour.numOfAdjacent.ToString();
                neighbour.adjacentTMP.gameObject.SetActive(true);
                // If adjacent is 0, burst again
                if(neighbour.numOfAdjacent == 0)
                {
                    StartCoroutine(BurstDelay(neighbour));
                }

            }
        }
    }

    // Delay for ZeroBurst to make it look cool
    public IEnumerator BurstDelay(TileSO tile)
    {
        yield return new WaitForSeconds(0.05f);
        ZeroBurst(tile);
    }

    // Delay animation for Game Over
    public IEnumerator GameOverAnim()
    {
        foreach (TileSO bomb in gridMG.bombTileSOs)
        {
            yield return new WaitForSeconds(0.05f);
            bomb.spriteHolder.sprite = bomb.tileSprites[2];
        }
        foreach (TileSO bomb in gridMG.bombTileSOs)
        {
            yield return new WaitForSeconds(0.05f);
            bomb.spriteHolder.color = Color.red;
        }
        yield return new WaitForSeconds(1);
        inGamePanel.SetActive(false);
        losePanel.SetActive(true);
    }

    // Delay animation for Win
    public IEnumerator WinAnim()
    {
        foreach (TileSO bomb in gridMG.bombTileSOs)
        {
            yield return new WaitForSeconds(0.05f);
            bomb.spriteHolder.color = Color.green;
        }
        yield return new WaitForSeconds(1);
        inGamePanel.SetActive(false);
        victoryPanel.SetActive(true);
    }

    //// Delay to show up Panels
    //public IEnumerator PanelWaiting()
    //{
    //      yield return new WaitForSeconds(50.0f);    
    //}
}
