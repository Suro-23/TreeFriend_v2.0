using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreeFriend.Models.Entity {
    public class SkillPost {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SkillPostId { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [ForeignKey("Category")]
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        //是否為啟用狀態，flase為刪除貼文(軟刪除)
        [Required]
        public bool Status { get; set; } = true;

        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<HashtagDetail> Hashtags { get; set; }
        //public virtual ICollection<Hashtag> Hashtags { get; set; }
    }
}
