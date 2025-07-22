using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceManagementSystem.Repository.Entities
{
    public class Product
    {
        public int ProductID { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int CategoryID { get; set; }
        public int? OrderID { get; set; }

        public Category Category { get; set; }
        public Order Order { get; set; }
    }
}
