using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Advent_of_Code_day_3
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string relativeFilePath = Path.Combine("input", "input.txt");
            //string relativeFilePath = Path.Combine("input", "test1.txt");

            string filePath = Path.Combine(exeDirectory, relativeFilePath);

            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    var lines = File.ReadLines(relativeFilePath);

                    // Display the content
                    //Console.WriteLine("Part One: " + PartOne(lines!.ToList()));
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
            string current = "AAA";
            string destination = "ZZZ";

            char[] instructions = rows[0].Select(c => c).ToArray();

            rows.RemoveRange(0, 2);

            int steps = 0;
            while (!current.Equals(destination))
            {
                current = Travel(current, rows, instructions, steps);
                steps++;
            }

            return steps;
        }

        private static long PartTwo(List<string> rows)
        {
            var instructions = rows[0].Select(x => x == 'L' ? 0 : 1).ToArray();
            var nodes =
                rows.Skip(2)
                .Select(x => x.Split(new[] { ' ', ',', '(', ')', '=' }, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(x => x[0], x => x[1..]);

            var findloopFrequency = (string node) =>  // Scan until an end node is seen twice, first index is phase, index difference is period
            {
                var endSeen = new Dictionary<string, long>();
                for (long i = 0; true; i++)
                {
                    if (node[2] == 'Z')
                    {
                        if (endSeen.TryGetValue(node, out var lastSeen))
                            return (phase: lastSeen, period: i - lastSeen);
                        else
                            endSeen[node] = i;
                    }
                    node = nodes[node][instructions[i % instructions.Length]];
                }
            };
            var frequencies =
                nodes.Keys
                .Where(x => x[2] == 'A')
                .Select(x => findloopFrequency(x))
                .ToList();

            // Find harmony by moving harmony phase forward and increasing harmony period until it matches all frequencies
            var harmonyPhase = frequencies[0].phase;
            var harmonyPeriod = frequencies[0].period;
            foreach (var freq in frequencies.Skip(1))
            {
                // Find new harmonyPhase by increasing phase in harmony period steps until harmony matches freq
                for (; harmonyPhase < freq.phase || (harmonyPhase - freq.phase) % freq.period != 0; harmonyPhase += harmonyPeriod) ;

                // Find the new harmonyPeriod by looking for the next position the harmony frequency matches freq (brute force least common multiplier)
                long sample = harmonyPhase + harmonyPeriod;
                for (; (sample - freq.phase) % freq.period != 0; sample += harmonyPeriod) ;
                harmonyPeriod = sample - harmonyPhase;
            }

            return harmonyPhase;
        }


        //private static int PartTwo(List<string> rows)
        //{
        //    char[] instructions = rows[0].Select(c => c).ToArray();
        //    rows.RemoveRange(0, 2);

        //    List<string> positions = rows
        //        .Where(row => row.Substring(2, 1) == "A")
        //        .Select(row => row[..3])
        //        .ToList();

        //    int steps = 0;
        //    bool[] finished = new bool[positions.Count];

        //    while (finished.Contains(false))
        //    {
        //        for (int i = 0; i < positions.Count; i++)
        //        {
        //            positions[i] = Travel(positions[i], rows, instructions, steps);

        //            if (!IsLastCharacterZ(positions[i]))
        //            {
        //                finished[i] = false;
        //                continue;
        //            }

        //            finished[i] = true;
        //        }

        //        steps++;
        //    }

        //    return steps;
        //}

        private static string Travel(string current, List<string> rows, char[] instructions, int step)
        {
            string newCurrent = "";

            char direction = instructions[step % instructions.Length];
            int index = rows.FindIndex(str => str.Substring(0, 3).Contains(current));
            int startIndex = 0;
            switch (direction)
            {
                case 'L':
                    startIndex = 7;
                    break;
                case 'R':
                    startIndex = 12;
                    break;
                default:
                    break;
            }

            newCurrent = rows[index].Substring(startIndex, 3);

            return newCurrent;
        }

        static bool IsLastCharacterZ(string input) => !string.IsNullOrEmpty(input) && input[input.Length - 1] == 'Z';
    }
}
