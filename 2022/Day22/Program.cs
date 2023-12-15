using AoC.Shared.Strings;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

// TODO: Clean this up
Console.WriteLine("AOC - Day 22\n");

var input = File.ReadLines("sample.txt")
    .ToList();

var board = new List<string>();
var moves = new string[] { };

for (var i = 0; i < input.Count; i++)
{
    if (string.IsNullOrEmpty(input[i]))
    {
        var pattern = @"(L|R)";
        moves = Regex.Split(input[i + 1], pattern);
        break;
    }

    board.Add(input[i]);
}

var row = 0;
var col = board[0].IndexOf('.');
var direction = 0;
var validSpaces = new[] { '.', '#' };

var sides = new List<Tuple<Vector2, Vector2>>
{
    Tuple.Create(new Vector2(0, 49), new Vector2(50, 99)),
    Tuple.Create(new Vector2(0, 49), new Vector2(100, 149)),
    Tuple.Create(new Vector2(50, 99), new Vector2(50, 99)),
    Tuple.Create(new Vector2(100, 149), new Vector2(0, 49)),
    Tuple.Create(new Vector2(100, 149), new Vector2(50, 99)),
    Tuple.Create(new Vector2(150, 199), new Vector2(0, 49))
};

var p = 0;

foreach (var move in moves)
{

    switch (move)
     {
        /*
         * 0 R
         * 1 D
         * 2 L
         * 3 U
        */

        case "L":
            direction = direction switch
            {
                0 => 3,
                1 => 0,
                2 => 1,
                3 => 2,
            };
            break;
        case "R":
            direction = direction switch
            {
                0 => 1,
                1 => 2,
                2 => 3,
                3 => 0,
            };
            break;
        default:
            var spaces = move.ToInt32();

            while (spaces-- > 0)
            {
                if (direction == 1) // Down
                {
                    var newRow = row + 1;
                    var newCol = col;
                    if (newRow >= board.Count || col >= board[newRow].Length || board[newRow][col] == ' ')
                    {
                        if (row == 49)
                        {
                            newRow = col - 50;
                            newCol = 99;
                            direction = 2; // L
                        }
                        if (row == 149)
                        {
                            newRow = col + 100;
                            newCol = 49;
                            direction = 2; // L
                        }
                        if (row == 199)
                        {

                            newRow = 0;
                            newCol = col + 100;
                        }

                        //newRow = 0;
                        //while (col >= board[newRow].Length || !validSpaces.Contains(board[newRow][col]))
                        //{
                        //    newRow++;
                        //}
                    }              

                    if (board[newRow][newCol] == '.')
                    {
                        row = newRow;
                        col = newCol;
                        continue;
                    }

                    if (board[newRow][newCol] == '#')
                    {
                        direction = 1;
                        break;
                    }
                }
                else if (direction == 2) // L
                {
                    var newRow = row;
                    var newCol = col - 1;
                    if (newCol < 0 || board[row][newCol] == ' ')
                    {
                        if (col == 50)
                        {
                            if (row < 50)
                            {
                                // 0 = 149
                                newRow = 149 - row;
                                newCol = 0;
                                direction = 0; // R
                            }
                            else
                            {
                                // 50 = 0
                                // 99 = 49
                                newRow = 100;
                                newCol = row - 50;
                                direction = 1; // D
                            }
                        }

                        if (col == 0)
                        {
                            if (row < 150)
                            {
                                // 100 = 49
                                // 149 = 0
                                newRow = 149 - row;
                                newCol = 50;
                                direction = 0; // R
                            }
                            else
                            {
                                // 150 = 50
                                newRow = 0;
                                newCol = row - 100;
                                direction = 1; // D
                            }
                        }


                        //newCol = board[row].LastIndexOfAny(new[] { '.', '#' });
                    }


                    if (board[newRow][newCol] == '.')
                    {
                        row = newRow;
                        col = newCol;
                        continue;
                    }

                    if (board[newRow][newCol] == '#')
                    {
                        direction = 2;
                        break;
                    }
                }
                else if (direction == 3) // U
                {
                    var newRow = row - 1;
                    var newCol = col;
                    if (newRow < 0 || col >= board[newRow].Length || board[newRow][col] == ' ')
                    {
                        if (row == 0)
                        {
                            if (col < 100)
                            {
                                newRow = col + 100;
                                newCol = 0;
                                direction = 0; // R
                            }
                            else
                            {
                                newRow = 199;
                                newCol = col - 100;
                            }
                        }

                        if (row == 100)
                        {
                            newRow = col + 50;
                            newCol = 50;
                            direction = 0; // R
                        }

                        //newRow = board.Count - 1;
                        //while (col >= board[newRow].Length || !validSpaces.Contains(board[newRow][col]))
                        //{
                        //    newRow--;
                        //}
                    }

                    if (board[newRow][newCol] == '.')
                    {
                        row = newRow;
                        col = newCol;
                        continue;
                    }

                    if (board[newRow][newCol] == '#')
                    {
                        direction = 3;
                        break;
                    }
                }
                else if (direction == 0) // R
                {
                    //var newCol = col + 1 < board[row].Length ? col + 1 : board[row].IndexOfAny(new[] { '.', '#' });
                    var newRow = row;
                    var newCol = col + 1;
                    if (newCol == board[row].Length || board[row][newCol] == ' ')
                    {
                        if (col == 149)
                        {
                            // 0 = 149
                            newRow = 149 - row;
                            newCol = 99;
                            direction = 2; // L
                        }

                        if (col == 99)
                        {
                            if (row < 100)
                            {
                                // 50 = 100
                                // 99 = 149
                                newRow = 49;
                                newCol = row + 50;
                                direction = 3; // U
                            }
                            else
                            {
                                newRow = 149 - row;
                                newCol = 149;
                                direction = 2; // L
                            }
                        }

                        if (col == 49)
                        {
                            newRow = 149;
                            newCol = row - 100;
                            direction = 3; // U
                        }


                        //newCol = board[row].IndexOfAny(new[] { '.', '#' });
                    }

                    if (board[newRow][newCol] == '.')
                    {
                        row = newRow;
                        col = newCol;
                        continue;
                    }

                    if (board[newRow][newCol] == '#')
                    {
                        direction = 0;
                        break;
                    }
                }
            }

            var dir = direction switch
            {
                0 => 'R',
                1 => 'D',
                2 => 'L',
                3 => 'U'
            };

            //Console.WriteLine($"{row} {col} {dir} {move}");

            //if (p++ % 20 == 0)
            //{
            //    Console.ReadKey();
            //}
            break;
    }

    //Console.Clear();
    //Console.WriteLine($"{row} {col} {direction} {move}");

    //for (var i = 0; i < board.Count; i++)
    //{
    //    if (row == i)
    //    {
    //        var line = board[i].ToCharArray();
    //        line[col] = direction switch
    //        {
    //            0 => '>',
    //            1 => 'V',
    //            2 => '<',
    //            3 => '^'
    //        };
    //        Console.WriteLine(line);
    //    }
    //    else
    //    {
    //        Console.WriteLine(board[i]);
    //    }
    //}
}

Console.WriteLine($"{(1000 * ++row) + (4 * ++col) + direction}\n");