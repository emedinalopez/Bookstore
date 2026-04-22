using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCalculateDiscountFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER FUNCTION dbo.fn_CalculateDiscountedPrice
                (
                    @OriginalPrice DECIMAL(18,2),
                    @DiscountPercentage DECIMAL(5,2)
                )
                RETURNS DECIMAL(18,2)
                AS
                BEGIN
                    DECLARE @DiscountedPrice DECIMAL(18,2);
                    SET @DiscountedPrice = @OriginalPrice - (@OriginalPrice * (@DiscountPercentage / 100.0));
                    RETURN @DiscountedPrice;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS dbo.fn_CalculateDiscountedPrice");
        }
    }
}
