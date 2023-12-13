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

            return sum;
        }

        private static int PartTwo(List<string> rows)
        {
            int sum = 0;
            return sum;
        }

     }
}
