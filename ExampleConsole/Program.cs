using PredicateBuilder;

List<DataEntity> data = new()
{
    new() { Id = 1, Name = "Alice" },
    new() { Id = 2, Name = "Bella" },
    new() { Id = 3, Name = "Christina" },
    new() { Id = 4, Name = "Diana" },
    new() { Id = 5, Name = "Emily" },
};

PredicateBuilder<DataEntity> predicate = new();
predicate.Or(x => x.Id == 4);
predicate.Or(x => x.Name.ToLower(), Operation.EQUALS, "emily");
foreach (DataEntity entity in data.Where(predicate.GetLambda().Compile()))
{
    Console.WriteLine($"id: {entity.Id}, name: {entity.Name}");
}

class DataEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}