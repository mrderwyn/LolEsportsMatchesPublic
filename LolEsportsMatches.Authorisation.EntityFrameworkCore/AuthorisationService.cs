using LolEsportsMatches.Authorisation.EntityFrameworkCore.Context;
using LolEsportsMatches.Authorisation.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace LolEsportsMatches.Authorisation.EntityFrameworkCore
{
    /// <summary>
    /// Simple authorization service on Entity Framework Core (<see cref="AuthContext"/> context).
    /// </summary>
    /// <seealso cref="LolEsportsMatches.Authorisation.IAuthorisationService" />
    public class AuthorisationService : IAuthorisationService
    {
        private readonly AuthContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorisationService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public AuthorisationService(AuthContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="role">The role.</param>
        /// <returns>
        /// true if the user has been ce=reated, otherwise - false.
        /// </returns>
        public async Task<bool> CreateUser(string email, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                return false;
            }

            UserEntity? entity = new()
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role,
            };

            try
            {
                await this.context.Users.AddAsync(entity);
                await this.context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified user is admin.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        /// <returns>
        /// true if password is correct and user is admin, otherwise - false.
        /// </returns>
        public async Task<bool> IsAdmin(string email, string password)
        {
            UserEntity? user = await this.context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, user.Password) && user.Role.Equals("admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}