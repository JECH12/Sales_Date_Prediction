using Entities.Models;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IShipperService
    {
        Task<GenericResponse<List<AllShippers>>> GetAllShippersAsync();
    }
}
