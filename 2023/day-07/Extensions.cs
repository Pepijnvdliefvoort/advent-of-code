using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_7
{
    public static class Extensions
    {
        public static int IndexOf<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(array[i], value))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
