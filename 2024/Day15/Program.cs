// Advent of Code challenge: https://adventofcode.com/2024/day/15

using System.Drawing;
using AoC.Shared.Collections;
using AoC.Shared.Enumerable;
using AoC.Shared.Grid;
using AoC.Shared.PixelWriter;
using AoC.Shared.Points;

Console.Clear();
Console.WriteLine("AoC - Day 15\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }
    
    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .ToList();

    var moves = input[1].Values
        .SelectMany(ln => ln)
        .Select(chr =>
        {
            return chr switch
            {
                'v' => GridDirections.South,
                '^' => GridDirections.North,
                '>' => GridDirections.East,
                '<' => GridDirections.West,
                _ => throw new ArgumentOutOfRangeException(nameof(chr), chr, "Unexpected move")
            };
        })
        .ToArray();
    
    var (walls, boxes, robot, rows, _) = ParseInput(input[0].Values);
    
    foreach (var move in moves)
    {
        robot = MoveRobot(walls, boxes, robot, move);
    }
    
    var part1 = boxes
        .Select(p => 100 * (rows - 1 - p.Y) + p.X)
        .Sum();
    
    Console.WriteLine($"Part 1: {part1}");

    (walls, boxes, robot, rows, _) = ParseInput(input[0].Values);
    (walls, boxes, robot) = ExpandWarehouse(walls, boxes, robot);
    
    foreach (var move in moves)
    {
        robot = MoveRobotExpanded(walls, boxes, robot, move);
    }
    
    var part2 = boxes
        .Select(p => 100 * (rows - 1 - p.Y) + p.X)
        .Sum();
    
    Console.WriteLine($"Part 2: {part2}\n");
}

PlayGame();

return;

static Point MoveRobotExpanded(HashSet<Point> walls, HashSet<Point> boxes, Point robot, Point move)
{
    var pos = robot.Add(move);
    
    if (walls.Contains(pos))
    {
        return robot;
    }
    
    var boxStack = new Stack<Point>();
    
    if (boxes.Contains(pos) || boxes.Contains(pos.Add(GridDirections.West)))
    {
        var left = boxes.Contains(pos) ? pos : pos.Add(GridDirections.West);
        var queue = new Queue<Point> { left };

        while (queue.TryDequeue(out var leftPos))
        {
            boxStack.Push(leftPos);
            var next = leftPos.Add(move);
            
            if ((GridDirections.Horizontal.Contains(move) && walls.Contains(move == GridDirections.West ? next : next.Add(move))) ||
                GridDirections.Vertical.Contains(move) && walls.Contains(next) || walls.Contains(next.Add(GridDirections.East)))
            {
                return robot;
            }
            
            var boxCheck = new List<Point>();
            
            if (GridDirections.Vertical.Contains(move))
            {
                boxCheck.AddRange([next, next.Add(GridDirections.West), next.Add(GridDirections.East)]);
            }
            else
            {
                boxCheck.Add(next.Add(move));
            }
            
            foreach (var box in boxCheck.Where(check => boxes.Contains(check) && !queue.Contains(check)))
            {
                queue.Enqueue(box);
            }
        }
    }
        
    while (boxStack.TryPop(out var box))
    {
        boxes.Add(box.Add(move));
        boxes.Remove(box);
    }

    return robot.Add(move);
}

static Point MoveRobot(HashSet<Point> walls, HashSet<Point> boxes, Point robot, Point move)
{
    var pos = robot.Add(move);
    var boxStack = new Stack<Point>();
    
    while (true)
    {
        if (walls.Contains(pos))
        {
            return robot;
        }
            
        if (boxes.Contains(pos))
        {
            boxStack.Push(pos);
        }
        else
        {
            break;
        }

        pos = pos.Add(move);
    }
        
    while (boxStack.TryPop(out var box))
    {
        boxes.Add(box.Add(move));
        boxes.Remove(box);
    }

    return robot.Add(move);
}

static (HashSet<Point>, HashSet<Point>, Point) ExpandWarehouse(HashSet<Point> walls, HashSet<Point> boxes, Point robot)
{
    var newWalls = walls
        .SelectMany(oldWall =>
        {
            var newWall = oldWall with { X = oldWall.X * 2 };
            return new[] { newWall, newWall.Add(GridDirections.East) };
        })
        .ToHashSet();
    
    var newBoxes = boxes
        .Select(oldBox => oldBox with { X = oldBox.X * 2 })
        .ToHashSet();
    
    return (newWalls, newBoxes, robot with { X = robot.X * 2 });
}

static void PlayGame()
{
    var (walls, boxes, robot, rows, cols) = ParseInput(File.ReadAllLines("game.txt"));
    var expanded = false;
    var cursor = Console.GetCursorPosition();
    var pw = new PixelWriter(cols);

    while (true)
    {
        Console.SetCursorPosition(cursor.Left, cursor.Top);
        Console.WriteLine("Arrow keys: Move | e: Expand Warehouse | q: Quit");
        for (var y = rows - 1; y >= 0; y--)
        {
            for (var x = 0; x < cols; x++)
            {
                var pos = new Point(x, y);

                if (walls.Contains(pos))
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    pw.Write('#');
                }
                else if (boxes.Contains(pos))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    if (expanded)
                    {
                        pw.Write('[');
                        pw.Write(']');
                        x++;
                    }
                    else
                    {
                        pw.Write('O');
                    }
                }
                else if (pos == robot)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    pw.Write('@');
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    pw.Write('.');
                }
            }
        }

        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.DownArrow:
                robot = expanded 
                    ? MoveRobotExpanded(walls, boxes, robot, GridDirections.South) 
                    : MoveRobot(walls, boxes, robot, GridDirections.South);
                break;
            case ConsoleKey.UpArrow:
                robot = expanded 
                    ? MoveRobotExpanded(walls, boxes, robot, GridDirections.North) 
                    : MoveRobot(walls, boxes, robot, GridDirections.North);
                break;
            case ConsoleKey.RightArrow:
                robot = expanded 
                    ? MoveRobotExpanded(walls, boxes, robot, GridDirections.East) 
                    : MoveRobot(walls, boxes, robot, GridDirections.East);
                break;
            case ConsoleKey.LeftArrow:
                robot = expanded 
                    ? MoveRobotExpanded(walls, boxes, robot, GridDirections.West) 
                    : MoveRobot(walls, boxes, robot, GridDirections.West);
                break;
            case ConsoleKey.Q:
                return;
            case ConsoleKey.E when !expanded:
                (walls, boxes, robot) = ExpandWarehouse(walls, boxes, robot);
                cols *= 2;
                pw = new PixelWriter(cols);
                expanded = true;
                break;
        }
    }
}

static (HashSet<Point> walls, HashSet<Point> boxes, Point robot, int rows, int cols) ParseInput(IEnumerable<string> input)
{
    var grid = input.Reverse().ToArray();

    var cols = grid[0].Length;
    var rows = grid.Length;
    
    var walls = new HashSet<Point>();
    var boxes = new HashSet<Point>();
    var robot = new Point(0, 0);
    
    for (var x = 0; x < cols; x++)
    {
        for (var y = 0; y < rows; y++)
        {
            switch (grid[y][x])
            {
                case '#':
                    walls.Add(new Point(x, y));
                    break;
                case 'O':
                    boxes.Add(new Point(x, y));
                    break;
                case '@':
                    robot = new Point(x, y);
                    break;
            }
        }
    }

    return (walls, boxes, robot, rows, cols);
}
