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

        private static int PartOne(List<string> rows)
        {
            return 0;
        }

        private static int PartTwo(List<string> rows)
        {
            return 0;
        }

        private static int GetArrangements(string input, List<int> order)
        {
            int arrangements = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].Equals('.'))
                    continue;

                // Beginning of string
                if (input[i].Equals('.') && (input[i + 1].Equals('#') || input[i + 1].Equals('?')))
                    continue;

                arrangements++;
            }

            return arrangements;
        }
    }
}
