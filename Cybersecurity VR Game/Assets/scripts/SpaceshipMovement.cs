using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform[] waypoints;
    private int currentWaypointIndex;

    void Start()
    {
        // Initialize with a random waypoint
        currentWaypointIndex = Random.Range(0, waypoints.Length);
    }

    void Update()
    {
        if (waypoints.Length == 0)
            return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 targetDirection = (targetWaypoint.position - transform.position).normalized;

        transform.Translate(targetDirection * speed * Time.deltaTime, Space.World);

        // Rotate to face the waypoint (optional)
        transform.rotation = Quaternion.LookRotation(targetDirection);

        // Check if the spaceship has reached the vicinity of the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f) // Adjust proximity threshold as needed
        {
            // Select a new waypoint randomly
            currentWaypointIndex = Random.Range(0, waypoints.Length);
        }
    }
}
