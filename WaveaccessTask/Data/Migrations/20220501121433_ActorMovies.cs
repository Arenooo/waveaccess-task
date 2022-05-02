using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaveaccessTask.Data.Migrations
{
    public partial class ActorMovies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ActorStarredInMove",
                newName: "ActorStarredInMovie");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ActorStarredInMovie",
                newName: "ActorStarredInMove");
        }
    }
}
