using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealerDatabase.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dealers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", maxLength: 200, nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LegalName = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyNumber = table.Column<string>(type: "TEXT", nullable: true),
                    IncorporationDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    CompanyStatus = table.Column<string>(type: "TEXT", nullable: true),
                    VatNumber = table.Column<string>(type: "TEXT", nullable: true),
                    VatValidationStatus = table.Column<string>(type: "TEXT", nullable: true),
                    FcaFrn = table.Column<string>(type: "TEXT", nullable: true),
                    FcaStatus = table.Column<string>(type: "TEXT", nullable: true),
                    IcoRegistrationNumber = table.Column<string>(type: "TEXT", nullable: true),
                    IcoExpiryDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    SafStatus = table.Column<string>(type: "TEXT", nullable: true),
                    SafExpiryDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    LineageTag = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dealers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealerAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DealerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Line1 = table.Column<string>(type: "TEXT", nullable: true),
                    Line2 = table.Column<string>(type: "TEXT", nullable: true),
                    Line3 = table.Column<string>(type: "TEXT", nullable: true),
                    Town = table.Column<string>(type: "TEXT", nullable: true),
                    County = table.Column<string>(type: "TEXT", nullable: true),
                    Postcode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerAddress_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DealerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerContact_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerFieldSource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DealerId = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldName = table.Column<string>(type: "TEXT", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    SourceRecordId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerFieldSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerFieldSource_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerTradingName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DealerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerTradingName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerTradingName_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealerAddress_DealerId",
                table: "DealerAddress",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerContact_DealerId",
                table: "DealerContact",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerFieldSource_DealerId",
                table: "DealerFieldSource",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerTradingName_DealerId",
                table: "DealerTradingName",
                column: "DealerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealerAddress");

            migrationBuilder.DropTable(
                name: "DealerContact");

            migrationBuilder.DropTable(
                name: "DealerFieldSource");

            migrationBuilder.DropTable(
                name: "DealerTradingName");

            migrationBuilder.DropTable(
                name: "Dealers");
        }
    }
}
