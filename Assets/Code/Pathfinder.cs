using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour {

	[SerializeField]
	Grid grid;
	[SerializeField]
	Transform startPos;
	[SerializeField]
	Transform targetPos;
	[SerializeField]
	int regularCost = 10;
	[SerializeField]
	int diagonalCost = 14;

	Pathfinder Instance;

	void Awake() {
		if (Instance == null) {
			Instance = this;
		}
		else {
			Destroy(this);
		}
	}

	public Vector3[] RequestPath(Vector3 start, Vector3 end) {
		List<Vector3> path = Instance.FindPath(start, end);
		if (path == null || path.Count == 0) {
			return null;
		}
		return ReducePath(path).ToArray();
	}

	List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos) {
		if (grid == null) {
			return null;
		}

		Node startNode = grid.GetNodeFromPoint(startPos);
		Node targetNode = grid.GetNodeFromPoint(targetPos);

		if (targetNode.Type != NodeType.Walkable) {
			return null;
		}

		Heap<Node> openNodes = new Heap<Node>(grid.Area);
		HashSet<Node> closedNodes = new HashSet<Node>();

		openNodes.Add(startNode);
		while (openNodes.Count > 0) {
			Node current = openNodes.RemoveMin();
			closedNodes.Add(current);

			if (current == targetNode) {
				return RetracePath(startNode, targetNode);
			}

			foreach (Node neighbor in grid.GetNeighbors(current)) {
				if (current.Type != NodeType.Walkable || closedNodes.Contains(neighbor)) {
					continue;
				}

				int costToNeighbor = current.GCost + GetHeuristicDistance(current, neighbor, regularCost, diagonalCost);
				if (costToNeighbor < neighbor.GCost || !openNodes.Contains(neighbor)) {
					neighbor.GCost = costToNeighbor;
					neighbor.HCost = GetHeuristicDistance(neighbor, targetNode, regularCost, diagonalCost);
					neighbor.parent = current;
					if (!openNodes.Contains(neighbor)) {
						openNodes.Add(neighbor);
					}
					else {
						openNodes.UpdateItem(neighbor);
					}
				}
			}
		}
		return null;
	}

	Node GetMinFCostNode(List<Node> list) {
		int minIndex = 0;
		for (int i = 1; i < list.Count; i++) {
			if (list[i].FCost < list[minIndex].FCost) {
				minIndex = i;
			}
		}
		return list[minIndex];
	}

	int GetHeuristicDistance(Node node, Node target, int d, int d2) {
		int dx = (int)Mathf.Abs(node.GridPosX - target.GridPosX);
		int dy = (int)Mathf.Abs(node.GridPosY - target.GridPosY);
    	return d * (dx + dy) + (d2 - 2 * d) * (int)Mathf.Min(dx, dy);
	}

	List<Vector3> RetracePath(Node start, Node end) {
		List<Vector3> path = new List<Vector3>();
		Node current = end;
		while (current != start) {
			path.Add(current.WorldPosition);
			current = current.parent;
		}
		path.Add(start.WorldPosition);
		path.Reverse();
		return path;
	}

	List<Vector3> ReducePath(List<Vector3> path) {
		List<Vector3> newPath = new List<Vector3>();
		Vector3 direction = Vector3.zero;
		newPath.Add(path[0]);
		for (int i = 1; i < path.Count; i++) {
			Vector3 newDirection = path[i - 1] - path[i];
			if (newDirection != direction || i == path.Count - 1) {
				newPath.Add(path[i]);
			}
			direction = newDirection;
		}
		return newPath;
	}
}
