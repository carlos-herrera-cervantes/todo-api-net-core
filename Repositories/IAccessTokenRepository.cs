using System.Threading.Tasks;
using TodoApiNet.Models;

namespace TodoApiNet.Repositories
{
    public interface IAccessTokenRepository
    {
        Task CreateAsync(AccessToken accessToken);

        Task<AccessToken> GetOneAsync(string accessToken);
    }
}