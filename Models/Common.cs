using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using InMemoryIDistributedCache.EF;
using Microsoft.EntityFrameworkCore;
namespace InMemoryIDistributedCache.Models
{
    public class Common
    {
        public static readonly Common GetInstance;
        public readonly DataContext _datacontext;
        public Common(DataContext dataContext) //, IHttpClientFactory httpClientFactory
        {
            _datacontext = dataContext;
            // _httpClientFactory = httpClientFactory;
        }
        public DbConnection conString()
        { return ((System.Data.Common.DbConnection)_datacontext.Database.GetDbConnection()); }
    }
}
