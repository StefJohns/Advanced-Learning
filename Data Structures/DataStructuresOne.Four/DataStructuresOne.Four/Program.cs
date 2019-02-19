using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresOne.Four
{
    class Program
    {
        public class Stack<T>
        {
            Deque<T> _items = new Deque<T>();

            public void Push(T value)
            {
                _items.EnqueueFirst(value);
            }

            public T Pop() => _items.DequeueFirst();

            public T Peek() => _items.PeekFirst();

            public int Count => _items.Count;
        }

        public class Queue<T>
        {
            LinkedList<T> _items = new LinkedList<T>();

            public void Enqueue(T value) => _items.AddFirst(value);

            public T Dequeue()
            {
                if (_items.Count == 0)
                {
                    throw new InvalidOperationException("The Queue Is Empty.");
                }

                T last = _items.Last.Value;

                _items.RemoveLast();

                return last;
            }

            public T Peek()
            {
                if (_items.Count == 0)
                {
                    throw new InvalidOperationException("The Queue Is Empty");
                }

                return _items.Last.Value;
            }

            public int Count => _items.Count;
        }

        public class Deque<T>
        {
            T[] _items = new T[0];

            // The number of items in the queue
            int _size = 0;

            // The index of the first(oldest) item in the queue
            int _head = 0;

            // The index of the last(newest) item in the queue
            int _tail = -1;

            
            public void EnqueueFirst(T item)
            {
                // If the array needs to grow.
                if (_items.Length == _size)
                {
                    allocateNewArray(1);
                }

                //Since we know the array isn't full and _head is greater than 0
                //we know the slot in front of head is open
                if (_head > 0)
                {
                    _head--;
                }
                else
                {
                    //Otherwise we need to wrap around to the end of the array.
                    _head = _items.Length - 1;
                }

                _items[_head] = item;
                _size++;
            }

            private void allocateNewArray(int startingIndex)
            {
                int newLength = (_size == 0) ? 4 : _size * 2;

                T[] newArray = new T[newLength];

                if (_size > 0)
                {
                    int targetIndex = startingIndex;

                    //Copy the contents...
                    //If the array has no wrapping, just copy the valid range.
                    //Else, copy from head to end of the array and then from 0 to the tail.

                    //If tail is less than head, we've wrapped.
                    if (_tail < _head)
                    {
                        //Copy the _items[head].._items[end] -> newArray[0]..newArray[N].
                        for (int index = _head; index < _items.Length; index++)
                        {
                            newArray[targetIndex] = _items[index];
                            targetIndex++;
                        }

                        //Copy_items[0].._items[tail] -> newArray[N+1]
                        for (int index = 0; index <= _tail; index++)
                        {
                            newArray[targetIndex] = _items[index];
                            targetIndex++;
                        }
                    }
                    else
                    {
                        //Copy the _items[head].._items[tail] -> newArray[0]..newArray[N]
                        for (int index = _head; index < _tail; index++)
                        {
                            newArray[targetIndex] = _items[index];
                            targetIndex++;
                        }
                    }

                    _head = startingIndex;
                    _tail = targetIndex - 1; //Compensate for extra bump
                }
                else
                {
                    //Nothing in the array
                    _head = 0;
                    _tail = -1;
                }

                _items = newArray;
            }

            public void EnqueueLast(T item)
            {
                //If the array needs to grow.
                if (_items.Length == _size)
                {
                    allocateNewArray(0);
                }

                //Now we have a properly sized array and can focus on wrapping issues.
                //If _tail is at the end of the array we need to wrap around.
                if (_tail == _items.Length -1)
                {
                    _tail = 0;
                }
                else
                {
                    _tail++;
                }

                _items[_tail] = item;
                _size++;
            }

            public T DequeueFirst()
            {
                if (_size == 0)
                {
                    throw new InvalidOperationException("The deque is empty.");
                }

                T value = _items[_head];

                if (_head == _items.Length - 1)
                {
                    //If the head is at the last index in the array, wrap it around.
                    _head = 0;
                }
                else
                {
                    //Move to the next slot.
                    _head++;
                }

                _size--;

                return value;
            }

            public T DequeueLast()
            {
                if (_size == 0)
                {
                    throw new InvalidOperationException("The deque is empty.");
                }

                T value = _items[_tail];

                if (_tail == 0)
                {
                    //If the tail is at the first index in the array, wrap it around.
                    _tail = _items.Length - 1;
                }
                else
                {
                    //Move to the previous slot
                    _tail--;
                }

                _size--;
                return value;
            }

            public T PeekFirst()
            {
                if (_size == 0)
                {
                    throw new InvalidOperationException("The deque is empty.");
                }

                return _items[_head];
            }

            public T PeekLast()
            {
                if (_size == 0)
                {
                    throw new InvalidOperationException("The deque is empty.");
                }

                return _items[_tail];
            }

            public int Count => _size;
        }

        void RpnLoop()
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input.Trim().ToLower() == "quit")
                {
                    break;
                }

                //The stack of integers not yet operated on
                Stack<int> values = new Stack<int>();

                foreach (string token in input.Split(new char[] { ' ' }))
                {
                    // If the calue is an integer
                    int value;
                    if (int.TryParse(token, out value))
                    {
                        // push it to the stack
                        values.Push(value);
                    }
                    else
                    {
                        // Otherwise evaluate the expression
                        int rhs = values.Pop();
                        int lhs = values.Pop();

                        // and pop the result back to the stack
                        switch (token)
                        {
                            case "+":
                                values.Push(lhs + rhs);
                                break;
                            case "-":
                                values.Push(lhs - rhs);
                                break;
                            case "*":
                                values.Push(lhs * rhs);
                                break;
                            case "/":
                                values.Push(lhs / rhs);
                                break;
                            case "%":
                                values.Push(lhs % rhs);
                                break;
                            default:
                                throw new ArgumentException(string.Format("Unrecognized token: {0}", token));
                                
                        }
                    }
                }

                // The last item on the stack is the result.
                Console.WriteLine(values.Pop());
            }
        }

        static void Main(string[] args)
        {
            Deque<int> deque = new Deque<int>();

            deque.EnqueueFirst(1);
            deque.EnqueueLast(7);
            deque.EnqueueFirst(5);
            Console.WriteLine(deque.PeekFirst());
            Console.WriteLine(deque.PeekLast());
            
            deque.DequeueFirst();
            deque.DequeueLast();
            Console.WriteLine(deque.PeekFirst());
            Console.WriteLine(deque.PeekLast());
        }
    }
}
