using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TreeFriend.Models;
using System.Linq;
using System;
using TreeFriend.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections.Generic;

namespace TreeFriend.Controllers.Api {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase {
        private readonly TreeFriendDbContext _db;

        //設定上船路徑為根目錄 wwwroot\UploadHeadshot
        private readonly string _folder;

        public MemberController(TreeFriendDbContext db, IHostingEnvironment env) {
            _db = db;
            _folder = $@"{env.WebRootPath}\UploadHeadshot";
        }

        //取得當前使用者基本資料
        [Route("GetUserInfo")]
        [HttpGet]
        public UserDetailViewModel GetUserInfo() {
            int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
            var memberInfo = _db.usersDetail.FirstOrDefault(u => u.UserId == userId);
            UserDetailViewModel userDetail = new UserDetailViewModel() {
                UserName = memberInfo.UserName,
                Birthday = memberInfo.Birthday.ToString("yyyy-MM-dd"),
                Sex = memberInfo.Sex == true ? "1" : "0",
                SelfIntrodution = memberInfo.SelfIntrodution,
                HeadshotPath = memberInfo.HeadshotPath
            };
            return userDetail;
        }

        //編輯基本資料
        [Route("UpdateUserInfo")]
        [HttpPut]
        public string UpdateUserInfo([FromBody] UserDetailViewModel userVM) {
            //獲取Cookies中的UserId後找尋該筆Entity資料
            int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
            var memberInfo = _db.usersDetail.FirstOrDefault(u => u.UserId == userId);

            //將字串轉成日期格式 輸入格式: YYYY-MM-DD
            var birthDay = DateTime.Parse(userVM.Birthday);

            //將UserDetailViewModel資料對應至Entity中
            //即更新資料，完成後儲存
            try {
                memberInfo.UserName = userVM.UserName;
                memberInfo.Birthday = birthDay;
                memberInfo.Sex = userVM.Sex == "1" ? true : false;
                memberInfo.HeadshotPath = userVM.HeadshotPath;
                memberInfo.SelfIntrodution = userVM.SelfIntrodution;
                _db.SaveChanges();
                return "更新成功";
            } catch (Exception) {
                return "更新失敗";
            }
        }

        //上傳圖片
        [Route("UploadFile")]
        [HttpPost]
        public string UploadFile(IFormFile file) {
            if (file != null) {
                string imgName = $@"User{ HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value}_{ file.FileName}";
                var path = $@"{_folder}\{imgName}";
                using (var stream = new FileStream(path, FileMode.Create)) {
                    file.CopyTo(stream);
                    return $@"/UploadHeadshot/{imgName}";
                }
            }
            return "未選擇圖片";
        }

        #region 歷史訂單

        [Route("GetOrder")]
        [HttpGet]
        public List<OrderHistoryViewModel> GetOrder() {
            int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
            var result = _db.OrderDetails.Where(od => od.UserId == userId && od.Lecture.EventDate>=DateTime.Now && od.OrderStatus==true).OrderByDescending(od => od.CreateDate).Select(od => new OrderHistoryViewModel
            {
                OrderDetailId=od.OrderDetailId,
                CreateDate=od.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                TotoalAmount = Convert.ToInt32(od.Price*od.Count),
                PayTime=od.PayTime.HasValue ? od.PayTime.Value.ToString("yyyy-MM-dd HH:mm"):"",
                PaymentStatus=od.PaymentStatus,
                OrderStatus=od.OrderStatus,
                Theme=od.Lecture.Theme,
                EventDate= od.Lecture.EventDate.ToString("yyyy-MM-dd"),
                EventTimeStart= od.Lecture.EventTimeStart.ToString("HH:mm"),
                EventTimeEnd= od.Lecture.EventTimeEnd.ToString("HH:mm"),
                Venue= od.Lecture.Venue,
                Price=od.Price,
                Count=od.Count,
                ImgPath=od.Lecture.ImgPath,
                LectureId=od.LectureId
            }).ToList();

            return result;

        }

        [Route("GetOrderHistory")]
        [HttpGet]
        public List<OrderHistoryViewModel> GetOrderHistory()
        {
            int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
            var result = _db.OrderDetails.Where(od => od.UserId == userId &&( od.Lecture.EventDate < DateTime.Now || od.OrderStatus == false)).OrderByDescending(od=>od.CreateDate).Select(od => new OrderHistoryViewModel
            {
                OrderDetailId = od.OrderDetailId,
                CreateDate = od.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                TotoalAmount = Convert.ToInt32(od.Price * od.Count),
                PayTime = od.PayTime.HasValue ? od.PayTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                PaymentStatus = od.PaymentStatus,
                OrderStatus = od.OrderStatus,
                Theme = od.Lecture.Theme,
                EventDate = od.Lecture.EventDate.ToString("yyyy-MM-dd"),
                EventTimeStart = od.Lecture.EventTimeStart.ToString("HH:mm"),
                EventTimeEnd = od.Lecture.EventTimeEnd.ToString("HH:mm"),
                Venue = od.Lecture.Venue,
                Price = od.Price,
                Count = od.Count,
                ImgPath = od.Lecture.ImgPath
            }).ToList();

            return result;

        }
        
        #endregion
    }
}
