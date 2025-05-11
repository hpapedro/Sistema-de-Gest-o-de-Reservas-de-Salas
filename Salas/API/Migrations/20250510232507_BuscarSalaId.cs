using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class BuscarSalaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataReserva",
                table: "Reservas",
                newName: "DataHoraInicio");

            migrationBuilder.RenameColumn(
                name: "CridadoEm",
                table: "Auditorias",
                newName: "CriadoEm");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataHoraFim",
                table: "Reservas",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataHoraFim",
                table: "Reservas");

            migrationBuilder.RenameColumn(
                name: "DataHoraInicio",
                table: "Reservas",
                newName: "DataReserva");

            migrationBuilder.RenameColumn(
                name: "CriadoEm",
                table: "Auditorias",
                newName: "CridadoEm");
        }
    }
}
