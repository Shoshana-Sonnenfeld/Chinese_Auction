using Microsoft.EntityFrameworkCore;
using Server.Dal.Interfaces;
using Server.Models;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Server.Dal
{
    public class UserDal : IUserDal
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserDal> _logger;

        public UserDal(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<UserDal> logger)
        {
            _context = context;
            this._httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            _logger.LogInformation($"Getting user by username: {username}");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                _logger.LogWarning($"User with username {username} not found");
            }
            return user;
        }

        public async Task AddUser(User user)
        {
            if (user == null)
            {
                _logger.LogWarning("Attempted to add null user");
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }
            _logger.LogInformation($"Adding user: {user.Username}");
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {user.Username} added successfully");
        }

        public async Task<User> GetUserFromToken()
        {
            var usernameFromToken = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
            _logger.LogInformation($"Getting user from token: {usernameFromToken}");
            if (string.IsNullOrEmpty(usernameFromToken))
            {
                _logger.LogWarning("Username not found in token");
                throw new UnauthorizedAccessException("Username not found in token.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == usernameFromToken);
            if (user == null)
            {
                _logger.LogWarning("User not found from token");
                throw new UnauthorizedAccessException("User not found.");
            }
            _logger.LogInformation($"User {user.Username} found from token");
            return user;
        }

        public async Task<bool> UsernameExist(string username)
        {
            _logger.LogInformation($"Checking if username exists: {username}");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var exists = user != null;
            _logger.LogInformation($"Username {username} exists: {exists}");
            return exists;
        }
    }
}
