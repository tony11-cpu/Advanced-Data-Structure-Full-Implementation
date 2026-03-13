using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructureStudy
{
    public class MinHeap<T>
    {
        private List<T> _heap;

        public MinHeap()
        {
            _heap = new List<T>();
        }

        public void Insert(T value)
        {
            _heap.Add(value);
            _heapifyUp(_heap.Count - 1);
        }

        private void _heapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (_isBigger(_heap[index], _heap[parentIndex]))
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

        public T ExtractMin()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Heap is empty.");

            T minValue = _heap[0];
            _heap[0] = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);

            _heapifyDown(0);

            return minValue;
        }

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

        private bool _isBigger(T Value1, T Value2) => Comparer<T>.Default.Compare(Value1, Value2) > 0;

        private void _heapifyDown(int index)
        {
            while (index < _heap.Count)
            {
                int leftChildIndex = 2 * index + 1;
                int rightChildIndex = 2 * index + 2;

                int smallestIndex = index;

                if (leftChildIndex < _heap.Count && _isBigger(_heap[smallestIndex], _heap[leftChildIndex]))
                    smallestIndex = leftChildIndex;

                if (rightChildIndex < _heap.Count && _isBigger(_heap[smallestIndex], _heap[rightChildIndex]))
                    smallestIndex = rightChildIndex;

                if (smallestIndex == index)
                    break;

                (_heap[index], _heap[smallestIndex]) = (_heap[smallestIndex], _heap[index]);

                index = smallestIndex;
            }
        }

        public void DisplayHeap()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Heap is empty.");

            Console.WriteLine("\nHeap Elements: ");
            foreach (T value in _heap)
                Console.Write(value + " ");

            Console.WriteLine();
        }

        public int Count => _heap.Count;

        public bool IsEmpty() => Count == 0;

        public void Clear() => _heap.Clear();
    }
}
