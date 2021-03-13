using System.Collections.Generic;


namespace MyBox.Internal
{
    internal class FastComparable : IEqualityComparer<int>
    {
        public static FastComparable Default = new FastComparable();

        public bool Equals(int x, int y)
        {
            return x == y;
        }

        public int GetHashCode(int obj)
        {
            return obj.GetHashCode();
        }
    }
}