using UnityEngine;

public class Node {

    public Vector3 WorldPosition;
    public bool Walkable;

    public Node(Vector3 worldPosition, bool walkable) {
        WorldPosition = worldPosition;
        Walkable = walkable;
    }
}