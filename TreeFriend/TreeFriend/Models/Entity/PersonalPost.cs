using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreeFriend.Models.Entity {
    public class PersonalPost {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonalPostId { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Subtitle { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string PostPhotoPath { get; set; }

        //使用getdate()
        public DateTime CreateDate { get; set; }


        public virtual User User { get; set; }
        //public virtual ICollection<PersonalPostMessage> PersonalPostMessages { get; set; }


    }
}
