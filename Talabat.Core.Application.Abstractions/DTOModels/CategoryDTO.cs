﻿namespace Talabat.Core.Application.Abstractions.DTOModels
{
    public class CategoryDTO
    {
        public string Id { get; set; } = null!;
        public required string Name { get; set; }
        public List<string> Products { get; set; } = new List<string>();
    }
}
