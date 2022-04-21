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
        public DateTime CreateDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PicPath { get; set; }
        public bool IsDelete { get; set; } = false;
        public string UserId { get; set; }
    }
}
