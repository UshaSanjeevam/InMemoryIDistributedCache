using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryIDistributedCache;
using System.Data;
namespace InMemoryIDistributedCache.Services
{
    public interface IUserService
    {
        List<string> GetCacheData(int UserID);
      DataTable Authenticate(string username, string password);
    }
}
