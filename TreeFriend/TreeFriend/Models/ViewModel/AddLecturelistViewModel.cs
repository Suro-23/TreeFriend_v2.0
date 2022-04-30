using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.ViewModel
{
    public class AddLecturelistViewModel
    {
        public int LectureId { get; set; }
        public string CreateDate { get; set; }
        public string Theme { get; set; }
        public string EventDate { get; set; }
        public string EventTimeStart { get; set; }
        public string EventTimeEnd { get; set; }
        public string Venue { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string SpeakerImgPath { get; set; }
        public string Speaker { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public string ImgPath { get; set; }

        public string UpdateTime { get; set; }
        public string Status { get; set; }
    }
}
