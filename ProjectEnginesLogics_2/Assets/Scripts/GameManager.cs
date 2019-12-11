using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Sprite> tileSprites;
    public int gridSizeX;
    public int gridSizeZ;
    public int numberOfBombs;
    //public int bombDevide;

    public bool isPaused = false;

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


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
        //Debug.Log("Exiting the game");
        Application.Quit();
    }

}
