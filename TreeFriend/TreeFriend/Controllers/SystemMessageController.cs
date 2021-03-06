using Microsoft.AspNetCore.Authorization;
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


        #region 渲染系統專欄輪播圖
        [AllowAnonymous]
        [HttpGet]
        public List<SystemlistViewModel> GetSystemMessageCarouselPic()
        {
            var result = _db.SystemPost.OrderByDescending(x => x.ArticleID).Take(3)      // 限制只出現5筆渲染資料
             .Where(x => x.IsDelete == false).Select(x => new SystemlistViewModel()      // 先抓取資料 在改寫資料庫欄位 new一個新的Model
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

        #region 渲染系統文章頁面
        [AllowAnonymous]
        [HttpGet]
        public List<SystemlistViewModel> GetAllSystemMessage() 
        {
            var result = _db.SystemPost.OrderByDescending(x => x.ArticleID).Take(7)      // 限制只出現5筆渲染資料
             .Where(x => x.IsDelete == false).Select(x => new SystemlistViewModel()      // 先抓取資料 在改寫資料庫欄位 new一個新的Model
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

        #region 系統專欄文章詳細頁面
        [AllowAnonymous]
        [HttpGet]
        public SystemPost SystemDetailContent(int articleId)
        {
            var post = _db.SystemPost.FirstOrDefault(x => x.ArticleID == articleId);
            var systemPost = new SystemPost();
            systemPost.ArticleID = post.ArticleID;
            systemPost.Title = post.Title;
            systemPost.Description = post.Description;
            systemPost.PicPath = "/SystemPicture/" + post.PicPath;

            return systemPost;

        }
        #endregion
    }
}
