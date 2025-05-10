﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepiEcommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_Orders_OrderId",
                table: "OrderProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_Products_ProductId",
                table: "OrderProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProduct",
                table: "OrderProduct");

            migrationBuilder.RenameTable(
                name: "OrderProduct",
                newName: "orderProducts");

            migrationBuilder.RenameIndex(
                name: "IX_OrderProduct_ProductId",
                table: "orderProducts",
                newName: "IX_orderProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderProduct_OrderId",
                table: "orderProducts",
                newName: "IX_orderProducts_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orderProducts",
                table: "orderProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_orderProducts_Orders_OrderId",
                table: "orderProducts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orderProducts_Products_ProductId",
                table: "orderProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderProducts_Orders_OrderId",
                table: "orderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_orderProducts_Products_ProductId",
                table: "orderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orderProducts",
                table: "orderProducts");

            migrationBuilder.RenameTable(
                name: "orderProducts",
                newName: "OrderProduct");

            migrationBuilder.RenameIndex(
                name: "IX_orderProducts_ProductId",
                table: "OrderProduct",
                newName: "IX_OrderProduct_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_orderProducts_OrderId",
                table: "OrderProduct",
                newName: "IX_OrderProduct_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProduct",
                table: "OrderProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_Orders_OrderId",
                table: "OrderProduct",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_Products_ProductId",
                table: "OrderProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
