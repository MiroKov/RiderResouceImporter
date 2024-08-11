namespace RiderResouceImporter.Interfaces
{
    public interface IResourceFileWriter
    {
        void Write(ImportResult result, string rootPath);
    }
}