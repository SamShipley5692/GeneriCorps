using UnityEngine;

public class enemyPlatform : MonoBehaviour
{
    [SerializeField] waypointPath _waypointPath;
    [SerializeField] float speed;

    int targetWaypointIndex;

    Transform previousWaypoint;
    Transform targetWaypoint;

    float timeToWaypoint;
    float elapsedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        float elapsedPercentage = elapsedTime / timeToWaypoint;
        transform.position = Vector3.Lerp(previousWaypoint.position, targetWaypoint.position, elapsedPercentage);

        if(elapsedPercentage >= 1)
        {
            targetNextWaypoint();
        }
    }

    private void targetNextWaypoint()
    {
        previousWaypoint = _waypointPath.getWaypoint(targetWaypointIndex);
        targetWaypointIndex = _waypointPath.getNextWaypoint(targetWaypointIndex);
        targetWaypoint = _waypointPath.getWaypoint(targetWaypointIndex);

        elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(previousWaypoint.position, targetWaypoint.position);
        timeToWaypoint = distanceToWaypoint / speed;
    }
}
