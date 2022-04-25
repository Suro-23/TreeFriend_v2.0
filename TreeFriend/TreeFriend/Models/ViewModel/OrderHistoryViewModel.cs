using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.ViewModel
{
    public class OrderHistoryViewModel
    {
        public int OrderDetailId { get; set; }
        public string CreateDate { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string PayTime { get; set; }
        public int TotoalAmount { get; set; }
        public bool PaymentStatus { get; set; } = false;
        public bool OrderStatus { get; set; } = true;
        public string Theme { get; set; }
        public string EventDate { get; set; }
        public string EventTimeStart { get; set; }
        public string EventTimeEnd { get; set; }
        public string Venue { get; set; }
        public string UserName { get; set; }
        public string ImgPath { get; set; }
        public int LectureId { get; set; }
    }
}
