using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.DataStructures
{
    public class PriorityQueue<T>
    {

        private List<(T Item, int Priority)> items;

        public PriorityQueue()
        {
            items = new List<(T Item, int Priority)> ();
        }

        public void Enqueue(T item, int priority)
        {
            items.Add((item, priority));
        }

        public T Dequeue()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException();
            }
            int lowest = 0;

            for (int i = 1; i < items.Count; i++)
            {
                if (items[i].Priority < items[lowest].Priority)
                {
                    lowest = i;
                }
            }
            T item = items[lowest].Item;
            items.RemoveAt(lowest);

            return item;
        }
        
        public bool IsEmpty()
        {
            return items.Count == 0;
        }

        public int Count()
        {
            return items.Count;
        }


    }
}
