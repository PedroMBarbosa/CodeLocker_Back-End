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
    public class StatusController : ControllerBase
    {
        private readonly DBCodeLockerContext _context;

        public StatusController(DBCodeLockerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Status>>> Get()
        {
            var status = await _context.Status.ToListAsync();
            return Ok(status);
        }
    }
}