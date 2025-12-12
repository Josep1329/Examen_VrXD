using System;
using UnityEngine;

/// <summary>
/// Simple game manager that tracks score and a countdown timer.
/// Put this on a persistent GameObject in the scene and assign initialTimeMinutes if needed.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Score { get; private set; }

    public float timeRemaining = 180f; // 3 minutes default
    public bool isRunning { get; private set; }

    public event Action<int> OnScoreChanged;
    public event Action<float> OnTimeChanged;
    public event Action OnGameEnded;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        Score = 0;
        isRunning = true;
        OnScoreChanged?.Invoke(Score);
        OnTimeChanged?.Invoke(timeRemaining);
    }

    void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            OnTimeChanged?.Invoke(timeRemaining);
            OnGameEnded?.Invoke();
            Debug.Log("GameManager: Time up. Final score: " + Score);
            return;
        }

        OnTimeChanged?.Invoke(timeRemaining);
    }

    public void AddScore(int amount)
    {
        if (!isRunning) return;
        Score += amount;
        OnScoreChanged?.Invoke(Score);
        Debug.Log("GameManager: score added " + amount + ", total=" + Score);
    }
}
