using AoC.Shared.PaperRockScissors;

Console.WriteLine("AOC - Day 2\n\n");

static int scoreRound(PlayerResult result)
{
    return result.Shape switch
    {
        Shape.Rock => 1,
        Shape.Paper => 2,
        Shape.Scissors => 3
    } +
    result.Result switch
    {
        Outcome.Win => 6,
        Outcome.Draw => 3,
        Outcome.Lose => 0
    };
};

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var part1 = File.ReadLines(inputFile)
        .Select(l =>
        {
            var parts = l.Split(' ');
            var opponent = parts[0] switch
            {
                "A" => Shape.Rock,
                "B" => Shape.Paper,
                "C" => Shape.Scissors,
                _ => throw new ArgumentException()
            };
            var self = parts[1] switch
            {
                "X" => Shape.Rock,
                "Y" => Shape.Paper,
                "Z" => Shape.Scissors,
                _ => throw new ArgumentException()
            };

            return (opponent, self);
        })
        .Select(strat => PaperRockScissors.Play(strat.opponent, strat.self).Player2Result)
        .Select(scoreRound)
        .Sum();
    
    Console.WriteLine($"Part 1: {part1}");

    var part2 = File.ReadLines(inputFile)
        .Select(l =>
        {
            var parts = l.Split(' ');
            var opponent = parts[0] switch
            {
                "A" => Shape.Rock,
                "B" => Shape.Paper,
                "C" => Shape.Scissors,
                _ => throw new ArgumentException()
            };
            var result = parts[1] switch
            {
                "X" => Outcome.Lose,
                "Y" => Outcome.Draw,
                "Z" => Outcome.Win,
                _ => throw new ArgumentException()
            };

            return (opponent, result);
        })
        .Select(strat => PaperRockScissors.PlayWithOutcome(strat.opponent, strat.result, false).Player2Result)
        .Select(scoreRound)
        .Sum();

    Console.WriteLine($"Part 2: {part2}\n");
}