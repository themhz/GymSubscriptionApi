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
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptions()
        {
            return await _context.Subscriptions
                .Include(s => s.User)
                .Include(s => s.SubscriptionType)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetSubscription(int id)
        {
            var subscription = await _context.Subscriptions
                .Include(s => s.User)
                .Include(s => s.SubscriptionType)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

        [HttpPost]
        public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
        {
            var subscriptionType = await _context.SubscriptionTypes.FindAsync(subscription.SubscriptionTypeId);
            if (subscriptionType == null)
            {
                return BadRequest("Invalid Subscription Type");
            }

            subscription.StartDate = DateTime.UtcNow;
            subscription.EndDate = subscription.StartDate.AddDays(subscriptionType.DurationInDays);

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscription", new { id = subscription.Id }, subscription);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscription(int id, Subscription subscription)
        {
            if (id != subscription.Id)
            {
                return BadRequest();
            }

            var subscriptionType = await _context.SubscriptionTypes.FindAsync(subscription.SubscriptionTypeId);
            if (subscriptionType == null)
            {
                return BadRequest("Invalid Subscription Type");
            }

            _context.Entry(subscription).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("check-payments")]
        public async Task<ActionResult<IEnumerable<User>>> CheckPayments()
        {
            var today = DateTime.UtcNow;
            var usersWithPaymentsDue = await _context.Subscriptions
                .Where(s => s.EndDate <= today)
                .Include(s => s.User)
                .Select(s => s.User)
                .Distinct()
                .ToListAsync();

            return usersWithPaymentsDue;
        }
    }
}
