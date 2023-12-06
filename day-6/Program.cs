using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Advent_of_Code_day_3
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
                    //Console.WriteLine("Part Two: " + PartTwo(lines!.ToList()));
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
            Tuple<int, int> races;

            string pattern = @"\D";


            List<int> times = Regex.Split(rows[0], pattern).Where(c => !string.IsNullOrWhiteSpace(c)).Select(int.Parse).ToList();
            List<int> record = Regex.Split(rows[1], pattern).Where(c => !string.IsNullOrWhiteSpace(c)).Select(int.Parse).ToList();

            int sum = 1;

            for (int i = 0; i < times.Count; i++)
            {
                sum *= GetPossibleWaysToBeat(times[i], record[i]);
            }

            return sum;
        }

        private static int PartTwo(List<string> rows)
        {
            return 0;
        }

        private static int GetPossibleWaysToBeat(int time, int record)
        {
            int possibleWays = 0;
            int speed = 0;

            for (int i = time; i > 0; i--)
            {
                if (speed * i > record)
                {
                    possibleWays++;
                }

                speed++;
            }

            return possibleWays;
        }
    }
}
