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
                    var lines = File.ReadAllText(relativeFilePath);

                    // Display the content
                    Console.WriteLine("Part One: " + PartOne(lines));
                    Console.WriteLine("Part Two: " + PartTwo(lines));
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

        private static int PartOne(string rows)
        {
            var patterns = rows.Split(new[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.None);

            int verticals = 0, horizontals = 0;

            foreach (string pattern in patterns)
            {
                var currVert = GetVerticalNumber(pattern.Split(Environment.NewLine).ToList());
                verticals += currVert;
                var currHor = GetHorizontalNumer(pattern.Split(Environment.NewLine).ToList());
                horizontals += currHor;

                if (currVert != 0)
                {
                    Console.WriteLine("Vertical mirror: " + currVert);
                    string[] lines = pattern.Split("\n");
                    for (int j = 0; j < lines.Length; j++)
                    {
                        for (int i = 0; i < lines[j].Length; i++)
                        {
                            if (i == currVert)
                            {
                                Console.Write("|");
                            }

                            Console.Write(lines[j][i]);
                        }
                        Console.WriteLine();
                    }
                }
                else if (currHor != 0)
                {
                    Console.WriteLine("Horizontal mirror: " + currHor);
                    string[] lines = pattern.Split("\n");
                    for (int j = 0; j < lines.Length; j++)
                    {
                        if (j == currHor)
                        {
                            string seperator = "";

                            for (int i = 0; i < lines[j].Length; i++)
                            {
                                seperator += "-";
                            }

                            Console.WriteLine(seperator);
                        }

                        Console.WriteLine(lines[j]);
                    }
                }
                else
                {
                    Console.WriteLine("no mirror");
                    Console.WriteLine(pattern);
                }

                Console.WriteLine();
                Console.WriteLine();
            }

            return 100 * horizontals + verticals;
        }

        private static int PartTwo(string rows)
        {
            return 0;
        }

        private static int GetVerticalNumber(List<string> pattern)
        {
            string left = "", right = "";

            for (int pointer = 1; pointer <= pattern[0].Length; pointer++)
            {
                if (pointer > pattern[0].Count() / 2)
                {
                    int toCompare = pattern[0].Count() - pointer;

                    for (int col = 0; col < pointer && col < pattern[0].Count(); col++)
                    {
                        int leftCol = col + pointer - toCompare;
                        int rightCol = col + pointer;

                        if (rightCol >= pattern[0].Count())
                            break;

                        for (int row = 0; row < pattern.Count; row++)
                        {
                            left += pattern[row][leftCol];
                            right += pattern[row][rightCol];
                        }
                    }
                }
                else
                {
                    for (int col = 0; col < pointer; col++)
                    {
                        for (int row = 0; row < pattern.Count; row++)
                        {
                            left += pattern[row][col];
                            right += pattern[row][col + pointer];
                        }
                    }

                }

                List<string> leftParts = left.SplitInParts(pattern.Count()).ToList();
                List<string> rightParts = right.SplitInParts(pattern.Count()).ToList();
                leftParts.Reverse();

                if (AreListsEqual(leftParts, rightParts) && pointer < pattern[0].Length)
                    return pointer;

                left = "";
                right = "";
            }

            return 0;
        }

        private static bool AreListsEqual(List<string> list1, List<string> list2)
        {
            if (list1.Count != list2.Count)
                return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                    return false;
            }

            return true;
        }

        private static int GetHorizontalNumer(List<string> pattern)
        {
            List<string> top = new List<string>(), bottom = new List<string>();

            for (int pointer = 1; pointer <= pattern.Count; pointer++)
            {
                int toCompare = Math.Min(pointer, pattern.Count - pointer);

                for (int row = 0; row < toCompare; row++)
                {
                    top.Add(pattern[row + pointer - toCompare]);
                    bottom.Add(pattern[row + pointer]);
                }

                top.Reverse();
                if (AreListsEqual(top, bottom) && pointer < pattern.Count)
                    return pointer;

                top.Clear();
                bottom.Clear();
            }

            return 0;
        }
    }
}
