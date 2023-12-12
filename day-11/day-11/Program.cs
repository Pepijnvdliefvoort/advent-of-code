using System.Reflection;
using System.Text;

namespace Advent_of_Code
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
                    var lines = File.ReadAllLines(filePath);

                    // Display the content
                    Console.WriteLine($"Part One: {PartOne(lines)}");
                    Console.WriteLine($"Part Two: {PartTwo(lines)}");
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

        private static string PartOne(string[] rows)
        {
            var map = GalaxyMap.FromInput(rows);
            return map.SumOfShortestDistances().ToString();
        }


        private static string PartTwo(string[] rows)
        {
            var map = GalaxyMap.FromInput(rows, 1000000);
            return map.SumOfShortestDistances().ToString();
        }

        public record GalaxyMap(IList<Vector> Galaxies)
        {
            public long SumOfShortestDistances()
            {
                var pairs = Galaxies.SelectMany((g1, i) => Galaxies.Skip(i + 1).Select(g2 => (g1, g2))).ToArray();
                var sum = 0L;
                foreach (var (g1, g2) in pairs)
                {
                    sum += g1.VectorTo(g2).NumberSteps;
                }

                return sum;
            }

            public static GalaxyMap FromInput(IList<string> lines, long expansionMultiplier = 2)
            {
                var rowsToAdd = Enumerable.Range(0, lines.Count).Where(row => lines[row].All(c => c == '.')).ToArray();
                var colsToAdd = Enumerable.Range(0, lines[0].Length).Where(col => lines.All(l => l[col] == '.')).ToArray();

                var galaxies = new List<Vector>();
                for (var row = 0; row < lines.Count; row++)
                {
                    var rowOffset = rowsToAdd.Count(r => r <= row) * (expansionMultiplier - 1);
                    for (var col = 0; col < lines[0].Length; col++)
                    {
                        if (lines[row][col] != '#') continue;

                        var colOffset = colsToAdd.Count(c => c <= col) * (expansionMultiplier - 1);
                        galaxies.Add(new Vector(row + rowOffset, col + colOffset));
                    }
                }

                return new GalaxyMap(galaxies);
            }
        }

        public record Vector(long Row, long Col)
        {
            public override string ToString() => $"[{Row}, {Col}]";

            public Vector VectorTo(Vector other) => new(other.Row - Row, other.Col - Col);

            public long NumberSteps { get; } = Math.Abs(Row) + Math.Abs(Col);
        }
    }
}
