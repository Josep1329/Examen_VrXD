using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Updates on-screen UI (score and timer). Assign TextMeshProUGUI components in the inspector.
/// </summary>
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    bool subscribed = false;

    void Start()
    {
        // Ensure we subscribe to the GameManager events even if this component
        // is enabled before the GameManager's Awake runs. We'll attempt to
        // resolve the instance here and subscribe. If the instance is still
        // null, we attempt a short retry (in case of script execution order).
        TrySubscribe();

        // initialize UI values
        if (scoreText != null) scoreText.text = "Score: 0";
        if (timerText != null) timerText.text = FormatTime(GameManager.Instance != null ? GameManager.Instance.timeRemaining : 0f);
    }

    void OnDisable()
    {
        Unsubscribe();
    }

    void TrySubscribe()
    {
        if (subscribed) return;

        // If GameManager.Instance is not yet assigned (activation order), we'll retry below.

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += UpdateScore;
            GameManager.Instance.OnTimeChanged += UpdateTime;
            subscribed = true;
        }
        else
        {
            // If still null, try again shortly (one-time delayed retry)
            Invoke(nameof(TrySubscribe), 0.1f);
        }
    }

    void Unsubscribe()
    {
        if (!subscribed) return;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= UpdateScore;
            GameManager.Instance.OnTimeChanged -= UpdateTime;
        }
        subscribed = false;
    }



    void UpdateScore(int newScore)
    {
        if (scoreText != null) scoreText.text = $"Score: {newScore}";
    }

    void UpdateTime(float seconds)
    {
        if (timerText != null) timerText.text = FormatTime(seconds);
    }

    string FormatTime(float seconds)
    {
        seconds = Mathf.Max(0, seconds);
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"Time: {minutes:00}:{secs:00}";
    }
}
