﻿using System;
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
            if(!celestialObjects.Any())
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
    }
}
