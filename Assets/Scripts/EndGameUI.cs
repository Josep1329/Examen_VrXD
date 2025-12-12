using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Shows an end-of-game panel when the GameManager signals the game ended.
/// Panel should contain two buttons wired to RestartGame and QuitGame.
/// </summary>
public class EndGameUI : MonoBehaviour
{
    [Tooltip("Panel (GameObject) that contains the end-game UI. Should be inactive at start.")]
    public GameObject endPanel;

    [Tooltip("Button used to restart the current level")]
    public Button restartButton;

    [Tooltip("Button used to quit the application")]
    public Button quitButton;

    void OnEnable()
    {
        // Leave subscription to Start/TrySubscribe to handle script execution order
    }

    void OnDisable()
    {
        if (subscribed && GameManager.Instance != null)
        {
            GameManager.Instance.OnGameEnded -= OnGameEnded;
        }
    }

    void Start()
    {
        if (endPanel != null) endPanel.SetActive(false);

        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        TrySubscribe();
    }

    bool subscribed = false;

    void TrySubscribe()
    {
        if (subscribed) return;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameEnded += OnGameEnded;
            subscribed = true;
        }
        else
        {
            Invoke(nameof(TrySubscribe), 0.1f);
        }
    }

    void OnGameEnded()
    {
        Debug.Log("EndGameUI: OnGameEnded received");
        if (endPanel != null) endPanel.SetActive(true);
        // Pause time so the world freezes; if you don't want that, remove this line
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Resume time (important if we paused it)
        Time.timeScale = 1f;
        Scene active = SceneManager.GetActiveScene();
        SceneManager.LoadScene(active.name);
    }

    public void QuitGame()
    {
        // Resume time to avoid editor pause issues
        Time.timeScale = 1f;
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
