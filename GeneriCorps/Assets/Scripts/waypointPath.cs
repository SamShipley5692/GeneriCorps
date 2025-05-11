using UnityEngine;

public class waypointPath : MonoBehaviour
{
    public Transform getWaypoint (int wayPointIndex)
    {
        return transform.GetChild(wayPointIndex);
    }
   
    public int getNextWaypoint(int currentWaypointIndex)
    {
        int nextWaypointIndex = currentWaypointIndex + 1;

        if(nextWaypointIndex == transform.childCount)
        {
            nextWaypointIndex = 0;
        }

        return nextWaypointIndex;
    }
}
