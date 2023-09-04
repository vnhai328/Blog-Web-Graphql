using auth_graphql.Models;
using auth_graphql.Services.People;

namespace auth_graphql.DataLoaders;

public class PersonByUserIdDataLoader : BatchDataLoader<Guid, Person>
{
    private readonly IPersonRepository _personRepository;

    public PersonByUserIdDataLoader(
        IPersonRepository personRepository,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options = null) : base(batchScheduler, options)
    {
        _personRepository = personRepository;
    }
    protected override async Task<IReadOnlyDictionary<Guid, Person>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetPersonByUserIds(keys);
        return person.ToDictionary(i => i.UserId);
    }
}