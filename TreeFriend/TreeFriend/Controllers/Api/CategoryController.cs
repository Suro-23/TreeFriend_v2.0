using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using TreeFriend.Models.Entity;
using TreeFriend.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace TreeFriend.Controllers.Api {
    [Authorize(Roles = "Admin,Member")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase {
        private readonly TreeFriendDbContext _db;

        //注入DbContext
        public CategoryController(TreeFriendDbContext db) {
            _db = db;
        }

        //新增使用者
        [AllowAnonymous]
        [Route("CreateUser")]
        [HttpPost]
        public string CreateUser([FromBody] User user) {
            var AddUser = user;
            try {
                _db.users.Add(AddUser);
                _db.SaveChanges();
                return "新增成功";
            } catch (System.Exception) {
                return "新增失敗";
            }
        }

        #region 類別功能

        //檢查是否有重複的類別
        [Route("CheckCategory")]
        public bool CheckCategory(string categoryName) {
            //檢查是否有相同類別存在
            var result = _db.categories.Where(c => c.CategoryName == categoryName).FirstOrDefault();
            if (result != null) {
                return true;
            }
            return false;
        }

        //新增類別
        [Authorize(Roles ="Admin")]
        [Route("AddCategory")]
        [HttpPost]
        public bool AddCategory([FromBody] Category category) {
            //取的當前使用者ID
            var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
            category.UserId = UserId;
            //寫入資料庫
            var AddCategory = category;
            try {
                _db.categories.Add(AddCategory);
                _db.SaveChanges();
                return true;
            } catch (Exception) {
                return false;
            }
        }

        //刪除類別
        [Authorize(Roles = "Admin")]
        [Route("DeleteCategory")]
        [HttpDelete]
        public bool DeleteCategory(int categoryId) {
            try {
                var result = _db.categories.Find(categoryId);
                _db.categories.Remove(result);
                _db.SaveChanges();

                return true;
            } catch (Exception) {
                return false;
            }
        }

        //列出現有類別
        [Route("GetAllCategory")]
        [HttpGet]
        public List<Category> GetAllCategory() {
            var result = _db.categories.ToList();
            return result;
        }
        #endregion

        #region 標籤功能

        //檢查是否有重複的類別
        [Route("CheckHashTag")]
        public bool CheckHashtag(string hashtagName) {
            //檢查是否有相同類別存在
            var result = _db.hashtags.Where(c => c.HashtagName == hashtagName).FirstOrDefault();
            if (result != null) {
                return true;
            }
            return false;
        }

        //新增標籤
        [Authorize(Roles = "Admin")]
        [Route("AddHashTag")]
        [HttpPost]
        public bool AddTag([FromBody] Hashtag hashtag) {
            //取的當前使用者ID
            var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
            hashtag.UserId = UserId;
            //寫入資料庫
            var AddCategory = hashtag;
            try {
                _db.hashtags.Add(hashtag);
                _db.SaveChanges();
                return true;
            } catch (Exception) {
                return false;
            }
        }

        //刪除類別
        [Authorize(Roles = "Admin")]
        [Route("DeleteHashtag")]
        [HttpDelete]
        public bool DeleteHashtag(int hashtagId) {
            try {
                var result = _db.hashtags.Find(hashtagId);
                _db.hashtags.Remove(result);
                _db.SaveChanges();

                return true;
            } catch (Exception) {
                return false;
            }
        }

        //列出現有類別
        [Route("GetAllHashtag")]
        [HttpGet]
        public List<Hashtag> GetAllHashtag() {
            var result = _db.hashtags.ToList();
            return result;
        }
        #endregion
    }
}
