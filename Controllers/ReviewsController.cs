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
    public class ReviewsController : ControllerBase
    {
        private readonly NNContext _context;

        public ReviewsController(NNContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            var reviews = await _context.Reviews.ToListAsync();

            var reviewDtos = reviews.Select(review => new Review
            {
                Id = review.Id,
                Name = review.Name,
                Rating = review.Rating,
                Message = review.Message,
                Date = review.Date,
                ImageBase64 = review.Image != null ? Convert.ToBase64String(review.Image) : null
            }).ToList();

            return Ok(reviewDtos);
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            var reviewDto = new Review
            {
                Id = review.Id,
                Name = review.Name,
                Rating = review.Rating,
                Message = review.Message,
                Date = review.Date,
                ImageBase64 = review.Image != null ? Convert.ToBase64String(review.Image) : null
            };

            return reviewDto;
        }

  

        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            try
            {

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetReview", new { id = review.Id }, review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    
    }
}
