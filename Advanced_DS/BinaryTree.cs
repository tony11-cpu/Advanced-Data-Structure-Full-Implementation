using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace C23_DS
{
    public class BinaryTreeNode<T>
    {
        public T Value;
        public BinaryTreeNode<T>? Left;
        public BinaryTreeNode<T>? Right;

        public BinaryTreeNode(T Value)
        {
            this.Value = Value;

            Left = null;
            Right = null;
        }
    }

    public class BinaryTree<T>
    {
        public BinaryTreeNode<T>? Root;

        public BinaryTree() => this.Root = null;

        /// <summary>
        /// Insert To The Tree By Level-order Traversal
        /// </summary>
        public void Insert_BFS(T value)
        {
            if (Root == null)
            {
                Root = new BinaryTreeNode<T>(value);
                return;
            }

            Queue<BinaryTreeNode<T>> queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(Root);

            while (queue.Count > 0)
            {
                BinaryTreeNode<T> current = queue.Dequeue();

                if (current.Left == null)
                {
                    current.Left = new BinaryTreeNode<T>(value);
                    return;
                }

                if (current.Right == null)
                {
                    current.Right = new BinaryTreeNode<T>(value);
                    return;
                }

                queue.Enqueue(current.Left);
                queue.Enqueue(current.Right);
            }
        }

        /// <summary>
        /// Traversal Through The Tree usign Level-order Traversal
        /// </summary>
        public static void Transverse_BFS(BinaryTreeNode<T>? Root, Action<BinaryTreeNode<T>> action)
        {
            if (Root == null || action == null)
                return;

            Queue<BinaryTreeNode<T>> nodes = new Queue<BinaryTreeNode<T>>();
            nodes.Enqueue(Root);

            while(nodes.Count > 0)
            {
                BinaryTreeNode<T> node = nodes.Dequeue();

                action?.Invoke(node);

                if (node.Left != null)
                    nodes.Enqueue(node.Left);

                if(node.Right != null)
                   nodes.Enqueue(node.Right);
            }
        }

        public void Transverse_BFS(Action<BinaryTreeNode<T>> action) => Transverse_BFS(this.Root, action);

        private void PreOrderTraversal(BinaryTreeNode<T>? node , Action<BinaryTreeNode<T>>? action)
        {
            if (node == null || action == null)
                return;

            action?.Invoke(node);
            PreOrderTraversal(node.Left, action);
            PreOrderTraversal(node.Right, action);
        }

        public void PreOrderTraversal(Action<BinaryTreeNode<T>> action) => PreOrderTraversal(Root , action);

        private void PostOrderTraversal(BinaryTreeNode<T>? node, Action<BinaryTreeNode<T>> action)
        {
            if (node == null || action == null)
                return;

            PostOrderTraversal(node.Left, action);
            PostOrderTraversal(node.Right, action);
            action?.Invoke(node);
        }

        public void PostOrderTraversal(Action<BinaryTreeNode<T>> action) => PostOrderTraversal(Root , action);

        private static bool _isLarger(T Value, T ValueToCompareWith) => Comparer<T>.Default.Compare(Value, ValueToCompareWith) > 0;

        private static bool _isEqual(T Value, T ValueToCompareWith) => EqualityComparer<T>.Default.Equals(Value, ValueToCompareWith);

        public void InOrderTraversal_BTS(Action<BinaryTreeNode<T>> action) => InOrderTraversal_BTS(Root, action);

        private void InOrderTraversal_BTS(BinaryTreeNode<T>? node, Action<BinaryTreeNode<T>>? action)
        {
            if (node == null || action == null)
                return;

            InOrderTraversal_BTS(node.Left, action);
            action?.Invoke(node);
            InOrderTraversal_BTS(node.Right, action);
        }

        private BinaryTreeNode<T> Insert_BTS(BinaryTreeNode<T>? Node, T Value)
        {
            if(Node == null) return new BinaryTreeNode<T>(Value);

            if (_isLarger(Node.Value, Value))
                Node.Left = Insert_BTS(Node.Left, Value);
            else
                Node.Right = Insert_BTS(Node.Right, Value);

            return Node;
        }

        public void Insert_BTS(T Value) => this.Root = Insert_BTS(this.Root, Value);

        public static BinaryTreeNode<T>? Find(BinaryTreeNode<T>? Node, T Value)
        {
            if(Node == null || _isEqual(Node.Value , Value))
                return Node;

            if (_isLarger(Node.Value, Value))
                return Find(Node.Left, Value);

            return Find(Node.Right, Value);
        }

        public BinaryTreeNode<T>? Find(T Value) => Find(this.Root, Value);

        public static int height(BinaryTreeNode<T>? root) => root == null ? 0 : Math.Max(height(root.Left), height(root.Right)) + 1;

        public int height() => height(this.Root);
    }
}
