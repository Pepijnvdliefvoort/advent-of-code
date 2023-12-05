using Advent_Of_Code_day_5;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Advent_of_Code_day_3
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
                    Almanac almanac = ReadAlmanacFromFile(filePath);

                    var result1 = PartOne(almanac);
                    var result2 = PartTwo(almanac);

                    Console.WriteLine("Part One: " + result1);
                    Console.WriteLine("Part Two: " + result2);
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

        private static long PartOne(Almanac almanac)
        {
            List<long> currentNumbers = almanac.Seeds;

            foreach (var map in almanac.Maps.Values)
            {
                currentNumbers = MapNumbers(currentNumbers, map);
            }

            return currentNumbers.Min();
        }

        private static long PartTwo(Almanac almanac)
        {
            return 0;
        }

        private static List<long> MapNumbers(List<long> numbers, CategoryMap categoryMap)
        {
            List<long> result = new List<long>();

            foreach (long number in numbers)
            {
                foreach (Mapping mapping in categoryMap.Mappings)
                {
                    if (mapping.SourceStart <= number && number < mapping.SourceStart + mapping.Length)
                    {
                        result.Add(mapping.DestinationStart + (number - mapping.SourceStart));
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Map input to <see cref="Almanac"/> object.
        /// </summary>
        /// <param name="filePath">The relative file path to the input file</param>
        /// <returns>Mapped Almanac</returns>
        private static Almanac ReadAlmanacFromFile(string filePath)
        {
            Almanac almanac = new Almanac
            {
                Seeds = new List<long>(),
                Maps = new Dictionary<string, CategoryMap>()
            };

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("seeds:"))
                    {
                        almanac.Seeds = line.Split(':')[1]
                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(long.Parse)
                        .ToList();
                    }
                    else if (line.EndsWith(" map:"))
                    {
                        string categoryName = line.Trim().Split('-')[0].ToLower();
                        CategoryMap categoryMap = new CategoryMap
                        {
                            Mappings = new List<Mapping>()
                        };

                        while ((line = reader.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
                        {
                            long[] values = line.Split().Select(long.Parse).ToArray();
                            categoryMap.Mappings.Add(new Mapping
                            {
                                DestinationStart = values[0],
                                SourceStart = values[1],
                                Length = values[2]
                            });
                        }

                        almanac.Maps[categoryName] = categoryMap;
                    }
                }
            }

            return almanac;
        }
    }
}
