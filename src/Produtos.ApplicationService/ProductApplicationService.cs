using AutoMapper;
using MediatR;
using Produtos.Domain.Model;
using Produtos.Domain.Model.ApiContracts;
using Produtos.Domain.Model.Dtos.Filters;
using Produtos.Domain.Model.Interfaces;
using Produtos.Domain.Model.Interfaces.ApplicationServices;
using Produtos.Domain.Model.Interfaces.Repositories;
using Produtos.Domain.Model.ViewModels.Products;
using Produtos.Domain.Products.Delete;
using Produtos.Domain.Products.Edit;
using Produtos.Domain.Products.Register;

namespace Produtos.ApplicationService
{
    public class ProductApplicationService : BaseApplicationService, IProductApplicationService
    {
        private readonly IProductRepository _produtctRepository;
        private readonly IMediatorHandler _bus;
        private readonly IMapper _mapper;

        public ProductApplicationService(IProductRepository produtctRepository, IMediatorHandler bus, IMapper mapper, INotificationHandler<DomainNotification> notifications) : base(notifications)
        {
            _produtctRepository = produtctRepository;
            _bus = bus;
            _mapper = mapper;
        }
        
        public async Task<ServiceResult<ProductResponseViewModel>> GetById(int id)
        {
            var result = await _produtctRepository.GetById(id);
            var mappedResult = _mapper.Map<ProductResponseViewModel>(result);

            var serviceResult = ProcessServiceResult(mappedResult, $"The product with id: {id} was not found");

            return serviceResult;
        }

        public async Task<ServiceResult<PaginatedResult<List<PaginatedProductResponseViewModel>>>> GetByFilter(GetProductsByFilter getProductsByFilter)
        {
            var filter = _mapper.Map<ProductFilter>(getProductsByFilter);

            var result = await _produtctRepository.GetByFilter(filter);
            var paginatedResult = new PaginatedResult<List<PaginatedProductResponseViewModel>>(result.Data, getProductsByFilter.Page, result.CountData, getProductsByFilter.Size);

            return ServiceResult<PaginatedResult<List<PaginatedProductResponseViewModel>>>.Ok(paginatedResult, _notifications.GetNotifications());
        }

        public async Task<ServiceResult<int>> Register(RegisterProductViewModel registerProductViewModel)
        {
            var command = RegisterProductCommand.CreateByRegisterViewModel(registerProductViewModel);
            var result = await _bus.SendCommand<RegisterProductCommand, int>(command);

            return ServiceResult<int>.Created($"products/{result}", _notifications.GetNotifications());
        }

        public async Task<ServiceResult> Edit(int id, RegisterProductViewModel registerProductViewModel)
        {
            var command = EditProductCommand.CreateByRegisterViewModel(id, registerProductViewModel);
            await _bus.SendCommand(command);

            return ServiceResult.OkEmpty(_notifications.GetNotifications());
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var command = new DeleteProductCommand(id);
            await _bus.SendCommand(command);

            return ServiceResult.OkEmpty(_notifications.GetNotifications());
        }
    }
}
