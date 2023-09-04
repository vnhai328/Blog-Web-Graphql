using auth_graphql.Models;
using auth_graphql.Services.People;

namespace auth_graphql.DataLoaders;

public class PersonByEmailDataLoader : BatchDataLoader<string, Person>
{
    private readonly IPersonRepository _personRepository;

    public PersonByEmailDataLoader(
        IPersonRepository personRepository,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options = null) : base(batchScheduler, options)
    {
        _personRepository = personRepository;
    }
    protected override async Task<IReadOnlyDictionary<string, Person>> LoadBatchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
    {
        var personReponse = await _personRepository.GetPersonByEmails(keys);
        return personReponse.ToDictionary(i => i.Email);
    }
}