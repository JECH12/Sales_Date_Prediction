using Entities.Models;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<GenericResponse<List<AllEmployees>>> GetAllEmployeesAsync();
    }
}
