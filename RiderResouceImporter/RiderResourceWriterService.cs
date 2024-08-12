using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources.NetStandard;
using RiderResouceImporter.Interfaces;

namespace RiderResouceImporter
{
    public class RiderResourceWriterService : IResourceFileWriter
    {
        public WriteResult Write(ImportResult import, string rootPath)
        {
            var result = new WriteResult();
            foreach (var resourceFile in import.Resources)
            {
                if (resourceFile.DefaultPath == null) continue;
                var basePath = Path.GetFullPath(Path.Combine(rootPath, resourceFile.DefaultPath + ".resx"));
                result.Resources.AddRange(ProcessResourceFile(resourceFile, basePath));
            }

            return result;
        }

        private List<WriteResourceFile> ProcessResourceFile(ResourceFile newResourceFile, string basePath)
        {
            var result = new List<WriteResourceFile>();
            var directoryPath = Path.GetDirectoryName(basePath);
            if (!string.IsNullOrEmpty(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var translations = newResourceFile.Entries.Select(_ => _.Language).Distinct().ToList();
            foreach (var translation in translations)
            {
                var translationExtension = translation.Contains(Constants.DefaultCulture) ? string.Empty : $"{translation}.";
                var tempResourceFileName = basePath.Substring(0, basePath.Length - 4) + translationExtension + "temp." + "resx";
                var targetResourceFileName = basePath.Substring(0, basePath.Length - 4) + translationExtension + "resx";
                var entries = newResourceFile.Entries.Where(_ => _.Language == translation).ToList();
                result.Add(new WriteResourceFile(translation, targetResourceFileName, entries));
                WriteTempResourceFile(tempResourceFileName, entries);
                DeleteOldResourceFile(targetResourceFileName);
                RenameNewResourceFile(tempResourceFileName, targetResourceFileName);
            }

            return result;
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