using UnityEngine;

public class PathfinderDemo : MonoBehaviour
{

    [SerializeField]
    Transform start;
    [SerializeField]
    Transform end;
    [SerializeField]
    Pathfinder pathfinder;
    [SerializeField]
    LineRenderer lineRenderer;

    Vector3 lastStartPosition;
    Vector3 lastEndPosition;

    Vector3[] path;

    void Awake()
    {
        lastStartPosition = Vector3.zero;
        lastEndPosition = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (HasChangedPosition())
        {
            lastStartPosition = start.position;
            lastEndPosition = end.position;
            GetPath();
            DisplayPath();
        }
    }

    bool HasChangedPosition()
    {
        if ((start != null) && (lastStartPosition != start.position))
        {
            return true;
        }
        if ((end != null) && (lastEndPosition != end.position))
        {
            return true;
        }
        return false;
    }

    void DisplayPath()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetVertexCount(0);
            if (path != null)
            {
                lineRenderer.SetVertexCount(path.Length);
                lineRenderer.SetPositions(path);
            }

        }
    }

    void GetPath()
    {
        if (pathfinder != null && start != null && end != null)
        {
            path = pathfinder.RequestPath(start.position, end.position);
        }
    }
}
