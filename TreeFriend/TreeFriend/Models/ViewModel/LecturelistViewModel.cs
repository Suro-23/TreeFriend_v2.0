using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.ViewModel
{
    public class LecturelistViewModel
    {
        public int LectureId { get; set; }
        public string Theme { get; set; }
        public string EventDate { get; set; }
        public string EventTimeStart { get; set; }
        public string EventTimeEnd { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImgPath { get; set; }
    }
}
