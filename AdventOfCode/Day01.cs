using System.Collections.Frozen;

namespace AdventOfCode;

public sealed class Day01 : BaseDay
{
    private readonly FrozenSet<string> _lines;

    public Day01()
    {
        _lines = File.ReadAllLines(InputFilePath).ToFrozenSet();
    }

    public override ValueTask<string> Solve_1()
    {
        var sum = (from line in _lines.Where(l => !string.IsNullOrWhiteSpace(l))
            let firstDigit = line.FirstOrDefault(char.IsDigit)
            let lastDigit = line.LastOrDefault(char.IsDigit)
            select int.Parse(firstDigit.ToString() + lastDigit)).Sum();
        return new ValueTask<string>(sum.ToString());
    }

    private static readonly Dictionary<string, int> NumberMap = new()
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 },
    };

    public override ValueTask<string> Solve_2()
    {
        var sum = 0;

        foreach (var line in _lines)
        {
            var firstDigit = line.FirstOrDefault(char.IsDigit);
            var firstDigitIndex = line.IndexOf(firstDigit);
            if (firstDigitIndex == -1) firstDigitIndex = int.MaxValue;
            var lastDigit = line.LastOrDefault(char.IsDigit);
            var lastDigitIndex = line.LastIndexOf(lastDigit);

            var firstWord = GetIndexOf(line, true);
            var lastWord = GetIndexOf(line, false);

            var d1 = firstWord.Item1 < firstDigitIndex ? firstWord.Item2.ToString() : firstDigit.ToString();
            var d2 = lastWord.Item1 > lastDigitIndex ? lastWord.Item2.ToString() : lastDigit.ToString();

            sum += int.Parse(d1 + d2);
        }


        return new ValueTask<string>(sum.ToString());
    }

    private (int, int) GetIndexOf(string str, bool isFirst)
    {
        var index = isFirst ? int.MaxValue : -1;
        var s = 0;
        foreach (var kv in NumberMap)
        {
            var i = isFirst ? str.IndexOf(kv.Key, StringComparison.Ordinal) : str.LastIndexOf(kv.Key, StringComparison.Ordinal);
            if (isFirst ? i >= index || i == -1 : i <= index) continue;
            index = i;
            s = kv.Value;
        }

        return (index, s);
    }
}