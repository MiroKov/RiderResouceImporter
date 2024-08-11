using System.Diagnostics.CodeAnalysis;

namespace RiderResouceImporter
{
    [ExcludeFromCodeCoverage]
    public class MissingImport(string name, string language)
    {
        public string Name { get; set; } = name;
        public string Language { get; set; } = language;
    }
}