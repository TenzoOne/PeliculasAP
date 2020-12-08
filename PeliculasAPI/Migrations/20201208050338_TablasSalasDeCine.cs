using Microsoft.EntityFrameworkCore.Migrations;

namespace PeliculasAPI.Migrations
{
    public partial class TablasSalasDeCine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalasDeCine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalasDeCine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "peliculasSalasDeCines",
                columns: table => new
                {
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    SalaDeCineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peliculasSalasDeCines", x => new { x.PeliculaId, x.SalaDeCineId });
                    table.ForeignKey(
                        name: "FK_peliculasSalasDeCines_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_peliculasSalasDeCines_SalasDeCine_SalaDeCineId",
                        column: x => x.SalaDeCineId,
                        principalTable: "SalasDeCine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_peliculasSalasDeCines_SalaDeCineId",
                table: "peliculasSalasDeCines",
                column: "SalaDeCineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "peliculasSalasDeCines");

            migrationBuilder.DropTable(
                name: "SalasDeCine");
        }
    }
}
