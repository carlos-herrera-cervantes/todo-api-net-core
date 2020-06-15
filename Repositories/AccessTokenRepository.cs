using System.Threading.Tasks;
using MongoDB.Driver;
using TodoApiNet.Contexts;
using TodoApiNet.Models;

namespace TodoApiNet.Repositories
{
    public class AccessTokenRepository : IAccessTokenRepository
    {
        private readonly IMongoCollection<AccessToken> _context;

        public AccessTokenRepository(IMongoDBSettings context)
        {
            var client = new MongoClient(context.ConnectionString);
            var database = client.GetDatabase(context.Database);
            _context = database.GetCollection<AccessToken>("AccessToken");
        }

        /// <summary>
        /// GET
        /// </summary>

        #region snippet_GetOne

        public Task<AccessToken> GetOneAsync(string accessToken) => _context.Find(t => t.Token == accessToken).FirstOrDefaultAsync();

        #endregion

        /// <summary>
        /// POST
        /// </summary>

        #region snippet_Create

        public async Task CreateAsync(AccessToken accessToken) => await _context.InsertOneAsync(accessToken);

        #endregion
    }
}