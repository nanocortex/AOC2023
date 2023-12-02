using System.Collections.Frozen;

namespace AdventOfCode;

public sealed class Day02 : BaseDay
{
    private readonly FrozenSet<string> _lines;

    public Day02()
    {
        _lines = File.ReadAllLines(InputFilePath).ToFrozenSet();
    }

    public override ValueTask<string> Solve_1()
    {
        const int maxRed = 12;
        const int maxGreen = 13;
        const int maxBlue = 14;
        var sum = _lines.Select(Game.Parse).Where(game => game.MaxRed <= maxRed && game.MaxGreen <= maxGreen && game.MaxBlue <= maxBlue).Sum(game => game.Id);
        return new ValueTask<string>(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var sum = _lines.Select(Game.Parse).Sum(game => game.MaxRed * game.MaxGreen * game.MaxBlue);
        return new ValueTask<string>(sum.ToString());
    }
    
    public record Game(int Id, List<GameStep> Steps)
    {
        public static Game Parse(string line)
        {
            var parts = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
            var parts2 = parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var id = int.Parse(parts2[1].Trim());
            var steps = parts[1].Trim().Split(";", StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Select(GameStep.Parse).ToList();
            return new Game(id, steps);
        }

        public int MaxRed => Steps.Max(s => s.Red);

        public int MaxGreen => Steps.Max(s => s.Green);

        public int MaxBlue => Steps.Max(s => s.Blue);
    }

    public record GameStep(int Red, int Green, int Blue)
    {
        public static GameStep Parse(string line)
        {
            var parts = line.Split(",", StringSplitOptions.RemoveEmptyEntries);

            var red = 0;
            var green = 0;
            var blue = 0;

            foreach (var p in parts)
            {
                var count = int.Parse(p.Trim().Split(" ")[0]);
                var color = p.Trim().Split(" ")[1];

                switch (color)
                {
                    case "red":
                        red = count;
                        break;
                    case "green":
                        green = count;
                        break;
                    case "blue":
                        blue = count;
                        break;
                    default:
                        throw new Exception("Unknown color: " + color);
                }
            }


            return new GameStep(red, green, blue);
        }
    }
}