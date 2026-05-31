namespace ScienceResultsWeb.Models
{
    public class ReferenceDto
    {
        public int Id { get; set; }
        public string Citation { get; set; } = "";
        public int ScientificResultId { get; set; }
    }
}