using AutoMapper;
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
            CreateMap<Supplier, SupplierResponseViewModel>()
                .IgnoreAllUnmapped()
                .ReverseMap();

            CreateMap<Supplier, RegisterSupplierViewModel>()
                .IgnoreAllUnmapped()
                .ReverseMap();

            //PRODUCTS
            CreateMap<Product, ProductResponseViewModel>()
                .ForMember(x => x.Supplier, y => y.MapFrom(x => x.Supplier))
                .IgnoreAllUnmapped()
                .ReverseMap();

            CreateMap<Product, RegisterProductViewModel>()
                .IgnoreAllUnmapped()
                .ReverseMap();
        }
    }
}
