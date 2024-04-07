using UnityEngine;

public class WaypointsFollower : MonoBehaviour
{
    [SerializeField]
    private float movingSpeed;
    [SerializeField]
    private Transform[] waypoints;

    int nextWaypoint = 0;

    private void Start()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            movingSpeed *= 4;
        }
    }

    private void Update()
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
        Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
    }
}
