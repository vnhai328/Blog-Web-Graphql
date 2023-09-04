using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using auth_graphql.Data;
using auth_graphql.DataLoaders;
using auth_graphql.Models;
using auth_graphql.Services.People;
using auth_graphql.Services.Users;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace auth_graphql.Schema.Mutations.Users;

[ExtendObjectType(typeof(Mutation))]
public class UserMutation
{
    private readonly IUserRepository _userRepository;
    private readonly IPersonRepository _personRepository;

    public UserMutation(IUserRepository userRepository, IPersonRepository personRepository)
    {
        _userRepository = userRepository;
        _personRepository = personRepository;
    }
    
    [UseDbContext(typeof(MSSqlDbContext))]
    public async Task<SignUpPayLoad> SignUp(
        SignUpInput input,
        [ScopedService] MSSqlDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrEmpty(input.Name))
        {
            throw new QueryException(
                ErrorBuilder.New()
                .SetMessage("The name can not empty.")
                .SetCode("USERNAME_EMPTY")
                .Build()
            );
        }

        if (string.IsNullOrEmpty(input.Email))
        {
            throw new QueryException(
                ErrorBuilder.New()
                .SetMessage("The email can not empty")
                .SetCode("EMAIL_EMPTY")
                .Build()
            );
        }
        
        var checkEmail = await _userRepository.FindUserByEmail(input.Email);
        if(checkEmail != null)
        {
            throw new QueryException(
                ErrorBuilder.New()
                .SetMessage("Account already exists")
                .SetCode("EMAIL_EXITS")
                .Build()
            );
        }

        if (string.IsNullOrEmpty(input.Password))
        {
            throw new QueryException(
                ErrorBuilder.New()
                .SetMessage("The password can not empty")
                .SetCode("PASSWORD_EMPTY")
                .Build()
            );
        }

        string salt = Guid.NewGuid().ToString("N");

        using var sha = SHA512.Create();
        byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input.Password + salt));

        Guid personId = Guid.NewGuid();

        var user = new User
        {
            Id = Guid.NewGuid(),
            PersonId1 = personId,
            Email = input.Email,
            PassWordHash = Convert.ToBase64String(hash),
            Salt = salt
        };

        var person = new Person
        {
            Id = personId,
            UserId = user.Id,
            Name = input.Name,
            Email = input.Email,
            LastSeen = DateTimeOffset.UtcNow,
            ImageUri = input.image
        };

        dbContext.Users.Add(user);
        dbContext.People.Add(person);
        await dbContext.SaveChangesAsync();

        return new SignUpPayLoad(user, input.ClientMutationId);
    }

    public async Task<LoginPayLoad> LoginAsync(
        LoginInput input,
        [Service] PersonByEmailDataLoader personByEmail,
        [Service] ITopicEventSender sender,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrEmpty(input.Email))
        {
            throw new QueryException(
                ErrorBuilder.New()
                .SetMessage("The email mustn't be empty.")
                .SetCode("EMPTY_EMAIL")
                .Build()
            );
        }

        if (string.IsNullOrEmpty(input.Password))
        {
            throw new QueryException(
                ErrorBuilder.New()
                .SetMessage("The password mustn't be empty.")
                .SetCode("PASSWORD_EMPTY")
                .Build()
            );
        }

        User? user = await _userRepository.FindUserByEmail(input.Email);
        Person? person = await _personRepository.FindPersonByEmail(input.Email);

        if (user is null)
        {
            throw new QueryException(
                ErrorBuilder.New()
                .SetMessage("Username is invalid")
                .SetCode("INVALID_ACCOUNT")
                .Build()
            );
        }

        using var sha = SHA512.Create();
        byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input.Password + user.Salt));

        if (!Convert.ToBase64String(hash).Equals(user.PassWordHash, StringComparison.Ordinal))
        {
            throw new QueryException(
                ErrorBuilder.New()
                .SetMessage("Password is wrong")
                .SetCode("PASSWWORD_WRONG")
                .Build()
            );
        }

        var me = await personByEmail.LoadAsync(input.Email, cancellationToken);

        var identity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, person.Name)
        });

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Startup.SharedSecret),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        // await sender.SendAsync<string, Person>("oneline", me);

        return new LoginPayLoad(me, tokenString, "Bearer", input.ClientMutationId);
    }
}