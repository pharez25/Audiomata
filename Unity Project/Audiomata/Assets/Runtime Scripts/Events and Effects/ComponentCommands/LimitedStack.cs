using System.Collections.Generic;

namespace Audiomata
{
    /// <summary>
    /// Class used by Audiomata to create stacks that do not use too much memory (Inherits LinkedList)
    /// </summary>
    /// <typeparam name="T">The Value type that will be pushed onto the stack</typeparam>
   public class LimitedStack<T>: LinkedList<T>
    {
        public int MaxSize { private set; get; }

        public const int RecommendedMaxSize = 20;

        public LimitedStack(int size = RecommendedMaxSize)
        {
            MaxSize = size;
        }

        public void Push(T item)
        {
            AddFirst(item);

            if (Count > MaxSize)
            {
                RemoveLast();
            }
        }

        public T Pop()
        {
            T item = First.Value; ;
            RemoveFirst();
            return item;
        }

        public T Peek()
        {
            return First.Value;
        }
        
    }
}
