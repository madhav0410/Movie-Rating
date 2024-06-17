using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAceess.Migrations
{
    /// <inheritdoc />
    public partial class addMovieReviewColumnInRatingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MovieRating",
                table: "Ratings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "MovieReview",
                table: "Ratings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieReview",
                table: "Ratings");

            migrationBuilder.AlterColumn<int>(
                name: "MovieRating",
                table: "Ratings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
