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

            string relativeFilePath = Path.Combine("input", "input.txt");
            //string relativeFilePath = Path.Combine("input", "test.txt");

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
            int sum = 0;
            foreach (string row in rows)
            {
                List<int> sequence = row.Split(' ').Select(int.Parse).ToList();
                List<List<int>> report = new List<List<int>>();
                report.Add(sequence);

                int index = 0;
                var test = report.Last().GroupBy(num => num).Count();
                while (report.Last()[0] != 0 || report.Last().GroupBy(num => num).Count() != 1)
                {
                    report.Add(GetDifferences(report[index]));

                    index++;
                }

                int num = CalculateNextNumber(report);
                sum += num;
            }

            return sum;
        }

        private static int PartTwo(List<string> rows)
        {
            return 0;
        }

        private static List<int> GetDifferences(List<int> sequence)
        {
            List<int> differences = new List<int>();

            int previous = -9999999;
            foreach (int current in sequence)
            {
                if (previous == -9999999)
                {
                    previous = current;
                    continue;
                }

                int diff = current - previous;
                previous = current;
                differences.Add(diff);
            }

            return differences;
        }

        private static int CalculateNextNumber(List<List<int>> report)
        {
            report.Last().Add(0);

            for (int i = report.Count() - 2; i >= 0; i--)
            {
                var down = report[i + 1].Last();
                var left = report[i].Last();

                report[i].Add(down + left);
            }

            return report[0].Last();
        }
    }
}
