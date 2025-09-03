using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStoredProcedureExecutor
    {
        Task<List<T>> ExecuteAsync<T>(string sql, params object[] parameters) where T : class;
    }
}
