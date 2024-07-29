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
    public class RestaurantsController : ControllerBase
    {
        private readonly NNContext _context;

        public RestaurantsController(NNContext context)
        {
            _context = context;
        }

        // GET: api/Restaurants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
           var restaurant= await _context.Restaurants.Include(p => p.Place).ToListAsync();

            var restaurantData = restaurant.Select(r => new
            {
                r.Id,
                r.RestaurantName,
                r.Rating,
                r.PlaceId,
                r.Speciality,
                r.Type,
                r.Price,
                ImageBase64 = r.Img != null ? Convert.ToBase64String(r.Img) : null,
                PlaceName = r.Place != null ? r.Place.Country : null
            }).ToList();
        return Ok(restaurantData);
        }

        //GET: api/Restaurants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int id)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.Place) 
                .FirstOrDefaultAsync(r => r.Id == id);

            if (restaurant == null)
            {
                return NotFound();
            }

            var restaurantData = new
            {
                restaurant.Id,
                restaurant.RestaurantName,
                restaurant.Rating,
                restaurant.PlaceId,
                restaurant.Speciality,
                restaurant.Type,
                restaurant.Price,
                ImageBase64 = restaurant.Img != null ? Convert.ToBase64String(restaurant.Img) : null,
                PlaceName = restaurant.Place != null ? restaurant.Place.Country : null 
            };

            return Ok(restaurantData);
        }
    }
}
