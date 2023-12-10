using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Advent_of_Code_day_3
{
    internal class main
    {
        private static List<Asterisk> _asterisks;

        public static void Main(string[] args)
        {
            _asterisks = new List<Asterisk>();

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
            string validSymbols = "!\"#$%&\\'()*+,-/:;<=>?@[\\]^_`{|}~";

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
                            //Console.Write(curr);
                            if (validSymbols.Contains(curr))
                            {
                                valid = true;
                            }
                        }
                        //Console.WriteLine();
                    }
                    //Console.WriteLine(valid);
                    //Console.WriteLine();

                    if (valid)
                    {
                        sum += int.Parse(match.Value);
                    }
                }
            }

            return sum;
        }

        private static int PartTwo(List<string> rows)
        {
            var gearLocations = new List<(int row, int col, int partNumber)>();

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                Regex regex = new Regex(@"\d+");
                foreach (Match match in regex.Matches(row))
                {
                    var partNumber = int.Parse(match.Value);
                    var startingIndex = match.Index;
                    var endingIndex = startingIndex + match.Value.Length - 1;

                    var searchCells = GetSearchCells(startingIndex, endingIndex, i, rows.Count, row.Length);
                    var borderingGearLocations = GetBorderingGearLocations(searchCells, rows, partNumber);
                    gearLocations.AddRange(borderingGearLocations);
                }
            }

            //gearLocations.ForEach(gl => Console.WriteLine($"{gl.row}, {gl.col}, {gl.partNumber}"));

            var gearRatioSum = gearLocations
                .GroupBy(gearLocation => new
                {
                    gearLocation.row,
                    gearLocation.col,
                })
                .Select(gearLocationGroup => new
                {
                    row = gearLocationGroup.Key.row,
                    col = gearLocationGroup.Key.col,
                    partNumbers = gearLocationGroup.Select(glg => glg.partNumber).ToList(),
                })
                .Where(gearLocationGroup => gearLocationGroup.partNumbers.Count() == 2)
                .Select(gearLocationGroup => gearLocationGroup.partNumbers[0] * gearLocationGroup.partNumbers[1])
                .Sum();

            return gearRatioSum;
        }
        
        private static List<(int row, int col, int partNumber)> GetBorderingGearLocations(List<(int row, int col)> searchCells, List<string> rows, int partNumber)
        {
            var gearLocations = new List<(int row, int col, int partNumber)>();
            foreach (var searchCell in searchCells)
            {
                var cell = rows[searchCell.row][searchCell.col];
                if (cell == '*')
                {
                    gearLocations.Add((searchCell.row, searchCell.col, partNumber));
                }
            }
            return gearLocations;
        }

        private static List<(int row, int col)> GetSearchCells(int startingIndex, int endingIndex, int rowIndex, int numRows, int numColumns)
        {
            var searchCells = new List<(int row, int col)>();
            for (int i = rowIndex - 1; i < rowIndex + 2; i++)
            {
                for (int j = startingIndex - 1; j <= endingIndex + 1; j++)
                {
                    searchCells.Add((i, j));
                }
            }
            return searchCells
                .Where(sc => sc.row >= 0 && sc.row < numRows && sc.col >= 0 && sc.col < numColumns)
                .ToList();
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
                        char curr = rows[row + i][j];

                        cells[i + 1].Add(curr);
                    }
                }
            }

            return cells;
        }
    }
}
