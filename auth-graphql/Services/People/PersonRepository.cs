using auth_graphql.Data;
using auth_graphql.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_graphql.Services.People;

public class PersonRepository : IPersonRepository
{
    private readonly IDbContextFactory<MSSqlDbContext> _contextFactory;

    public PersonRepository(IDbContextFactory<MSSqlDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Person> FindPersonByEmail(string email)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.People.FirstOrDefaultAsync(u => u.Email == email);
        }
    }

    public async Task<IEnumerable<Person>> GetPersonByEmails(IReadOnlyList<string> emails)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.People
                .Where(p => emails.Contains(p.Email))
                .ToListAsync();
        }
    }

    public async Task<IEnumerable<Person>> GetPersonByUserIds(IReadOnlyList<Guid> ids)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.People
                .Where(p => ids.Contains(p.UserId))
                .ToListAsync();
        }
    }

    public async Task<Person> Create(Person person)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            await context.People.AddAsync(person);
            await context.SaveChangesAsync();

            return person;
        }
    }
}