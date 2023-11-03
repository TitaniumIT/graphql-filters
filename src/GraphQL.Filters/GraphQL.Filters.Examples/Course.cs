namespace GraphQL.Filters.Examples;

public record Course(string Name, DateOnly Started, DateOnly? Finished,CourseResults CourseResult );

public enum CourseResults { InProgress, SuccessFull, Failed }
