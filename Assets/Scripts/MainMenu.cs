using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public StateController StateControl;

    public void StartGame()
        => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void QuitGame()
        => Application.Quit();

    public void ResumeGame()
        => StateControl.Resume();
}
