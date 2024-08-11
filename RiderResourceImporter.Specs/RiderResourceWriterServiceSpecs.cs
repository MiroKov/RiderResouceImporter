using System.Collections.Generic;
using RiderResouceImporter;

namespace RiderResourceImporter.Specs
{
    public class RiderResourceWriterServiceSpecs
    {
        [Fact]
        public void WritesResourceFiles()
        {
            // arrange
            var importPath = "Resources.xlsx";
            var exportPath = ".\\";
            var importer = new RiderImporterService();
            var languages = new List<string> { Constants.DefaultCulture, "en", "it" };
            var result = importer.Import(importPath, languages);
            var filler = new RiderResourceWriterService();
            // act
            filler.Write(result, exportPath);
        }
    }
}