﻿namespace Talabat.APIs.Controllers.DTOModels
{
    public class Pagination<TEntity>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<TEntity> Data { get; set; } = new List<TEntity>();
    }
}
