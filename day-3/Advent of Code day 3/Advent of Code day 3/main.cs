using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Advent_of_Code_day_3
{
    internal class main
    {
        public static void Main(string[] args)
        {
            // Get the directory where the executable is located
            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Specify the relative path to the file
            string relativeFilePath = Path.Combine("input", "input.txt");
            //string relativeFilePath = Path.Combine("input", "test.txt");

            // Combine the executable directory with the relative file path
            string filePath = Path.Combine(exeDirectory, relativeFilePath);

            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    var lines = File.ReadLines(relativeFilePath);

                    var result = PartOne(lines!.ToList());

                    Part1(lines!.ToList());

                    // Display the content
                    Console.WriteLine("Part One: " + result);
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
            string symbols = "!\"#$%&\\'()*+,-/:;<=>?@[\\]^_`{|}~";

            int sum = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                Regex regex = new Regex(@"(\d+)");

                int index = 0;

                foreach (Match match in regex.Matches(rows[i]).Cast<Match>())
                {
                    var pos1 = rows[i].Substring(index);
                    var pos2 = pos1.IndexOf(match.Value);
                    var pos3 = pos2 + index;
                    var length = match.Value.Length;
                    index = rows[i].IndexOf(match.Value) + length;


                    var cells = GetSurroundingCells(rows, i, pos3, length);

                    bool valid = false;

                    foreach (List<char> chars in cells)
                    {
                        foreach (char curr in chars)
                        {
                            Console.Write(curr);
                            if (symbols.Contains(curr))
                            {
                                valid = true;
                            }
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine(valid);
                    Console.WriteLine();

                    if (valid)
                    {
                        sum += int.Parse(match.Value);
                    }
                }
            }

            return sum;
        }

        private static List<List<char>> GetSurroundingCells(List<string> rows, int row, int pos, int length)
        {
            List<List<char>> cells = new List<List<char>>();

            for (int i = -1; i < 2; i++)
            {
                cells.Add(new List<char>());

                for (int j = pos - 1; j < pos + length + 1; j++)
                {
                    if (row + i == -1 || j == -1 || row + i >= rows.Count || j >= rows[row + i].Length)
                    {
                        continue;
                    }
                    else
                    {
                        cells[i + 1].Add(rows[row + i][j]);
                    }
                }
            }

            return cells;
        }
    }
}
