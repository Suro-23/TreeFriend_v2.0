using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TreeFriend.Models;
using TreeFriend.Models.Entity;
using TreeFriend.Models.ViewModel;

namespace TreeFriend.Controllers.Api {
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase {
        private readonly TreeFriendDbContext _db;

        //注入DbContext
        public CategoryController(TreeFriendDbContext db) {
            _db = db;
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

        //新增類別or標籤
        [Route("AddCategory")]
        [HttpPost]
        public bool AddCategory([FromBody] CategoryViewModel category) {
            //取的使用者ID
            int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
            if (category.id == 1) {
                _db.categories.Add(new Category {
                    CategoryName = category.input,
                    UserId = userId
                });
            } else {
                _db.hashtags.Add(new Hashtag {
                    HashtagName = category.input,
                    UserId = userId
                });
            }
            _db.SaveChanges();
            return true;
        }

        //編輯類別or標籤
        [Route("EditCategory")]
        [HttpPut]
        public bool EditCategory([FromBody] CategoryViewModel category) {
            if (category.id == 1) {
                _db.categories.Find(category.cId).CategoryName = category.input;
            }else {
                _db.hashtags.Find(category.cId).HashtagName = category.input;
            }
            _db.SaveChanges();
            return true;
        }

        //刪除類別
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

        //刪除標籤
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
