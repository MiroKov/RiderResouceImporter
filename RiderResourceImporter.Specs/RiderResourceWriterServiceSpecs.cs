using System.Collections.Generic;
using FluentAssertions;
using RiderResouceImporter;

namespace RiderResourceImporter.Specs
{
    public class RiderResourceWriterServiceSpecs
    {
        [Fact]
        public void WritesResourceFiles()
        {
            // arrange
            var importPath = "TestResources.xlsx";
            var exportPath = ".\\";
            var importer = new RiderImporterService();
            var languages = new List<string> { Constants.DefaultCulture, "en", "it" };
            var result = importer.Import(importPath, languages);
            var filler = new RiderResourceWriterService();
            // act
            filler.Write(result, exportPath);
        }

        [Fact]
        public void WritesCorrectValues()
        {
            // arrange
            var importPath = "TestResources.xlsx";
            var exportPath = ".\\";
            var importer = new RiderImporterService();
            var languages = new List<string> { Constants.DefaultCulture, "en", "it" };
            var importResult = importer.Import(importPath, languages);
            var filler = new RiderResourceWriterService();
            // act
            var writeResult = filler.Write(importResult, exportPath);

            // assert
            writeResult.Resources.Count.Should().Be(4);
            writeResult.Resources.SingleOrDefault(_ => string.Equals(Path.GetFileName(_.FileName), "Dtos.V7.HazardMaterialDto.resx")).Should().NotBeNull();
            writeResult.Resources.SingleOrDefault(_ => string.Equals(Path.GetFileName(_.FileName), "Dtos.V7.HazardMaterialDto.resx"))!.Entries.Count.Should().Be(7);
            writeResult.Resources.SingleOrDefault(_ => string.Equals(Path.GetFileName(_.FileName), "Dtos.V7.HazardMaterialDto.resx"))!
                .Entries.All(_ => _.Language == Constants.DefaultCulture).Should().BeTrue();
            writeResult.Resources.SingleOrDefault(_ => string.Equals(Path.GetFileName(_.FileName), "Dtos.V7.HazardMaterialDto.resx"))!
                .Entries.FirstOrDefault(_ => _.Value == "IBC Behälter aus Stahl").Should().NotBeNull();
        }
    }
}