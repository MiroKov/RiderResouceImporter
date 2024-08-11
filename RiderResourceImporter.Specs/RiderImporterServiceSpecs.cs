using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using RiderResouceImporter;

namespace RiderResourceImporter.Specs;

public class RiderImporterServiceSpecs
{
    [Fact]
    public void ImportsResourcesSuccesfuly()
    {
        var path = "Resources.xlsx";
        var importer = new RiderImporterService();
        var languages = new List<string> { Constants.DefaultCulture, "en" };
        var result = importer.Import(path, languages);
        result.Should().NotBeNull();
    }

    [Fact]
    public void ReadsExcelFileSuccesfuly()
    {
        var path = "Resources.xlsx";
        var importer = new RiderImporterService();
        var languages = new List<string> { Constants.DefaultCulture, "en" };
        var result = importer.Import(path, languages);
        result.Resources.Count.Should().NotBe(0);
    }

    [Fact]
    public void FindsTargetResourceLocations()
    {
        var path = "Resources.xlsx";
        var importer = new RiderImporterService();
        var languages = new List<string> { Constants.DefaultCulture, "en" };
        var result = importer.Import(path, languages);
        result.Resources.First().DefaultPath.Should().NotBeEmpty();
    }

    [Fact]
    public void ThrowsNoErrorOnUndefinedLanguages()
    {
        var path = "Resources.xlsx";
        var importer = new RiderImporterService();
        var languages = new List<string> { Constants.DefaultCulture, "en", "it" };
        var result = importer.Import(path, languages);
        result.Resources.First().DefaultPath.Should().NotBeEmpty();
    }

    
}