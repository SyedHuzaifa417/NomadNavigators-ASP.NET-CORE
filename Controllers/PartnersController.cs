using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NomadNavigator_BE_.Models;

namespace NomadNavigator_BE_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnersController : ControllerBase
    {
        private readonly NNContext _context;

        public PartnersController(NNContext context)
        {
            _context = context;
        }

        // POST: api/Partners
        [HttpPost]
        public async Task<IActionResult> PostPartner(Partner model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }

            if (_context.Partners.Any(p => p.Email == model.Email))
            {
                return BadRequest("Email already exists.");
            }

            var partner = new Partner
            {
                Name = model.Name,
                Email = model.Email,
                Contact = model.Contact,
                AreaOfInterest = model.AreaOfInterest,
                Country = model.Country
            };

            _context.Partners.Add(partner);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Partner registered successfully" });
        }

        // DELETE: api/Partners/5

        private bool PartnerExists(int id)
        {
            return _context.Partners.Any(e => e.PartnerId == id);
        }
    }
}
