using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NomadNavigator_BE_.Models;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly NNContext _context;

        public UsersController(NNContext context)
        {
            _context = context;
        }

        // POST: api/Users/Register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return BadRequest("Email already exists.");
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                Dob = model.Dob
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

           

            return Ok(new { message = "User registered successfully", isAuthenticated = true });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "The Email and Password fields are required." });
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.PasswordHash == HashPassword(model.Password));

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            return Ok(new { message = "Login successful." });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Since no session or token is used, simply return a successful response
            return Ok(new { message = "Logout successful." });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }
}
