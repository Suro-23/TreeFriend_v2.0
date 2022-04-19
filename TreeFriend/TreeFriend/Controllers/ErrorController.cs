using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeFriend.Models;
using TreeFriend.Models.ViewModel;

namespace TreeFriend.Controllers
{
    public class ErrorController : Controller
    {
        private readonly TreeFriendDbContext _db;
        public ErrorController(TreeFriendDbContext db)
        {
            _db = db;
        }
        public IActionResult Quantity(string whatever)
        {
            ViewBag.message = whatever;
            return View();
        }

        public List<LecturelistViewModel> GetLecture()
        {
            var result = _db.Lectures.Where(x => x.IsDelete == false && x.Count > 0 && x.EventDate >= DateTime.Now).OrderBy(x=>x.LectureId).Take(4).Select(x => new LecturelistViewModel
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
    }
}
