using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructureStudy
{
    public class MinPriorityQueue<T>
    {
        private MinHeap<T> _minHeap = new MinHeap<T>();
        public void Insert(T Value) => _minHeap.Insert(Value);
        public T Peek() => _minHeap.Peek();
        public T ExtractMin() => _minHeap.ExtractMin();
        public int Count => _minHeap.Count;
        public bool IsEmpty() => _minHeap.IsEmpty();
        public void Clear() => _minHeap.Clear();
        public void DisplayQueue() => _minHeap.DisplayHeap();
        public bool Remove(T Value) => _minHeap.Remove(Value);
    }
}
