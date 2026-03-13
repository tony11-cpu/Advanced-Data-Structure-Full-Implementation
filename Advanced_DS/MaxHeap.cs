using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructureStudy
{
    public class MaxHeap<T>
    {
        private List<T> _heap;

        public MaxHeap()
        {
            _heap = new List<T>();
        }

        public void Insert(T Value)
        {
            _heap.Add(Value);
            _heapifyUp(_heap.Count - 1);
        }

        private bool _isBigger(T Value1, T Value2) => Comparer<T>.Default.Compare(Value1, Value2) > 0;

        private void _heapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (!_isBigger(_heap[index], _heap[parentIndex]))
                    break;

                (_heap[index], _heap[parentIndex]) = (_heap[parentIndex], _heap[index]);
                index = parentIndex;
            }
        }

        public T Peek()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Heap is empty.");

            return _heap[0];
        }

        public T ExtractMax()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Heap is empty.");

            T maxTempVal = _heap[0];
            _heap[0] = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);

            _heapifyDown(0);
            return maxTempVal;
        }

        private void _heapifyDown(int index)
        {
            while (index < _heap.Count)
            {
                int leftChildIndex = 2 * index + 1;
                int rightChildIndex = 2 * index + 2;

                int MaxIndex = index;

                if (leftChildIndex < _heap.Count && _isBigger(_heap[leftChildIndex], _heap[MaxIndex]))
                    MaxIndex = leftChildIndex;

                if (rightChildIndex < _heap.Count && _isBigger(_heap[rightChildIndex], _heap[MaxIndex]))
                    MaxIndex = rightChildIndex;

                if (MaxIndex == index)
                    break;

                (_heap[index], _heap[MaxIndex]) = (_heap[MaxIndex], _heap[index]);

                index = MaxIndex;
            }
        }

        public void DisplayHeap()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Heap is empty.");

            Console.WriteLine("\nHeap Elements: ");
            foreach (T value in _heap) Console.Write(value + " ");

            Console.WriteLine();
        }

        public int Count => _heap.Count;

        public bool IsEmpty() => Count == 0;

        public void Clear() => _heap.Clear();

        public bool Remove(T value)
        {
            int index = _heap.IndexOf(value);

            if (index == -1)
                return false;

            _heap[index] = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);

            if (index < _heap.Count)
            {
                _heapifyUp(index);
                _heapifyDown(index);
            }

            return true;
        }

    }
}
