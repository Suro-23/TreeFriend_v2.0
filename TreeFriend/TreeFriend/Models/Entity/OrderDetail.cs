using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.Entity
{
    public class OrderDetail
    {
      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 商品單價
        /// </summary>
        [Required]
        public decimal Price { get; set; } 
        /// <summary>
        /// 購買數量
        /// </summary>
        [Required]
        public int Count { get; set; } 

        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 付款狀態 尚未付款0 付款成功1
        /// </summary>
        [Required]
        public bool PaymentStatus { get; set; } = false;

        /// <summary>
        /// 訂單狀態 訂單成立0 訂單取消1
        /// </summary>
        [Required]
        public bool OrderStatus { get; set; } = false;

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }


        [Required]
        public int LectureId { get; set; }
        public virtual Lecture Lecture { get; set; }

    }
}
