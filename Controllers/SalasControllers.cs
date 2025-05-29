using Api.Models;
using Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalasController : ControllerBase
    {
        private readonly DBCodeLockerContext _context;

        public SalasController(DBCodeLockerContext context)
        {
            _context = context;
        }

        [HttpGet] // Trazer todas as salas
        public async Task<ActionResult<IEnumerable<Salas>>> Get()
        {
            var salas = await _context.Salas.ToListAsync();
            return Ok(salas);
        }
    }
}
