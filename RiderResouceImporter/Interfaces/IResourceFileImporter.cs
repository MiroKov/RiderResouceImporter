namespace RiderResouceImporter.Interfaces
{
    public interface IResourceFileImporter
    {
        ImportResult Import(string path, List<string> languages);
    }
}