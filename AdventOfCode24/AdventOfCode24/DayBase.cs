namespace AdventOfCode24;

public abstract class DayBase
{
    private const int DayNumber = 0;
    
    public static void Solve()
    {
        Console.WriteLine($"For day {DayNumber} we have");
        Console.WriteLine($"P1: {SolveP1()}");
        Console.WriteLine($"P2: {SolveP2()}");
    }
    
    private static List<string> GetInputs()
    {
        return InputHelper
            .ReadLines(DayNumber, EInputType.Real)
            .ToList();
    }
    
    private static int SolveP1()
    {
        return -1;
    }

    private static int SolveP2()
    {
        return -1;
    }
}