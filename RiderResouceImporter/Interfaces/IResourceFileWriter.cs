namespace RiderResouceImporter.Interfaces
{
    public interface IResourceFileWriter
    {
        WriteResult Write(ImportResult result, string rootPath);
    }
}