﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeFriend.Models;
using TreeFriend.Models.Entity;
using TreeFriend.Models.ViewModel;
//渲染系統文章
namespace TreeFriend.Controllers
{
    public class SystemMessageController : Controller
    {
        private readonly TreeFriendDbContext _db;  //注入資料庫建構子

        public SystemMessageController(TreeFriendDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        [HttpGet]
        public List<SystemlistViewModel> GetAllSystemMessage() //渲染系統文章頁面
        {
            var result = _db.SystemPost.OrderByDescending(x => x.ArticleID).Take(5)
             .Where(x => x.IsDelete == false).Select(x => new SystemlistViewModel()         // 先抓取資料 在改寫資料庫欄位 new一個新的Model
             {
                 ArticleID = x.ArticleID,
                 CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                 Title = x.Title,
                 Description = x.Description,
                 PicPath = "/SystemPicture/" + x.PicPath  //對應資料庫欄位

             }).ToList();

            return result;

        }


        #region 首頁系統專欄文章渲染
        [AllowAnonymous]
        [HttpGet]
        public List<SystemlistViewModel> GetAllHomeSystemMessage() 
        {
            var result = _db.SystemPost.OrderByDescending(x => x.ArticleID).Take(4)
             .Where(x => x.IsDelete == false).Select(x => new SystemlistViewModel()         // 先抓取資料 在改寫資料庫欄位 new一個新的Model
             {
                 ArticleID = x.ArticleID,
                 CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
                 Title = x.Title,
                 Description = x.Description,
                 PicPath = "/SystemPicture/" + x.PicPath  //對應資料庫欄位

             }).ToList();

            return result;

        }
        #endregion

        [AllowAnonymous]
        //詳細頁面
        [HttpGet]
        //[Route("[controller]/[action]/{articleId}")]
        public SystemPost FullContent1(int articleId)
        {
            var post = _db.SystemPost.FirstOrDefault(x => x.ArticleID == articleId);
            var systemPost = new SystemPost();
            systemPost.ArticleID = post.ArticleID;
            systemPost.Title = post.Title;
            systemPost.Description = post.Description;
            systemPost.PicPath = "/SystemPicture/" + post.PicPath;

            return systemPost;


            //var result = _db.SystemPost.Where(x => x.ArticleID == articleId).Select(x => new SystemDetailViewModel
            //{
            //    ArticleID = x.ArticleID,
            //    Title = x.Title,
            //    Description = x.Description,
            //    PicPath = "/SystemPicture/" + x.PicPath
            //}).ToList();


            //return result;
        }
    }
}