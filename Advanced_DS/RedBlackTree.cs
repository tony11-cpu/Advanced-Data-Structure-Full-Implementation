using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructureStudy
{
    public class RedBlackNode<T>
    {
        public T Value = default!;

        public RedBlackNode<T>? Left;
        public RedBlackNode<T>? Right;

        public RedBlackNode<T>? Parent;
        public bool IsRed;

        public RedBlackNode(T Value)
        {
            this.Value = Value;

            Left = null;
            Right = null;
            Parent = null;

            IsRed = true;
        }
    }

    public class RedBlackTree<T>
    {
        private RedBlackNode<T>? _root;

        public RedBlackTree() => _root = null;

        private bool _isBigger(T Val1, T Val2) => Comparer<T>.Default.Compare(Val1, Val2) > 0;

        public void Insert(T Value)
        {
            if(_root == null)
            {
                _root = new RedBlackNode<T>(Value);
                _root.IsRed = false;
                return;
            }

            RedBlackNode<T> NewNode = new RedBlackNode<T>(Value);
            RedBlackNode<T> CurrentNode = _root;
            RedBlackNode<T>? ParentNode = null;

            while(CurrentNode != null)
            {
                ParentNode = CurrentNode;

                if (_isBigger(CurrentNode.Value, Value)) 
                    CurrentNode = CurrentNode.Left!;
                else 
                    CurrentNode = CurrentNode.Right!;
            }

            if (ParentNode == null)
                return;

            NewNode.Parent = ParentNode;

            if (_isBigger(ParentNode.Value, NewNode.Value))
                ParentNode.Left = NewNode;
            else
                ParentNode.Right = NewNode;

            _checkRB_TreeProperty(NewNode);
        }

        private void _checkRB_TreeProperty(RedBlackNode<T> nodeToCheck)
        {
            RedBlackNode<T> ParentNode;
            RedBlackNode<T> GrandParentNode;
            RedBlackNode<T> UncleNode;

            while(nodeToCheck != _root && (nodeToCheck.Parent!.IsRed && nodeToCheck.IsRed))
            {
                ParentNode = nodeToCheck.Parent;
                GrandParentNode = ParentNode.Parent!;

                if (ParentNode == GrandParentNode.Left)
                {
                    UncleNode = GrandParentNode.Right!;

                    if(UncleNode != null && UncleNode.IsRed)
                    {
                        GrandParentNode.IsRed = true;
                        ParentNode.IsRed = false;
                        UncleNode.IsRed = false;

                        nodeToCheck = GrandParentNode;
                    }
                    else
                    {
                        if(ParentNode.Right == nodeToCheck)
                        {
                            _leftRotation(ParentNode);

                            nodeToCheck = ParentNode;     
                            ParentNode = nodeToCheck.Parent!;
                        }

                        _rightRotation(GrandParentNode);

                        bool tempColor = GrandParentNode.IsRed;
                        GrandParentNode.IsRed = ParentNode.IsRed;
                        ParentNode.IsRed = tempColor;

                        nodeToCheck = ParentNode;
                    }
                }
                else
                {
                    UncleNode = GrandParentNode.Left!;

                    if(UncleNode != null && UncleNode.IsRed)
                    {
                        UncleNode.IsRed = false;
                        ParentNode.IsRed = false;
                        GrandParentNode.IsRed = true;

                        nodeToCheck = GrandParentNode;
                    }
                    else
                    {
                        if(ParentNode.Left == nodeToCheck)
                        {
                            _rightRotation(ParentNode);

                            nodeToCheck = ParentNode;
                            ParentNode = nodeToCheck.Parent!;
                        }

                        _leftRotation(GrandParentNode);

                        bool tempColor = GrandParentNode.IsRed;
                        GrandParentNode.IsRed = ParentNode.IsRed;
                        ParentNode.IsRed = tempColor;

                        nodeToCheck = ParentNode;
                    }
                }
            }

            _root!.IsRed = false;
        }

        private void _leftRotation(RedBlackNode<T> Node)
        {
            if (Node.Right == null)
                return;

            RedBlackNode<T> NewRoot = Node.Right;
            NewRoot.Parent = Node.Parent;

            Node.Right = NewRoot.Left;

            if (Node.Right != null)
                Node.Right.Parent = Node;

            if (Node.Parent == null)
                _root = NewRoot;
            else if (Node.Parent.Left == Node)
                Node.Parent.Left = NewRoot;
            else
                Node.Parent.Right = NewRoot;

            NewRoot.Left = Node;
            Node.Parent = NewRoot;
        }

        private void _rightRotation(RedBlackNode<T> Node)
        {
            if (Node.Left == null)
                return;

            RedBlackNode<T> NewRoot = Node.Left;
            NewRoot.Parent = Node.Parent;
            Node.Left = NewRoot.Right;

            if (Node.Left != null)
                Node.Left.Parent = Node;

            if (Node.Parent == null)
                _root = NewRoot;
            else if (Node.Parent.Left == Node)
                Node.Parent.Left = NewRoot;
            else
                Node.Parent.Right = NewRoot;

            NewRoot.Right = Node;
            Node.Parent = NewRoot;
        }

        public RedBlackNode<T>? Find(T Value) => Value == null ? null : this._find(this._root, Value);

        private RedBlackNode<T>? _find(RedBlackNode<T>? Node , T Value)
        {
            if (Node == null || EqualityComparer<T>.Default.Equals(Node.Value, Value))
                return Node;

            return _isBigger(Node.Value, Value) ? _find(Node.Left, Value) : _find(Node.Right, Value);
        }

        public bool Delete(T Value)
        {
            RedBlackNode<T>? NodeToDelete = this.Find(Value);

            if (NodeToDelete == null)
                return false;

            _deleteNode_FixViolations(NodeToDelete);
            return true;
        }

        private void _deleteNode_FixViolations(RedBlackNode<T> NodeToDelete)
        {
            bool OriginalColor = NodeToDelete.IsRed;
            RedBlackNode<T>? Child;

            if(NodeToDelete.Left == null)
            {
                Child = NodeToDelete.Right;
                _transplant(NodeToDelete, Child);
            }
            else if(NodeToDelete.Right == null)
            {
                Child = NodeToDelete.Left;
                _transplant(NodeToDelete, Child);
            }
            else
            {
                RedBlackNode<T>? Successor = Minmum(NodeToDelete.Right);
                OriginalColor = Successor.IsRed;
                Child = Successor.Right;

                if (NodeToDelete != Successor.Parent)
                {
                    _transplant(Successor, Child);

                    Successor.Right = NodeToDelete.Right;
                    Successor.Right.Parent = Successor;
                }

                Successor.Left = NodeToDelete.Left;
                Successor.Left.Parent = Successor;

                Successor.IsRed = NodeToDelete.IsRed;

                _transplant(NodeToDelete, Successor);
            }

            if (!OriginalColor) _fixViolations(Child);
        }

        private void _transplant(RedBlackNode<T> Source , RedBlackNode<T>? Replacement)
        {
            if (Source.Parent == null) _root = Replacement;
            else if (Source.Parent.Left == Source) Source.Parent.Left = Replacement;
            else Source.Parent.Right = Replacement;

            if (Replacement != null) Replacement.Parent = Source.Parent;
        }

        private RedBlackNode<T> Minmum(RedBlackNode<T> Node)
        {
            while (Node.Left != null) 
                Node = Node.Left;

            return Node;
        }

        private void _fixViolations(RedBlackNode<T>? NodeToFix)
        {
            if (NodeToFix == null) return;

            while (NodeToFix != _root && !NodeToFix!.IsRed)
            {
                if (NodeToFix == NodeToFix.Parent!.Left)
                {
                    RedBlackNode<T>? Sibling = NodeToFix.Parent.Right;

                    // Case 1 - Red Sibling - Leading To Other Cases -> 2.1 , 2.2.1 , 2.2.2
                    if (Sibling!.IsRed)
                    {
                        Sibling.IsRed = false;
                        NodeToFix.Parent.IsRed = true;
                        _leftRotation(NodeToFix.Parent);
                        Sibling = NodeToFix.Parent.Right;     
                    }

                    // Case 2.1:All Black Children 
                    if (!Sibling!.Left!.IsRed && !Sibling.Right!.IsRed)
                    {
                        Sibling.IsRed = true;
                        NodeToFix = NodeToFix.Parent;
                    }
                    else // Case 2.2.2:Near Red Child
                    {
                        if (!Sibling.Right!.IsRed)
                        {
                            Sibling.Left.IsRed = false;
                            Sibling.IsRed = true;

                            _rightRotation(Sibling);
                            
                            Sibling = NodeToFix.Parent.Right;
                        }

                        // Case 2.2.1:Far Red Child
                        Sibling!.IsRed = NodeToFix.Parent.IsRed; 
                        NodeToFix.Parent.IsRed = false;
                        Sibling.Right!.IsRed = false;

                        _leftRotation(NodeToFix.Parent);

                        NodeToFix = _root;
                    }
                }
                else // NodeToFix On The Right of The Parent -> Sibling Must Be Left
                {
                    RedBlackNode<T>? Sibling = NodeToFix.Parent.Left;

                    if(Sibling!.IsRed)
                    {
                        Sibling.IsRed = false;
                        NodeToFix.Parent.IsRed = true;
                        
                        _rightRotation(NodeToFix.Parent);

                        Sibling = NodeToFix.Parent.Left;
                    }

                    if(!Sibling!.Right!.IsRed && !Sibling.Left!.IsRed)
                    {
                        Sibling.IsRed = true;
                        NodeToFix = NodeToFix.Parent;
                    }
                    else
                    {
                        if(!Sibling.Left!.IsRed)
                        {
                            Sibling.Right.IsRed = false;
                            Sibling.IsRed = true;

                            _leftRotation(Sibling);
                            Sibling = NodeToFix.Parent.Left;
                        }

                        Sibling!.IsRed = NodeToFix.Parent.IsRed;
                        NodeToFix.Parent.IsRed = false;
                        Sibling.Left!.IsRed = false;
                        
                        _rightRotation(NodeToFix.Parent);

                        NodeToFix = _root;
                    }
                }
            }

            NodeToFix!.IsRed = false;
        }
    }
}
