using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour
{
    void Update()
    {
        //LeftClick
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // On Left Click set reveal either the bomb and end the game 
                // Or the number of adjacent bomb tiles 
                // Or "left click" all adjeacent tiles if it has no adjacent bomb tiles
            }
        }
        //RightClick
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // On right click set the TileSo.isMarked to true and Tile.tileSprite to marked
            }
        }

    }
}
