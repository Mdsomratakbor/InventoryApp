namespace ProductApp.API.Dtos
{


    public class SubCategoryDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; } // Foreign key reference to Category
    }


}
