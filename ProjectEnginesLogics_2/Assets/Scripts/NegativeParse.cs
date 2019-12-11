using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NegativeParse : MonoBehaviour
{
    public TextMeshProUGUI message;
    public TMP_InputField beeNumberIn;
    public GameManager gameManager;
    public SceneChange sceneChange;
    public bool readyToStart;

    private void Start()
    {
        sceneChange = FindObjectOfType<SceneChange>();
        gameManager = FindObjectOfType<GameManager>();
        readyToStart = false;
    }

    public void Parse()
    {

        if (beeNumberIn.text == null)
        {
            message.text = "Please enter a number of bees";
            readyToStart = false;
        }
        else if (int.Parse(beeNumberIn.text) == 0)
        {
            message.text = "YOU WIN!\nNow put in a positive number for more challenge...";
            readyToStart = false;
        }
        else if (int.Parse(beeNumberIn.text) < 0)
        {
            message.text = "Please input a positive number.\nWe are currently out of negative bees...";
            readyToStart = false;
        }
        else if (int.Parse(beeNumberIn.text) > 0 && int.Parse(beeNumberIn.text) <= 33)
        {
            message.text = "Thank you for selecting!";
            gameManager.numberOfBombs = int.Parse(beeNumberIn.text);
            readyToStart = true;

        }
        else if (int.Parse(beeNumberIn.text) > 33 && int.Parse(beeNumberIn.text) <= 60)
        {
            message.text = "You must be quite brave!\nDon't expect and easy time";
            gameManager.numberOfBombs = int.Parse(beeNumberIn.text);
            readyToStart = true;
        }else if (int.Parse(beeNumberIn.text) > 60)
        {
            message.text = "We'll do our best to fit in as many\n as we can, but no promises...\nGood Luck, you're gonna need it";
            gameManager.numberOfBombs = int.Parse(beeNumberIn.text);
            readyToStart = true;
        }
        else
        {
            message.text = "ERROR";
        }
    }

    public void Continue()
    {
        if(readyToStart)
        {
            sceneChange.ChangeScene();
        }
        else
        {
            message.text = "Input the number of bees";
        }
    }

}
