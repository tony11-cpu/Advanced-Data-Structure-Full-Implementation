using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace DataStructureStudy
{
    public class TreeNode<T>
    {
        public T Value { get; private set; }
        public List<TreeNode<T>> Children { get; private set; }

        public TreeNode(T value)
        {
            Value = value;
            Children = new List<TreeNode<T>>();
        }

        public void AddChild(TreeNode<T> Node) => this.Children.Add(Node);

        public void AddChild(T Value) => this.Children.Add(new TreeNode<T>(Value));

    }

    public class Tree<T>
    {
        public TreeNode<T>? Root { get; private set; } 

        public Tree(T Value)
        {
            Root = new TreeNode<T>(Value);
        }
        public Tree()
        {
            Root = null;
        }

        public void Insert(TreeNode<T> Node)
        {
            if (Node == null) 
                return;

            if (Root == null)
            {
                Root = Node;
                return;
            }

            Root.AddChild(Node);
        }

        public void Insert(T Node)
        {
            if (Node == null)
                return;

            if (Root == null)
            {
                Root = new TreeNode<T>(Node);
                return;
            }

            Root.AddChild(Node);
        }

        private TreeNode<T>? _find(TreeNode<T>? node, T value)
        {
            if (node == null || value == null) return null;
            if (EqualityComparer<T>.Default.Equals(node.Value, value)) return node;

            foreach (TreeNode<T> child in node.Children)
            {
                TreeNode<T>? result = _find(child, value);
                if (result != null) return result;
            }

            return null;
        }

        public TreeNode<T>? Find(T Value) => this._find(this.Root, Value);

        public void Traverse(Action<T>? action)
        {
            if (Root == null)
                return;

            foreach (TreeNode<T> node in Root.Children)
            {
                action?.Invoke(node.Value);
            }
        }
    }
}
