using UnityEngine;

/// <summary>
/// Ball behavior:
/// - Detects collisions with the object tagged "Target" and awards points if impact is strong enough.
/// - Self-destroys after a timeout to avoid cluttering the scene.
/// This script does not assume any specific VR interaction package; make the ball grabbable by adding
/// an XRGrabInteractable component in the editor if you use the XR Interaction Toolkit.
/// </summary>
public class Ball : MonoBehaviour
{
    [Tooltip("Minimum collision relative velocity magnitude to count as a hit (treat as a throw)")]
    public float minImpactSpeed = 2f;

    [Tooltip("Points awarded when this ball successfully hits the target")]
    public int pointsOnHit = 100;

    [Tooltip("Seconds before the ball is automatically destroyed")]
    public float lifeTime = 20f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // schedule destroy in case player doesn't pick it up
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // If we hit the target and the impact was strong enough, award points
        if (collision.collider != null && collision.collider.CompareTag("Target"))
        {
            float impactSpeed = collision.relativeVelocity.magnitude;
            if (impactSpeed >= minImpactSpeed)
            {
                // Award points via GameManager if available
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddScore(pointsOnHit);
                }
                else
                {
                    Debug.Log("Ball: hit target, but GameManager.Instance is null");
                }

                // Optionally play FX here

                // Destroy the ball on successful hit
                Destroy(gameObject);
            }
        }
    }
}
