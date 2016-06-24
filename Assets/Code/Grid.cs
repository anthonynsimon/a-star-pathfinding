using UnityEngine;

public class Grid : MonoBehaviour {

	[SerializeField]
	LayerMask obstacleLayer;
	[SerializeField]
	Vector2 gridSize;
	[SerializeField]
	Vector3 nodeSize;
	Node[,] grid;

	void Start() {
		GenerateGrid();
	}

	void GenerateGrid() {
		int sizeX = (int)gridSize.x;
		int sizeY = (int)gridSize.y;
		grid = new Node[sizeX, sizeY];

		Vector2 centerOffset = new Vector2((gridSize.x / 2) * nodeSize.x - nodeSize.x / 2, (gridSize.y / 2) * nodeSize.y - nodeSize.y / 2);

		for (int x = 0; x < sizeX; x++) {
			for (int y = 0; y < sizeY; y++) {
				float xCoor = x * nodeSize.x - centerOffset.x;
				float yCoor = y * nodeSize.y - centerOffset.y;
				Vector3 center = new Vector3(xCoor, yCoor, 0);

				bool walkable = !Physics.CheckSphere(center, nodeSize.x/2, obstacleLayer);
				grid[x, y] = new Node(center, walkable);
			}
		}
	}

	void OnDrawGizmos() {
		if (grid != null) {
			foreach (Node n in grid) {
				Gizmos.color = n.Walkable ? Color.black : Color.red;
				Gizmos.DrawWireCube(n.WorldPosition, nodeSize);
			}
		}
	}
}
