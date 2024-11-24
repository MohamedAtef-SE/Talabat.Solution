using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Application.Abstractions.DTOModels.Basket
{
    public class BasketItemDTO
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required]
        public required string Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? PictureUrl { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage = "Quantity must be one item at least!!")]
        public int Quantity { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price can not be Zero!!")]
        public decimal Price { get; set; }
        [Required]
        public string? ProductBrandId { get; set; }
        [Required]
        public string? ProductCategoryId { get; set; }
    }
}
