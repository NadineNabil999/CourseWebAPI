using System.ComponentModel.DataAnnotations;
using CourseWebAPI.DTOs.ProductDTOs;

namespace CourseWebAPI.DTOs.CategoryDTOs
{
    public class CategoryDTO
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ProductDTO> Products { get; set; }
    }
}
