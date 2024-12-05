namespace AdventOfCode24;

public class Day5
{
    private const int DayNumber = 5;
    
    public static void Solve()
    {
        Console.WriteLine($"For day {DayNumber} we have");
        Console.WriteLine($"P1: {SolveP1()}");
        Console.WriteLine($"P2: {SolveP2()}");
    }
    
    private static List<String> GetInputs()
    {
        return InputHelper
            .ReadLines(DayNumber, EInputType.Real)
            .ToList();
    }

    private static (List<(int pageNumber, int beforePageNumber)> requirements, List<List<int>> manuals) GetPreppedInputs()
    {
        var inputs = GetInputs();
        var emptyLineIndex = inputs.IndexOf(string.Empty);

        var requirementInputs = inputs[..emptyLineIndex];
        var manualInputs = inputs[(emptyLineIndex + 1)..];

        var requirements = requirementInputs.Select(line =>
        {
            var lineSplit = line.Split('|');
            return (int.Parse(lineSplit[0]), int.Parse(lineSplit[1]));
        }).ToList();

        var pageOrders = manualInputs
            .Select(line => line.Split(',').Select(int.Parse).ToList())
            .ToList();

        return (requirements, pageOrders);
    }
    
    private static int SolveP1()
    {
        var (requirements, manualPages) = GetPreppedInputs();

        // var requirementsPredicates = new List<Func<int, int, int>>();
        // foreach (var (pageNumber, beforePageNumber) in requirements)
        // {
        //     requirementsPredicates.Add((a, b) =>
        //     {
        //         if (a != pageNumber || b != pageNumber)
        //         {
        //             return 0;
        //         }
        //
        //         if (a == pageNumber && b == beforePageNumber)
        //         {
        //             return -1;
        //         }
        //         if (a == beforePageNumber && b == pageNumber)
        //         {
        //             return 1;
        //         }
        //
        //         return 0;
        //     });
        // }
        
        var requirementsPredicates = new List<Func<int, int, bool>>();
        foreach (var (pageNumber, beforePageNumber) in requirements)
        {
            requirementsPredicates.Add((a, b) => !(b == pageNumber && a == beforePageNumber));
        }
        
        

        var sum = 0;
        // foreach (var manual in manualPages)
        // {
        //     var manualCopy = new int[manual.Count];
        //     manual.CopyTo(manualCopy);
        //     manual.Sort((x, y) =>
        //     {
        //         return Math.Max(0, Math.Min(1,
        //             requirementsPredicates.Sum(pred => pred(x, y))
        //         ));
        //     });
        //     if (ListAreTheSame(manual, manualCopy))
        //     {
        //         sum += GetMiddle(manual);
        //     }
        // }

        foreach (var manual in manualPages)
        {
            var invalidManual = false;
            for (int i = 0; i < manual.Count; i++)
            {
                if (invalidManual)
                {
                    break;
                }
                
                var iNumber = manual[i];
                
                for (int j = i + 1; j < manual.Count; j++)
                {
                    var jNumber = manual[j];

                    if (requirementsPredicates.Any(pred => !pred(iNumber, jNumber)))
                    {
                        invalidManual = true;
                        break;
                    }
                }
            }

            if (!invalidManual)
            {
                sum += GetMiddle(manual);
            }
        }

        return sum;
    }

    private static bool ListAreTheSame(List<int> listA, int[]listB)
    {
        return listA.Zip(listB).All(numbers => numbers.First == numbers.Second);
    }

    private static int GetMiddle(List<int> list)
    {
        return list[list.Count / 2];
    }

    private static int SolveP2()
    {
        var (requirements, manualPages) = GetPreppedInputs();
        
        var requirementsPredicates = new List<Func<int, int, bool>>();
        foreach (var (pageNumber, beforePageNumber) in requirements)
        {
            requirementsPredicates.Add((a, b) => !(b == pageNumber && a == beforePageNumber));
        }

        var incorrectlyOrderedManuals = new List<List<int>>();
        
        foreach (var manual in manualPages)
        {
            var invalidManual = false;
            for (int i = 0; i < manual.Count; i++)
            {
                if (invalidManual)
                {
                    break;
                }
                
                var iNumber = manual[i];
                
                for (int j = i + 1; j < manual.Count; j++)
                {
                    var jNumber = manual[j];

                    if (requirementsPredicates.Any(pred => !pred(iNumber, jNumber)))
                    {
                        incorrectlyOrderedManuals.Add(manual);
                        invalidManual = true;
                        break;
                    }
                }
            }
        }

        var orderedManuals = new List<List<int>>();

        foreach (var manual in incorrectlyOrderedManuals)
        {
            var orderedList = new LinkedList<int>();
            
            foreach (var page in manual)
            {
                var orderedListCopy = new LinkedList<int>(orderedList);
                var currentItem = orderedListCopy.First;
                while (currentItem is not null)
                {
                    orderedListCopy.AddBefore(currentItem, new LinkedListNode<int>(page));

                    if (IsValid(requirementsPredicates, orderedListCopy.ToList()))
                    {
                        break;
                    }
                    
                    orderedListCopy = new LinkedList<int>(orderedList);
                    currentItem = orderedListCopy.Find(currentItem.Value)!.Next;
                }

                if (currentItem is null)
                {
                    orderedListCopy.AddLast(page);
                    if (!IsValid(requirementsPredicates, orderedListCopy.ToList()))
                    {
                        Console.WriteLine("that is not good");
                    }
                }
                
                orderedList = orderedListCopy;
            }

            orderedManuals.Add(orderedList.ToList());
        }

        return orderedManuals.Sum(GetMiddle);
    }

    private static bool IsValid(List<Func<int, int, bool>> requirementsPredicates, List<int> manual)
    {
        for (var i = 0; i < manual.Count; i++)
        {
            var iNumber = manual[i];
                
            for (int j = i + 1; j < manual.Count; j++)
            {
                var jNumber = manual[j];

                if (requirementsPredicates.Any(pred => !pred(iNumber, jNumber)))
                {
                    return false;
                }
            }
        }

        return true;
    }
}