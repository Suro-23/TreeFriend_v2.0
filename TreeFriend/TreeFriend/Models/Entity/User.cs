using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace TreeFriend.Models.Entity {
    public class User {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public byte[] Salt { get; set; }

        /// <summary>
        /// 是否為啟用狀態? true = 啟用，false = 停用
        /// </summary>
        [Required]
        public bool UserStatus { get; set; } = false;

        /// <summary>
        /// 是否為管理者? true = 管理者，false = 一般會員
        /// </summary>
        [Required]
        public bool UserLevel { get; set; } = false;
        
        //設定GetDate()，自動取的時間
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public bool Status { get; set; } = true;

        public virtual UserDetail UserDetail { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Hashtag> Hashtags { get; set; }
        public virtual ICollection<PersonalPost> Posts { get; set; }

        //public virtual ICollection<PersonalPostMessage> PostsMessages { get; set; }

        public virtual ICollection<SkillPost> SkillPosts { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
