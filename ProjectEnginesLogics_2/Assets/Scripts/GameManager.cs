using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Sprite> tileSprites;
    public int gridSizeX;
    public int gridSizeZ;
    public int bombDevide;

    public void ExitGame()
    {
        Debug.Log("Exiting the game");
        Application.Quit();
    }

}
