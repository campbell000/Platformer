using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformerGame.Utils.Pools
{
    public class PairPool<T, U> : Pool<Pair<T, U>>
    {
        public Pair<T, U> get(T key, U value)
        {
            Pair<T, U> o = null;

            if (pool.Count == 0)
                throw new Exception("POOL IS DRAINED!");
            else
            {
                o = pool.Dequeue();
                o.key = key;
                o.value = value;
            }

            return o;
        }
    }
}
