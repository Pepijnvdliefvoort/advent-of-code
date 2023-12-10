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

        private static long PartOne(List<string> rows)
        {
            string pattern = @"\D";


            List<int> times = Regex.Split(rows[0], pattern).Where(c => !string.IsNullOrWhiteSpace(c)).Select(int.Parse).ToList();
            List<int> record = Regex.Split(rows[1], pattern).Where(c => !string.IsNullOrWhiteSpace(c)).Select(int.Parse).ToList();

            long sum = 1;

            for (int i = 0; i < times.Count; i++)
            {
                sum *= GetPossibleWaysToBeat(times[i], record[i]);
            }

            return sum;
        }

        private static long PartTwo(List<string> rows)
        {
            string pattern = @"\D";


            long time = long.Parse(rows[0].Split(':')[1].Replace(" ", ""));
            long record = long.Parse(rows[1].Split(':')[1].Replace(" ", ""));

            long sum = GetPossibleWaysToBeat(time, record);

            return sum;
        }

        private static long GetPossibleWaysToBeat(long time, long record)
        {
            long possibleWays = 0;
            long speed = 0;

            for (long i = time; i > 0; i--)
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
