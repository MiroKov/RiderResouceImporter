namespace RiderResouceImporter
{
    public class WriteResourceFile(string language, string fileName, List<ResourceEntry> entries)
    {
        public string Language { get; } = language;
        public string FileName { get; } = fileName;
        public List<ResourceEntry> Entries { get; } = entries;
    }
}