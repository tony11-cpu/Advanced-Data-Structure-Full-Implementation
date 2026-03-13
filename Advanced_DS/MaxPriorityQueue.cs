using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructureStudy
{
    public class MaxPriorityQueue<T>
    {
        private MaxHeap<T> _maxHeap = new MaxHeap<T>();
        public void Insert(T Value) => _maxHeap.Insert(Value);
        public T Peek() => _maxHeap.Peek();
        public T ExtractMax() => _maxHeap.ExtractMax();
        public int TasksCount => _maxHeap.Count;
        public bool IsEmpty() => _maxHeap.IsEmpty();
        public void Clear() => _maxHeap.Clear();
        public void DisplayQueue() => _maxHeap.DisplayHeap();
    }
}
