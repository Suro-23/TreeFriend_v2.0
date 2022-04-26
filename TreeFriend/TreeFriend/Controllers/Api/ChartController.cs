using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TreeFriend.Models;
using TreeFriend.Models.ViewModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TreeFriend.Models.Entity;
using Org.BouncyCastle.Asn1.Tsp;

namespace TreeFriend.Controllers.Api {
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase {
        private readonly TreeFriendDbContext _db;

        //注入DbContext test
        public ChartController(TreeFriendDbContext db) {
            _db = db;
        }


        [HttpGet]
        [Route("GetTodayInfo")]
        public TodayInfoViewModel GetTodayInfo() {
            TodayInfoViewModel todayInfo = new();
            var today = DateTime.Parse(DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd"));

            //今日銷售額
            var todaySales = (from od in _db.OrderDetails
                             where od.CreateDate >= today && od.CreateDate < today.AddDays(1)
                             select od).Sum(od => od.Price);
            todayInfo.Sales = todaySales;

            //今日貼文數
            var todayPosts = (from p in _db.skillPosts
                              where p.CreateDate >= today && p.CreateDate < today.AddDays(1)
                              select p).Count();
            todayInfo.PostCount = todayPosts;

            //新進會員數
            var newMember = (from u in _db.users
                              where u.CreateDate >= today && u.CreateDate < today.AddDays(1)
                              select u).Count();
            todayInfo.MemberCount = newMember;

            //總分類貼文數量
            var postCountByCategory = from p in _db.skillPosts
                                      group p by p.Category.CategoryName into newGroup
                                      select new PostInfo{
                                          Category = newGroup.Key,
                                          Count = newGroup.Count()
                                      };
            foreach (var item in postCountByCategory) {
                todayInfo.CategoryPostList.Add(item);
            }

            //總標籤貼文數量
            var postCountByHashtag = from p in _db.hashtagDetails
                                      group p by p.Hashtag.HashtagName into newGroup
                                      select new PostInfo {
                                          Category = newGroup.Key,
                                          Count = newGroup.Count()
                                      };
            foreach (var item in postCountByHashtag) {
                todayInfo.HashtagPostList.Add(item);
            }

            return todayInfo;
        }
    }
}
