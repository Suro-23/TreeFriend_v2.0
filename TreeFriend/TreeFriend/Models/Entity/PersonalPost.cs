using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreeFriend.Models.Entity
{
    public class PersonalPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonalPostId { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Content { get; set; }


        //使用getdate()
        public DateTime CreateDate { get; set; } = DateTime.UtcNow.AddHours(8);


        public virtual User User { get; set; }
        //public virtual ICollection<PersonalPostMessage> PersonalPostMessages { get; set; }
        public virtual ICollection<PersonalPostImage> PersonalPostImages { get; set; }


    }
}
