using GymSubscriptionApi.Data;
using GymSubscriptionApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymSubscriptionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Secure the entire controller
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        /// <param name="id">The user ID</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// Creates a new user and their subscription.
        /// </summary>
        /// <param name="userCreateDto">The user creation DTO</param>
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserCreateDto userCreateDto)
        {
            var subscriptionType = await _context.SubscriptionTypes.FindAsync(userCreateDto.SubscriptionTypeId);
            if (subscriptionType == null)
            {
                return BadRequest("Invalid Subscription Type ID");
            }

            var service = await _context.Services.FindAsync(userCreateDto.ServiceId);
            if (service == null)
            {
                return BadRequest("Invalid Service ID");
            }

            var user = new User
            {
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                Email = userCreateDto.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var subscription = new Subscription
            {
                UserId = user.Id,
                SubscriptionTypeId = userCreateDto.SubscriptionTypeId,
                ServiceId = userCreateDto.ServiceId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(subscriptionType.DurationInDays)
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <param name="user">The updated user object</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The user ID</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
