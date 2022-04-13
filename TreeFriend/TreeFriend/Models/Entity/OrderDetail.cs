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
        //訂單編號: 複合主鍵 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int OrderDetailId { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public decimal Price { get; set; } //單價
        [Required]
        public int Count { get; set; } //買家購買數量
        
        public DateTime? PayTime { get; set; }

        [Required]
        public bool PaymentStatus { get; set; } = false;


        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }


        [Required]
        public int LectureId { get; set; }
        public virtual Lecture Lecture { get; set; }

    }
}
