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
    public class BookingsController : ControllerBase
    {
        private readonly NNContext _context;

        public BookingsController(NNContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            var bookings = await _context.Bookings
                .Include(p => p.Place)
                .Include(p => p.Restaurant)
                .Include(p => p.Flight)
                .ToListAsync();

            var bookMe = bookings.Select(booking => new
            {
                booking.Id,
                booking.FlightId,
                booking.PlaceId,
                booking.RestaurantId,
                Restaurant = booking.Restaurant != null ? new
                {
                    booking.Restaurant.Id,
                    booking.Restaurant.RestaurantName,
                    booking.Restaurant.Price
                } : null,
                Place = booking.Place != null ? new
                {
                    booking.Place.Id,
                    booking.Place.Name,
                    booking.Place.Price
                } : null,
                Flight = booking.Flight != null ? new
                {
                    booking.Flight.Id,
                    booking.Flight.From,
                    booking.Flight.To,
                    booking.Flight.Price
                } : null,
            }).ToList();

            return Ok(bookMe);
        }
   



        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookingDto>> PostBooking(BookingInputModel bookingInput)
        {
            var booking = new Booking
            {
                BookerName = bookingInput.BookerName,
                BookerEmail = bookingInput.BookerEmail,
                PlaceId = bookingInput.PlaceId,
                FlightId = bookingInput.FlightId,
                RestaurantId = bookingInput.RestaurantId,
                BookingDate = bookingInput.BookingDate ?? DateOnly.FromDateTime(DateTime.Now),
                BookingTime = bookingInput.BookingTime ?? TimeOnly.FromDateTime(DateTime.Now),
                NumberOfPersons = bookingInput.NumberOfPersons,
                TotalPrice = bookingInput.TotalPrice
            };

            // Validate required fields
            if (string.IsNullOrEmpty(booking.BookerName) || string.IsNullOrEmpty(booking.BookerEmail))
            {
                return BadRequest(new { message = "Booker name and email are required." });
            }

            // Validate foreign keys
            var place = await _context.Places.FindAsync(booking.PlaceId);
            if (place == null)
            {
                return BadRequest("Invalid Place ID.");
            }

            var flight = await _context.Flights.FindAsync(booking.FlightId);
            if (flight == null)
            {
                return BadRequest("Invalid Flight ID.");
            }

            Restaurant restaurant = null;
            if (booking.RestaurantId.HasValue)
            {
                restaurant = await _context.Restaurants.FindAsync(booking.RestaurantId);
                if (restaurant == null)
                {
                    return BadRequest("Invalid Restaurant ID.");
                }
            }

            // Calculate total price if not provided
            if (booking.TotalPrice == 0)
            {
                booking.TotalPrice = CalculateTotalPrice(booking, place, flight, restaurant);
            }

            _context.Bookings.Add(booking);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookingExists(booking.Id))
                {
                    return Conflict("A booking with this ID already exists.");
                }
                else
                {
                    throw;
                }
            }

            var bookingDto = new BookingDto
            {
                Id = booking.Id,
                BookerName = booking.BookerName,
                BookerEmail = booking.BookerEmail,
                PlaceId = booking.PlaceId,
                RestaurantId = booking.RestaurantId,
                FlightId = booking.FlightId,
                BookingDate = booking.BookingDate,
                BookingTime = booking.BookingTime,
                NumberOfPersons = booking.NumberOfPersons,
                TotalPrice = booking.TotalPrice,
                PlaceName = place.Name,
                FlightFrom = flight.From,
                FlightTo = flight.To,
                RestaurantName = restaurant?.RestaurantName
            };

            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, bookingDto);
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Place)
                .Include(b => b.Flight)
                .Include(b => b.Restaurant)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var bookingDto = new BookingDto
            {
                Id = booking.Id,
                BookerName = booking.BookerName,
                BookerEmail = booking.BookerEmail,
                PlaceId = booking.PlaceId,
                RestaurantId = booking.RestaurantId,
                FlightId = booking.FlightId,
                BookingDate = booking.BookingDate,
                BookingTime = booking.BookingTime,
                NumberOfPersons = booking.NumberOfPersons,
                TotalPrice = booking.TotalPrice,
                PlaceName = booking.Place?.Name,
                FlightFrom = booking.Flight?.From,
                FlightTo = booking.Flight?.To,
                RestaurantName = booking.Restaurant?.RestaurantName
            };

            return bookingDto;
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }

        private decimal CalculateTotalPrice(Booking booking, Place place, Flight flight, Restaurant restaurant)
        {
            decimal totalPrice = place.Price + flight.Price;

            if (restaurant != null)
            {
                totalPrice += (decimal)restaurant.Price.GetValueOrDefault();
            }

            totalPrice *= booking.NumberOfPersons;

            return totalPrice;
        }
    }

    public class BookingInputModel
    {
        public string BookerName { get; set; }
        public string BookerEmail { get; set; }
        public int PlaceId { get; set; }
        public int FlightId { get; set; }
        public int? RestaurantId { get; set; }
        public DateOnly? BookingDate { get; set; }
        public TimeOnly? BookingTime { get; set; }
        public int NumberOfPersons { get; set; }
        public decimal TotalPrice { get; set; }
    }
}