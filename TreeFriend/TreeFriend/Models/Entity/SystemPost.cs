using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.Entity
{
    public class SystemPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleID { get; set; } //Guid 流水編號

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string PicPath { get; set; }

        [Required]
        public bool IsDelete { get; set; } = false;

        [Required]
        public int UserId { get; set; }

        //對應關聯到User表，User表那邊也要新增到這的關聯
        public virtual User User { get; set; }
    }
}
