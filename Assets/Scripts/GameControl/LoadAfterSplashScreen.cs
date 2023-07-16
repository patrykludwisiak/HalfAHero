using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LoadAfterSplashScreen : MonoBehaviour
{
    public void StartGame() 
    {
        SceneManager.LoadScene("Prolog");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
