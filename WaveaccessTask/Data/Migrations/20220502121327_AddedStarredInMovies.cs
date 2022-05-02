using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaveaccessTask.Data.Migrations
{
    public partial class AddedStarredInMovies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actor_Movie_MovieId",
                table: "Actor");

            migrationBuilder.DropIndex(
                name: "IX_Actor_MovieId",
                table: "Actor");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Actor");

            migrationBuilder.CreateTable(
                name: "ActorMovie",
                columns: table => new
                {
                    ActorCastId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MoviesStarredInId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorMovie", x => new { x.ActorCastId, x.MoviesStarredInId });
                    table.ForeignKey(
                        name: "FK_ActorMovie_Actor_ActorCastId",
                        column: x => x.ActorCastId,
                        principalTable: "Actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorMovie_Movie_MoviesStarredInId",
                        column: x => x.MoviesStarredInId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovie_MoviesStarredInId",
                table: "ActorMovie",
                column: "MoviesStarredInId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorMovie");

            migrationBuilder.AddColumn<Guid>(
                name: "MovieId",
                table: "Actor",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actor_MovieId",
                table: "Actor",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actor_Movie_MovieId",
                table: "Actor",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id");
        }
    }
}
