using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsureYouAI.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoUrlPropertyToTrailerVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "TrailerVideos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "TrailerVideos");
        }
    }
}
