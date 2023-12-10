using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent_Of_Code_day_5
{
    internal static class Utilities
    {
        public static List<string> SplitByDoubleNewline(this string input, bool blankLines = false, bool shouldTrim = true)
        {
            return input
               .Split(new[] { "\r\n\r\n", "\r\r", "\n\n" }, StringSplitOptions.None)
               .Where(s => blankLines || !string.IsNullOrWhiteSpace(s))
               .Select(s => shouldTrim ? s.Trim() : s)
               .ToList();
        }

        public static IEnumerable<long> ExtractPosLongs(this string str)
        {
            return Regex.Matches(str, "\\d+").Select(m => long.Parse(m.Value));
        }

        public static IEnumerable<long> ExtractLongs(this string str)
        {
            return Regex.Matches(str, "-?\\d+").Select(m => long.Parse(m.Value));
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> array, int size)
        {
            for (int i = 0; i < (float)array.Count() / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }
    }
}
