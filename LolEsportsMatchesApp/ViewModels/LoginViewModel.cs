using System.ComponentModel.DataAnnotations;

namespace LolEsportsMatchesApp.ViewModels
{
    public class LoginViewModel
    {
        private string url = "/";

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public string? ReturnUrl
        {
            get => url;
            set
            {
                url = string.IsNullOrWhiteSpace(value)
                    ? "/"
                    : value;
            }
        }
    }
}
