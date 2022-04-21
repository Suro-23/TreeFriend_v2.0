using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.ViewModel
{
    public class UpdateLectureViewModel
    {
        public int LectureId { get; set; }
        public string Theme { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EventTimeStart { get; set; }
        public DateTime EventTimeEnd { get; set; }
        public string Venue { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string SpeakerImgPath { get; set; }
        public string Speaker { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public string ImgPath { get; set; }
        public List<IFormFile> Picture { get; set; }
        public List<IFormFile> SpeakerPicture { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
