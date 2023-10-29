namespace GraphQL.Filters.Examples;

public interface IDivers
{
    IEnumerable<Diver> Divers { get; }
}

public interface IDives
{
    IEnumerable<Dive> Dives { get; }
}
