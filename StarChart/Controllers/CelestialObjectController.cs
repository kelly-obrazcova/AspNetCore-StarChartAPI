using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController()]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("int:id", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            if (id != _context.CelestialObjects.Id)
            {
                return NotFound();
            }

            else
            {
                _context.CelestialObjects.Satellite = CelestialObject.OrbitedObjectId;
                return Ok();
            }
        }
    }
}
