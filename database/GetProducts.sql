alter PROCEDURE [dbo].[GetProducts]
    @PageNumber INT,
    @PageSize INT,
    @Title NVARCHAR(255) = NULL,
    @Brand NVARCHAR(255) = NULL,
    @TotalCount INT OUTPUT -- Adding an output parameter
AS
BEGIN
    SET NOCOUNT ON;

    -- Calculate the offset for pagination
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    -- Declare a table variable to hold the filtered products
    DECLARE @Products TABLE (
        Id INT,
        ProductTitle NVARCHAR(255),
        Code NVARCHAR(50),
        Brand NVARCHAR(50),
        Price DECIMAL(18, 2),
        Description NVARCHAR(MAX),
        CreatedAt DATETIME,
        UpdatedAt DATETIME,
        IsDeleted BIT
    );

    -- Insert filtered products into the table variable
    INSERT INTO @Products (Id, ProductTitle, Code, Brand, Price, Description, CreatedAt, UpdatedAt, IsDeleted)
    SELECT Id, ProductTitle, Code, Brand, Price, Description, CreatedAt, UpdatedAt, IsDeleted
    FROM Product
    WHERE (@Title IS NULL OR ProductTitle LIKE '%' + @Title + '%')
      AND (@Brand IS NULL OR Brand LIKE '%' + @Brand + '%')
    ORDER BY CreatedAt DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Get total count of filtered products and set it to the output parameter
    SELECT @TotalCount = COUNT(*)
    FROM Product
    WHERE (@Title IS NULL OR ProductTitle LIKE '%' + @Title + '%')
      AND (@Brand IS NULL OR Brand LIKE '%' + @Brand + '%');

    -- Return the results
    SELECT 
        p.Id,
        p.ProductTitle,
        p.Code,
        p.Brand,
        p.Price,
        p.Description,
        p.CreatedAt,
        p.UpdatedAt,
        p.IsDeleted
    FROM @Products p;
END
