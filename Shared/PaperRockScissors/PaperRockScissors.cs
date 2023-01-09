namespace AoC.Shared.PaperRockScissors
{
    public static class PaperRockScissors
    {
        private static readonly IDictionary<Tuple<Shape, Shape>, Outcome> Rules = new Dictionary<Tuple<Shape, Shape>, Outcome>
        {
            { Tuple.Create(Shape.Paper, Shape.Paper), Outcome.Draw },
            { Tuple.Create(Shape.Paper, Shape.Rock), Outcome.Win },
            { Tuple.Create(Shape.Paper, Shape.Scissors), Outcome.Lose },
            { Tuple.Create(Shape.Rock, Shape.Paper), Outcome.Lose },
            { Tuple.Create(Shape.Rock, Shape.Rock), Outcome.Draw },
            { Tuple.Create(Shape.Rock, Shape.Scissors), Outcome.Win },
            { Tuple.Create(Shape.Scissors, Shape.Paper), Outcome.Win },
            { Tuple.Create(Shape.Scissors, Shape.Rock), Outcome.Lose },
            { Tuple.Create(Shape.Scissors, Shape.Scissors), Outcome.Draw }
        };

        public static MatchResult Play(Shape player1Shape, Shape player2Shape)
        {
            var results = new MatchResult
            {
                Player1Result = new PlayerResult
                {
                    Shape = player1Shape,
                    Result = Rules[Tuple.Create(player1Shape, player2Shape)]
                },
                Player2Result = new PlayerResult
                {
                    Shape = player2Shape,
                    Result = Rules[Tuple.Create(player2Shape, player1Shape)]
                }
            };

            return results;
        }

        public static MatchResult PlayWithOutcome(Shape player1Shape, Outcome outcome, bool asPlayer1 = true)
        {
            foreach (var player2Shape in Enum.GetValues<Shape>())
            {
                var round = asPlayer1 ? 
                    Tuple.Create(player1Shape, player2Shape) :
                    Tuple.Create(player2Shape, player1Shape);

                if (Rules[round] == outcome)
                {
                    return Play(player1Shape, player2Shape);
                }
            }

            throw new InvalidOperationException("Unable to achieve desired outcome!");
        }
    }
}
