using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceManagementSystem.Repository.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public string Description { get; set; }


        public ICollection<Product> Products { get; set; }
    }
}
