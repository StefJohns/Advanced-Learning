using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresOne
{
    class Program
    {
        public class Node
        {
            public int Value { get; set; }
            public Node Next { get; set; }
        }

        public class LinkedListNode<T>
        {
            /// <summary>
            /// Constructs a new node with the specified value.
            /// </summary>
            public LinkedListNode(T value)
            {
                Value = value;
            }

            /// <summary>
            /// The node value.
            /// </summary>
            public T Value { get; internal set; }

            /// <summary>
            /// The next node in the list (null if last node).
            /// </summary>
            public LinkedListNode<T> Next { get; internal set; }

            /// <summary>
            /// The previous node in the list (null if first node).
            /// </summary>
            public LinkedListNode<T> Previous { get; internal set; }
        }

        public class LinkedList<T> : ICollection<T>
        {
            public LinkedListNode<T> Head
            {
                get
                {
                    return _head;
                }
            }
            public LinkedListNode<T> Tail
            {
                get
                {
                    return _tail;
                }
            }
            private LinkedListNode<T> _head;
            private LinkedListNode<T> _tail;
            public int Count
            {
                get;
                private set;
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public void Add(T value)
            {
                LinkedListNode<T> node = new LinkedListNode<T>(value);

                if(_head == null)
                {
                    _head = node;
                    _tail = node;
                } else
                {
                    _tail.Next = node;
                    _tail = node;
                }

                Count++;
            }

            public void AddFirst(T value)
            {
                LinkedListNode<T> node = new LinkedListNode<T>(value);

                // Save off the head node so as to not lose it
                LinkedListNode<T> temp = _head;

                // Point head to new node
                _head = node;

                // Insert the rest of the list behind head
                _head.Next = temp;

                if (Count == 0)
                {
                    //If the list was empty then head and tail should both point to new node
                    _tail = _head;
                }
                else
                {
                    // Before: head -------> 5 <-> 7 -> null
                    // After: head -> 3 <-> 5 <-> 7 -> null
                    temp.Previous = _head;
                }

                Count++;
            }

            public void AddLast(T value)
            {
                LinkedListNode<T> node = new LinkedListNode<T>(value);

                if (Count == 0)
                {
                    _head = node;
                }
                else
                {
                    _tail.Next = node;

                    // Before: Head -> 3 <-> 5 -> null
                    // After:  Head -> 3 <-> 5 <-> 7 -> null
                    // 7.Previous = 5
                    node.Previous = _tail;
                }

                _tail = node;
                Count++;
            }

            public void Clear()
            {
                _head = null;
                _tail = null;
                Count = 0;
            }

            public bool Contains(T item)
            {
                LinkedListNode<T> current = _head;
                while (current != null)
                {
                    if (current.Value.Equals(item))
                    {
                        return true;
                    }

                    current = current.Next;
                }

                return false;
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                LinkedListNode<T> current = _head;
                while (current != null)
                {
                    array[arrayIndex++] = current.Value;
                    current = current.Next;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<T>)this).GetEnumerator();
            }

            public bool Remove(T item)
            {
                LinkedListNode<T> previous = null;
                LinkedListNode<T> current = _head;

                // 1: Empty list: Do nothing.
                // 2: Single node: Previous is null.
                // 3: Many nodes:
                //    a. Node to remove is the first node.
                //    b. Node to remove is the middle or last.

                while (current != null)
                {
                    // Head -> 3 -> 5 -> 7 -> null
                    // Head -> 3 ------> 7 -> null
                    if (current.Value.Equals(item))
                    {
                        // It's a node in the middle or end.
                        if (previous != null)
                        {
                            // Case 3b.
                            previous.Next = current.Next;

                            // It was the end, so update _tail.
                            if (current.Next == null)
                            {
                                _tail = previous;
                            }
                            else
                            {
                                // Before: Head -> 3 <-> 5 <-> 7 -> null
                                // After:  Head -> 3 <-------> 7 -> null

                                // previous = 3
                                // current = 5
                                // current.Next = 7
                                // So, 7.Previous = 3
                                current.Next.Previous = previous;
                            }

                            Count--;
                        }
                        else
                        {
                            // Case 2 or 3a
                            RemoveFirst();
                        }
                        return true;
                    }
                    previous = current;
                    current = current.Next;
                }
                return false;
            }

            public void RemoveFirst()
            {
                if (Count != 0)
                {
                    // Before: Head -> 3 <-> 5
                    // After:  Head -------> 5

                    // Head -> 3 -> null
                    // Head ------> null
                    _head = _head.Next;

                    Count--;

                    if (Count == 0)
                    {
                        _tail = null;
                    }
                    else
                    {
                        // 5.Previous was 3; now it is null
                        _head.Previous = null;

                    }
                }
            }

            public void RemoveLast()
            {
                if (Count != 0)
                {
                    if (Count == 1)
                    {
                        _head = null;
                        _tail = null;
                    }
                    else
                    {
                        // Before: Head --> 3 --> 5 --> 7
                        //         Tail = 7
                        // After:  Head --> 3 --> 5 --> null
                        //         Tail = 5
                        // Null out 5.Next  property
                        _tail.Previous.Next = null;
                        _tail = _tail.Previous;
                    }

                    Count--;
                }
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                LinkedListNode<T> current = _head;
                while (current != null)
                {
                    yield return current.Value;
                    current = current.Next;
                }
            }

            public void ProcessListBackwards()
            {
                LinkedList<int> list = new LinkedList<int>();
                PopulateList(list);

                LinkedListNode<int> current = list.Tail;
                while (current != null)
                {
                    ProcessNode(current);
                    current = current.Previous;
                }
            }

            private void PopulateList(LinkedList<int> list)
            {
                throw new NotImplementedException();
            }

            private void ProcessNode(LinkedListNode<int> current)
            {
                throw new NotImplementedException();
            }
        }

        private static void PrintList(Node node)
        {
            while (node != null)
            {
                Console.WriteLine(node.Value);
                node = node.Next;
            }
        }

        static void Main(string[] args)
        {
            Node first = new Node { Value = 3 };
            Node middle = new Node { Value = 5 };
            first.Next = middle;
            Node last = new Node { Value = 7 };
            middle.Next = last;
            PrintList(first);
            //PrintList(first.Next);
            //PrintList(middle);
            //PrintList(middle.Next);
            //PrintList(last);
        }
    }
}
