using System.ComponentModel.DataAnnotations;

namespace TreeFriend.Models.KallinAndYolan {
    public class UserLoginViewModel {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}
