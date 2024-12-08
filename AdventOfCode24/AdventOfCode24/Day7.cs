namespace AdventOfCode24;

public class Day7
{
    private const int DayNumber = 7;
    
    public static void Solve()
    {
        Console.WriteLine($"For day {DayNumber} we have");
        Console.WriteLine($"P1: {SolveP1()}");
        Console.WriteLine($"P2: {SolveP2()}");
    }
    
    private static List<(long testValue, List<long> equationNumbers)> GetInputs()
    {
        return InputHelper
            .ReadLines(DayNumber, EInputType.Real)
            .Select(line =>
            {
                var splitted = line.Split(':');
                var testValue = long.Parse(splitted[0]);
                var numbers = splitted[1].TrimStart().Split(' ').Select(long.Parse).ToList();
                return (testValue, numbers);
            })
            .ToList();
    }
    
    private static long SolveP1()
    {
        return GetInputs()
            .Where(tuple => ValidEquation(tuple.equationNumbers.Skip(1).ToList(), tuple.testValue, tuple.equationNumbers.First()))
            .Sum(tuple => tuple.testValue);
    }

    private static bool ValidEquation(List<long> numbers, long result, long sum)
    {
        if (numbers.Count == 0)
        {
            return result == sum;
        }

        return ValidEquation(numbers.Skip(1).ToList(), result, sum + numbers.First()) ||
               ValidEquation(numbers.Skip(1).ToList(), result, sum * numbers.First());
    }

    private static long SolveP2()
    {
        return GetInputs()
            .Where(tuple => ValidEquationWithConcatOperator(tuple.equationNumbers.Skip(1).ToList(), tuple.testValue, tuple.equationNumbers.First()))
            .Sum(tuple => tuple.testValue);
    }
    
    private static bool ValidEquationWithConcatOperator(List<long> numbers, long result, long sum)
    {
        if (numbers.Count == 0)
        {
            return result == sum;
        }

        return ValidEquationWithConcatOperator(numbers.Skip(1).ToList(), result, sum + numbers.First()) ||
               ValidEquationWithConcatOperator(numbers.Skip(1).ToList(), result, sum * numbers.First()) ||
               ValidEquationWithConcatOperator(numbers.Skip(1).ToList(), result, long.Parse($"{sum}{numbers.First()}"));
    }
}