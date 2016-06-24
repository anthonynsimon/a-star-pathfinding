using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T> {

    T[] items;
    int size;

    public Heap(int maxSize) {
        items = new T[maxSize];
    }

    public void Add(T item) {
        item.HeapIndex = size;
        items[size] = item;
        size++;
        PercolateUp(item);
    }

    public T RemoveMin() {
        T root = items[0];
        T newRoot = items[size - 1];
        newRoot.HeapIndex = 0;
        items[0] = newRoot;
        items[size - 1] = default(T);
        size--;
        PercolateDown(newRoot);
        return root;
    }

    public void UpdateItem(T item) {
        PercolateUp(item);
    }

    public int Count {
        get {
            return size;
        }
    }

    public bool Contains(T item) {
        return Equals(items[item.HeapIndex], item);
    }

    void PercolateUp(T item) {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true) {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0) {
                Swap(item, parentItem);
            }
            else {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void PercolateDown(T item) {
        while (true) {
            int leftIndex = item.HeapIndex * 2 + 1;
            int rightIndex = item.HeapIndex * 2 + 2;
            int swapindex = 0;

            if (leftIndex < size) {
                swapindex = leftIndex;

                if (rightIndex < size) {
                    if (items[leftIndex].CompareTo(items[rightIndex]) < 0) {
                        swapindex = rightIndex;
                    }
                }
                
                T minChild = items[swapindex];
                if (minChild.CompareTo(item) > 0) {
                    Swap(item, minChild);
                }
                else {
                    return;
                }
            }
            else {
                return;
            }
        }
    }

    void Swap(T itemA, T itemB) {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex {
        get;
        set;
    }
}
