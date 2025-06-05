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
    public class TiposController : ControllerBase
    {
        private readonly DBCodeLockerContext _context;

        public TiposController(DBCodeLockerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tipo>>> Get()
        {
            var tipos = await _context.Tipos.ToListAsync();
            return Ok(tipos);
        }
    }
}