using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class StartScreen : MonoBehaviour
{
    [SerializeField] private string gameLevelName = "LevelTesting";
    // This method is used to go to another scene. 
    // The sceneName parameter should be the name of the scene you want to load.
    public void GoToGame()
    {
        SceneManager.LoadScene(gameLevelName);
    }

    // This method quits the game. 
    // In the editor, it will stop play mode. In a build, it will close the application.
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}