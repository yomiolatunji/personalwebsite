using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YomiOlatunji.DataSource.Migrations
{
    public partial class UpdatePost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PublishStatuses_PublishStatusId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "PostLink");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PublishStatusId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PublishStatusId",
                table: "Posts");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Posts");

            migrationBuilder.AddColumn<short>(
                name: "PublishStatusId",
                table: "Posts",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "PostLink",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkTypeId = table.Column<short>(type: "smallint", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostLink_LinkTypes_LinkTypeId",
                        column: x => x.LinkTypeId,
                        principalTable: "LinkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostLink_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PublishStatusId",
                table: "Posts",
                column: "PublishStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLink_LinkTypeId",
                table: "PostLink",
                column: "LinkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLink_PostId",
                table: "PostLink",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PublishStatuses_PublishStatusId",
                table: "Posts",
                column: "PublishStatusId",
                principalTable: "PublishStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
