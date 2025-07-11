namespace CourseWebAPI.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime CreateDate {  get; set; } = DateTime.Now;
        public DateTime LastUpdateDate { get; set; } = DateTime.Now;

        public bool? DeleteFlag { get; set; } = false;

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
