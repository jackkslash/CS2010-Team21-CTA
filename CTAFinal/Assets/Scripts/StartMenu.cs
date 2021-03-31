using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
   public void PlayGame()
    {
        SceneManager.LoadScene("MainMenu");
        //PauseMenu.GameIsPaused = false;
        //Cursor.visible = false;
    }

    public void PlayAgain(){
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
