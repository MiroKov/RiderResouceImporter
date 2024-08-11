namespace RiderResouceImporter
{
    public class HeaderColumn(string text, int start)
    {
        public string Text { get; set; } = text;
        public int Start { get; set; } = start;
    }
}