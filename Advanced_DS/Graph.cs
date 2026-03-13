using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataStructureStudy
{
    public class Graph<T> where T : notnull
    {
        public enum enGraphDirectionType { Directed, unDirected }

        private enGraphDirectionType _GraphDirectionType = enGraphDirectionType.unDirected;

        private int _numberOfVertices;

        private int?[,] _adjacencyMatrix;

        private Dictionary<T, int> _vertexDictionary  = new Dictionary<T, int>();

        private Dictionary<int, T> _indexDictionary = new Dictionary<int, T>();

        public Graph(IList<T> vertices, enGraphDirectionType GraphDirectionType)
        {
            _GraphDirectionType = GraphDirectionType;
            _numberOfVertices = vertices.Count;

            _adjacencyMatrix = new int?[_numberOfVertices, _numberOfVertices];

            for (int i = 0; i < _numberOfVertices; i++)
            {
                _vertexDictionary[vertices[i]] = i;
                _indexDictionary[i] = vertices[i];
            }
        }

        private bool _isElementsExists(T source, T destination) => _vertexDictionary.ContainsKey(source) && _vertexDictionary.ContainsKey(destination);

        public void AddEdge(T source, T destination, int weight)
        {
            if (!_isElementsExists(source , destination))
                throw new InvalidOperationException("Source Or Destination Not Found!");
            
            _adjacencyMatrix[_vertexDictionary[source], _vertexDictionary[destination]] = weight;
            
            if (_GraphDirectionType == enGraphDirectionType.unDirected)
                _adjacencyMatrix[_vertexDictionary[destination], _vertexDictionary[source]] = weight;
        }

        public void RemoveEdge(T source, T destination)
        {
            if (!_isElementsExists(source, destination))
                throw new InvalidOperationException("Source Or Destination Not Found!");

            _adjacencyMatrix[_vertexDictionary[source], _vertexDictionary[destination]] = null;

            if(_GraphDirectionType == enGraphDirectionType.unDirected)
               _adjacencyMatrix[_vertexDictionary[destination], _vertexDictionary[source]] = null;  
        }

        public void DisplayGraph(string message = "Graph Values")
        {
            Func<T, string> str = v => v?.ToString() ?? string.Empty;
            int maxLen = 0;
            foreach (var v in _vertexDictionary.Keys) maxLen = Math.Max(maxLen, str(v).Length);
            for (int i = 0; i < _numberOfVertices; i++)
                for (int j = 0; j < _numberOfVertices; j++)
                    maxLen = Math.Max(maxLen, (_adjacencyMatrix[i, j] ?? 0).ToString().Length);

            int col = maxLen + 2;
            Console.WriteLine("\n" + message + "\n");
            Console.Write(new string(' ', col));
            foreach (var v in _vertexDictionary.Keys) Console.Write(str(v).PadRight(col));
            Console.WriteLine();
            foreach (var s in _vertexDictionary)
            {
                Console.Write(str(s.Key).PadRight(col));
                for (int j = 0; j < _numberOfVertices; j++)
                    Console.Write((_adjacencyMatrix[s.Value, j] ?? 0).ToString().PadRight(col));
                Console.WriteLine();
            }
        }

        public bool IsEdge(T source, T destination)
        {
            if (!_isElementsExists(source, destination))
                throw new InvalidOperationException("Source Or Destination Not Found!");

            return _adjacencyMatrix[_vertexDictionary[source], _vertexDictionary[destination]].HasValue;
        }

        public int GetInDegree(T vertex)
        {
            int degree = 0; 
            if (_vertexDictionary.ContainsKey(vertex))
                for (int i = 0; i < _numberOfVertices; i++)
                    if (_adjacencyMatrix[i, _vertexDictionary[vertex]].HasValue)  degree++;

            return degree;
        }

        public int GetOutDegree(T vertex)
        {
            int degree = 0;
            if (_vertexDictionary.ContainsKey(vertex))
                for (int i = 0; i < _numberOfVertices; i++)
                    if (_adjacencyMatrix[_vertexDictionary[vertex], i].HasValue) degree++;

            return degree;
        }

        public bool IsEmpty() => _vertexDictionary.Count == 0;

        public IEnumerable<T> BFS(T StartVertix)
        {
            if (!_vertexDictionary.ContainsKey(StartVertix)) throw new InvalidOperationException("Starting Vertix Do Not Exists!");

            Queue<int> valueIndexes = new Queue<int>();
            BitArray IsVisited = new BitArray(_numberOfVertices , false);

            int StartIndex = _vertexDictionary[StartVertix];
            valueIndexes.Enqueue(StartIndex);
            IsVisited[StartIndex] = true;

            while (valueIndexes.Count > 0)
            {
                int ValIndex = valueIndexes.Dequeue();
                yield return _indexDictionary[ValIndex];

                for (int i = 0; i < _numberOfVertices; i++)
                {
                    if (_adjacencyMatrix[ValIndex , i].HasValue && !IsVisited[i])
                    {
                        valueIndexes.Enqueue(i);
                        IsVisited[i] = true;
                    }
                }
            }
        }

        public IEnumerable<T> DFS(T StartVertix)
        {
            if (!_vertexDictionary.ContainsKey(StartVertix))
                throw new InvalidOperationException("Starting Vertix Do Not Exists!");

            Stack<int> valueIndexes = new Stack<int>();
            BitArray IsVisited = new BitArray(_numberOfVertices, false);

            int StartVertixIndex = _vertexDictionary[StartVertix];
            valueIndexes.Push(StartVertixIndex);
            IsVisited[StartVertixIndex] = true;

            while (valueIndexes.Count > 0)
            {
                int ValueToInspect = valueIndexes.Pop();

                yield return _indexDictionary[ValueToInspect];

                for (int i = _numberOfVertices - 1; i >= 0; i--)
                {
                    if (_adjacencyMatrix[ValueToInspect, i].HasValue && !IsVisited[i])
                    {
                        valueIndexes.Push(i);
                        IsVisited[i] = true;
                    }
                }
            }
        }

        /// <summary>
        /// Dijkstra's Algorithm — finds the shortest path from a starting node to all other nodes in a weighted graph.
        /// </summary>
        /// <returns>Dictionary Filled With The Shortest Path From "startVertix" To All Other Nodes</returns>
        /// <exception cref="InvalidOperationException">If "startVertix" Is Not Found It Might Raise An Exeption</exception>
        public Dictionary<T, (int Distance, string Path)> Dijkstra(T startVertix)
        {
            if (!_vertexDictionary.ContainsKey(startVertix))
                throw new InvalidOperationException("Invalid Starting Vertix, Please Try Onther One!");

            Dictionary<T, (int Distance, string Path)> results = new Dictionary<T, (int Distance, string Path)>();

            int[] shortestDistances = new int[_numberOfVertices];
            T[] predecessors = new T[_numberOfVertices];
            BitArray isVisited = new BitArray(_numberOfVertices , false);

            for (int i = 0; i < _numberOfVertices; i++) shortestDistances[i] = int.MaxValue;

            shortestDistances[_vertexDictionary[startVertix]] = 0;

            Func<int> MinDistanceIndex = () =>
            {
                int minIndex = -1, minDistance = int.MaxValue;
                for (int i = 0; i < _numberOfVertices; i++)
                {
                    if (!isVisited[i] && shortestDistances[i] < minDistance)
                    {
                        minIndex = i;
                        minDistance = shortestDistances[i];
                    }
                } 
                return minIndex;
            };

            for (int vertexCount = 0; vertexCount < _numberOfVertices - 1; vertexCount++)
            {
                int minIndex = MinDistanceIndex();
                isVisited[minIndex] = true;  

                for (int l = 0; l < _numberOfVertices; l++)
                {
                    int? EdgeWeight = _adjacencyMatrix[minIndex, l];

                    if (!isVisited[l] && shortestDistances[minIndex] < int.MaxValue 
                        && EdgeWeight.HasValue && shortestDistances[minIndex] + EdgeWeight < shortestDistances[l])
                    {
                        shortestDistances[l] = shortestDistances[minIndex] + EdgeWeight.Value;
                        predecessors[l] = _indexDictionary[minIndex];
                    }
                }
            }

            for (int i = 0; i < _numberOfVertices; i++) results[_indexDictionary[i]] = (shortestDistances[i], _getPath(predecessors, i));

            return results;
        }

        /// <summary>
        /// This updated version is more efficient than normal "Dijkstra" and suitable for larger graphs.
        /// </summary>
        /// <returns>Dictionary Filled With The Shortest Path From "startVertix" To All Other Nodes</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Dictionary<T, (int Distance, string Path)> EfficientDijkstra(T startVertix)
        {
            if (!_vertexDictionary.ContainsKey(startVertix)) 
                throw new InvalidOperationException("Invalid Starting Vertix, Please Try Onther One!");

            MinPriorityQueue<(int distance, int vertexIndex)> queue = new MinPriorityQueue<(int, int)>();
            Dictionary<T, (int Distance, string Path)> result = new Dictionary<T, (int, string)>();
            BitArray visited = new BitArray(_numberOfVertices, false);
            int[] vertexDistance = new int[_numberOfVertices];
            T[] predecessors = new T[_numberOfVertices];

            for (int i = 0; i < _numberOfVertices; i++) vertexDistance[i] = int.MaxValue;

            int startIndex = _vertexDictionary[startVertix];
            vertexDistance[startIndex] = 0;
            queue.Insert((0, startIndex));

            while(queue.Count > 0)
            {
                (int distance, int vertexIndex) shortestIndexVal = queue.ExtractMin();

                if (visited[shortestIndexVal.vertexIndex]) continue;

                visited[shortestIndexVal.vertexIndex] = true;

                for (int neighborVertix = 0; neighborVertix < _numberOfVertices; neighborVertix++)
                {
                    int? vDistance = _adjacencyMatrix[shortestIndexVal.vertexIndex, neighborVertix];
                    if (vDistance.HasValue && !visited[neighborVertix])
                    {
                        int newDistance = vDistance.Value + shortestIndexVal.distance;
                        if (newDistance < vertexDistance[neighborVertix])
                        {
                            queue.Remove((vertexDistance[neighborVertix] , neighborVertix));

                            vertexDistance[neighborVertix] = newDistance;
                            predecessors[neighborVertix] = _indexDictionary[shortestIndexVal.vertexIndex];

                            queue.Insert((newDistance, neighborVertix));
                        }
                    }
                }
            }

            for (int i = 0; i < _numberOfVertices; i++) result[_indexDictionary[i]] = (vertexDistance[i], _getPath(predecessors, i));

            return result;
        }

        private string _getPath(T[] predecessors, int currentIndex)
        {
            if (EqualityComparer<T>.Default.Equals(predecessors[currentIndex], default(T)))
                return _indexDictionary[currentIndex]?.ToString() ?? string.Empty;

            return _getPath(predecessors, _vertexDictionary[predecessors[currentIndex]]) + " -> " + _indexDictionary[currentIndex]?.ToString();
        }
    }
}
