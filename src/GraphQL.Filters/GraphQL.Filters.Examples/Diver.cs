using System.Net.Mail;

namespace GraphQL.Filters.Examples;

public record Diver(string Name, MailAddress Email, int Id)
{
    public IReadOnlyCollection<Course>? Courses { get; set; } 
  
}
