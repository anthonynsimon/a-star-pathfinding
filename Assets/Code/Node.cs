using UnityEngine;

public enum NodeType {
    Walkable,
    Obstacle
}

public class Node {

    public NodeType Type;
    public Vector3 WorldPosition;
    public int GridPosX;
    public int GridPosY;
    public int FCost { get { return GCost + HCost; } }
    public int GCost;
    public int HCost;
    public Node parent;

    public Node(Vector3 worldPosition, int gridPosX, int gridPosY, NodeType type) {
        WorldPosition = worldPosition;
        Type = type;
        GridPosX = gridPosX;
        GridPosY = gridPosY;
    }
}
