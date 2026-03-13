using System;
using System.Collections.Generic;
using System.Text;

namespace C23_DS
{
    public class clsSimpleSortsAlgo<T>
    {
        private T[] _arr;

        public clsSimpleSortsAlgo(T[] arr) => this._arr = arr;

        public void PrintArray() => Console.WriteLine(string.Join(",", _arr));

        private void _swap(int i, int j)
        {
            T temp = _arr[i];
            _arr[i] = _arr[j];
            _arr[j] = temp;
        }

        private bool _isBigger(int ValueIndex, int ValueToCompeteIndex) => Comparer<T>.Default.Compare(_arr[ValueIndex], _arr[ValueToCompeteIndex]) > 0;

        private bool _isBigger(T ValueIndex, T ValueToCompeteIndex) => Comparer<T>.Default.Compare(ValueIndex, ValueToCompeteIndex) > 0;

        public void BubbleSort()
        {
            for (int i = 0; i < _arr.Length; i++)
            {
                bool swapped = false;

                for (int j = 0; j < _arr.Length - 1 - i; j++)
                {
                    if (_isBigger(j, j + 1))
                    {
                        _swap(j, j + 1);
                        swapped = true;
                    }
                }

                if (!swapped)
                    break;
            }
        }

        public void SelectionSort(bool AscendingOrder = true)
        {
            for (int i = 0, ValueIndex = 0; i < _arr.Length; i++)
            {
                ValueIndex = i;
                for (int j = i + 1; j < _arr.Length; j++)
                    if (AscendingOrder ? _isBigger(ValueIndex, j) : _isBigger(j, ValueIndex))
                        ValueIndex = j;

                _swap(ValueIndex, i);
            }
        }

        public void InsertionSort()
        {
            for(int i = 1; i < _arr.Length; i++)
            {
                int j = i - 1;
                T Value = _arr[i];
                
                while(j >= 0 && _isBigger(_arr[j] , Value))
                {
                    _arr[j + 1] = _arr[j];
                    j = j - 1;
                }

                _arr[j + 1] = Value; 
            }
        }
    }
}
