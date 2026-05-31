namespace ScienceResultsWeb.Models
{
    public class ScientificResultDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int Year { get; set; }
        public string DOI { get; set; } = "";
        public int ResultTypeId { get; set; }
    }
}