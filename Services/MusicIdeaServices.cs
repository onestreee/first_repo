namespace MusicIdeaBot.Services;

public class MusicIdeaService
{
    private static readonly string[] Keys =
    { 
        "C minor","D minor","E minor","F minor","G minor","A minor", "B minor",
        "C major","D major","E major","F major","G major","A major", "B major"
    };

    private static readonly string[] Styles =
    {
        "Dark",
        "Melodic",
        "Lo-Fi",
        "Drill",
        "Ambient",
        "Hyperpop"        
    };

    private static readonly Random Rng = new();

    public static string GenerateIdea()
    {
        var bpm = Rng.Next(100,180);
        var key = Keys[Rng.Next(0, Keys.Length)];
        var style = Styles[Rng.Next(0, Styles.Length)];

        return $"""
        🎵 Идея для бита:

        BPM: {bpm}
        Key: {key}
        Style: {style}
        """;
    }
}