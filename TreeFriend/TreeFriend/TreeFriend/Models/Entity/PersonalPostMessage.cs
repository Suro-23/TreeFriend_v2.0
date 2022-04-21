using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreeFriend.Models.Entity {
    public class PersonalPostMessage {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }

        [Required]
        //[ForeignKey("PersonalPost")]
        public int PersonalPostId { get; set; }

        [Required]
        //[ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public string Message { get; set; }
        
        public DateTime CreateDate { get; set; }


        //public virtual PersonalPost PersonalPost { get; set; }
        //public virtual User User { get; set; }
    }
}
