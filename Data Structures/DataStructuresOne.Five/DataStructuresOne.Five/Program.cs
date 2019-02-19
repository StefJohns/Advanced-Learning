using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresOne.Five
{
    class Program
    {
        class BinaryTreeNode<TNode> : IComparable<TNode>
            where TNode : IComparable<TNode>
        {
            public BinaryTreeNode(TNode value)
            {
                Value = value;
            }

            public BinaryTreeNode<TNode> Left { get; set; }
            public BinaryTreeNode<TNode> Right { get; set; }
            public TNode Value { get; private set; }

            /// <summary>
            /// Compares the current node to the provided value.
            /// </summary>
            /// <param name="other"> The node value to compare to</param>
            /// <returns>1 if the instance value is greater than
            /// the provided value, -1 if less or 0 if equal.</returns>
            
            public int CompareTo(TNode other)
            {
                return Value.CompareTo(other);
            }


        }

        public class BinaryTree<T> : IEnumerable<T>
            where T : IComparable<T>
        {
            private BinaryTreeNode<T> _head;
            private int _count;

            public void Add(T value)
            {
                // Case 1: The tree is empty. Allocate the head.
                if (_head == null)
                {
                    _head = new BinaryTreeNode<T>(value);
                }
                // Case 2: The tree is not empty. so recursively
                // find the right location to insert the node
                else
                {
                    AddTo(_head, value);
                }

                _count++;
            }

            // Rescursive add algorithm
            private void AddTo(BinaryTreeNode<T> node, T value)
            {
                // Case 1: Value is less than the current node value
                if (value.CompareTo(node.Value) < 0)
                {
                    // If there is no left child, make this the new left
                    if (node.Left == null)
                    {
                        node.Left = new BinaryTreeNode<T>(value);
                    }
                    else
                    {
                        // else add it to the left node
                        AddTo(node.Left, value);
                    }
                }
                // Case 2: Value is equal to or greater than the current value.
                else
                {
                    // If there is no right, add it to the right,
                    if (node.Right == null)
                    {
                        node.Right = new BinaryTreeNode<T>(value);
                    }
                    else
                    {
                        // else add it to the right node
                        AddTo(node.Right, value);
                    }
                }
            }

            public bool Contains(T value)
            {
                //
                BinaryTreeNode<T> parent;
                return FindWithParent(value, out parent) != null;
            }

            public bool Remove(T value)
            {
                BinaryTreeNode<T> current, parent;

                // Find the node to remove
                current = FindWithParent(value, out parent);

                if (current == null)
                {
                    return false;
                }
                _count--;

                // Case 1: If the current has no right child, current's left replaces current
                if (current.Right == null)
                {
                    if (parent == null)
                    {
                        _head = current.Left;
                    }
                    else
                    {
                        int result = parent.CompareTo(current.Value);
                        if (result > 0)
                        {
                            // If parent value is greater than current value,
                            // make the current left child a left child of parent
                            parent.Left = current.Left;
                        }
                        else if (result < 0)
                        {
                            // If the parent value if less than current value
                            // make the current left child a right child of parent
                            parent.Right = current.Left;
                        }
                    }
                }
                // Case 2: If current's right child has no left child, current's right child
                //         replaces current.
                else if (current.Right.Left == null)
                {
                    current.Right.Left = current.Left;

                    if (parent == null)
                    {
                        _head = current.Right;
                    }
                    else
                    {
                        int result = parent.CompareTo(current.Value);
                        if (result > 0)
                        {
                            //If parent value is greater than current value,
                            //make the current right child a left child of parent.
                            parent.Left = current.Right;
                        }
                        else if (result < 0)
                        {
                            //
                            //
                            parent.Right = current.Right;
                        }
                    }
                }
                //
                //
                else
                {
                    //
                    BinaryTreeNode<T> leftmost = current.Right.Left;
                    BinaryTreeNode<T> leftmostParent = current.Right;

                    while (leftmost.Left != null)
                    {
                        leftmostParent = leftmost;
                        leftmost = leftmost.Left;
                    }

                    // 
                    leftmostParent.Left = leftmost.Right;

                    //
                    leftmost.Left = current.Left;
                    leftmost.Right = current.Right;

                    if (parent == null)
                    {
                        _head = leftmost;
                    }
                    else
                    {
                        int result = parent.CompareTo(current.Value);
                        if (result > 0)
                        {
                            //
                            //
                            parent.Left = leftmost;
                        }
                        else if (result < 0)
                        {
                            //
                            //
                            parent.Right = leftmost;
                        }
                    }
                }

                return true;
            }

            private BinaryTreeNode<T> FindWithParent(T value, out BinaryTreeNode<T> parent)
            {
                //
                BinaryTreeNode<T> current = _head;
                parent = null;

                //
                while (current != null)
                {
                    int result = current.CompareTo(value);

                    if (result > 0)
                    {
                        //
                        parent = current;
                        current = current.Left;
                    }
                    else if (result < 0)
                    {
                        //
                        parent = current;
                        current = current.Right;
                    }
                    else
                    {
                        //
                        break;
                    }
                }

                return current;
            }

            public void PreOrderTraversal(Action<T> action)
            {
                PreOrderTraversal(action, _head);
            }

            private void PreOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
            {
                if (node != null)
                {
                    action(node.Value);
                    PreOrderTraversal(action, node.Left);
                    PreOrderTraversal(action, node.Right);
                }
            }

            public void PostOrderTraversal(Action<T> action)
            {
                PostOrderTraversal(action, _head);
            }

            private void PostOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
            {
                if (node != null)
                {
                    PostOrderTraversal(action, node.Left);
                    PostOrderTraversal(action, node.Right);
                    action(node.Value);
                }
            }

            public void InOrderTraversal(Action<T> action)
            {
                InOrderTraversal(action, _head);
            }

            private void InOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
            {
                if (node != null)
                {
                    InOrderTraversal(action, node.Left);
                    action(node.Value);
                    InOrderTraversal(action, node.Right);
                }
            }

            public IEnumerator<T> InOrderTraversal()
            {
                //
                //
                if (_head != null)
                {
                    //
                    Stack<BinaryTreeNode<T>> stack = new Stack<BinaryTreeNode<T>>();

                    BinaryTreeNode<T> current = _head;

                    //
                    //
                    bool goLeftNext = true;

                    //
                    stack.Push(current);

                    while (stack.Count > 0)
                    {
                        //
                        if (goLeftNext)
                        {
                            //
                            //
                            while (current.Left != null)
                            {
                                stack.Push(current);
                                current = current.Left;
                            }
                        }

                        //
                        yield return current.Value;

                        //
                        if (current.Right != null)
                        {
                            current = current.Right;

                            //
                            //
                            goLeftNext = true;
                        }
                        else
                        {
                            //
                            //
                            current = stack.Pop();
                            goLeftNext = false;
                        }
                    }
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return InOrderTraversal();  
         }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Clear()
            {
                _head = null;
                _count = 0;
            }

            public int Count
            {
                get
                {
                    return _count;
                }
            }
        }

        static void Main(string[] args)
        {
            // Processing information
        }
    }
}
