namespace ProductApp.API.Dtos
{
    public class ProductDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public long CategoryId { get; set; } // Foreign key reference to Category
        public long SubCategoryId { get; set; } // Foreign key reference to SubCategory (nullable)
    }

    public class ProductListDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

}
