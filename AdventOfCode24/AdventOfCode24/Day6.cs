namespace AdventOfCode24;

public class Day6
{
    private const int DayNumber = 6;
    
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

    private static ESpot[][] GetMap()
    {
        return GetInputs().Select(line => line.Select(spotRaw => spotRaw switch
        {
            '.' => ESpot.EMPTY,
            '#' => ESpot.OBSTACLE,
            '^' => ESpot.GUARD_Y_NEG,
            'v' => ESpot.GUARD_Y_POS,
            '>' => ESpot.GUARD_X_POS,
            '<' => ESpot.GUARD_X_NEG
        }).ToArray()).ToArray();
    }
    
    private static int SolveP1()
    {
        var map = GetMap();
        var mapHeight = map.Length;
        var mapWidth = map[0].Length;

        var visitedLocations = new HashSet<(int, int)>();
        var guardDetails = GetGuard(map);
        while (IsWithinBounds(guardDetails.x, guardDetails.y, mapWidth, mapHeight))
        {
            visitedLocations.Add((guardDetails.x, guardDetails.y));
            
            (int x, int y) nextPos = guardDetails.direction switch
            {
                ESpot.GUARD_Y_POS => (guardDetails.x, guardDetails.y + 1),
                ESpot.GUARD_Y_NEG => (guardDetails.x, guardDetails.y - 1),
                ESpot.GUARD_X_POS => (guardDetails.x + 1, guardDetails.y),
                ESpot.GUARD_X_NEG => (guardDetails.x - 1, guardDetails.y)
            };

            if (IsWithinBounds(nextPos.x, nextPos.y, mapWidth, mapHeight) && map[nextPos.y][nextPos.x] == ESpot.OBSTACLE)
            {
                guardDetails = (guardDetails.x, guardDetails.y, Rotate90DegreesClockwise(guardDetails.direction));
            }
            else
            {
                guardDetails = (nextPos.x, nextPos.y, direction: guardDetails.direction);
            }
        }

        return visitedLocations.Count;
    }

    private static (int x, int y, ESpot direction) GetGuard(ESpot[][] map)
    {
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                var spot = map[y][x];
                var isGuard = ((int)spot) > ((int)ESpot.OBSTACLE);
                if (isGuard)
                {
                    return (x, y, spot);
                }
            }
        }

        return (-1, -1, ESpot.EMPTY);
    }

    private static bool IsWithinBounds(int x, int y, int mapWidth, int mapHeight)
    {
        return x >= 0 && x < mapWidth && y >= 0 && y < mapHeight;
    }

    private static ESpot Rotate90DegreesClockwise(ESpot guardDirection)
    {
        return guardDirection switch
        {
            ESpot.GUARD_Y_POS => ESpot.GUARD_X_NEG,
            ESpot.GUARD_X_NEG => ESpot.GUARD_Y_NEG,
            ESpot.GUARD_Y_NEG => ESpot.GUARD_X_POS,
            ESpot.GUARD_X_POS => ESpot.GUARD_Y_POS
        };
    }

    private static int SolveP2()
    {
        var map = GetMap();
        var mapHeight = map.Length;
        var mapWidth = map[0].Length;

        var numberOfValidObstaclePositions = 0;
        
        var guardDetails = GetGuard(map);
        (int x, int y, ESpot direction) startingPosition = (guardDetails.x, guardDetails.y, guardDetails.direction);
        for (var y = 0; y < mapHeight; y++)
        {
            for (var x = 0; x < mapWidth; x++)
            {
                if (startingPosition.x == x && startingPosition.y == y)
                {
                    continue;
                }

                if ((int) map[y][x] > (int) ESpot.EMPTY)
                {
                    continue;
                }

                Console.WriteLine($"Trying for ({x},{y})");
                
                var modifiedMap = GetMapCopy(map);
                modifiedMap[y][x] = ESpot.OBSTACLE;
                
                guardDetails = (startingPosition.x, startingPosition.y, startingPosition.direction);
                var visitedSpots = new HashSet<(int, int, ESpot)>();
                while (IsWithinBounds(guardDetails.x, guardDetails.y, mapWidth, mapHeight))
                {

                    (int x, int y) nextPos = guardDetails.direction switch
                    {
                        ESpot.GUARD_Y_POS => (guardDetails.x, guardDetails.y + 1),
                        ESpot.GUARD_Y_NEG => (guardDetails.x, guardDetails.y - 1),
                        ESpot.GUARD_X_POS => (guardDetails.x + 1, guardDetails.y),
                        ESpot.GUARD_X_NEG => (guardDetails.x - 1, guardDetails.y)
                    };

                    if (IsWithinBounds(nextPos.x, nextPos.y, mapWidth, mapHeight) && modifiedMap[nextPos.y][nextPos.x] == ESpot.OBSTACLE)
                    {
                        // if (rotatesBetweenSteps > 4)
                        // {
                        //     break;
                        // }
                        
                        guardDetails = (guardDetails.x, guardDetails.y, Rotate90DegreesClockwise(guardDetails.direction));
                    }
                    else
                    {
                        guardDetails = (nextPos.x, nextPos.y, guardDetails.direction);
                    }

                    if (!visitedSpots.Add(guardDetails))
                    {
                        numberOfValidObstaclePositions++;
                        break;
                    }
                }

            }
        }

        return numberOfValidObstaclePositions;
    }

    private static ESpot[][] GetMapCopy(ESpot[][] map)
    {
        var copiedMap = new ESpot[map.Length][];
        for (var y = 0; y < map.Length; y++)
        {
            copiedMap[y] = new ESpot[map[y].Length];
            for (var x = 0; x < copiedMap.Length; x++)
            {
                copiedMap[y][x] = map[y][x];
            }
        }

        return copiedMap;
    }

    private enum ESpot
    {
        EMPTY = 0, OBSTACLE = 1, GUARD_Y_POS = 2, GUARD_Y_NEG = 3, GUARD_X_POS = 4, GUARD_X_NEG = 5
    }
}