using System.Text.RegularExpressions;

namespace AdventOfCode24;

public class Day3
{
    private const int DayNumber = 3;

    private static Regex _multiplicationInstructionRegex = new Regex(@"^mul\([0-9]{1,3},[0-9]{1,3}\).*");
    
    public static void Solve()
    {
        Console.WriteLine($"For day {DayNumber} we have");
        Console.WriteLine($"P1: {SolveP1()}");
        Console.WriteLine($"P2: {SolveP2()}");
    }
    
    private static List<string> GetInputs()
    {
        return InputHelper.ReadLines(DayNumber, EInputType.Real)
            .ToList();
    }
    
    private static int SolveP1()
    {
        var inputs = GetInputs();
        return inputs.Sum(SolveLine);
    }

    private static int SolveP2()
    {
        var oneLineInput = string.Join(string.Empty, GetInputs());
        return SolveLineWithEnabling(oneLineInput);
    }

    private static int SolveLine(string line)
    {
        var sum = 0;

        const int minimumInstructionLength = 8;
        for (var i = 0; i + minimumInstructionLength < line.Length; i++)
        {
            var instruction = line.Substring(i, 4);
            if (instruction != "mul(")
            {
                continue;
            }

            var subsection = line[i..];
            if (!_multiplicationInstructionRegex.IsMatch(subsection))
            {
                continue;
            }

            var numbersString = subsection[(subsection.IndexOf('(') + 1)..(subsection.IndexOf(')'))];
            var stringsSplit = numbersString.Split(',');
            var numbers = stringsSplit.Select(int.Parse)
                .ToArray();

            sum += numbers[0] * numbers[1];
        }

        return sum;
    }
    
    private static int SolveLineWithEnabling(string line)
    {
        var sum = 0;

        var isEnabled = true;
        const int minimumInstructionLength = 7;
        for (var i = 0; i + minimumInstructionLength < line.Length; i++)
        {
            var iLine = line[i..];
            if (iLine.StartsWith("do()"))
            {
                isEnabled = true;
                continue;
            }
            if (iLine.StartsWith("don't()"))
            {
                isEnabled = false;
                continue;
            }

            if (!isEnabled)
            {
                continue;
            }
            
            if (!iLine.StartsWith("mul("))
            {
                continue;
            }

            var subsection = line[i..];
            if (!_multiplicationInstructionRegex.IsMatch(subsection))
            {
                continue;
            }

            var numbersString = subsection[(subsection.IndexOf('(') + 1)..(subsection.IndexOf(')'))];
            var stringsSplit = numbersString.Split(',');
            var numbers = stringsSplit.Select(int.Parse)
                .ToArray();

            sum += numbers[0] * numbers[1];
        }

        return sum;
    }
}