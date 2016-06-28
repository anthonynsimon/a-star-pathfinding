using UnityEngine;

public class Handle : MonoBehaviour
{

    Transform thisTransform;

    void Awake()
    {
        thisTransform = gameObject.transform;
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        thisTransform.position = newPosition;
    }
}