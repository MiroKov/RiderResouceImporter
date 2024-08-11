namespace RiderResouceImporter
{
    public class ResourceEntry(string name, string value, string language)
    {
        public string Name { get; set; } = name;
        public string Value { get; set; } = value;
        public string Language { get; set; } = language;
    }
}