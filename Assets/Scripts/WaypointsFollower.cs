using UnityEngine;

public class WaypointsFollower : MonoBehaviour
{
    [SerializeField]
    private float movingSpeed;
    [SerializeField]
    private Transform[] waypoints;

    int nextWaypoint = 0;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[nextWaypoint].position, movingSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, waypoints[nextWaypoint].position) < 0.1f)
        {
            nextWaypoint++;
        }
        if (nextWaypoint >= waypoints.Length)
        {
            nextWaypoint = 0;
        }
    }
}
