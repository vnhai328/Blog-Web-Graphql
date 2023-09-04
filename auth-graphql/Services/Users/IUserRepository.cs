using auth_graphql.Models;

namespace auth_graphql.Services.Users;

public interface IUserRepository
{
    Task<User> FindUserByEmail(string email);
    Task<User> Create(User user);
}