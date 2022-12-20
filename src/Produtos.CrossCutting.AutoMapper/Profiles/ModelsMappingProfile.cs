using AutoMapper;
using Produtos.Domain.Model.Dtos.Filters;
using Produtos.Domain.Model.Entities;
using Produtos.Domain.Model.ViewModels.Products;
using Produtos.Domain.Model.ViewModels.Suppliers;

namespace Produtos.CrossCutting.AutoMapper.Profiles
{
    public class ModelsMappingProfile : Profile
    {
        public ModelsMappingProfile()
        {
            //SUPPLIERS
            CreateMap<Supplier, SupplierResponseViewModel>();

            //PRODUCTS
            CreateMap<Product, ProductResponseViewModel>();
            CreateMap<GetProductsByFilter, ProductFilter>();
        }
    }
}
