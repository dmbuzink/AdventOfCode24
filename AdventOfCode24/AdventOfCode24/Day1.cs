namespace AdventOfCode24;

public class Day1
{
    private const int DayNumber = 1;
    
    public static void Solve()
    {
        Console.WriteLine($"For day {DayNumber} we have");
        Console.WriteLine($"P1: {SolveP1()}");
        Console.WriteLine($"P2: {SolveP2()}");
    }
    
    private static (List<int> listA, List<int> listB) GetInputs()
    {
        return InputHelper
            .ReadLines(DayNumber, EInputType.Real)
            .Aggregate((new List<int>(), new List<int>()), (lists, line) =>
            {
                var split = line.Split("   ");
                lists.Item1.Add(int.Parse(split[0]));
                lists.Item2.Add(int.Parse(split[1]));
                return lists;
            });
    }
    
    private static int SolveP1()
    {
        var (listA, listB) = GetInputs();
        var listAOrdered = listA.Order();
        var listBOrdered = listB.Order();

        return listAOrdered.Zip(listBOrdered).Sum(tuple => Math.Abs(tuple.First - tuple.Second));
    }

    private static int SolveP2()
    {
        var (listA, listB) = GetInputs();
        var rightListOccurrences = GetRightListOccurrencesDict(listB);

        return listA.Sum(number => rightListOccurrences.TryGetValue(number, out var occurrence)
            ? occurrence * number
            : 0);
    }

    private static Dictionary<int, int> GetRightListOccurrencesDict(List<int> numbers)
    {
        var dict = new Dictionary<int, int>();

        foreach (var number in numbers)
        {
            if (dict.ContainsKey(number))
            {
                dict[number]++;
            }
            else
            {
                dict[number] = 1;
            }
        }

        return dict;
    }
}