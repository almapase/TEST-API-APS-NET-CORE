using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaisesAPI.Models;

namespace PaisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Pais> Get()
        {
            return _context.Paises.ToList();
        }

        [HttpGet("{id}", Name = "Pais")]
        public IActionResult GetById(int id)
        {
            var pais = _context.Paises.Include(x => x.Provincias).FirstOrDefault(x => x.Id == id);

            if (pais == null)
            {
                return NotFound();
            }
            return Ok(pais);
        }

        [HttpPost]
        public IActionResult CreatePais([FromBody] Pais pais)
        {
            if (ModelState.IsValid)
            {
                _context.Paises.Add(pais);
                _context.SaveChanges();
                return new CreatedAtRouteResult("Pais", new { Id = pais.Id }, pais);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult EditPais([FromBody] Pais pais, int id)
        {
            if (pais.Id != id)
            {
                return BadRequest();
            }

            _context.Entry(pais).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(pais);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePais(int id)
        {
            var pais = _context.Paises.FirstOrDefault(x => x.Id == id);

            if (pais == null)
            {
                return NotFound();
            }

            _context.Paises.Remove(pais);
            _context.SaveChanges();
            return Ok(pais);
        }
    }
}