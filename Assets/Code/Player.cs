using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    [SerializeField]
    Pathfinder pathfinder;
    [SerializeField]
    int speed = 1;

    Transform thisTransform;
    Vector3[] waypoints;

    void Awake()
    {
        thisTransform = gameObject.transform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (pathfinder != null)
            {
                Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                waypoints = pathfinder.RequestPath(thisTransform.position, targetPos);
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath", waypoints);
            }
        }
    }

    IEnumerator FollowPath()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            int index = 0;
            Vector3 currentWaypoint = waypoints[index];
            while (true)
            {
                if (thisTransform.position == currentWaypoint)
                {
                    index++;
                    if (index < waypoints.Length)
                    {
                        currentWaypoint = waypoints[index];
                    }
                    else
                    {
                        waypoints = null;
                        yield break;
                    }
                }
                thisTransform.position = Vector3.MoveTowards(thisTransform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            for (int i = 1; i < waypoints.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(waypoints[i], Vector3.one * 0.1f);
                Gizmos.DrawLine(waypoints[i - 1], waypoints[i]);
            }
        }
    }
}
