using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent_of_Code_day_3
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            //string relativeFilePath = Path.Combine("input", "input.txt");
            string relativeFilePath = Path.Combine("input", "test1.txt");

            string filePath = Path.Combine(exeDirectory, relativeFilePath);

            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    var lines = File.ReadLines(relativeFilePath);

                    // Display the content
                    //Console.WriteLine("Part One: " + PartOne(lines!.ToList()));
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
            string current = "AAA";
            string destination = "ZZZ";

            char[] instructions = rows[0].Select(c => c).ToArray();

            rows.RemoveRange(0, 2);

            int steps = 0;
            while (!current.Equals(destination))
            {
                current = Travel(current, rows, instructions, steps);
                steps++;
            }

            return steps;
        }

        private static int PartTwo(List<string> rows)
        {
            char[] instructions = rows[0].Select(c => c).ToArray();
            rows.RemoveRange(0, 2);

            List<string> positions = rows
                .Where(row => row.Substring(2, 1) == "A")
                .Select(row => row[..3])
                .ToList();

            int steps = 0;
            bool[] finished = new bool[positions.Count];

            while (finished.Contains(false))
            {
                for (int i = 0; i < positions.Count; i++)
                {
                    positions[i] = Travel(positions[i], rows, instructions, steps);

                    if (!IsLastCharacterZ(positions[i]))
                    {
                        finished[i] = false;
                        continue;
                    }

                    finished[i] = true;
                }

                steps++;
            }

            return steps;
        }

        private static string Travel(string current, List<string> rows, char[] instructions, int step)
        {
            string newCurrent = "";

            char direction = instructions[step % instructions.Length];
            int index = rows.FindIndex(str => str.Substring(0, 3).Contains(current));
            int startIndex = 0;
            switch (direction)
            {
                case 'L':
                    startIndex = 7;
                    break;
                case 'R':
                    startIndex = 12;
                    break;
                default:
                    break;
            }

            newCurrent = rows[index].Substring(startIndex, 3);

            return newCurrent;
        }

        static bool IsLastCharacterZ(string input) =>!string.IsNullOrEmpty(input) && input[input.Length - 1] == 'Z';
    }
}
