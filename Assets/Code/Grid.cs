using UnityEngine;

public class Grid : MonoBehaviour {

	[SerializeField]
	LayerMask obstacleLayer;
	[SerializeField]
	Vector2 gridSize;
	[SerializeField]
	Vector3 nodeSize;
	[SerializeField]
	Transform player;

	Node[,] grid;
	Vector2 centerOffset;
	Vector2 gridWorldSize;

	void Start() {
		CalculateOffset();
		CalculateGridWorldSize();
		GenerateGrid();
	}

	public Node GetNodeFromPoint(Vector3 worldPosition) {
		float x = (worldPosition.x + centerOffset.x) / gridWorldSize.x;
		float y = (worldPosition.y + centerOffset.y) / gridWorldSize.y;

		x = Mathf.Clamp(x * gridSize.x, 0, gridSize.x - 1);
		y = Mathf.Clamp(y * gridSize.y, 0, gridSize.y - 1);

		return grid[(int)x, (int)y];
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

				bool walkable = !Physics.CheckSphere(center, nodeSize.x / 2, obstacleLayer);
				grid[x, y] = new Node(center, walkable);
			}
		}
	}

	void OnDrawGizmos() {
		if (grid != null) {
			Node p = player != null ? GetNodeFromPoint(player.position) : null;
			foreach (Node n in grid) {
				if (n == p) {
					Gizmos.color = Color.cyan;
					Gizmos.DrawCube(n.WorldPosition, nodeSize);
				}
				else {
					Gizmos.color = n.Walkable ? Color.black : Color.red;
					Gizmos.DrawWireCube(n.WorldPosition, nodeSize);
				}
			}
		}
	}
}
