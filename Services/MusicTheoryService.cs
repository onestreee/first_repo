namespace MusicIdeaBot.Services;

public static class MusicTheoryService
{
    private static readonly string[] Notes = 
    {
        "C", "C#", "D", "D#", "E", "F",
        "F#", "G", "G#", "A", "A#", "B"
    };

    private static readonly Dictionary<string, int> MinorDegrees = new()
    {
        ["i"] = 0,
        ["ii"] = 2,
        ["iii"] = 3,
        ["iv"] = 5,
        ["v"] = 7,
        ["vi"] = 8,
        ["vii"] = 10 
    };

    public static string TransposeMinorProgression(string key, string progression)
    {
        // key = "A minor" ==> берем A
        var root = key.Split(' ')[0];
        var rootIndex = Array.IndexOf(Notes, root);

        if (rootIndex == -1) return progression;

        var parts = progression.Split('-', StringSplitOptions.TrimEntries);

        var chords = new List<string>();

        foreach (var part in parts)
        {
            var degree = part.ToLower();

            if (!MinorDegrees.ContainsKey(degree))
            {
                chords.Add(part);
                continue;
            }

            var offset = MinorDegrees[degree];
            var noteIndex = (rootIndex + offset) % 12;
            var note = Notes[noteIndex];

            var chord = degree == "i" || degree == "iv" || degree == "v"
                ? note + "m"
                : note;
            chords.Add(chord);
        }

        return string.Join(" - ", chords);
    }
}