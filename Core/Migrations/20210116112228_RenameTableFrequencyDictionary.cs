using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class RenameTableFrequencyDictionary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DictionaryEntries",
                table: "DictionaryEntries");

            migrationBuilder.RenameTable(
                name: "DictionaryEntries",
                newName: "FrequencyDictionary");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FrequencyDictionary",
                table: "FrequencyDictionary",
                column: "Word");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FrequencyDictionary",
                table: "FrequencyDictionary");

            migrationBuilder.RenameTable(
                name: "FrequencyDictionary",
                newName: "DictionaryEntries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DictionaryEntries",
                table: "DictionaryEntries",
                column: "Word");
        }
    }
}
