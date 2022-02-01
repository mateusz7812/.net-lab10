using Microsoft.EntityFrameworkCore.Migrations;

namespace lab10.Migrations
{
    public partial class PropNameUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Article",
                newName: "ImageName");

            migrationBuilder.CreateTable(
                name: "ArticleViewModel",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Money = table.Column<decimal>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleViewModel", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_ArticleViewModel_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleViewModel_CategoryId",
                table: "ArticleViewModel",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleViewModel");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Article",
                newName: "ImagePath");
        }
    }
}
