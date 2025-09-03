using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class StoredProcedureExecutor: IStoredProcedureExecutor
    {
        private readonly StoreSampleContext _context;

        public StoredProcedureExecutor(StoreSampleContext context)
        {
            _context = context;
        }

        public async Task<List<T>> ExecuteAsync<T>(string sql, params object[] parameters) where T : class
        {
            return await _context.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }
    }
}
