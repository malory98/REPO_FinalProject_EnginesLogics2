using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Sprite> tileSprites;
    public int gridSizeX;
    public int gridSizeZ;
    public int bombDevide;

    public bool isPaused = false;

    public void PauseGame()
    {
        if(isPaused == false)
            isPaused = true;
    }

    public void UnpauseGame()
    {
        if (isPaused == true)
            isPaused = false;
    }

    public void ExitGame()
    {
        Debug.Log("Exiting the game");
        Application.Quit();
    }

}
