using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string sceneName;
    public string resetName;

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(resetName);
    }

}