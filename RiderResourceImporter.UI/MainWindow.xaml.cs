using System.IO;
using System.Windows;
using RiderResouceImporter;
using RiderResouceImporter.Interfaces;

namespace RiderResourceImporter.UI;

public partial class MainWindow : Window
{
    private readonly IResourceFileWriter resourceFileWriter;
    private readonly IResourceFileImporter resourceFileImporter;
    private readonly char[] separator = [';', ',', '|'];

    public MainWindow(IResourceFileWriter resourceFileWriter, IResourceFileImporter resourceFileImporter)
    {
        this.resourceFileWriter = resourceFileWriter;
        this.resourceFileImporter = resourceFileImporter;
        InitializeComponent();
        LanguagesTextBox.Text = $"{Constants.DefaultCulture};en;";
    }

    private void ExcelFileButton_Click(object sender, RoutedEventArgs e)
    {
        // Create OpenFileDialog 
        var dlg = new Microsoft.Win32.OpenFileDialog
        {
            // Set filter for file extension and default file extension 
            DefaultExt = ".xlsx",
            Filter = "Excel Files (*.xlsx) | *.xlsx",
            DefaultDirectory = Directory.GetCurrentDirectory()
        };

        // Display OpenFileDialog by calling ShowDialog method 
        var result = dlg.ShowDialog();

        // Get the selected file name and display in a TextBox 
        if (result == true)
        {
            // Open document 
            var filename = dlg.FileName;
            ExcelFileTextBox.Text = filename;
        }
    }

    private void ExportRootPathButton_Click(object sender, RoutedEventArgs e)
    {
        // Create OpenFileDialog 
        var dlg = new Microsoft.Win32.OpenFolderDialog()
        {
            Multiselect = false,
            Title = "Select a root folder where your resource files are to be saved"
        };

        // Display OpenFileDialog by calling ShowDialog method 
        var result = dlg.ShowDialog();

        // Get the selected file name and display in a TextBox 
        if (result == true)
        {
            // Open document 
            ExportRootPathTextBox.Text = dlg.FolderName;
        }
    }

    private void ImportButton_Click(object sender, RoutedEventArgs e)
    {
        if (!File.Exists(ExcelFileTextBox.Text))
        {
            MessageBox.Show("Please specify the excel file to import the resources from", "Enter import prerequesiters");
            return;
        }

        if (!Directory.Exists(ExportRootPathTextBox.Text))
        {
            MessageBox.Show("Please specify the root path where the resource files are to be over/written", "Enter export prerequesiters");
            return;
        }

        var languages = LanguagesTextBox.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Where(_ => _.Length == 2 || _ == Constants.DefaultCulture).ToList();
        if (!languages.Any())
        {
            MessageBox.Show("Please specify the resources languages (ISOZ2) ", "Enter translation languages");
            return;
        }

        var importResult = resourceFileImporter.Import(ExcelFileTextBox.Text, languages);
        resourceFileWriter.Write(importResult, ExportRootPathTextBox.Text);
        MessageBox.Show("Import successful");
    }
}