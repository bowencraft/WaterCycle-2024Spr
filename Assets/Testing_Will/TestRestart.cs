using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestRestart : MonoBehaviour
{
    // Define the key to restart the scene
    public KeyCode restartKey = KeyCode.R;

    void Update()
    {
        // Check if the restart key is pressed
        if (Input.GetKeyDown(restartKey))
        {
            // Restart the current scene
            RestartScene();
        }
    }

    void RestartScene()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Restart the scene by loading it again
        SceneManager.LoadScene(currentSceneIndex);
    }
}
