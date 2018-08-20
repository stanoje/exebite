using System.Threading.Tasks;
using Exebite.Common;

namespace WebClient.Services.Core
{
    public interface IRestService<TCreate, TQuery, TUpdate, TResult>
    {
        Task<int> CreateAsync(TCreate model, string authToke);

        Task<PagingResult<TResult>> QueryAsync(TQuery query, string authToke);

        Task DeleteByIdAsync(int id, string authToke);

        Task<bool> UpdateAsync(int id, TUpdate model, string authToke);
    }
}