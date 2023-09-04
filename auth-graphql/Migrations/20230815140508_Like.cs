using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auth_graphql.Migrations
{
    /// <inheritdoc />
    public partial class Like : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateLiked = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Like_Id = table.Column<int>(type: "int", nullable: false),
                    Like_Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");
        }
    }
}
