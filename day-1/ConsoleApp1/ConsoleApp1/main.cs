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
                    // Calculate result
                    int result = GetSum(filePath);

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

        private static int GetSum(string fileContent)
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
                    Console.WriteLine($"{firstNumber}, {lastNumber}, {intSum}, {line}");
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
