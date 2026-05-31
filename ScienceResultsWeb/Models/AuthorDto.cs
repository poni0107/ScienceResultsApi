namespace ScienceResultsWeb.Models
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Institution { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
