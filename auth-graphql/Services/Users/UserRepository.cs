using auth_graphql.Data;
using auth_graphql.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_graphql.Services.Users;

public class UserRepository : IUserRepository
{
    private readonly IDbContextFactory<MSSqlDbContext> _contextFactory;
    public UserRepository(IDbContextFactory<MSSqlDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<User> FindUserByEmail(string email)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
    public async Task<User> Create(User user)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }
    }

}