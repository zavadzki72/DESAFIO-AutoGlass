using AutoMapper;
using MediatR;
using Produtos.Domain.Model;
using Produtos.Domain.Model.Interfaces;
using Produtos.Domain.Model.Interfaces.ApplicationServices;
using Produtos.Domain.Model.Interfaces.Repositories;
using Produtos.Domain.Model.ViewModels.Products;

namespace Produtos.ApplicationService
{
    public class ProductApplicationService : BaseApplicationService, IProductApplicationService
    {
        private readonly IProdutctRepository _produtctRepository;
        private readonly IMediatorHandler _bus;
        private readonly IMapper _mapper;

        public ProductApplicationService(IProdutctRepository produtctRepository, IMediatorHandler bus, IMapper mapper, INotificationHandler<DomainNotification> notifications) : base(notifications)
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
        public Task<ServiceResult<List<PaginatedProductResponseViewModel>>> GetByFilter(GetProductsByFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<int>> Register(RegisterProductViewModel registerProductViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> Edit(int id, RegisterProductViewModel registerProductViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
