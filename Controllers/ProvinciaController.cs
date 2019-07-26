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
    [Route("api/Pais/{PaisId}/[controller]")]
    [ApiController]
    public class ProvinciaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProvinciaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pais/{PaisId}/Provincia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Provincia>>> GetProvincias(int paisId)
        {
            return await _context.Provincias.Where(x => x.PaisId == paisId).ToListAsync();
        }

        // GET: api/Pais/{PaisId}/Provincia/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Provincia>> GetProvincia(int id)
        {
            var provincia = await _context.Provincias.FindAsync(id);

            if (provincia == null)
            {
                return NotFound();
            }

            return provincia;
        }

        // PUT: api/Pais/{PaisId}/Provincia/5
        /* Body json:
          {
                "id": 5,
                "nombre": "Magallanes_edit",
                "paisId": 1
           }
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvincia(int id, Provincia provincia, int paisId)
        {
            if (id != provincia.Id || paisId != provincia.PaisId)
            {
                return BadRequest();
            }

            _context.Entry(provincia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinciaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(provincia);
        }

        // POST: api/Pais/{PaisId}/Provincia
        /* Body json:
         {
               "nombre": "Magallanes_new",
         }
        */
        [HttpPost]
        public async Task<ActionResult<Provincia>> PostProvincia(Provincia provincia, int PaisId)
        {
            provincia.PaisId = PaisId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Provincias.Add(provincia);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetProvincia", new { id = provincia.Id }, provincia);
            return new CreatedAtRouteResult("GetProvincia", new { id = provincia.Id }, provincia);
        }

        // DELETE: api/Pais/{PaisId}/Provincia/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Provincia>> DeleteProvincia(int id, int paisId)
        {
            var provincia = await _context.Provincias.FindAsync(id);
            if (provincia == null || provincia.PaisId != paisId)
            {
                return NotFound();
            }

            _context.Provincias.Remove(provincia);
            await _context.SaveChangesAsync();

            return provincia;
        }

        private bool ProvinciaExists(int id)
        {
            return _context.Provincias.Any(e => e.Id == id);
        }
    }
}
