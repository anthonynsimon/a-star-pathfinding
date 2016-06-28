using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour {

	[SerializeField]
	Grid grid;
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
		List<Node> path = Instance.FindPath(start, end);
		if (path == null || path.Count == 0) {
			return null;
		}
		path = ReducePath(path);
		Vector3[] finalPath = GridPathToWorldPositions(path);
		return finalPath;
	}

	List<Node> FindPath(Vector3 startPos, Vector3 targetPos) {
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

	List<Node> RetracePath(Node start, Node end) {
		List<Node> path = new List<Node>();
		Node current = end;
		while (current != start) {
			path.Add(current);
			current = current.parent;
		}
		path.Reverse();
		return path;
	}

	List<Node> ReducePath(List<Node> path) {
		List<Node> newPath = new List<Node>();
		Vector2 direction = Vector2.zero;
		for (int i = 1; i < path.Count; i++) {
			Vector2 newDirection = new Vector2(path[i - 1].GridPosX - path[i].GridPosX, path[i - 1].GridPosY - path[i].GridPosY);
			if (newDirection != direction || i == path.Count - 1) {
				newPath.Add(path[i]);
			}
			direction = newDirection;
		}
		return newPath;
	}

	Vector3[] GridPathToWorldPositions(List<Node> path) {
		Vector3[] newPath = new Vector3[path.Count];
		for (int i = 0; i < path.Count; i++) {
			newPath[i] = path[i].WorldPosition;
		}
		return newPath;
	}
}
