using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FurnitureStore.Shared
{
    public class ProductCategory
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set;}
    }
}
