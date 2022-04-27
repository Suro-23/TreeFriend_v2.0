using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TreeFriend.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TreeFriend.Controllers.Api {
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ManageUser : ControllerBase {
        private readonly TreeFriendDbContext _db;

        public ManageUser(TreeFriendDbContext db) {
            _db = db;
        }

        [Route("GetAllUserInfo")]
        [HttpGet]
        public async Task<List<ManageUserViewModel>> GetAllUserInfo() {
            var userData = await _db.users.Select(u => new  ManageUserViewModel{
                Id = u.UserId,
                Email = u.Email,
                Name = u.UserDetail.UserName,
                Level = u.UserLevel == true ? "Admin" : "Member",
                Status = u.UserStatus,
                Headshot = u.UserDetail.HeadshotPath,
                Sex = u.UserDetail.Sex == true ? "男" : "女",
                Birthday = u.UserDetail.Birthday,
                SelfIntro = u.UserDetail.SelfIntrodution,
                PostCount = _db.skillPosts.Where(p => p.UserId == u.UserId).Count() +
                            _db.personalPosts.Where(p => p.UserId == u.UserId).Count(),
                TotalAmount = _db.OrderDetails.Where(od => od.UserId == u.UserId && od.PaymentStatus == true)
                            .Sum(od => od.Price * od.Count),
                TotalTime = (DateTime.UtcNow.AddHours(8) - u.UserDetail.UpdateTime).Days.ToString()
            }).ToListAsync();
            return userData;
        }
    }
}
