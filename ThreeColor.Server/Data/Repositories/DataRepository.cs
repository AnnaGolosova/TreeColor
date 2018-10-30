using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using TreeColor.Server.Models;
using TreeColor.Server.Abstract;
using ThreeColor.Data.Models;
using TreeColor.Models;

namespace TreeColor.Server.Data.Repositories
{
    public class DataRepository : IAsyncRepository<Tests>
    {
        private readonly TestContext _dataContext;

        /// <inheritdoc/>
        public DataRepository(TestContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <inheritdoc/>
        public async Task<Tests> AddAsync(Tests item)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<Tests> items)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tests> Get(Expression<Func<Tests, bool>> condition = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tests> GetAsync(params object[] id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Tests item)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRangeAsync(IEnumerable<Tests> items)
        {
            throw new NotImplementedException();
        }

        public Task<Tests> UpdateAsync(Tests item)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(IEnumerable<Tests> items)
        {
            throw new NotImplementedException();
        }
    }
}