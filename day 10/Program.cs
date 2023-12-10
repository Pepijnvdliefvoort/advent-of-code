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
            //string relativeFilePath = Path.Combine("input", "test.txt");
            string relativeFilePath = Path.Combine("input", "test1.txt");
            //string relativeFilePath = Path.Combine("input", "test2.txt");

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
            Tuple<int, int> startPosition = FindCharacterPosition(rows, 'S');
            List<Tuple<int, int>> closedLoop = FindClosedLoop(rows, startPosition);

            Console.WriteLine("Original Grid:");
            PrintGrid(rows);

            RemoveUnconnectedPipes(rows, closedLoop);

            Console.WriteLine("Modified Grid:");
            PrintGrid(rows);

            return 0;
        }

        private static int PartTwo(List<string> rows)
        {
            return 0;
        }

        static void PrintGrid(List<string> grid)
        {
            foreach (var line in grid)
            {
                Console.WriteLine(line);
            }
        }

        static void RemoveUnconnectedPipes(List<string> input, List<Tuple<int, int>> closedLoop)
        {
            int rows = input.Count;
            int cols = input[0].Length;

            char[,] grid = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i, j] = input[i][j];
                }
            }

            // Mark pipes on the closed loop
            foreach (var coordinate in closedLoop)
            {
                int x = coordinate.Item1;
                int y = coordinate.Item2;
                char cell = grid[x, y];

                if (IsPipe(cell))
                {
                    MarkConnectedPipes(grid, x, y);
                }
            }

            // Remove unconnected pipes
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (IsPipe(grid[i, j]) && grid[i, j] != 'C')  // 'C' marks connected pipes
                    {
                        input[i] = input[i].Remove(j, 1).Insert(j, ".");
                    }
                }
            }
        }

        static void MarkConnectedPipes(char[,] grid, int startX, int startY)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
            stack.Push(Tuple.Create(startX, startY));

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                int x = current.Item1;
                int y = current.Item2;

                if (x < 0 || x >= rows || y < 0 || y >= cols || grid[x, y] == '.' || grid[x, y] == 'C')
                    continue;

                grid[x, y] = 'C';  // Mark as connected

                // Add adjacent cells to the stack
                stack.Push(Tuple.Create(x + 1, y)); // Move down
                stack.Push(Tuple.Create(x - 1, y)); // Move up
                stack.Push(Tuple.Create(x, y + 1)); // Move right
                stack.Push(Tuple.Create(x, y - 1)); // Move left
            }
        }

        static bool IsPipe(char cell)
        {
            return cell == '|' || cell == '-';
        }

        static List<Tuple<int, int>> FindClosedLoop(List<string> input, Tuple<int, int> startPosition)
        {
            int rows = input.Count;
            int cols = input[0].Length;

            char[,] grid = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i, j] = input[i][j];
                }
            }

            int[,] visited = new int[rows, cols];
            List<Tuple<int, int>> closedLoop = new List<Tuple<int, int>>();

            DFS(grid, startPosition.Item1, startPosition.Item2, visited, closedLoop);

            return closedLoop;
        }

        static void DFS(char[,] grid, int x, int y, int[,] visited, List<Tuple<int, int>> closedLoop)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            if (x < 0 || x >= rows || y < 0 || y >= cols || visited[x, y] == 1 || grid[x, y] == '.')
                return;

            visited[x, y] = 1;
            closedLoop.Add(Tuple.Create(x, y));

            // Move in all four directions
            DFS(grid, x + 1, y, visited, closedLoop); // Move down
            DFS(grid, x - 1, y, visited, closedLoop); // Move up
            DFS(grid, x, y + 1, visited, closedLoop); // Move right
            DFS(grid, x, y - 1, visited, closedLoop); // Move left
        }

        private static Tuple<int, int> FindCharacterPosition(List<string> grid, char target)
        {
            for (int i = 0; i < grid.Count; i++)
            {
                int index = grid[i].IndexOf(target);
                if (index != -1)
                {
                    // The character is found
                    return Tuple.Create(index, i);
                }
            }

            // Character not found
            return Tuple.Create(-1, -1);
        }
    }
}
