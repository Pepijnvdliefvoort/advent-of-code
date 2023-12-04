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
            //string relativeFilePath = Path.Combine("input", "test.txt");

            // Combine the executable directory with the relative file path
            string filePath = Path.Combine(exeDirectory, relativeFilePath);

            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Calculate result
                    int partOne = PartOne(fileContent: filePath);
                    int partTwo = PartTwo(filePath);

                    // Display the content
                    Console.WriteLine(partOne);
                    Console.WriteLine(partTwo);
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

        private static int PartOne(string fileContent, int reds = 12, int greens = 13, int blues = 14)
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

        private static int PartTwo(string fileContent)
        {
            var lines = File.ReadAllLines(fileContent);
            var sum = 0;
            
            foreach (var game in lines)
            {
                int gameId = Int32.Parse(game.Split(':')[0].Split(' ')[1]);
                string[] gameColourSets = game.Split(":")[1].Split(';');
                Dictionary<string, int> gameColourSetAmounts = new Dictionary<string, int>();
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
                            if (dataAmount > gameColourSetAmounts[colourName])
                            {
                                gameColourSetAmounts[colourName] = dataAmount;
                            }
                        }
                        else
                        {
                            gameColourSetAmounts.Add(colourName, dataAmount);
                        }
                    }
                }

                var powers = 1;
                foreach (var gameColourSet in gameColourSetAmounts)
                {
                    powers *= gameColourSet.Value;
                }
                sum += powers;
            }

            return sum;
        }
    }
}
