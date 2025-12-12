using UnityEngine;

/// <summary>
/// Moves the target left and right between two points.
/// Ensure this GameObject has a Collider (not trigger) and the Tag set to "Target".
/// </summary>
public class MovingTarget : MonoBehaviour
{
    [Tooltip("If true, pointA/pointB are treated as offsets from the initial position of the object. If false, they are treated as world-space targets.")]
    public bool useOffsets = true;

    public Vector3 pointA = new Vector3(-2f, 0f, 0f);
    public Vector3 pointB = new Vector3(2f, 0f, 0f);
    public float speed = 1.5f;

    bool forward = true;

    Vector3 worldPointA;
    Vector3 worldPointB;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        if (useOffsets)
        {
            worldPointA = startPosition + pointA;
            worldPointB = startPosition + pointB;
        }
        else
        {
            worldPointA = pointA;
            worldPointB = pointB;
        }
    }

    void Update()
    {
        Vector3 target = forward ? worldPointB : worldPointA;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            forward = !forward;
        }
    }
}
