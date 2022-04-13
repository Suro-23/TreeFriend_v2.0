using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.Entity
{
    public class OrderDetail
    {
        //訂單編號: 複合主鍵 
        //TODO藍新金流回傳欄位
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public decimal Price { get; set; }//單價
        [Required]
        public int Count { get; set; } //買家購買數量




        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }


        [Required]
        public int LectureId { get; set; }
        public virtual Lecture Lecture { get; set; }

    }
}
