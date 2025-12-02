using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ConsoleApp1
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
                    Console.WriteLine("Part One: " + PartOne(filePath));
                    Console.WriteLine("Part Two: " + PartTwo(filePath));
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

        private static int PartOne(string fileContent)
        {
            int sum = 0;

            try
            {
                string[] lines = File.ReadAllLines(fileContent);

                foreach (string line in lines)
                {
                    int firstNumber = GetDigit(line);
                    int lastNumber = GetDigit(ReverseString(line));

                    string lineSum = $"{firstNumber}{lastNumber}";
                    int intSum = int.Parse(lineSum);
                    sum += intSum;
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return sum;
        }

        private static int PartTwo(string fileContent)
        {
            var list = File.ReadAllLines(fileContent);

            var numbers = new Dictionary<string, int>() {
                {"one" ,   1}
                ,{"two" ,  2}
                ,{"three" , 3}
                ,{"four" , 4}
                ,{"five" , 5}
                ,{"six" , 6}
                ,{"seven" , 7}
                ,{"eight" , 8}
                , {"nine" , 9 }
            };
            int total = 0;
            string digit1 = string.Empty;
            string digit2 = string.Empty;
            foreach (var item in list)
            {
                //forward
                digit1 = calculateTotal(item, numbers);
                digit2 = calculateTotal(new string(item.Reverse().ToArray()), numbers.ToDictionary(k => new string(k.Key.Reverse().ToArray()), k => k.Value));
                total += Int32.Parse(digit1 + digit2);
            }

            Console.WriteLine(total);

            string calculateTotal(string item, Dictionary<string, int> numbers)
            {
                int index = 0;
                int digit = 0;
                foreach (var c in item)
                {
                    var sub = item.AsSpan(index++);
                    foreach (var n in numbers)
                    {
                        if (sub.StartsWith(n.Key))
                        {
                            digit = n.Value;
                            goto end;
                        }
                    }

                    if ((int)c >= 48 && (int)c <= 57)
                    {
                        digit = ((int)c) - 48;
                        break;
                    }
                }
                end:
                return digit.ToString();
            }

            return total;
        }

        private static int GetDigit(string line)
        {
            foreach (char character in line)
            {
                string current = character.ToString();

                if (int.TryParse(current, out int result))
                {
                    return result;
                }
            }

            return 0;
        }

        public static string ReverseString(string input)
        {
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
