using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllers : MonoBehaviour   // Mouse clicks Left/Right
{
    TileLocation tileLoc;
    int tileLocationX;
    int tileLocationZ;

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
            /*Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tileLocationX = tileLoc.locationX;
            tileLocationZ = tileLoc.locationZ;

            Debug.Log("RIGHT CLICK: " + tileLocationX + ", " + tileLocationZ);

            foreach (TileSO tile in gridMG.safeTileSOs)
            {
                
            }*/

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000) && (!(hit.rigidbody == null)
                || !(hit.collider == null)))
            {
                if (hit.collider.gameObject.tag == "Tile")
                {
                    TileLocation tileLoc = hit.collider.gameObject.GetComponent<TileLocation>();
                    // in case there is a problem, try using GetComponentInParent<TileLocation>
                    print(tileLoc.locationX.ToString() + ", " + tileLoc.locationZ.ToString());
                }

            }
        }
    }

    public void LeftClick()   // NO MARKING / just revealing tiles
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }

}
