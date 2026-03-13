using System;
using System.Collections.Generic;
using System.Text;

namespace C23_DS
{
    public class clsBasicSearchingAlgorithms<T>
    {
        private T[] _arrToSearchIn;

        public clsBasicSearchingAlgorithms(T[] arrToSearchIn) => this._arrToSearchIn = arrToSearchIn;

        private bool _checkGivenValues(T value) => value != null && _arrToSearchIn != null;

        public int LinearSearch(T Value)
        {
            if (!_checkGivenValues(Value))
                return -1;

            for (int i = 0; i < _arrToSearchIn.Length; i++)
                if (EqualityComparer<T>.Default.Equals(_arrToSearchIn[i], Value))
                    return i; 

            return -1;
        }

        /// <summary>
        /// Find value index using binary search algorithm. Note that the array must be sorted in ascending order for this to work correctly.
        /// </summary>
        public int BinarySearch(T Value)
        {
            if (!_checkGivenValues(Value))
                return -1;

            int start = 0,
                end = _arrToSearchIn.Length - 1;

            while (start <= end)  
            {
                int mid = start + (end - start) / 2;

                if (EqualityComparer<T>.Default.Equals(_arrToSearchIn[mid], Value))
                    return mid;  

                if (Comparer<T>.Default.Compare(_arrToSearchIn[mid], Value) > 0)
                    end = mid - 1;
                else
                    start = mid + 1;
            }

            return -1;  
        }
    }
}
