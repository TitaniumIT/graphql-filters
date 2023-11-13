namespace GraphQL.Filters.Examples;

 public interface ITimed 
 {
    TimeOnly Start {get;}
    TimeOnly End {get;}
 }
public interface IDive : ITimed
{
    string Location {get;}
    DateOnly On {get;}
    double AverageDepth {get;}
}

public record Dive(string Location, DateOnly On, TimeOnly Start, TimeOnly End, double AverageDepth) : IDive {
    public Diver? Diver{get;set;} 
}
