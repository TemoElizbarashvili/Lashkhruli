using UnityEngine;

public class StateController : MonoBehaviour
{
    public GameObject HUD;
    public GameObject PauseMenu;

    private bool isPaused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Optionally, you can add functionality to unlock the cursor with a key press
        if (!Input.GetKeyDown(KeyCode.Escape)) 
            return;

        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PauseMenu.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        BackGroundMusic.Instance.Mute(false);
    }

    public void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PauseMenu.SetActive(true);
        HUD.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        BackGroundMusic.Instance.Mute(true);
    }

}
