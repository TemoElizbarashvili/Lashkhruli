using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateController : MonoBehaviour
{
    public static StateController Instance { get; private set; }

    private GameObject HUD;
    private GameObject pauseMenu;

    private bool isPaused;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
        => SceneManager.sceneLoaded += OnSceneLoaded;

    void OnDisable()
        => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        var canvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        pauseMenu = canvases.FirstOrDefault(c => c.CompareTag("PauseMenu"))?.gameObject;
        HUD = canvases.FirstOrDefault(c => c.CompareTag("HUD"))?.gameObject;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isPaused)
                return;

            var market = FindObjectsByType<Market>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .FirstOrDefault();
            if (market.gameObject.activeSelf)
            {
                ExitMarket(market.gameObject);
            }
            else
            {
                OpenMarket(market!.gameObject);
                market.UpdateMarketUI();
            }
        }
    }

    public void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        HUD.SetActive(true);
        var market = FindObjectsByType<Market>(FindObjectsInactive.Include, FindObjectsSortMode.None).FirstOrDefault();
        if (market != null && market.gameObject.activeSelf)
        {
            market.gameObject.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
        BackGroundMusic.Instance.Mute(false);
    }

    public void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        HUD.SetActive(false);
        var market = FindObjectsByType<Market>(FindObjectsInactive.Include, FindObjectsSortMode.None).FirstOrDefault();
        if (market != null && market.gameObject.activeSelf)
        {
            market.gameObject.SetActive(false);
        }
        Time.timeScale = 0f;
        isPaused = true;
        BackGroundMusic.Instance.Mute(true);
    }

    public void OpenMarket(GameObject market)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        market.SetActive(true);
        HUD.SetActive(false);
        Time.timeScale = 0f;
        BackGroundMusic.Instance.Mute(true);
    }

    public void ExitMarket(GameObject market)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        market.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1f;
        BackGroundMusic.Instance.Mute(false);
    }
}
