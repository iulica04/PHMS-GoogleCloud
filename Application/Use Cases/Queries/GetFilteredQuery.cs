using Application.Utils;
using Domain.Common;
using MediatR;
using System.Linq.Expressions;

namespace Application.Queries
{
    public class GetFilteredQuery<T, TDto> : IRequest<Result<PagedResult<TDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public Expression<Func<T, bool>>? Filter { get; set; }
    }
}
