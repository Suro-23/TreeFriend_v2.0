using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TreeFriend.Models.Enum;

namespace TreeFriend.Models.Entity
{
    public class Lecture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LectureId { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public string Theme { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime EventDate { get; set; }
        [Required]
        public DateTime EventTimeStart { get; set; }
        [Required]
        public DateTime EventTimeEnd { get; set; }
        [Required]
        public string Venue { get; set; }
        [Required]
        public string SpeakerImgPath { get; set; }
        [Required]
        public string Speaker { get; set; }
        /// <summary>
        /// 商品庫存
        /// </summary>
        [Required]
        public int Count { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string ImgPath { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Content { get; set; }
        ///// <summary>
        ///// 刪除狀態 未刪除0 已刪除1
        ///// </summary>
        //[Required]
        //public bool IsDelete { get; set; } = false;

        public LectureStatus? Status { get; set; }
        public DateTime? UpdateTime { get; set; }


        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
