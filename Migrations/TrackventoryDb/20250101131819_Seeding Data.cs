using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace trackventory_backend.Migrations.TrackventoryDb
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("0055638c-3362-47a7-94cd-056c7993b8b3"), "Retails", new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("90d6a812-8b39-408e-bb30-4dd5a3d3664d"), "Components", new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CategoryId", "ProductName", "SKU", "Site", "UpdatedDate", "Warehouse" },
                values: new object[,]
                {
                    { new Guid("3707d0d7-643d-4c42-9ce5-ad0a32f9d23a"), new Guid("90d6a812-8b39-408e-bb30-4dd5a3d3664d"), "Espresso Roast Decaf 1lb", 102895, "A009", new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "A009" },
                    { new Guid("5161df48-6b34-496f-9957-61077b79e56c"), new Guid("0055638c-3362-47a7-94cd-056c7993b8b3"), "WB Ethiopia 250g CORE", 102316, "A009", new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "A009" },
                    { new Guid("845ee88d-e92f-49fb-8140-18b72ac96631"), new Guid("0055638c-3362-47a7-94cd-056c7993b8b3"), "Teavana Retail Chai CT/4 CORE", 102895, "A009", new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "A009" },
                    { new Guid("869ebbb7-8457-40b8-8bba-66b900a56f32"), new Guid("90d6a812-8b39-408e-bb30-4dd5a3d3664d"), "Cold Brew Coffee", 102895, "A009", new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "A009" }
                });

            migrationBuilder.InsertData(
                table: "StockCount",
                columns: new[] { "Id", "Counted", "CountedBy", "CountingReasonCode", "OnHand", "ProductId", "Quantity", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("5050a777-032c-4f9b-815d-c0ff65571f27"), 0f, new Guid("db5758d4-4a6c-43f3-957d-c04ffa1bde69"), "Stock Update", 5f, new Guid("5161df48-6b34-496f-9957-61077b79e56c"), 0f, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("67d55f8b-a78b-4f82-a970-108c5aa46739"), 0f, new Guid("db5758d4-4a6c-43f3-957d-c04ffa1bde69"), "Stock Update", 19.71f, new Guid("3707d0d7-643d-4c42-9ce5-ad0a32f9d23a"), 0f, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6f138c1d-ee76-4f2d-8d14-49b5377fa3bd"), 0f, new Guid("db5758d4-4a6c-43f3-957d-c04ffa1bde69"), "Stock Update", 64.62f, new Guid("869ebbb7-8457-40b8-8bba-66b900a56f32"), 0f, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("ff92736d-696e-48a2-b6ad-329df9d14881"), 0f, new Guid("db5758d4-4a6c-43f3-957d-c04ffa1bde69"), "Stock Update", 5f, new Guid("845ee88d-e92f-49fb-8140-18b72ac96631"), 0f, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockCount",
                keyColumn: "Id",
                keyValue: new Guid("5050a777-032c-4f9b-815d-c0ff65571f27"));

            migrationBuilder.DeleteData(
                table: "StockCount",
                keyColumn: "Id",
                keyValue: new Guid("67d55f8b-a78b-4f82-a970-108c5aa46739"));

            migrationBuilder.DeleteData(
                table: "StockCount",
                keyColumn: "Id",
                keyValue: new Guid("6f138c1d-ee76-4f2d-8d14-49b5377fa3bd"));

            migrationBuilder.DeleteData(
                table: "StockCount",
                keyColumn: "Id",
                keyValue: new Guid("ff92736d-696e-48a2-b6ad-329df9d14881"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("3707d0d7-643d-4c42-9ce5-ad0a32f9d23a"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("5161df48-6b34-496f-9957-61077b79e56c"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("845ee88d-e92f-49fb-8140-18b72ac96631"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("869ebbb7-8457-40b8-8bba-66b900a56f32"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("0055638c-3362-47a7-94cd-056c7993b8b3"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("90d6a812-8b39-408e-bb30-4dd5a3d3664d"));
        }
    }
}
