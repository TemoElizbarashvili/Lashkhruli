using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private StateController stateControl;

    void Start()
    {
        stateControl = FindFirstObjectByType<StateController>(FindObjectsInactive.Include);
    }

    public void StartGame()
        => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void QuitGame()
        => Application.Quit();

    public void ResumeGame()
        => stateControl.Resume();
}
