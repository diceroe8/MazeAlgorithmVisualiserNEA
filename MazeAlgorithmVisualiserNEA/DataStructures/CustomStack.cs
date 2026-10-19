using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.DataStructures
{
    // called custom stack so it does not interfere with built in C# stack
    public class CustomStack<T> // T is a generic type
    {
        private List<T> items;

        public CustomStack() // constructor
        {
            items = new List<T>();
        }

        // checker methods
        public bool IsEmpty()
        {
            return items.Count == 0; // checks if stack is empty
        }
        public int Count()
        {
            return items.Count;
        }

        // PUSH POP PEEK METHODS
        public void Push(T item)
        {
            items.Add(item); // don't really need to check if stack if full
        }

        public T Pop()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("The stack is empty");
            }

            T item = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);

            return item;
        }

        public T Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("The stack is empty");
            }
            return items[items.Count - 1];
        }
    }}
