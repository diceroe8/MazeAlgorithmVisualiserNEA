using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.DataStructures
{
    public class Queue<T>
    {
        private List<T> items;

        public Queue()
        {
            items = new List<T>();
        }

        public bool IsEmpty()
        {
            return items.Count == 0;
        }

        public int Count()
        {
            return items.Count;
        }

        public void Enqueue(T item)
        {
            items.Add(item);
        }

        public T Dequeue()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("queue is empty");
            }

            T item = items[0];
            items.RemoveAt(0);

            return item;
        }

        public T Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("queue is empty");
            }

            return items[0];
        }
    }
}
