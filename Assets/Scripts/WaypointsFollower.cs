using UnityEngine;

/// <summary>
/// The WaypointsFollower class handles the movement of an object along a set of waypoints.
/// </summary>
public class WaypointsFollower : MonoBehaviour
{
    /// <value><c>movingSpeed</c> is the speed at which the object moves between waypoints.</value>
    [SerializeField]
    private float movingSpeed;

    /// <value><c>waypoints</c> is an array of Transform objects that the object will move between.</value>
    [SerializeField]
    private Transform[] waypoints;

    /// <value><c>nextWaypoint</c> is the index of the next waypoint in the waypoints array that the object will move towards.</value>
    private int nextWaypoint = 0;

    /// <summary>
    /// The Start method is called before after game starts. It checks if the object has a Rigidbody component and if so, multiplies the movingSpeed by 4.
    /// </summary>
    private void Start()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            movingSpeed *= 4;
        }
    }

    /// <summary>
    /// The Update method called once per frame. It moves the object towards the next waypoint and updates the nextWaypoint index when the object reaches a waypoint.
    /// </summary>
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

        // set the object's rotation to look at the next waypoint
        transform.LookAt(waypoints[nextWaypoint]);
    }

    /// <summary>
    /// Method to draw lines between waypoints in the Scene view to help visualize the path the object will take.
    /// </summary>
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