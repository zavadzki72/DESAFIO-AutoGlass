using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Produtos.Domain.Model.Attributes;

namespace Produtos.Domain.Model.ApiContracts
{
    public abstract class PaginatedRequest<TResponse> 
        where TResponse : PaginatedResult
    {
        [BindProperty(Name = "_page", SupportsGet = true)]
        public int Page { get; set; } = 0;

        [BindProperty(Name = "_size", SupportsGet = true)]
        public int Size { get; set; } = 10;

        /// <summary>
        /// Ordenação da pagina (Pode ser mais de um, separado por virgula, o padrão é ASC).
        /// Exemplo: Id asc, Ano desc
        /// </summary>
        [BindProperty(Name = "_order", SupportsGet = true)]
        [Order("The Field _order is in invalid Format", "The Field {0} is Invalid to sort")]
        public string Order { get; set; }

        [BindNever]
        public Dictionary<string, string> FieldOrders { get; set; } = new();
    }
}
