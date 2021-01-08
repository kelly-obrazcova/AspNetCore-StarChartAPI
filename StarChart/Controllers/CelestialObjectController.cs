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

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            CelestialObject celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            else
            {
                List<CelestialObject> satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id).ToList();
                celestialObject.Satellites = satellites;
                return Ok(celestialObject);
            }
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where(c => c.Name == name).ToList();
            if (!celestialObjects.Any())
            {
                return NotFound();
            }

            else
            {
                foreach (CelestialObject celestialObject in celestialObjects)
                {
                    List<CelestialObject> satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
                    celestialObject.Satellites = satellites;
                }
                return Ok(celestialObjects);
            }
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.ToList();
            foreach (CelestialObject celestialObject in celestialObjects)
            {
                List<CelestialObject> satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
                celestialObject.Satellites = satellites;
            }
            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject inputCelestialObject)
        {
            CelestialObject selectedObject = _context.CelestialObjects.Find(id);

            if (selectedObject == null)
            {
                return NotFound();
            }
            else
            {
                selectedObject.Name = inputCelestialObject.Name;
                selectedObject.OrbitalPeriod = inputCelestialObject.OrbitalPeriod;
                selectedObject.OrbitedObjectId = inputCelestialObject.OrbitedObjectId;

                _context.Update(selectedObject);
                _context.SaveChanges();

                return NoContent();
            }
        }


        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            CelestialObject selectedObject = _context.CelestialObjects.Find(id);
            if(selectedObject == null)
            {
                return NotFound();
            }
            else
            {
                selectedObject.Name = name;

                _context.Update(selectedObject);
                _context.SaveChanges();

                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            List<CelestialObject> selectedObjects = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id).ToList();

            if(selectedObjects.Count == 0)
            {
                return NotFound();
            }
            else
            {
                _context.RemoveRange(selectedObjects);
                _context.SaveChanges();

                return NoContent();
            }
        }
    }
}
