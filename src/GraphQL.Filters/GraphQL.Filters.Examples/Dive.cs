namespace GraphQL.Filters.Examples;

public record Dive(string Location, DateOnly On, TimeOnly Start, TimeOnly End, double AverageDepth){
    public Diver? Diver{get;set;} 
}