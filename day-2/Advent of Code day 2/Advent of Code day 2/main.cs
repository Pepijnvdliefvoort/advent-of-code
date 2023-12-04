using System.Reflection;
using System.Text.RegularExpressions;

namespace Advent_of_Code_day_2
{
    internal class main
    {
        public static void Main(string[] args)
        {
            // Get the directory where the executable is located
            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Specify the relative path to the file
            string relativeFilePath = Path.Combine("input", "input.txt");

            // Combine the executable directory with the relative file path
            string filePath = Path.Combine(exeDirectory, relativeFilePath);

            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Calculate result
                    int result = GetSumOfGameIds(fileContent: filePath);

                    // Display the content
                    Console.WriteLine(result);
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

        private static int GetSumOfGameIds(string fileContent, int reds = 12, int greens = 13, int blues = 14)
        {
            var lines = File.ReadAllLines(fileContent);
            var sum = 0;

            foreach (var game in lines)
            {
                int gameId = Int32.Parse(game.Split(':')[0].Split(' ')[1]);
                string[] gameColourSets = game.Split(":")[1].Split(';');
                Dictionary<string, int> gameColourSetAmounts = new Dictionary<string, int>();
                var isGameValid = true;
                foreach (var gameSet in gameColourSets)
                {
                    string[] colourSets = gameSet.Split(',');
                    foreach (var colourSet in colourSets)
                    {
                        var data = colourSet.Split(" ");
                        var dataAmount = Int32.Parse(data[1]);
                        var colourName = data[2];
                        if (gameColourSetAmounts.ContainsKey(colourName))
                        {
                            gameColourSetAmounts[colourName] += dataAmount;
                        }
                        else
                        {
                            gameColourSetAmounts.Add(colourName, dataAmount);
                        }
                    }

                    if (gameColourSetAmounts.ContainsKey("red") && gameColourSetAmounts["red"] > reds)
                    {
                        isGameValid = false;
                    }

                    if (gameColourSetAmounts.ContainsKey("green") && gameColourSetAmounts["green"] > greens)
                    {
                        isGameValid = false;
                    }

                    if (gameColourSetAmounts.ContainsKey("blue") && gameColourSetAmounts["blue"] > blues)
                    {
                        isGameValid = false;
                    }
                    gameColourSetAmounts.Clear();
                }

                if (isGameValid)
                {
                    sum += gameId;
                }
            }

            return sum;
        }

        static Dictionary<string, List<int>> GetNumbersBeforeColors(string input)
        {
            Dictionary<string, List<int>> numbersBeforeColors = new Dictionary<string, List<int>>
            {
                { "red", new List<int>() },
                { "green", new List<int>() },
                { "blue", new List<int>() }
            };

            var entries = input.Split(';');

            foreach (var entry in entries)  
            {
                var matches = Regex.Matches(entry, @"(\d+) (\w+)");
                foreach (Match match in matches)
                {
                    var number = int.Parse(match.Groups[1].Value);
                    var color = match.Groups[2].Value.ToLower();

                    foreach (var kvp in numbersBeforeColors)
                    {
                        if (kvp.Key == color)
                        {
                            kvp.Value.Add(number);
                            break;
                        }
                    }
                }
            }

            return numbersBeforeColors;
        }
    }
}
