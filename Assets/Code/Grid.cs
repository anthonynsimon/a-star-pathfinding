using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	[SerializeField]
	LayerMask obstacleLayer;
	[SerializeField]
	Vector2 gridSize;
	[SerializeField]
	Vector2 nodeSize;
	[SerializeField]
	bool displayGrid = false;
	[SerializeField]
	float obstacleDistanceOffset = 1.0f;

	Node[,] grid;
	Vector2 centerOffset;
	Vector2 gridWorldSize;

	void Awake() {
		CalculateOffset();
		CalculateGridWorldSize();
		GenerateGrid();
	}

	public int Area {
		get {
			return (int)gridSize.x * (int)gridSize.y;
		}
	}

	public Node GetNodeFromPoint(Vector3 worldPosition) {
		float x = (worldPosition.x + centerOffset.x) / gridWorldSize.x;
		float y = (worldPosition.y + centerOffset.y) / gridWorldSize.y;

		x = Mathf.Clamp(x * gridSize.x, 0, gridSize.x - 1);
		y = Mathf.Clamp(y * gridSize.y, 0, gridSize.y - 1);

		return grid[(int)x, (int)y];
	}

	public List<Node> GetNeighbors(Node node) {
		List<Node> neighbors = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0) {
					continue;
				}

				int xIndex = node.GridPosX + x;
				int yIndex = node.GridPosY + y;

				if (xIndex < 0 || xIndex >= gridSize.x || yIndex < 0 || yIndex >= gridSize.y) {
					continue;
				}

				neighbors.Add(grid[xIndex, yIndex]);
			}
		}

		return neighbors;
	}

	void CalculateOffset() {
		float offsetX = (gridSize.x / 2) * nodeSize.x - nodeSize.x / 2;
		float offsetY = (gridSize.y / 2) * nodeSize.y - nodeSize.y / 2;
		centerOffset = new Vector2(offsetX, offsetY);
	}

	void CalculateGridWorldSize() {
		float x = gridSize.x * nodeSize.x;
		float y = gridSize.y * nodeSize.y;

		gridWorldSize = new Vector2(x, y);
	}

	void GenerateGrid() {
		int sizeX = (int)gridSize.x;
		int sizeY = (int)gridSize.y;
		grid = new Node[sizeX, sizeY];

		for (int x = 0; x < sizeX; x++) {
			for (int y = 0; y < sizeY; y++) {
				float xCoor = x * nodeSize.x - centerOffset.x;
				float yCoor = y * nodeSize.y - centerOffset.y;
				Vector3 center = new Vector3(xCoor, yCoor, 0);

				bool walkable = !Physics.CheckSphere(center, nodeSize.x + obstacleDistanceOffset, obstacleLayer);
				grid[x, y] = new Node(center, x, y, walkable ? NodeType.Walkable : NodeType.Obstacle);
			}
		}
	}

	void OnDrawGizmos() {
		if (grid != null && displayGrid) {
			foreach (Node n in grid) {
				Gizmos.color = n.Type == NodeType.Walkable ? Color.black : Color.red;
				Gizmos.DrawWireCube(n.WorldPosition, nodeSize);
			}
		}
	}
}
