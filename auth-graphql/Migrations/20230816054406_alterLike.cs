using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auth_graphql.Migrations
{
    /// <inheritdoc />
    public partial class alterLike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Likes",
                newName: "CreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Likes",
                newName: "userId");
        }
    }
}
