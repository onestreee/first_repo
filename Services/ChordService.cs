using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using MusicIdeaBot.Models;

namespace MusicIdeaBot.Services;

public class ChordService
{
    private static List<ChordProgression> _data;

    static ChordService()
    {
        var json = File.ReadAllText("Data/Progressions.json");
        _data = JsonSerializer.Deserialize<List<ChordProgression>>(json)!;
    }

    public static string GetProgression(string mood)
    {
        var entry = _data.FirstOrDefault(x => 
        x.Mood.Equals(mood, StringComparison.OrdinalIgnoreCase));

        if (entry == null) 
            return "Нет такого настроения. Попробуй ещё раз: sad, dark, pop.";

        var rng = new Random();
        return entry.Progressions[rng.Next(entry.Progressions.Length)];
    }
}