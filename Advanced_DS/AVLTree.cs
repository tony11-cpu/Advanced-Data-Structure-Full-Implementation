using C23_DS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DataStructureStudy
{
    public class AVLNode<T>
    {
        public T Value;
        public AVLNode<T>? Left;
        public AVLNode<T>? Right;
        public int Height;

        public AVLNode(T Val)
        {
            Value = Val;
            Left = null;
            Right = null;
            Height = 1;
        }
    }

    public class AVLTree<T>
    {
        public AVLNode<T>? Root { get; private set; }
        
        public AVLTree() 
        {
            Root = null;
        }

        public AVLTree(IEnumerable<T> values)
        {
            foreach (T item in values)
                this.Insert(item);
        }

        private bool _isBigger(T val1 , T val2) => Comparer<T>.Default.Compare(val1 , val2) > 0;

        public void Insert(T Value) => this.Root = this._insert(this.Root, Value);

        private AVLNode<T>? _insert(AVLNode<T>? Node , T Value)
        {
            if (Node == null)
                return new AVLNode<T>(Value);

            if (_isBigger(Node.Value, Value))
                Node.Left = _insert(Node.Left, Value);
            else if (_isBigger(Value, Node.Value))
                Node.Right = _insert(Node.Right, Value);
            else
                return Node;

            _updateHeight(Node);
            return _balance(Node);
        }

        private void _updateHeight(AVLNode<T> Node) => Node.Height = 1 + Math.Max(_height(Node.Left), _height(Node.Right));

        private int _height(AVLNode<T>? Node) => Node == null ? 0 : Node.Height;

        private int _balanceFactor(AVLNode<T>? Node) => Node == null ? 0 : _height(Node.Left) - _height(Node.Right);

        private AVLNode<T> _balance(AVLNode<T> Node)
        {
            int BalanceFactor = _balanceFactor(Node);

            if (Math.Abs(BalanceFactor) < 2)
                return Node;

            if(BalanceFactor > 1)
            {
                if (_balanceFactor(Node.Left) >= 0)
                    return _rightRotation(Node);
                else
                {
                    Node.Left = _leftRotation(Node.Left!);
                    return _rightRotation(Node);
                }
            }

            if(BalanceFactor < -1)
            {
                if (_balanceFactor(Node.Right) <= 0)
                    return _leftRotation(Node);
                else
                {
                    Node.Right = _rightRotation(Node.Right!);
                    return _leftRotation(Node);
                }
            }

            return Node;
        }

        private AVLNode<T> _leftRotation(AVLNode<T> Node)
        {
            if (Node == null || Node.Right == null)
                return Node!;

            AVLNode<T>? NewRoot = Node.Right;
            AVLNode<T>? OriginalLeftChild = NewRoot.Left;

            NewRoot.Left = Node;
            Node.Right = OriginalLeftChild;

            _updateHeight(Node);
            _updateHeight(NewRoot);

            return NewRoot;
        }

        private AVLNode<T> _rightRotation(AVLNode<T> OriginalNode)
        {
            if (OriginalNode == null || OriginalNode.Left == null)
                return OriginalNode!;

            AVLNode<T>? NewRoot = OriginalNode.Left;
            AVLNode<T>? OriginalRightChild = NewRoot.Right;

            NewRoot.Right = OriginalNode;
            OriginalNode.Left = OriginalRightChild;

            _updateHeight(OriginalNode);
            _updateHeight(NewRoot);

            return NewRoot;
        }

        public void Traverse(Action<T> action)

        {
            if (action == null)
                return;

            this._traverse(Root, action);
        }

        private void _traverse(AVLNode<T>? Node , Action<T> action)
        {
            if (Node == null)
                return;

            _traverse(Node.Left , action);
            action?.Invoke(Node.Value);
            _traverse(Node.Right, action!);
        }

        public void Traverse_BFS(Action<AVLNode<T>> action) => _transverse_BFS(this.Root, action);

        private void _transverse_BFS(AVLNode<T>? Root, Action<AVLNode<T>> action)
        {
            if (Root == null || action == null)
                return;

            Queue<AVLNode<T>> nodes = new Queue<AVLNode<T>>();
            nodes.Enqueue(Root);

            while (nodes.Count > 0)
            {
                AVLNode<T> node = nodes.Dequeue();

                action?.Invoke(node);

                if (node.Left != null)
                    nodes.Enqueue(node.Left);

                if (node.Right != null)
                    nodes.Enqueue(node.Right);
            }
        }

        public void Delete(T Value) => Root = _deleteNode(Root, Value);

        private AVLNode<T>? _deleteNode(AVLNode<T>? node , T Value)
        {
            if (node == null) return node;

            if (_isBigger(node.Value , Value)) node.Left = _deleteNode(node.Left, Value);
            else if(_isBigger(Value, node.Value)) node.Right = _deleteNode(node.Right, Value);
            else
            {
                if (node.Left == null)
                    return node.Right;

                if(node.Right == null)
                    return node.Left;

                AVLNode<T> tempNode = _lowestRightNode(node.Right);

                node.Value = tempNode.Value;
                node.Right = _deleteNode(node.Right, tempNode.Value);
            }

            _updateHeight(node);
            return _balance(node);
        }

        private AVLNode<T> _lowestRightNode(AVLNode<T> node)
        {
            AVLNode<T> tempNode = node;

            while (tempNode.Left != null) 
                tempNode = tempNode.Left;

            return tempNode;
        }

        public AVLNode<T>? Find(T Value) => _find(this.Root, Value);

        private AVLNode<T>? _find(AVLNode<T>? node, T Value)
        {
            if (node == null) return node;

            if (_isBigger(node.Value, Value)) return _find(node.Left, Value);
            else if (_isBigger(Value, node.Value)) return _find(node.Right, Value);
            else return node;
        }

        public bool Exists(T Value) => this._find(this.Root ,Value) != null;

        public bool IsIdentical(AVLTree<T> other) => _isIdentical(Root, other.Root);

        private bool _isIdentical(AVLNode<T>? a, AVLNode<T>? b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;

            return a.Value!.Equals(b.Value) && _isIdentical(a.Left, b.Left) && _isIdentical(a.Right, b.Right);
        }

        public static int Depth(AVLNode<T>? node)
        {
            if (node == null)
                return 0;

            return 1 + Math.Max(Depth(node.Left), Depth(node.Right));
        }

        [Obsolete("For Practice And Will Be Deleted Use [void Traverse(Action<T> action)] instead!")]
        public void Print(int Width)
        {
            if (Root == null) return;

            List<List<string>> levels = new List<List<string>>();
            Queue<AVLNode<T>?> queue = new Queue<AVLNode<T>?>();
            queue.Enqueue(Root);

            while (queue.Any(n => n != null))
            {
                List<string> level = new List<string>();
                int size = queue.Count;
                for (int i = 0; i < size; i++)
                {
                    var node = queue.Dequeue();
                    level.Add(node == null ? " " : node.Value!.ToString()!);
                    queue.Enqueue(node?.Left);
                    queue.Enqueue(node?.Right);
                }
                levels.Add(level);
            }

            foreach (var level in levels)
            {
                int spacing = (Width * 10) / (level.Count + 1);
                string line = "";
                foreach (var val in level)
                    line += val.PadLeft(spacing);
                Console.WriteLine(line);
            }
        }
    }
}
