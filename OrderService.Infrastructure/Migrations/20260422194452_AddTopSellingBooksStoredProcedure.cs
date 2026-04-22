using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTopSellingBooksStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE dbo.usp_GetTopSellingBooks
                    @TopN INT = 5
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    CREATE TABLE #TopBooks (
                        BookId INT,
                        BookTitle NVARCHAR(200),
                        TotalQuantitySold INT
                    );

                    INSERT INTO #TopBooks (BookId, BookTitle, TotalQuantitySold)
                    SELECT 
                        BookId, 
                        MAX(BookTitle), -- Handle potential title changes gracefully
                        SUM(Quantity)
                    FROM 
                        dbo.OrderItems
                    GROUP BY 
                        BookId;
                    
                    SELECT TOP (@TopN) 
                        BookId, 
                        BookTitle, 
                        TotalQuantitySold
                    FROM 
                        #TopBooks
                    ORDER BY 
                        TotalQuantitySold DESC;
                    
                    DROP TABLE #TopBooks;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_GetTopSellingBooks");
        }
    }
}
