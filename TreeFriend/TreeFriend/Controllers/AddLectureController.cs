using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TreeFriend.Models;
using TreeFriend.Models.Entity;
using TreeFriend.Models.ViewModel;

namespace TreeFriend.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AddLectureController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly TreeFriendDbContext _db;
        private readonly string _folder1;
        private readonly string _folder2;

        public AddLectureController(IWebHostEnvironment environment, TreeFriendDbContext db)
        {
            _environment = environment;
            _folder1 = $@"{environment.WebRootPath}\LecturePicture";
            _folder2 = $@"{environment.WebRootPath}\SpeakerPicture";
            _db = db;
        }



        #region 渲染
        public List<AddLecturelistViewModel> GetAllLecture()
        {
            var result = _db.Lectures.OrderByDescending(x => x.CreateDate).Select(x => new AddLecturelistViewModel
            {
                LectureId = x.LectureId,
                CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                Theme = x.Theme,
                EventDate = x.EventDate.ToString("yyyy-MM-dd"),
                EventTimeStart = x.EventTimeStart.ToString("HH:mm"), //HH24小時 hh12小時
                EventTimeEnd = x.EventTimeEnd.ToString("HH:mm"),
                Venue = x.Venue,
                SpeakerImgPath = x.SpeakerImgPath,
                Speaker = x.Speaker,
                Count = x.Count,
                Price = x.Price,
                Description = x.Description,
                Content = x.Content,
                ImgPath = x.ImgPath,
                UpdateTime = x.UpdateTime.HasValue ? x.UpdateTime.Value.ToString("yyyy-MM-dd") : "",
                Status = x.Status.HasValue ? x.Status.GetType().GetMember(x.Status.ToString()).First().GetCustomAttribute<DisplayAttribute>().GetName() : ""

            }).ToList();

            return result;
        }
        #endregion

        #region 講座修改

        [HttpPost]
        public bool UpdateLecture([FromForm] UpdateLectureViewModel model)
        {
            var updateLecture = _db.Lectures.Where(x => x.LectureId == model.LectureId).FirstOrDefault();
            string pic;
            string speakerpic;

            if (model.SpeakerPicture == null)
            {
                speakerpic = updateLecture.SpeakerImgPath;
            }
            else
            {
                var speakerPicFileName = DateTime.Now.ToString("MMddHHmmss") + model.SpeakerPicture[0].FileName;
                var speakerPicPath = $@"{_folder2}\{speakerPicFileName}";
                using (var stream = new FileStream(speakerPicPath, FileMode.Create))
                {
                    model.SpeakerPicture[0].CopyTo(stream);
                    speakerpic = $@"/SpeakerPicture/{speakerPicFileName}";
                }

            }

            if (model.Picture == null)
            {
                pic = updateLecture.ImgPath;
            }
            else
            {
                var fileName = DateTime.Now.ToString("MMddHHmmss") + model.Picture[0].FileName;
                var path = $@"{_folder1}\{fileName}";
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    model.Picture[0].CopyTo(stream);
                    pic = $@"/Lecturepicture/{fileName}";
                }

            }
            if (updateLecture != null)
            {
                updateLecture.Theme = model.Theme;
                updateLecture.EventDate = model.EventDate;
                updateLecture.EventTimeStart = model.EventTimeStart;
                updateLecture.EventTimeEnd = model.EventTimeEnd;
                updateLecture.SpeakerImgPath = speakerpic;
                updateLecture.Speaker = model.Speaker;
                updateLecture.Venue = model.Venue;
                updateLecture.Price = model.Price;
                updateLecture.Count = model.Count;
                updateLecture.Description = model.Description;
                updateLecture.Content = model.Content;
                updateLecture.ImgPath = pic;
                updateLecture.UpdateTime = DateTime.UtcNow.AddHours(8);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion


        #region 講座新增

        [HttpPost]
        public bool UploadFile(UploadFileViewModel model)
        {
            var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);


            string pic;
            string speakerpic;

            var speakerPicFileName = DateTime.Now.ToString("MMddHHmmss") + model.SpeakerPicture[0].FileName;
            var speakerPicPath = $@"{_folder2}\{speakerPicFileName}";
            using (var stream = new FileStream(speakerPicPath, FileMode.Create))
            {
                model.SpeakerPicture[0].CopyTo(stream);
                speakerpic = $@"/SpeakerPicture/{speakerPicFileName}";
            }

            var picFileName = DateTime.Now.ToString("MMddHHmmss") + model.Picture[0].FileName;
            var picPath = $@"{_folder1}\{picFileName}";
            using (var stream = new FileStream(picPath, FileMode.Create))
            {
                model.Picture[0].CopyTo(stream);
                pic = $@"/Lecturepicture/{picFileName}";
            }

            if (pic != null || speakerpic != null)
            {

                _db.Lectures.Add(new Lecture()
                {
                    CreateDate = DateTime.UtcNow.AddHours(8),
                    Theme = model.Theme,
                    EventDate = model.EventDate,
                    EventTimeStart = model.EventTimeStart,
                    EventTimeEnd = model.EventTimeEnd,
                    Venue = model.Venue,
                    Description = model.Description,
                    Content = model.Content,
                    Speaker = model.Speaker,
                    Count = model.Count,
                    Price = model.Price,
                    ImgPath = pic,
                    SpeakerImgPath = speakerpic,
                    UserId = UserId,
                    Status = Models.Enum.LectureStatus.Unapproved

                });

                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 講座上下架
        [HttpDelete]
        public bool IsNotSoldLecture([FromQuery] int lectureId)
        {
            try
            {
                var result = _db.Lectures.Where(x => x.LectureId == lectureId).SingleOrDefault();
                Console.WriteLine(result);
                result.Status = Models.Enum.LectureStatus.NotSold;
                result.UpdateTime = DateTime.UtcNow.AddHours(8);
                _db.Lectures.Update(result);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

      
        public bool IsLaunchedLecture([FromQuery] int lectureId)
        {
            try
            {
                var result = _db.Lectures.Where(x => x.LectureId == lectureId).SingleOrDefault();
                Console.WriteLine(result);
                result.Status = Models.Enum.LectureStatus.Launched;
                _db.Lectures.Update(result);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }

}
