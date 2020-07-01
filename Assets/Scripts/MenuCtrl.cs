using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCtrl : MonoBehaviour {

    public bool isLevelPassed = false;

    public void LoadScene (string sceneName)
    {
        // Starting the game time flow in case it has been stopped
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextScene (string sceneName)
    {
        if (isLevelPassed)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
