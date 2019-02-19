using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skip_Lists
{
    class Program
    {
        internal class SkipListNode<T>
        {
            ///<summary>
            /// Creates a new node with the specified value
            /// at the indicated link height
            /// </summary>
            public SkipListNode(T value, int height)
            {
                Value = value;
                Next = new SkipListNode<T>[height];
            }

            public T Value { get; private set; }
            public SkipListNode<T>[] Next { get; private set; }
        }

        public class SkipList<T> : ICollection<T>
            where T : IComparable<T>
        {
            // used to determine the random height of the node links
            private readonly Random _rand = new Random();

            // the non-data node which starts the list
            private SkipListNode<T> _head;

            // there is always one level of depth (the base list)
            private int _levels = 1;

            // the number of items currently in the list
            private int _count = 0;

            public SkipList() { }

            public void Add(T item)
            {
                int level = PickRandomLevel();

                SkipListNode<T> newNode = new SkipListNode<T>(item, level + 1);
                SkipListNode<T> current = _head;

                for (int i = _levels - 1 ; i >= 0; i++)
                {
                    while (current.Next[i] != null)
                    {
                        if (current.Next[i].Value.CompareTo(item) > 0)
                        {
                            break;
                        }

                        current = current.Next[i];
                    }
                    if (i <= level)
                    {
                        // Adding "c" to the list: a -> b -> d -> e.
                        //Current is node b and current.Next[i] is d.

                        // 1. Link the new node (c) to the existing node (d):
                        //c.Next = d
                        newNode.Next[i] = current.Next[i];

                        // Insert c into the list after b
                        // b.Next = c
                        current.Next[i] = newNode;
                    }
                }

                _count++;
            }

            private int PickRandomLevel()
            {
                int rand = _rand.Next();
                int level = 0;

                while ((rand & 1) == 1)
                {
                    if (level == _levels)
                    {
                        _levels++;
                        break;
                    }
                    rand >>= 1;
                    level++;
                }

                return level;
            }

            public bool Contains(T item)
            {
                SkipListNode<T> cur = _head;
                for (int i = _levels -1; i >= 0; i--)
                {
                    while (cur.Next[i] != null)
                    {
                        int cmp = cur.Next[i].Value.CompareTo(item);

                        if (cmp > 0)
                        {
                            // the value is too large, so go down one level
                            // and take smaller steps.
                            break;
                        }
                        if (cmp == 0)
                        {
                            // found
                            return true;
                        }

                        cur = cur.Next[i];
                    }
                }
                return false;
            }
            
            public bool Remove(T item)
            {
                SkipListNode<T> cur = _head;

                bool removed = false;

                // walk down each leve in the list (make big jumps)
                for (int level = _levels; level >= 0; level--)
                {
                    // while we're not at the end of the list:
                    while (cur.Next[level] != null)
                    {
                        // if we found our node
                        if (cur.Next[level].Value.CompareTo(item) == 0)
                        {
                            // remove the node
                            cur.Next[level] = cur.Next[level].Next[level];
                            removed = true;

                            // and go down to the next level (where
                            // we will find our node again if we're
                            // not at the bottom level).
                            break;
                        }

                        // if we went too far, go down a level.
                        if (cur.Next[level].Value.CompareTo(item) > 0)
                        {
                            break;
                        }

                        cur = cur.Next[level];
                    }
                }

                if (removed)
                {
                    _count--;
                }

                return removed;
            }

            public void Clear()
            {
                _head = new SkipListNode<T>(default(T), 32 + 1);
                _count = 0;
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }

                int offset = 0;
                foreach (T item in this)
                {
                    array[arrayIndex + offset++] = item;
                }
            }

            public int Count { get { return _count; } }

            public bool IsReadOnly { get { return false; } }

            public IEnumerator<T> GetEnumerator()
            {
                SkipListNode<T> cur = _head.Next[0];
                while (cur != null)
                {
                    yield return cur.Value;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }

        static void Main(string[] args)
        {
            // data to be processed
        }
    }
}
