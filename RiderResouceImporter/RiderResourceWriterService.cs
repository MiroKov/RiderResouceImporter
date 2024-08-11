using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources.NetStandard;

namespace RiderResouceImporter
{
    public class RiderResourceWriterService
    {
        public void Write(ImportResult result, string rootPath)
        {
            foreach (var resourceFile in result.Resources)
            {
                if (resourceFile.DefaultPath == null) continue;
                var basePath = Path.GetFullPath(Path.Combine(rootPath, resourceFile.DefaultPath + ".resx"));
                ProcessResourceFile(resourceFile, basePath);
            }
        }

        private void ProcessResourceFile(ResourceFile newResourceFile, string basePath)
        {
            var directoryPath = Path.GetDirectoryName(basePath);
            if (!string.IsNullOrEmpty(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var translations = newResourceFile.Entries.Select(_ => _.Language).Distinct().ToList();
            foreach (var translation in translations)
            {
                var translationExtension = translation.Contains(Constants.DefaultCulture) ? string.Empty : $"{translation}.";
                var tempResourceFileName = basePath.Substring(0, basePath.Length - 4) + translationExtension + "temp." + "resx";
                var oldResourceFileName = basePath.Substring(0, basePath.Length - 4) + translationExtension + "resx";
                WriteTempResourceFile(tempResourceFileName, newResourceFile.Entries);
                DeleteOldResourceFile(oldResourceFileName);
                RenameNewResourceFile(tempResourceFileName, oldResourceFileName);
            }
        }

        private void RenameNewResourceFile(string tempResourceFileName, string oldResourceFileName)
        {
            File.Move(tempResourceFileName, oldResourceFileName);
        }

        private void DeleteOldResourceFile(string oldResourceFileName)
        {
            if (File.Exists(oldResourceFileName))
                File.Delete(oldResourceFileName);
        }

        private void WriteTempResourceFile(string tempResourceFileName, List<ResourceEntry> entries)
        {
            if (File.Exists(tempResourceFileName))
                File.Delete(tempResourceFileName);
            using var resourceWriter = new ResXResourceWriter(tempResourceFileName);
            foreach (var entry in entries)
                resourceWriter.AddResource(entry.Name, entry.Value);
            resourceWriter.Generate();
        }
    }
}