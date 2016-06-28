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

    Vector3 startPreviousPos;
    Vector3 endPreviousPos;

    Vector3[] path;

    void Awake()
    {
        startPreviousPos = Vector3.zero;
        endPreviousPos = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (IsPosChanged())
        {
            startPreviousPos = start.position;
            endPreviousPos = end.position;
            GetPath();
            DisplayPath();
        }
    }

    bool IsPosChanged()
    {
        if (start != null && startPreviousPos != start.position)
        {
            return true;
        }
        if (end != null && endPreviousPos != end.position)
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
