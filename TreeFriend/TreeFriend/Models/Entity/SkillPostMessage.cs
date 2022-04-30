using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreeFriend.Models.Entity {
    public class SkillPostMessage {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SkillPostMessageId { get; set; }

        [Required]
        public int SkillPostId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Content { get; set; }
        [Required]
        public bool MsgIsPrivate { get; set; } = false;

        [Required]
        public bool Status { get; set; } = true;
    }
}
