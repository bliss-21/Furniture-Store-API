using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FurnitureStore.Shared
{
    public class Product
    {

        public int Id { get; set; }
        
        [Required]
        [StringLength(40)]
        [MinLength(3, ErrorMessage = "Name field must be at least 3 characters long")]
        public string Name { get; set; }
        
        [Required]
        [Range(0.001, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Price { get; set; }
        
        [Required]
        public int ProductCategoryId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        
        public List<OrderItem>? OrderDetails { get; set; }
        public ProductCategory? ProductCategory { get; set; }
    }
}
