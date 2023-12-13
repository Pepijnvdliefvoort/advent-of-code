using System.Reflection;
using System.Text;

namespace Advent_of_Code
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string relativeFilePath = Path.Combine("input", "input.txt");
            //string relativeFilePath = Path.Combine("input", "test.txt");
            //string relativeFilePath = Path.Combine("input", "test1.txt");

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

        private static int PartTwo(string rows)
        {
            var patterns = rows.Split(new[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.None);

            int verticals = 0, horizontals = 0;

            foreach (string pattern in patterns)
            {
                var oldVertical = GetVerticalNumber(pattern.Split(Environment.NewLine).ToList());
                var oldHorizontal = GetHorizontalNumer(pattern.Split(Environment.NewLine).ToList());

                var tempVertical = oldVertical;
                var tempHorizontal = oldHorizontal;

                string initialPattern = pattern.ToString();
                string newPattern = pattern.ToString();

                for (int i = 0; i < initialPattern.Length; i++)
                {
                    // Swap # with . and vice versa
                    newPattern = Swap(newPattern, i);

                    var newVertical = GetVerticalNumber(newPattern.Split(Environment.NewLine).ToList());
                    var newHorizontal = GetHorizontalNumer(newPattern.Split(Environment.NewLine).ToList());

                    // Swap back
                    newPattern = Swap(newPattern, i);

                    if (newVertical != 0 && oldVertical != newVertical && newHorizontal == oldHorizontal)
                    {
                        verticals += newVertical;
                        break;
                    }
                    else if (newHorizontal != 0 && oldHorizontal != newHorizontal && newVertical == oldVertical)
                    {
                        horizontals += newHorizontal;
                        break;
                    }
                }
                
            }

            return 100 * horizontals + verticals;
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
            }

            return 100 * horizontals + verticals;
        }

        private static string Swap(string input, int index)
        {
            StringBuilder sb = new StringBuilder(input);

            if (sb[index].Equals('#'))
            {
                sb[index] = '.';
            }
            else if (sb[index].Equals('.'))
            {
                sb[index] = '#';
            }

            return sb.ToString();
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
    }
}
