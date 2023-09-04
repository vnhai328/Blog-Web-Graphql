using auth_graphql.Models;

namespace auth_graphql.Services.People;

public interface IPersonRepository
{
    Task<Person> FindPersonByEmail(string email);
    Task<IEnumerable<Person>> GetPersonByEmails(IReadOnlyList<string> emails);
    Task<IEnumerable<Person>> GetPersonByUserIds(IReadOnlyList<Guid> ids);
    Task<Person> Create(Person person);
}