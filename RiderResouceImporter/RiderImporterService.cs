using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using RiderResouceImporter.Interfaces;

namespace RiderResouceImporter
{
    public class RiderImporterService : IResourceFileImporter
    {
        public ImportResult Import(string path, List<string> languages)
        {
            var result = new ImportResult();
            var package = new ExcelPackage(new FileInfo(path));
            if (package.Workbook == null || package.Workbook.Worksheets.Count == 0) return result;
            foreach (var worksheet in package.Workbook.Worksheets)
            {
                if (worksheet.Dimension == null) continue;
                var rowStart = worksheet.Dimension.Start.Row;
                var rowEnd = worksheet.Dimension.End.Row;
                using var sortRange = worksheet.Cells[rowStart + 1, 1, rowEnd, 8];
                using var headers = worksheet.Cells[rowStart, 1, rowStart, 8];
                sortRange.Sort(1);
                result.Resources.AddRange(GetResourceFiles(sortRange, headers, languages));
            }

            return result;
        }

        private List<ResourceFile> GetResourceFiles(ExcelRange sortRange, ExcelRange headerRange, List<string> languagesToImport)
        {
            var headers = headerRange.Select(_ => new HeaderColumn(_.Text, _.Start.Column))
                .Where(_ => languagesToImport.Contains(_.Text, StringComparer.OrdinalIgnoreCase)).ToList();
            var defaultCultureIndex = headers.First(_ => _.Text == Constants.DefaultCulture).Start;
            var resourceFiles = new List<ResourceFile>();
            for (var i = 2; i < sortRange.Rows + 1; i++)
            {
                var defaultCulture = sortRange.FirstOrDefault(_ => _.Start.Column == defaultCultureIndex && _.Start.Row == i)?.Text;
                var resourceBasePath = sortRange.FirstOrDefault(_ => _.Start.Column == 1 && _.Start.Row == i)?.Text; // Resource path
                var name = sortRange.FirstOrDefault(_ => _.Start.Column == 2 && _.Start.Row == i)?.Text; // Name
                var resourceFile = new ResourceFile(resourceBasePath);
                foreach (var headerColumn in headers)
                {
                    var translation = sortRange.FirstOrDefault(_ => _.Start.Column == headerColumn.Start && _.Start.Row == i)?.Text;
                    resourceFile.AddEntry(name,translation, headerColumn.Text, defaultCulture);
                }

                resourceFiles.Add(resourceFile);
            }

            return resourceFiles;
        }
    }
}