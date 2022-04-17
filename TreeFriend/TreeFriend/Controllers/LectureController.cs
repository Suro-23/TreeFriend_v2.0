using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeFriend.Models;
using TreeFriend.Models.ViewModel;

namespace TreeFriend.Controllers
{
    public class LectureController : Controller
    {
        private readonly TreeFriendDbContext _db;


        public LectureController(TreeFriendDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public List<LecturelistViewModel> GetAllLecture()
        {
            //TODO 未過期日期篩選
            var result = _db.Lectures.Where(x => x.IsDelete == false && x.Count > 0 ).Select(x => new LecturelistViewModel
            {
                LectureId = x.LectureId,
                Theme = x.Theme,
                EventDate = x.EventDate.ToString("yyyy-MM-dd"),
                EventTimeStart = x.EventTimeStart.ToString("HH:mm"),
                EventTimeEnd = x.EventTimeEnd.ToString("HH:mm"),
                Price = x.Price,
                Description = x.Description,
                ImgPath = x.ImgPath
            }).ToList();

            return result;

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[controller]/[action]/{lecturedId}")]
        public List<LectureDetailViewModel> GetLectureDetails([FromRoute] int lecturedId)
        {
            var result = _db.Lectures.Where(x => x.LectureId == lecturedId).Select(x => new LectureDetailViewModel
            {
                LectureId = x.LectureId,
                Theme = x.Theme,
                EventDate = x.EventDate.ToString("yyyy-MM-dd"),
                EventTimeStart = x.EventTimeStart.ToString("HH:mm"),
                EventTimeEnd = x.EventTimeEnd.ToString("HH:mm"),
                Venue = x.Venue,
                SpeakerImgPath = x.SpeakerImgPath,
                Speaker = x.Speaker,
                Count = x.Count,
                Price = x.Price,
                Description = x.Description,
                Content = x.Content,
                ImgPath = x.ImgPath
            }
            ).ToList();

            return result;
        }
    }
}
