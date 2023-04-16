using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace API.FurnitureStore.Shared
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Client? Client { get; set; }
    }
}
