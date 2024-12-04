using System.Runtime.Intrinsics.X86;

namespace AdventOfCode24;

public class Day4
{
    private const int DayNumber = 4;

    private static char[][] _inputMatrix;
    private static int _width;
    private static int _height;
    
    public static void Solve()
    {
        Console.WriteLine($"For day {DayNumber} we have");
        Console.WriteLine($"P1: {SolveP1()}");
        Console.WriteLine($"P2: {SolveP2()}");
    }
    
    private static void SetUpInputMatrix()
    {
        _inputMatrix = InputHelper.ReadLines(DayNumber, EInputType.Real)
            .Select(inputLine => inputLine.ToArray())
            .ToArray();
        _height = _inputMatrix.Length;
        _width = _inputMatrix[0].Length;
    }
    
    private static int SolveP1()
    {
        SetUpInputMatrix();

        var sum = 0;
        for (var y = 0; y < _inputMatrix.Length; y++)
        {
            for (var x = 0; x < _inputMatrix[0].Length; x++)
            {
                sum += GetNumberOfStartingPointsOfXmas(new Point(x, y));
            }
        }
        return sum;
    }
    
    private static int GetNumberOfStartingPointsOfXmas(Point point)
    {
        var amount = 0;
        
        if (point.X >= 3)
        {
            // Check left
            var x = _inputMatrix[point.Y][point.X];
            var m = _inputMatrix[point.Y][point.X - 1];
            var a = _inputMatrix[point.Y][point.X - 2];
            var s = _inputMatrix[point.Y][point.X - 3];

            if (IsXmas(x, m, a, s))
            {
                amount++;
            }
        }

        if (point.X + 3 < _width)
        {
            // Check right
            var x = _inputMatrix[point.Y][point.X];
            var m = _inputMatrix[point.Y][point.X + 1];
            var a = _inputMatrix[point.Y][point.X + 2];
            var s = _inputMatrix[point.Y][point.X + 3];

            if (IsXmas(x, m, a, s))
            {
                amount++;
            }
        }
        
        if (point.Y >= 3)
        {
            // Check above
            var x = _inputMatrix[point.Y][point.X];
            var m = _inputMatrix[point.Y - 1][point.X];
            var a = _inputMatrix[point.Y - 2][point.X];
            var s = _inputMatrix[point.Y - 3][point.X];

            if (IsXmas(x, m, a, s))
            {
                amount++;
            }
        }

        if (point.Y + 3 < _height)
        {
            // Check below
            var x = _inputMatrix[point.Y][point.X];
            var m = _inputMatrix[point.Y + 1][point.X];
            var a = _inputMatrix[point.Y + 2][point.X];
            var s = _inputMatrix[point.Y + 3][point.X];

            if (IsXmas(x, m, a, s))
            {
                amount++;
            }
        }
        
        if (point.X >= 3 && point.Y >= 3)
        {
            // Check left above
            var x = _inputMatrix[point.Y][point.X];
            var m = _inputMatrix[point.Y - 1][point.X - 1];
            var a = _inputMatrix[point.Y - 2][point.X - 2];
            var s = _inputMatrix[point.Y - 3][point.X - 3];

            if (IsXmas(x, m, a, s))
            {
                amount++;
            }
        }

        if (point.X >= 3 && point.Y + 3 < _height)
        {
            // Check left below
            var x = _inputMatrix[point.Y][point.X];
            var m = _inputMatrix[point.Y + 1][point.X - 1];
            var a = _inputMatrix[point.Y + 2][point.X - 2];
            var s = _inputMatrix[point.Y + 3][point.X - 3];

            if (IsXmas(x, m, a, s))
            {
                amount++;
            }
        }

        if (point.X + 3 < _width && point.Y >= 3)
        {
            // Check right above
            var x = _inputMatrix[point.Y][point.X];
            var m = _inputMatrix[point.Y - 1][point.X + 1];
            var a = _inputMatrix[point.Y - 2][point.X + 2];
            var s = _inputMatrix[point.Y - 3][point.X + 3];

            if (IsXmas(x, m, a, s))
            {
                amount++;
            }
        }

        if (point.X + 3 < _width && point.Y + 3 < _height)
        {
            // Check right below
            var x = _inputMatrix[point.Y][point.X];
            var m = _inputMatrix[point.Y + 1][point.X + 1];
            var a = _inputMatrix[point.Y + 2][point.X + 2];
            var s = _inputMatrix[point.Y + 3][point.X + 3];

            if (IsXmas(x, m, a, s))
            {
                amount++;
            }
        }

        return amount;
    }

    private static bool IsXmas(char x, char m, char a, char s)
    {
        return x == 'X' && m == 'M' && a == 'A' && s == 'S';
    }

    private static int SolveP2()
    {
        SetUpInputMatrix();

        var sum = 0;
        for (var y = 0; y < _inputMatrix.Length; y++)
        {
            for (var x = 0; x < _inputMatrix[0].Length; x++)
            {
                var isOutOfBounds = x < 1 || x + 1 >= _height || y < 1 || y + 1 >= _width;
                if (isOutOfBounds)
                {
                    continue;
                }
                
                if (_inputMatrix[y][x] == 'A' && IsMiddleOfXMAS(new Point(x, y)))
                {
                    sum++;
                }
            }
        }
        return sum;
    }

    private static bool IsMiddleOfXMAS(Point point)
    {
        var topLeft = _inputMatrix[point.Y - 1][point.X - 1];
        var topRight = _inputMatrix[point.Y - 1][point.X + 1];
        var bottomLeft = _inputMatrix[point.Y + 1][point.X - 1];
        var bottomRight = _inputMatrix[point.Y + 1][point.X + 1];

        var leftTopToRightBottomIsMas = (topLeft == 'M' && bottomRight == 'S') || (topLeft == 'S' && bottomRight == 'M');
        var rightTopToLeftBottomIsMas =(topRight == 'M' && bottomLeft == 'S') || (topRight == 'S' && bottomLeft == 'M');

        return leftTopToRightBottomIsMas && rightTopToLeftBottomIsMas;
    }

    private record Point(int X, int Y);
}