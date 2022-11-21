namespace LolEsportsMatches.Authorisation
{
    /// <summary>
    /// Simple authorization service.
    /// </summary>
    public interface IAuthorisationService
    {
        /// <summary>
        /// Determines whether the specified user is admin.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        /// <returns>true if password is correct and user is admin, otherwise - false.</returns>
        Task<bool> IsAdmin(string email, string password);

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="role">The role.</param>
        /// <returns>true if the user has been ce=reated, otherwise - false.</returns>
        Task<bool> CreateUser(string email, string password, string role);
    }
}
