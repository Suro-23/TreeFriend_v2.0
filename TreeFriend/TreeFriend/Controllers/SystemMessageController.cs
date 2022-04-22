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

        [AllowAnonymous]
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


        //詳細頁面
        [HttpGet]
        public SystemPost FullContent1(int articleId)
        {
            var post = _db.SystemPost.FirstOrDefault(x => x.ArticleID == articleId);
            var systemPost = new SystemPost();
            systemPost.ArticleID = post.ArticleID;
            systemPost.Title = post.Title;
            systemPost.Description = post.Description;
            systemPost.PicPath = "/SystemPicture/" + post.PicPath;

            return systemPost;
        }
    }
}
