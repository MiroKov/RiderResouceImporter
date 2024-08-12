using System.Diagnostics.CodeAnalysis;

namespace RiderResouceImporter
{
    [ExcludeFromCodeCoverage]
    public class ResourceEntry(string name, string value, string language)
    {
        public string Name { get; } = name;
        public string Value { get; } = value;
        public string Language { get; } = language;
    }
}