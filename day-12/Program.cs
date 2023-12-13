using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Advent_of_Code
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // string relativeFilePath = Path.Combine("input", "input.txt");
            string relativeFilePath = Path.Combine("input", "test.txt");

            string filePath = Path.Combine(exeDirectory, relativeFilePath);

            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    var lines = File.ReadLines(relativeFilePath);

                    // Display the content
                    Console.WriteLine("Part One: " + PartOne(lines!.ToList()));
                    Console.WriteLine("Part Two: " + PartTwo(lines!.ToList()));
                }
                else
                {
                    Console.WriteLine("File not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static int PartOne(List<string> input)
        {
            int sum = 0;

            var part1 = 0L;
            var part2 = 0L;
            var cache = new Dictionary<string, long>();

            foreach (var line in input.Select(l => l.Split(' ')))
            {
                var springs = line[0];
                var groups = line[1].Split(',').Select(int.Parse).ToList();

                part1 += Calculate(springs, groups, cache);

                springs = string.Join('?', Enumerable.Repeat(springs, 5));
                groups = Enumerable.Repeat(groups, 5).SelectMany(g => g).ToList();

                part2 += Calculate(springs, groups, cache);
            }

            Console.WriteLine($"Part1: {part1}");
            Console.WriteLine($"Part2: {part2}");


            return sum;
        }

        private static int PartTwo(List<string> rows)
        {
            return 0;
        }

        private static long Calculate(string springs, List<int> groups, Dictionary<string, long> cache)
        {
            var key = $"{springs},{string.Join(',', groups)}";  // Cache key: spring pattern + group lengths

            if (cache.TryGetValue(key, out var value))
            {
                return value;
            }

            value = GetCount(springs, groups);
            cache[key] = value;

            return value;
        }

        private static long GetCount(string springs, List<int> groups, Dictionary<string, long> cache)
        {
            while (true)
            {
                if (groups.Count == 0)
                {
                    return springs.Contains('#') ? 0 : 1; // No more groups to match: if there are no springs left, we have a match
                }

                if (string.IsNullOrEmpty(springs))
                {
                    return 0; // No more springs to match, although we still have groups to match
                }

                if (springs.StartsWith('.'))
                {
                    springs = springs.Trim('.'); // Remove all dots from the beginning
                    continue;
                }

                if (springs.StartsWith('?'))
                {
                    return Calculate("." + springs[1..], groups, cache) + Calculate("#" + springs[1..], groups, cache); // Try both options recursively
                }

                if (springs.StartsWith('#')) // Start of a group
                {
                    if (groups.Count == 0)
                    {
                        return 0; // No more groups to match, although we still have a spring in the input
                    }

                    if (springs.Length < groups[0])
                    {
                        return 0; // Not enough characters to match the group
                    }

                    if (springs[..groups[0]].Contains('.'))
                    {
                        return 0; // Group cannot contain dots for the given length
                    }

                    if (groups.Count > 1)
                    {
                        if (springs.Length < groups[0] + 1 || springs[groups[0]] == '#')
                        {
                            return 0; // Group cannot be followed by a spring, and there must be enough characters left
                        }

                        springs = springs[(groups[0] + 1)..]; // Skip the character after the group - it's either a dot or a question mark
                        groups = Array.Copy(groups, 1, groups, 0, groups.Count);
                        continue;
                    }

                    springs = springs[groups[0]..]; // Last group, no need to check the character after the group
                    groups = groups[1..];
                    continue;
                }

                throw new Exception("Invalid input");
            }
        }
    }
}
