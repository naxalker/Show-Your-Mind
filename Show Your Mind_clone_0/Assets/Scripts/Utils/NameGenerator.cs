using System.Collections.Generic;
using System.Text;

public static class NameGenerator
{
    private static readonly List<string> Prefixes = new List<string>
    {
        "Al", "Bel", "Cin", "Dal", "El", "Fen", "Gal", "Hin", "In", "Jen",
        "Kor", "Lor", "Mal", "Nor", "Ori", "Pel", "Quin", "Ral", "Sel", "Tor",
        "Ul", "Ven", "Wen", "Xan", "Yel", "Zor", "Bran", "Cris", "Dorn", "Eld"
    };

    private static readonly List<string> Roots = new List<string>
    {
        "anor", "bryn", "cor", "deth", "erin", "fal", "gan", "hel", "ion", "jal",
        "kir", "lor", "mar", "nor", "ol", "por", "qua", "rel", "sar", "tor",
        "ul", "vor", "wen", "xil", "yar", "zor", "bran", "cris", "dorn", "eld"
    };

    private static readonly List<string> Suffixes = new List<string>
    {
        "a", "ar", "en", "ia", "is", "on", "or", "us", "ys", "al",
        "el", "il", "ol", "ul", "an", "ir", "ur", "as", "es", "os",
        "ys", "or", "is", "ar", "ion", "iel", "ian", "ina", "ara", "ora"
    };

    private static readonly System.Random Random = new System.Random();

    public static string GenerateName()
    {
        var prefix = Prefixes[Random.Next(Prefixes.Count)];
        var root = Roots[Random.Next(Roots.Count)];
        var suffix = Suffixes[Random.Next(Suffixes.Count)];

        var nameBuilder = new StringBuilder();
        nameBuilder.Append(prefix);
        nameBuilder.Append(root);
        nameBuilder.Append(suffix);

        return nameBuilder.ToString();
    }
}
