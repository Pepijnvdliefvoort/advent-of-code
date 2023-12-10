using System.Reflection;

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
            char[] order = new char[] { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };

            List<Hand> hands = new List<Hand>();

            foreach (string row in rows)
            {
                var line = row.Split(' ');
                Hand hand = new Hand(line[0], int.Parse(line[1]));
                hand.Score = CalculateScore(line[0], false);
                hands.Add(hand);
            }

            var sortedList = hands
                .OrderBy(hand => hand.Score, new ListComparer())
                .ThenBy(hand => hand.Cards, new CardComparer(order))
                .ToList();

            int i = sortedList.Count;
            int sum = 0;

            foreach (Hand hand in sortedList)
            {
                sum += hand.Bid * i;
                //Console.WriteLine($"{sum} += {hand.Bid} * {i}");
                i--;
            }

            return sum;
        }

        private static int PartTwo(List<string> rows)
        {
            char[] order = new char[] { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };

            List<Hand> hands = new List<Hand>();

            foreach (string row in rows)
            {
                var line = row.Split(' ');
                Hand hand = new Hand(line[0], int.Parse(line[1]));
                hand.Score = CalculateScore(line[0], true);
                hands.Add(hand);
                Console.WriteLine(hands.Count);
            }

            var sortedList = hands
                .OrderBy(hand => hand.Score, new ListComparer())
                .ThenBy(hand => hand.Cards, new CardComparer(order))
                .ToList();

            int i = sortedList.Count;
            int sum = 0;

            foreach (Hand hand in sortedList)
            {
                sum += hand.Bid * i;
                Console.WriteLine($"{sum} += {hand.Bid} * {i}");
                i--;
            }

            return sum;
        }

        private static List<int> CalculateScore(string cards, bool part2)
        {
            if (part2)
            {
                int maxLength = cards.Length;
                int jokers = CheckForJokers(cards);
                if (jokers > 0)
                    cards = cards.Replace("J", "");

                if (jokers == maxLength)
                    return new List<int> { maxLength };

                var score = cards.GroupBy(c => c)
                .Select(group => group.Count())
                .OrderByDescending(c => c)
                .ToList();

                score[0] += jokers;
                return score;
            }
            else
            {
                return cards.GroupBy(c => c)
                .Select(group => group.Count())
                .OrderByDescending(c => c)
                .ToList();
            }
            
        }

        private static int CheckForJokers(string cards)
        {
            int jokers = 0;
            foreach (char c in cards)
            {
                if (c.Equals('J'))
                    jokers++;
            }

            return jokers;
        }
    }

    class Hand
    {
        public string Cards { get; set; }
        public int Bid { get; set; }
        public List<int> Score { get; set; }

        public Hand(string cards, int bid)
        {
            Cards = cards;
            Bid = bid;
        }
    }

    class CardComparer : IComparer<string>
    {
        private readonly char[] customOrder;

        public CardComparer(char[] customOrder)
        {
            this.customOrder = customOrder;
        }

        public int Compare(string x, string y)
        {
            int minLength = Math.Min(x.Length, y.Length);

            for (int i = 0; i < minLength; i++)
            {
                int xIndex = Array.IndexOf(customOrder, x[i]);
                int yIndex = Array.IndexOf(customOrder, y[i]);

                if (xIndex != yIndex)
                {
                    return xIndex - yIndex;
                }
            }

            return x.Length - y.Length; // If all characters are the same, shorter string comes first
        }
    }

    class ListComparer : IComparer<List<int>>
    {
        public int Compare(List<int> x, List<int> y)
        {
            // Compare the values at the first position
            int firstValueComparison = y.First().CompareTo(x.First());

            if (firstValueComparison != 0)
            {
                return firstValueComparison; // Higher value at the first position is considered higher
            }

            // If values at the first position are the same, compare the lengths of the lists
            int lengthComparison = x.Count.CompareTo(y.Count);

            if (lengthComparison != 0)
            {
                return lengthComparison; // Longer list is considered higher
            }

            // If lengths are the same, compare values at each position
            for (int i = 0; i < x.Count; i++)
            {
                int valueComparison = x[i].CompareTo(y[i]);

                if (valueComparison != 0)
                {
                    return valueComparison; // Higher value is considered higher
                }
            }

            return 0; // Lists are equal
        }
    }
}
