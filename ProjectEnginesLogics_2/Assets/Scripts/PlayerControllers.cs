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

    public TextMeshProUGUI bombText;

    [SerializeField]
    public bool paused;

    private void Start()
    {
        tileLoc = FindObjectOfType<TileLocation>();
        //gridMG = FindObjectOfType<GridManager>();
        bombsCount = 10;  // place-holder number
        paused = FindObjectOfType<GameManager>().isPaused;
    }

    private void Update()
    {
        RightClick();
        LeftClick();
        bombText.text = bombsCount.ToString();
    }

    private void Initialize()
    {
        //tileLoc = FindObjectOfType<TileLocation>();
        gridMG = FindObjectOfType<GridManager>();
    }

    public void RightClick()   // ONLY MARKING / No revealing the tiles
    {
        Initialize();

        if (Input.GetMouseButtonDown(1) && paused == false)  // getting Mouse input AND CHECKING is the game is not paused
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

                foreach (TileSO tile in gridMG.safeTileSOs)
                {
                    if (tile.coordX == thisTileLocationX && tile.coordZ == thisTileLocationZ)
                    {
                        if (tile.isClicked == false)
                        {
                            if (tile.isMarked == false)
                            {
                                tile.isMarked = true;
                                tile.spriteHolder.sprite = tile.tileSprites[1];  // Change Sprite to the Marked Sprite
                                bombsCount--;  // count down the number of Bombs
                            }
                            else if (tile.isMarked == true)
                            {
                                tile.spriteHolder.sprite = tile.tileSprites[0];  // Change Sprite to the UNmarked Sprite
                                bombsCount++; // count up the bombs;
                            }
                        }
                    }

                }
            }
            

        }
    }

    public void LeftClick()   // NO MARKING / just revealing tiles
    {
        Initialize();

        if (Input.GetMouseButtonDown(0) && paused == false)  // CHECKING is the game is not paused
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

                foreach (TileSO tile in gridMG.safeTileSOs)
                {
                    if (tile.coordX == thisTileLocationX && tile.coordZ == thisTileLocationZ)
                    {
                        if (tile.isMarked == false)
                        {
                            tile.spriteHolder.gameObject.SetActive(false); 
                            tile.adjacentTMP.gameObject.SetActive(true);  
                            Debug.Log("Left click");
                        }
                    }
                }
            }
        }
    }

}
