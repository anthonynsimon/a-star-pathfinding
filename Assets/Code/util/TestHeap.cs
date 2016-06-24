using UnityEngine;
using System.Collections;

public class TestHeap : MonoBehaviour {

	Heap<Node> heap = new Heap<Node>(64);

	void Awake() {
		for (int i = 0; i < 64; i++) {
			Node item = new Node(Vector3.zero, 0, 0, NodeType.Walkable);
			item.HCost = (int)Random.Range(0, 256);
			item.GCost = (int)Random.Range(0, 256);
			heap.Add(item);
		}

		int previous = -1;
		while (heap.Count > 0) {
			int current = heap.RemoveMin().FCost;
			Debug.Assert(current >= previous);
			previous = current;
		}
	}
}
