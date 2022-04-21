using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeFriend.Models;
using TreeFriend.Models.Entity;
using TreeFriend.Models.ViewModel;
//後台管理
namespace TreeFriend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AddSystemMessageController : Controller
    {
        private readonly IWebHostEnvironment _environment; //注入抓取本機地端環境建構子
        private readonly TreeFriendDbContext _db; //注入資料庫建構子

        public AddSystemMessageController(IWebHostEnvironment environment, TreeFriendDbContext db)
        {
            _environment = environment;
            _db = db;
        }

        //#region 渲染後台畫面
        //    public List<SystemPostViewModel> GetAllMessage()                //查詢資料庫 撈資料後 去渲染畫面
        //                                                                    //List泛型 復數 的資料
        //    {
        //        var result = _db.SystemPost.OrderByDescending(x => x.ArticleID).Take(5)
        //        .Where(x => x.IsDelete == false).Select(x => new SystemPostViewModel()       // 先抓取資料 在改寫資料庫欄位 new一個新的Model
        //        {
        //            ArticleID = x.ArticleID,
        //            CreateDate = x.CreateDate.ToString("yyyy-MM-dd"),
        //            Title = x.Title,
        //            Description = x.Description,
        //            PicPath = "/SystemPicture/" + x.PicPath  //對應資料庫欄位

        //        }).ToList();

        //        return result;
        //    }
        //    #endregion

        //    #region 系統頁面新增
        //    [HttpPost]
        //    public bool CreateMessage(UploadFileMessageViewModel model) //前端送過來的資料 用view model來接
        //                                                                //建議自己在寫一個 不要參照資料庫的欄位
        //    {
        //        var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
        //        var path = _environment.WebRootPath + "/SystemPicture";

        //        //前端資料確定有送進來後 開始寫傳照片的事件
        //        var picture = model.PicPath.FirstOrDefault();
        //        if (picture != null)
        //        {
        //            var fileName = DateTime.Now.Ticks.ToString() + picture.FileName; //為了避免傳同一張照片 給他時間做區別

        //            using (var files = System.IO.File.Create($"{path}/{fileName}"))  //開啟檔案位置 {路徑}+{檔案名稱}
        //            {
        //                picture.CopyTo(files);  //圖片物件載完後 可以CopyTo 丟入另一個路徑地方
        //            }

        //            _db.SystemPost.Add(new SystemPost()
        //            {
        //                Title = model.Title,
        //                Description = model.Description,
        //                CreateDate = DateTime.Now,
        //                PicPath = fileName,
        //                UserId = model.UserId
        //            });
        //            _db.SaveChanges(); //存進資料庫
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    #endregion

        //    #region 系統頁面修改
        //    [HttpPost]
        //    public bool UpdateMessage([FromForm] UpdateMessageViewModel model)
        //    {
        //        var updateMessage = _db.SystemPost.FirstOrDefault(x => x.ArticleID == model.ArticleID);
        //        var path = _environment.WebRootPath + "/SystemPicture";

        //        string PicPath;

        //        if (model.PicPath == null)
        //        {
        //            PicPath = updateMessage.PicPath;
        //        }
        //        else
        //        {
        //            var fileName = DateTime.Now.Ticks.ToString("MMddHHmmss") + model.PicPath[0].FileName;
        //            using (var files = System.IO.File.Create($"{path}/{fileName}"))
        //            {
        //                model.PicPath[0].CopyTo(files);
        //                PicPath = fileName;
        //            }
        //        }


        //        if (updateMessage != null)
        //        {
        //            updateMessage.Title = model.Title;
        //            updateMessage.Description = model.Description;
        //            updateMessage.PicPath = PicPath;
        //            _db.SaveChanges();
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    #endregion

        //    #region 系統頁面軟刪除
        //    public bool DeleteMessage([FromQuery] int ArticleID)
        //    {
        //        try
        //        {
        //            var result = _db.SystemPost.Where(x => x.ArticleID == ArticleID).SingleOrDefault();
        //            Console.WriteLine(result);
        //            result.IsDelete = true;
        //            _db.SystemPost.Update(result);
        //            _db.SaveChanges();
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            return false;
        //        }
        //    }

        //    #endregion
        //}
    }
}
