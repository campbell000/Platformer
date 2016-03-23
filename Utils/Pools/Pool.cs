using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformerGame.Utils.Pools
{
    /**
     * This class holds a bunch of objects that are already initialized. It should be used for logic and computations that require
     * many short-lived objects to be created / destroyed, as it will alleiviate the penalty of Garbage Collection.
     * 
     * Note that, in order for this class to work properly, it relies on users getting, and ALSO RETURNING, objects in the pool.
     **/
    public class Pool<T> where T : new()
    {
        protected const int DEFAULT_SIZE = 10;
        protected int size;
        protected Queue<T> pool;

        public Pool()
        {
            size = DEFAULT_SIZE;
            pool = new Queue<T>();
            init();
        }

        public Pool(int size)
        {
            this.size = size;
            pool = new Queue<T>();
            init();
        }

        protected void init()
        {
            for (int i = 0; i < size; i++)
            {
                T t = new T();
                pool.Enqueue(t);
            }
        }

        public virtual T get()
        {
            if (pool.Count == 0)
                throw new Exception("POOL IS DRAINED!");
            else
            {
                return pool.Dequeue();
            }
        }

        public void returnToPool(T o)
        {
            pool.Enqueue(o);
        }

        public void returnToPool(T o1, T o2)
        {
            pool.Enqueue(o1);
            pool.Enqueue(o2);
        }

        public void returnToPool(T o1, T o2, T o3)
        {
            pool.Enqueue(o1);
            pool.Enqueue(o2);
            pool.Enqueue(o3);
        }
    }
}
