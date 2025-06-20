using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    [SerializeField] waypointPath _waypointPath;
    [SerializeField] float speed;

    int targetWaypointIndex;

    Transform previousWaypoint;
    Transform _targetWaypoint;

    float timeToWaypoint;
    float elapsedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TargetNextWaypoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        float elapsedPercentage = elapsedTime / timeToWaypoint;
        transform.position = Vector3.Lerp(previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(previousWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage);


        if (elapsedPercentage >= 1)
        {
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        previousWaypoint = _waypointPath.getWaypoint(targetWaypointIndex);
        targetWaypointIndex = _waypointPath.getNextWaypoint(targetWaypointIndex);
        _targetWaypoint = _waypointPath.getWaypoint(targetWaypointIndex);

        elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(previousWaypoint.position, _targetWaypoint.position);
        timeToWaypoint = distanceToWaypoint / speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
