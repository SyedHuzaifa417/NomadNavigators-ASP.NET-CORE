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
    public class PlacesController : ControllerBase
    {
        private readonly NNContext _context;

        public PlacesController(NNContext context)
        {
            _context = context;
        }

        // GET: api/Places
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces()
        {
            var places = await _context.Places
                .Include(p => p.PlaceGreatFors) // Include PlaceGreatFors
                .Include(p => p.Restaurants) // Include Restaurants
                .ToListAsync();

            // Convert images to base64 strings if needed
            var placesWithBase64 = places.Select(place => new
            {
                place.Id,
                place.Name,
                place.Country,
                place.Price,
                place.Popularity,
                place.Description,
                place.Latitude,
                place.Longitude,
                ImageBase64 = place.Img != null ? Convert.ToBase64String(place.Img) : null,
                PlaceGreatFors = place.PlaceGreatFors.Select(gf => new
                {
                    gf.GreatFor 
                }).ToList(),
                Restaurants = place.Restaurants.Select(r => new
                {
                    r.Id,
                    r.RestaurantName,
                    r.Rating,
                    r.Speciality,
                    r.Type,
                    r.Price,
                    ImageBase64 = r.Img != null ? Convert.ToBase64String(r.Img) : null
                }).ToList()
            }).ToList();

            return Ok(placesWithBase64);
        }

        // GET: api/Places/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Place>> GetPlace(int id)
        {
            var place = await _context.Places
                .Include(p => p.PlaceGreatFors) // Include PlaceGreatFors
                .Include(p => p.Restaurants) // Include Restaurants
                .FirstOrDefaultAsync(p => p.Id == id);

            if (place == null)
            {
                return NotFound();
            }

            // Convert image to base64 string if needed
            var placeWithBase64 = new
            {
                place.Id,
                place.Name,
                place.Country,
                place.Price,
                place.Popularity,
                place.Description,
                place.Latitude,
                place.Longitude,
                ImageBase64 = place.Img != null ? Convert.ToBase64String(place.Img) : null,
                PlaceGreatFors = place.PlaceGreatFors.Select(gf => new
                {
                    gf.GreatFor // Assuming PlaceGreatFor has a property `GreatFor`
                }).ToList(),
                Restaurants = place.Restaurants.Select(r => new
                {
                    r.Id,
                    r.RestaurantName,
                    r.Rating,
                    r.Speciality,
                    r.Type,
                    r.Price,
                    ImageBase64 = r.Img != null ? Convert.ToBase64String(r.Img) : null
                }).ToList()
            };

            return Ok(placeWithBase64);
        }
    }
}
