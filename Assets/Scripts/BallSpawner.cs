using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns ball prefabs at randomized positions above the play area.
/// Attach this to an empty GameObject and assign a ball prefab with a Rigidbody.
/// </summary>
public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public float spawnInterval = 1.2f;
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector3 spawnAreaSize = new Vector3(4f, 0f, 4f);
    public float spawnHeight = 3f;
    public int maxSimultaneous = 20;

    int currentCount = 0;

    void Start()
    {
        if (ballPrefab == null)
        {
            Debug.LogError("BallSpawner: ballPrefab not assigned.");
            enabled = false;
            return;
        }

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // If a GameManager exists and the game is not running anymore, stop spawning
            if (GameManager.Instance != null && !GameManager.Instance.isRunning)
            {
                yield break;
            }

            if (currentCount < maxSimultaneous)
            {
                SpawnOne();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnOne()
    {
        Vector3 randomInArea = new Vector3(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            0f,
            Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
        );

        Vector3 spawnPos = spawnAreaCenter + randomInArea + Vector3.up * spawnHeight;

        GameObject go = Instantiate(ballPrefab, spawnPos, Random.rotation);
        currentCount++;

        // Decrease count when ball is destroyed
        BallLifeTracker tracker = go.AddComponent<BallLifeTracker>();
        tracker.onDestroyed = () => currentCount--;
    }
}

/// <summary>
/// Small helper that calls a callback when the GameObject is destroyed.
/// Used by the spawner to track how many active balls exist.
/// </summary>
public class BallLifeTracker : MonoBehaviour
{
    public System.Action onDestroyed;
    void OnDestroy() => onDestroyed?.Invoke();
}
