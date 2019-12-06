using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllers : MonoBehaviour   // Mouse clicks Left/Right
{
    TileLocation tileLoc;
    int thisTileLocationX;
    int thisTileLocationZ;

    GridManager gridMG;

    private void Start()
    {
        tileLoc = FindObjectOfType<TileLocation>();
        //gridMG = FindObjectOfType<GridManager>();
    }

    private void Update()
    {
        RightClick();
        LeftClick();
    }

    private void Initialize()
    {
        //tileLoc = FindObjectOfType<TileLocation>();
        gridMG = FindObjectOfType<GridManager>();
    }

    public void RightClick()   // ONLY MARKING / No revealing the tiles
    {
        Initialize();

        if (Input.GetMouseButtonDown(1))
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
                            Debug.Log("Right c");
                            if (tile.isMarked == false)
                            {
                                tile.isMarked = true;
                                tile.spriteHolder.sprite = tile.tileSprites[1];  // Change Sprite to the Marked Sprite
                                Debug.Log("cheguei");
                            }
                            else if (tile.isMarked == true)
                            {
                                tile.spriteHolder.sprite = tile.tileSprites[0];  // Change Sprite to the UNmarked Sprite
                                Debug.Log("tb cheg");
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

        if (Input.GetMouseButtonDown(0))
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
