using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public sealed class Day03 : BaseDay
{
    private readonly ImmutableArray<string> _lines;

    public Day03()
    {
        _lines = File.ReadAllLines(InputFilePath).ToImmutableArray();
    }

    public override ValueTask<string> Solve_1()
    {
        var sum = 0;
        for (var i = 0; i < _lines.Length; i++)
        {
            var previousLine = i == 0 ? null : _lines[i - 1];
            var currentLine = _lines[i];
            var nextLine = i == _lines.Length - 1 ? null : _lines[i + 1];
            var numberMatches = Regex.Matches(currentLine, @"\d+");

            foreach (Match match in numberMatches)
            {
                var index = match.Index;

                var adjacentToSymbol = false;
                for (var j = 0; j < match.Length; j++)
                {
                    if (!IsAdjacentToSymbol(currentLine, previousLine, nextLine, index + j)) continue;
                    adjacentToSymbol = true;
                    break;
                }

                if (adjacentToSymbol)
                {
                    sum += int.Parse(match.ToString());
                }
            }
        }


        return new ValueTask<string>(sum.ToString());
    }

    private bool IsAdjacentToSymbol(string line, string previousLine, string nextLine, int index)
    {
        var indexProximityMap = new List<List<string>>();
        if (previousLine != null)
        {
            indexProximityMap.Add(previousLine.Where((x, i) => i >= index - 1 && i <= index + 1).Select(c => c.ToString()).ToList());
        }

        indexProximityMap.Add(line.Where((x, i) => i >= index - 1 && i <= index + 1).Select(c => c.ToString()).ToList());

        if (nextLine != null)
        {
            indexProximityMap.Add(nextLine.Where((x, i) => i >= index - 1 && i <= index + 1).Select(c => c.ToString()).ToList());
        }

        return indexProximityMap.SelectMany(x => x).Any(x => x.Any(y => !char.IsDigit(y) && y != '.'));
    }


    public override ValueTask<string> Solve_2()
    {
        var sum = 0;


        for (var i = 0; i < _lines.Length; i++)
        {
            var previousLine = i == 0 ? null : _lines[i - 1];
            var currentLine = _lines[i];
            var nextLine = i == _lines.Length - 1 ? null : _lines[i + 1];
            var starMatches = Regex.Matches(currentLine, @"\*");

            foreach (Match startMatch in starMatches)
            {
                var starIndex = startMatch.Index;
                var numbers = GetAdjacentNumbers(previousLine, currentLine, nextLine, starIndex);

                if (numbers.Count != 2)
                    continue;

                sum += numbers.Aggregate((x, y) => x * y);
            }
        }

        return new ValueTask<string>(sum.ToString());
    }


    private List<int> GetAdjacentNumbers(string previousLine, string currentLine, string nextLine, int starIndex)
    {
        var linesToCheck = new[] { previousLine, currentLine, nextLine }
            .Where(line => line != null)
            .Select(GetNumbersFromLine);

        return linesToCheck.SelectMany(numbers => numbers
            .Where(pair => starIndex >= pair.Key.Item1 - 1 && starIndex <= pair.Key.Item2 + 1)
            .Select(pair => pair.Value)).ToList();
    }

    private Dictionary<(int, int), int> GetNumbersFromLine(string line)
    {
        return Regex.Matches(line, @"\d+")
            .ToDictionary(match => (match.Index, match.Index + match.Length - 1), match => int.Parse(match.ToString()));
    }
}