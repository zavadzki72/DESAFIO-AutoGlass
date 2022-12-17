﻿using Produtos.Domain.Model.ApiContracts;

namespace Produtos.Domain.Model.ViewModels.Products
{
    public class GetProductsByFilter : PaginatedRequest<PaginatedResult<List<PaginatedProductResponseViewModel>>>
    {
        public List<int> Ids { get; set; } = new();
        public List<string> Descriptions { get; set; } = new();
        public DateTime? MinManufactureDate { get; set; }
        public DateTime? MaxManufactureDate { get; set; }
        public DateTime? MinValidDate { get; set; }
        public DateTime? MaxValidDate { get; set; }
        public List<int> SupplierIds { get; set; } = new();
        public List<string> SupplierDescriptions { get; set; } = new();
        public List<string> SupplierCnpjs { get; set; } = new();
    }
}
