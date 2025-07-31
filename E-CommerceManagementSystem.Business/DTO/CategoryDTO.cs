using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceManagementSystem.Business.DTO
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string? Description { get; set; }
    }
}
