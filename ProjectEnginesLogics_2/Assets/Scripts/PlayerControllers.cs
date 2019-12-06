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
        gridMG = FindObjectOfType<GridManager>();
    }

    private void Update()
    {
        RightClick();
        LeftClick();
    }

    public void RightClick()   // ONLY MARKING / No revealing the tiles
    {
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
            }

            foreach (TileSO safeTile in gridMG.safeTileSOs)
            {
                if (safeTile.coordX == thisTileLocationX && safeTile.coordZ == thisTileLocationZ)
                {
                    if (safeTile.isClicked == false)
                    {
                        if (safeTile.isMarked == false)
                        {
                            safeTile.isMarked = true;
                            safeTile.spriteHolder.sprite = safeTile.tileSprites[1];  // Change Sprite to the Marked Sprite
                        } else if(safeTile.isMarked == true)
                        {
                            safeTile.spriteHolder.sprite = safeTile.tileSprites[0];  // Change Sprite to the UNmarked Sprite
                        }
                    }
                }

            }
            

        }
    }

    public void LeftClick()   // NO MARKING / just revealing tiles
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (TileSO safeTile in gridMG.safeTileSOs)
            {
                if (safeTile.isMarked == false)
                {
                    safeTile.spriteHolder.gameObject.SetActive(false);
                    safeTile.adjacentTMP.gameObject.SetActive(true);
                }
            }
        }
    }

}
