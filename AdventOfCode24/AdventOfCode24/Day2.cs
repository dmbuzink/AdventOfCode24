namespace AdventOfCode24;

public class Day2
{
    private const int DayNumber = 2;
    
    public static void Solve()
    {
        Console.WriteLine($"For day {DayNumber} we have");
        Console.WriteLine($"P1: {SolveP1()}");
        Console.WriteLine($"P2: {SolveP2()}");
    }
    
    private static List<List<int>> GetInputs()
    {
        return InputHelper
            .ReadLines(DayNumber, EInputType.Real)
            .Select(line => line.Split(" ").Select(int.Parse).ToList())
            .ToList();
    }
    
    private static int SolveP1()
    {
        var inputs = GetInputs();

        var safeReports = 0;
        foreach (var lineNumbers in inputs)
        {
            Coefficient? prevCoefficient = null;
            var isValid = true;
            for (var i = 1; i < lineNumbers.Count; i++)
            {
                var leftNumber = lineNumbers[i - 1];
                var rightNumber = lineNumbers[i];
                
                var coefficient = GetCoefficient(leftNumber, rightNumber);

                if (Math.Abs(leftNumber - rightNumber) > 3)
                {
                    isValid = false;
                    break;
                }
                
                if (prevCoefficient is not null && prevCoefficient != coefficient)
                {
                    isValid = false;
                    break;
                }
                
                prevCoefficient ??= coefficient;
            }

            if (isValid)
            {
                safeReports++;
            }
        }

        return safeReports;
    }

    private static int SolveP2()
    {
        var inputs = GetInputs();

        var safeReports = inputs.Count(report =>
        {
            for (var i = 0; i < report.Count; i++)
            {
                var reportCopy = new int[report.Count];
                report.CopyTo(reportCopy);
                var reportCopyList = reportCopy.ToList();
                reportCopyList.RemoveAt(i);
                var listje = reportCopyList.ToList();
                if (SimpleIsValid(listje))
                {
                    return true;
                }
            }

            return false;
        });

        return safeReports;
    }

    private static bool IsValid(List<int> numbers, bool hasBeenAdjusted = false)
    {
        Coefficient? prevCoefficient = null;
        for (var i = 1; i < numbers.Count; i++)
        {
            var leftNumber = numbers[i - 1];
            var rightNumber = numbers[i];
                
            var coefficient = GetCoefficient(leftNumber, rightNumber);

            if (Math.Abs(leftNumber - rightNumber) > 3)
            {
                if (hasBeenAdjusted)
                {
                    return false;
                }

                if (i == 1)
                {
                    return IsValid(numbers[i..], true);
                }
                
                return IsValid(numbers[..(i - 1)].Concat(numbers[i..]).ToList(), true) || // without i -1
                    IsValid(numbers[..i].Concat(numbers[(i+1)..]).ToList(), true);
            }
            else if (prevCoefficient is not null && prevCoefficient != coefficient)
            {
                if (hasBeenAdjusted)
                {
                    return false;
                }
                
                if (i == 1)
                {
                    return IsValid(numbers[i..], true);
                }
                
                return IsValid(numbers[..(i - 1)].Concat(numbers[i..]).ToList(), true) || // without i -1
                       IsValid(numbers[..i].Concat(numbers[(i+1)..]).ToList(), true);
            }
                
            prevCoefficient ??= coefficient;
        }

        return true;
    }

    private static bool SimpleIsValid(List<int> lineNumbers)
    {
        Coefficient? prevCoefficient = null;
        for (var i = 1; i < lineNumbers.Count; i++)
        {
            var leftNumber = lineNumbers[i - 1];
            var rightNumber = lineNumbers[i];
                
            var coefficient = GetCoefficient(leftNumber, rightNumber);

            if (coefficient == Coefficient.Same)
            {
                return false;
            }

            if (Math.Abs(leftNumber - rightNumber) > 3)
            {
                return false;
            }
                
            if (prevCoefficient is not null && prevCoefficient != coefficient)
            {
                return false;
            }
                
            prevCoefficient ??= coefficient;
        }

        return true;
    }

    private static Coefficient GetCoefficient(int n1, int n2)
    {
        if (n1 < n2)
        {
            return Coefficient.Increasing;
        }
        else if (n1 == n2)
        {
            return Coefficient.Same;
        }
        else // n1 > n2
        {
            return Coefficient.Decreasing;
        }
    }

    enum Coefficient
    {
        Decreasing = -1,
        Same = 0,
        Increasing = 1
    }
}