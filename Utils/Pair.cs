using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformerGame.Utils
{
    public class Pair<T, U>
    {
        public T key { get; set; }
        public U value { get; set; }
        public Pair(T first, U second)
        {
            this.key = first;
            this.value = second;
        }

        public Pair()
        {
            this.key = default(T);
            this.value = default(U);
        }
    }
}
