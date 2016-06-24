using UnityEngine;

public enum NodeType {
    Walkable,
    Obstacle
}

public class Node : IHeapItem<Node> {

    public NodeType Type;
    public Vector3 WorldPosition;
    public int GridPosX;
    public int GridPosY;
    public int FCost { get { return GCost + HCost; } }
    public int GCost;
    public int HCost;
    public Node parent;

    int heapIndex;

    public Node(Vector3 worldPosition, int gridPosX, int gridPosY, NodeType type) {
        WorldPosition = worldPosition;
        Type = type;
        GridPosX = gridPosX;
        GridPosY = gridPosY;
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int CompareTo(Node other) {
        int compare = FCost.CompareTo(other.FCost);
        if (compare == 0) {
            compare = HCost.CompareTo(other.HCost);
        }
        return -compare;
    }
}
