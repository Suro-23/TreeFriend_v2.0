using System.ComponentModel.DataAnnotations;

namespace TreeFriend.Models.KallinAndYolan {
    public class SameEmail {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
        [Required]
        [MaxLength(10)]
        public string Phone { get; set; }
    }
}
