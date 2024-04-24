using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    [SerializeField] private GameObject oldInstruction;
    [SerializeField] private GameObject newInstruction;
    
    static PauseMenu instance;
    public static PauseMenu i
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PauseMenu>();
            }
            return instance;
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void UpdateInstruction()
    {
        oldInstruction.SetActive(false);
        newInstruction.SetActive(true);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor back
        Cursor.visible = false; // Make cursor invisible
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None; // Free the cursor
        Cursor.visible = true; // Make cursor visible
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}