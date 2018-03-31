using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class LastQueue<T> where T: struct 
    {
        private readonly Queue<T> queue = new Queue<T>();

        public int Count { get { return queue.Count; } }

        public T? Last { get; private set; }

        public void Enqueue(T item)
        {
            queue.Enqueue(item);
            Last = item;
        }

        public T Dequeue()
        {
            if (Count == 1)
            {
                Last = null;
            }
            return queue.Dequeue();
        }

        public T Peek()
        {
            return queue.Peek();
        }
    }
}
