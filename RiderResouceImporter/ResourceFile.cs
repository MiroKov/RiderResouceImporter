using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RiderResouceImporter
{
    [ExcludeFromCodeCoverage]
    public class ResourceFile(string? resourceBasePath)
    {
        public string? DefaultPath { get; } = resourceBasePath;
        public List<ResourceEntry> Entries { get; } = [];
        public List<MissingImport> MissingImports { get; } = [];

        public void AddEntry(string? name, string? value, string language, string? defaultCulture)
        {
            if (name == null)
            {
                MissingImports.Add(new MissingImport($"Noname {defaultCulture}", language));
                return;
            }

            if (value == null)
            {
                MissingImports.Add(new MissingImport(name, language));
                return;
            }

            Entries.Add(new ResourceEntry(name, value, language));
        }
    }
}