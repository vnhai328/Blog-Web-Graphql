using auth_graphql;
using auth_graphql.Data;
using auth_graphql.DataLoaders;
using auth_graphql.Schema.Mutations;
using auth_graphql.Schema.Mutations.Comments;
using auth_graphql.Schema.Mutations.Likes;
using auth_graphql.Schema.Mutations.PostImages;
using auth_graphql.Schema.Mutations.Posts;
using auth_graphql.Schema.Mutations.Users;
using auth_graphql.Schema.Queries;
using auth_graphql.Schema.Queries.Comments;
using auth_graphql.Schema.Queries.Likes;
using auth_graphql.Schema.Queries.Posts;
using auth_graphql.Services.Comments;
using auth_graphql.Services.Likes;
using auth_graphql.Services.People;
using auth_graphql.Services.PostImages;
using auth_graphql.Services.Posts;
using auth_graphql.Services.UploadFile;
using auth_graphql.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPooledDbContextFactory<MSSqlDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("Default")
).LogTo(Console.WriteLine));

builder.Services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddType<PostType>()
                .AddType<CommentType>()
                .AddTypeExtension<PostQuery>()
                .AddTypeExtension<CommentQuery>()
                .AddTypeExtension<LikeQuery>()
                .AddTypeExtension<UserMutation>()
                .AddTypeExtension<PostMutation>()
                .AddTypeExtension<CommentMutation>()
                .AddTypeExtension<ImageMutation>()
                .AddTypeExtension<LikeMutation>()
                .AddFiltering()
                .AddSorting()
                .AddProjections()
                .AddInMemorySubscriptions()
                .AddAuthorization()
                .AddMutationConventions();

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Startup.SharedSecret)
    };
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<AzureUploadExtensions>();
builder.Services.AddScoped<CommentDataLoader>();
builder.Services.AddScoped<PersonByEmailDataLoader>();
builder.Services.AddScoped<PersonByUserIdDataLoader>();
builder.Services.AddScoped<PostImageDataLoader>();
builder.Services.AddScoped<PostLikeDataLoader>();
builder.Services.AddScoped<CommentLikeDataLoader>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPersonRepository ,PersonRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IPostImageRepository, PostImageRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                      });
});

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();
