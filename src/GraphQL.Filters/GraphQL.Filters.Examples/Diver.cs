using System.Net.Mail;

namespace GraphQL.Filters.Examples;

public record Diver(string Name, MailAddress? Email, int Id,string? Bio,DateOnly? BirthDate)
{
    public IReadOnlyCollection<Course>? Courses { get; set; }
    public IReadOnlyCollection<Diver> Buddies {get;set;} = new List<Diver>();
  
}
