using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

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

	Node startNode;
	Node targetNode;
	List<Node> visited;
	List<Node> path;

	void Awake() {
		visited = new List<Node>();
		path = new List<Node>();
	}

	void Update() {
		if (startPos != null && targetPos != null) {
			visited.Clear();
			path.Clear();
			FindPath(startPos.position, targetPos.position);
		}
	}

	void FindPath(Vector3 startPos, Vector3 targetPos) {
		Stopwatch sw = new Stopwatch();
		sw.Start();
		if (grid == null) {
			return;
		}

		startNode = grid.GetNodeFromPoint(startPos);
		targetNode = grid.GetNodeFromPoint(targetPos);

		if (targetNode.Type != NodeType.Walkable) {
			return;
		}

		Heap<Node> openNodes = new Heap<Node>(grid.Area);
		HashSet<Node> closedNodes = new HashSet<Node>();

		openNodes.Add(startNode);
		while (openNodes.Count > 0) {
			Node current = openNodes.RemoveMin();
			closedNodes.Add(current);

			if (current == targetNode) {
				sw.Stop();
				print(string.Format("Pathfinder time: {0}ms", sw.ElapsedMilliseconds));
				RetracePath();
				return;
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
						openNodes.UpdateItem(neighbor);
					}
				}
			}
			visited.Add(current);
		}
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

	void RetracePath() {
		Node current = targetNode;
		while (current != startNode) {
			path.Add(current);
			current = current.parent;
		}
	}

	void OnDrawGizmos() {
		if (startNode != null) {
			Gizmos.color = Color.blue;
			Gizmos.DrawCube(startNode.WorldPosition, Vector3.one * 0.25f);
		}
		if (targetNode != null) {
			Gizmos.color = Color.green;
			Gizmos.DrawCube(targetNode.WorldPosition, Vector3.one * 0.25f);
		}
		if (visited != null) {
			foreach (Node n in visited) {
				Gizmos.color = Color.cyan;
				Gizmos.DrawCube(n.WorldPosition, Vector3.one * 0.125f);
			}
		}
		if (path != null) {
			foreach (Node n in path) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(n.WorldPosition, Vector3.one * 0.125f);
			}
		}
	}
}
