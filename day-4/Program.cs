using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Advent_of_Code_day_4
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
                    Console.WriteLine("Part Two: ");
                    PartTwo(File.ReadAllText(relativeFilePath));
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
            /*
             * example expected behaviour
             * 1 = 1
             * 2 = 1 * 2 = 2
             * 3 = 1 * 2 = 2 * 2 = 4
             * 4 = 1 * 2 = 2 * 2 = 4 * 2 = 8
             * 5 = 1 * 2 = 2 * 2 = 4 * 2 = 8 * 2 = 16
             * 6 = 1 * 2 = 2 * 2 = 4 * 2 = 8 * 2 = 16 * 2 = 32
             */

            int sum = 0;

            foreach (string row in rows)
            {
                sum += GetCardPoints(row);
            }

            return sum;
        }

        private static void PartTwo(string input)
        {
            var cards = input.Split(Environment.NewLine);
            var sum = 0.0;
            Func<int, int, string> Enlist = (start, amount) =>
            {
                List<int> ints = new List<int>();
                for (int i = start; i < start + amount; i++)
                {
                    ints.Add(i);
                }
                return string.Join(", ", ints);
            };
            var cardArray = new int[cards.Length];

            for (int i = 0; i < cards.Length; i++)
            {
                var split = cards[i].Split(new char[] { ':', '|' });
                var winningNumbers = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
                var picks = split[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
                var matches = winningNumbers.Intersect(picks).ToList();
                var points = matches.Count == 0 ? 0 : Math.Pow(2, matches.Count - 1);
                Console.WriteLine($"Card {split[0]} has {matches.Count} winning numbers, so it's worth {points}");
                sum += points;
                if (matches.Count > 0)
                {
                    Console.WriteLine($"{split[0]} has {matches.Count} matching numbers, so you win one copy each of the next {matches.Count} cards: cards {Enlist(i + 2, matches.Count)}");
                    for (int a = 1; a <= matches.Count; a++)
                    {
                        cardArray[a + i] += cardArray[i] + 1;
                    }
                }
            }
            Console.WriteLine(sum);
            Console.WriteLine($"Amount of scratchCards={cardArray.Sum() + cardArray.Length}");
        }

        private static int GetCardPoints(string row)
        {
            bool firstMatch = true;
            int sum = 0;
            var all = row.Split(':')[1].Split('|');
            var hand = all[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var winning = all[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            var cardArray = new List<int>();

            foreach (int winningNumber in winning)
            {
                foreach (int current in hand)
                {
                    if (current == winningNumber && !firstMatch)
                    {
                        sum *= 2;
                        break;
                    }
                    else if (current == winningNumber && firstMatch)
                    {
                        sum = 1;
                        firstMatch = false;
                        break;
                    }
                }
            }

            return sum;
        }
    }
}
