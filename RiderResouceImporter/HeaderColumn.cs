using System.Diagnostics.CodeAnalysis;

namespace RiderResouceImporter
{
    [ExcludeFromCodeCoverage]
    public class HeaderColumn(string text, int start)
    {
        public string Text { get; } = text;
        public int Start { get; } = start;
    }
}