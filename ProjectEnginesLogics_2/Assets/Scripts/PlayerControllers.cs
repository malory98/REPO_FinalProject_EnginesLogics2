using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllers : MonoBehaviour   // Mouse clicks Left/Right
{
    TileLocation tileLoc;
    int tileLocationX;
    int tileLocationZ;

    private void Start()
    {
        tileLoc = FindObjectOfType<TileLocation>();
    }

    private void Update()
    {
        RightClick();
        LeftClick();
    }

    public void RightClick()   // ONLY MARKING / No revealing the tiles
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            tileLocationX = tileLoc.locationX;
            tileLocationZ = tileLoc.locationZ;

            Debug.Log("after RIGHT click: " + tileLocationX + ", " + tileLocationZ);

            //foreach(TileSO tile in tile)
        }
    }

    public void LeftClick()   // NO MARKING / just revealing tiles
    {
        if (Input.GetMouseButtonDown(1))
        {

        }
    }

}
