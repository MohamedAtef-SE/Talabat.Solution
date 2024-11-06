using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Products;

namespace Talabat.APIs.Controllers.DTOModels
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal? Price { get; set; }
        public int? BrandId { get; set; } 
        public int? CategoryId { get; set; }
        public string? Brand { get; set; } = null!; 
        public string? Category { get; set; } = null!; 

    }
}
