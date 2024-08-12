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
        var path = "TestResources.xlsx";
        var importer = new RiderImporterService();
        var languages = new List<string> { Constants.DefaultCulture, "en" };
        var result = importer.Import(path, languages);
        result.Should().NotBeNull();
    }

    [Fact]
    public void ReadsExcelFileSuccesfuly()
    {
        var path = "TestResources.xlsx";
        var importer = new RiderImporterService();
        var languages = new List<string> { Constants.DefaultCulture, "en" };
        var result = importer.Import(path, languages);
        result.Resources.Count.Should().NotBe(0);
    }

    [Fact]
    public void FindsTargetResourceLocations()
    {
        var path = "TestResources.xlsx";
        var importer = new RiderImporterService();
        var languages = new List<string> { Constants.DefaultCulture, "en" };
        var result = importer.Import(path, languages);
        result.Resources.First().DefaultPath.Should().NotBeEmpty();
    }

    [Fact]
    public void ThrowsNoErrorOnUndefinedLanguages()
    {
        var path = "TestResources.xlsx";
        var importer = new RiderImporterService();
        var languages = new List<string> { Constants.DefaultCulture, "en", "it" };
        var result = importer.Import(path, languages);
        result.Resources.First().DefaultPath.Should().NotBeEmpty();
    }
    
    [Fact]
    public void HasCorrectValues()
    {
        var path = "TestResources.xlsx";
        var importer = new RiderImporterService();
        var languages = new List<string> { Constants.DefaultCulture, "en" };
        var result = importer.Import(path, languages);
        result.Resources.First().DefaultPath.Should().NotBeEmpty();
        result.Resources.Count.Should().Be(2);
        result.Resources.First().Entries.Count.Should().Be(14);
        result.Resources.Last().Entries.Count.Should().Be(4);
        result.Resources.First().Entries.SingleOrDefault(_ => _ is { Language: "en", Name: "0A1",Value:"Steel n. removable lid" }).Should().NotBeNull();
        result.Resources.First().Entries.SingleOrDefault(_ => _ is { Language: Constants.DefaultCulture, Name: "0A1",Value:"Stahl n. abnehmbarer Deckel" }).Should().NotBeNull();
        result.Resources.First().Entries.SingleOrDefault(_ => _ is { Language: "de", Name: "0A1",Value:"Stahl n. abnehmbarer Deckel" }).Should().BeNull();
        result.Resources.Last().Entries.SingleOrDefault(_ => _ is { Language: "en", Name: "11H1",Value:"St.Knst, FestSt. B/E Black Equip" }).Should().NotBeNull();
        result.Resources.Last().Entries.SingleOrDefault(_ => _ is { Language: Constants.DefaultCulture, Name: "11H1",Value:"St.Knst, FestSt. B/E Schw Ausr" }).Should().NotBeNull();
        result.Resources.Last().Entries.SingleOrDefault(_ => _ is { Language: "de", Name: "11H1",Value:"St.Knst, FestSt. B/E Schw Ausr" }).Should().BeNull();
    }

    
}