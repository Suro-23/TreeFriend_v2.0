using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TreeFriend.Models;
using TreeFriend.Models.Entity;
using System.Linq;
using TreeFriend.Models.ViewModel;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace TreeFriend.Controllers.Api {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SkillPostController : ControllerBase {
        private readonly TreeFriendDbContext _db;

        //注入DbContext
        public SkillPostController(TreeFriendDbContext db) {
            _db = db;
        }

        #region 獲取清單

        //取得所有標籤
        [AllowAnonymous]
        [Route("GetAllHashtag")]
        public object GetAllHashtag() {
            var tagList = _db.hashtags.Select(t => new { t.HashtagId, t.HashtagName }).ToList();
            return tagList;
        }

        //取得所有技能貼文
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllSkillPost")]
        public List<SkillPostViewModel> GetAllSkillPost() {
            //拿到該使用者的所有貼文
            //將skillPostList join hashtagDetail 拿到所有文章的所有標籤ID
            //存成新物件備用
            var skillPostList = _db.skillPosts.Where(p => p.Status == true).OrderByDescending(e => e.SkillPostId);

            //拿到文章總表
            //透過categoryId，HashtagId 關聯出 categoryName，hashtagName 供前端使用
            var skillPostJoin = _db.hashtagDetails.Join(skillPostList,
                h => h.SkillPostId,
                p => p.SkillPostId,
                (h, p) => new {
                    SkillPostId = p.SkillPostId,
                    UserId = p.UserId,
                    UserName = p.User.UserDetail.UserName,
                    Headshot = p.User.UserDetail.HeadshotPath,
                    Title = p.Title,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName,
                    Content = p.Content,
                    Region = p.Region,
                    HashtagId = h.HashtagId,
                    HashtagName = h.Hashtag.HashtagName
                }).ToList();

            var groupList = from skPost in skillPostJoin
                            group skPost by skPost.SkillPostId into newGroup
                            orderby newGroup.Key
                            select newGroup;

            //創建一個集合備用
            List<SkillPostViewModel> resultList = new();

            //將分組資料依序塞入陣列中
            //這裡的遍歷是根據
            foreach (var group in groupList) {
                List<SkillPostMessageViewModel> messageResult = null;
                var messageList = _db.skillPostMessages.Where(p => p.SkillPostId == group.Key);
                if (messageList.Count() != 0) {
                    messageResult = messageList.Select(p => new SkillPostMessageViewModel {
                        SkillPostId = p.SkillPostId,
                        UserId = p.UserId,
                        UserName = p.UserName,
                        UserHeadshot = p.UserHeadshot,
                        Content = p.Content
                    }).ToList();
                }


                resultList.Add(
                    new SkillPostViewModel {
                        SkillPostId = group.Key,
                        UserId = group.Select(x => x.UserId).FirstOrDefault(),
                        UserName = group.Select(x => x.UserName).FirstOrDefault(),
                        UserHeadshot = group.Select(x => x.Headshot).FirstOrDefault(),
                        Title = group.Select(x => x.Title).FirstOrDefault(),
                        CategoryId = group.Select(x => x.CategoryId).FirstOrDefault(),
                        CategoryName = group.Select(x => x.CategoryName).FirstOrDefault(),
                        Content = group.Select(x => x.Content).FirstOrDefault(),
                        Region = group.Select(x => x.Region).FirstOrDefault(),
                        HashtagId = group.Select(x => x.HashtagId).ToArray(),
                        HashtagName = group.Select(x => x.HashtagName).ToArray(),
                        Message = messageResult,
                        //讓前端用的欄位，後端不須使用
                        LeaveMsg = null
                    });
            }

            return resultList.OrderByDescending(x =>x.SkillPostId).ToList();
        }

        //根據標籤取得貼文
        [AllowAnonymous]
        [HttpPost]
        [Route("GetSKillPostByHashtag")]
        public List<SkillPostViewModel> GetSKillPostByHashtag(int[] hashtagIdList) {
            //創建一個集合備用
            List<SkillPostViewModel> skpList = new();

            //拿到當前"存在"的所有文章標籤
            var validPostHashtagList = _db.hashtagDetails.Where(p => p.SkillPost.Status == true);

            //根據使用者選擇的標籤依序找出相對應的文章
            foreach (int item in hashtagIdList) {
                //找到當前該標籤的所有貼文並存入"post"中
                //但此處史會找到一個貼文一筆標籤，會造成後續渲染有誤，故之後需再重新找到一次該貼文下所有的標籤
                var post = validPostHashtagList.Where(p => p.HashtagId == item);

                //確認該標籤底下有文章在進行下列動作
                if (post.Count() > 0) {
                    //為了讓下方foreach可以使用文章ID，所以先轉成集合一次並存成"p_id"備用，
                    //不轉型的話會以"IQueryable<HashtagDetail>"的形式存在，再跑回圈會出錯，必須轉成List
                    var p_id = post.Select(p => p.SkillPostId).ToList();
                    foreach (var p in p_id) {
                        //拿到該文章下的所有標籤
                        var postById = _db.hashtagDetails.Where(x => x.SkillPostId == p);

                        //判斷該文章是否已經存在"skpList"中，避免重複加入
                        if (!skpList.Exists(x => x.SkillPostId == p)) {
                            //拿到該文章的所有資訊
                            //透過categoryId，HashtagId 關聯出 categoryName，hashtagName 供前端使用
                            var skillPostJoin = postById.Join(_db.skillPosts,
                                h => h.SkillPostId,
                                p => p.SkillPostId,
                                (h, p) => new {
                                    SkillPostId = p.SkillPostId,
                                    UserId = p.UserId,
                                    UserName = p.User.UserDetail.UserName,
                                    Headshot = p.User.UserDetail.HeadshotPath,
                                    Title = p.Title,
                                    CategoryId = p.CategoryId,
                                    CategoryName = p.Category.CategoryName,
                                    Content = p.Content,
                                    Region = p.Region,
                                    HashtagId = h.HashtagId,
                                    HashtagName = h.Hashtag.HashtagName
                                }).ToList();

                            //先將第一筆的資料儲存起來
                            //因只有標籤ID、名稱不同，其餘資訊皆相同，故取第一筆資料內容來用即可
                            var postInfo = skillPostJoin.First();

                            //將當前貼文下的所有留言取出備用
                            List<SkillPostMessageViewModel> messageResult = null;
                            var messageList = _db.skillPostMessages.Where(p => p.SkillPostId == postInfo.SkillPostId);
                            if (messageList.Count() != 0) {
                                messageResult = messageList.Select(p => new SkillPostMessageViewModel {
                                    SkillPostId = p.SkillPostId,
                                    UserId = p.UserId,
                                    UserName = p.UserName,
                                    UserHeadshot = p.UserHeadshot,
                                    Content = p.Content
                                }).ToList();
                            }

                            //綜合以上資料，存進文章總集List中
                            skpList.Add(
                                new SkillPostViewModel {
                                    SkillPostId = postInfo.SkillPostId,
                                    UserId = postInfo.UserId,
                                    UserName = postInfo.UserName,
                                    UserHeadshot = postInfo.Headshot,
                                    Title = postInfo.Title,
                                    CategoryId = postInfo.CategoryId,
                                    CategoryName = postInfo.CategoryName,
                                    Content = postInfo.Content,
                                    Region = postInfo.Region,
                                    HashtagId = skillPostJoin.Select(x => x.HashtagId).ToArray(),
                                    HashtagName = skillPostJoin.Select(x => x.HashtagName).ToArray(),
                                    Message = messageResult,
                                    //讓前端用的欄位，後端不須使用
                                    LeaveMsg = null
                                });

                        } else {
                            //沒有的話直接進入下一次迴圈
                            continue;
                        }
                    }
                }
            }
            return skpList.OrderByDescending(x => x.SkillPostId).ToList();
        }

        #endregion

        #region 技能貼文
        //新增技能貼文
        [HttpPost]
        [Route("AddSkillPost")]
        public string AddSkillPost([FromBody] SkillPostViewModel skillPost) {
            //將ViewModel資料對應至Entity中
            SkillPost post = new() {
                UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value),
                Title = skillPost.Title,
                Content = skillPost.Content,
                Region = skillPost.Region,
                CategoryId = skillPost.CategoryId
            };

            try {
                //先將資料更新貼文，才能取得當前貼文ID
                _db.skillPosts.Add(post);
                _db.SaveChanges();
            } catch (Exception) {
                return "新增失敗";
            }

            //拿到該使用者的最後一筆新增貼文的ID後
            //存入標籤細節表，對應的標籤&貼文
            var PostId = _db.skillPosts.Where(p => p.UserId == post.UserId).ToList().LastOrDefault().SkillPostId;

            try {
                //將標籤陣列依序新增到標籤細節資料庫
                foreach (var item in skillPost.HashtagId) {
                    _db.hashtagDetails.Add(new HashtagDetail { SkillPostId = PostId, HashtagId = item });
                }
                _db.SaveChanges();

                return "新增成功";
            } catch (Exception) {
                return "新增失敗";
            }
        }

        [HttpGet]
        [Route("GetAllPersonalSkillPost")]
        public List<SkillPostViewModel> GetAllPersonalSkillPost() {
            int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
            string userName = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserName").Value;
            string headShot = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "Headshot").Value;

            //拿到該使用者的所有貼文
            //將skillPostList join hashtagDetail 拿到所有文章的所有標籤ID
            //存成新物件備用
            var skillPostList = _db.skillPosts.Where(p => p.UserId == userId && p.Status == true).OrderByDescending(e => e.SkillPostId);

            //拿到文章總表
            //透過categoryId，HashtagId 關聯出 categoryName，hashtagName 供前端使用
            var skillPostJoin = _db.hashtagDetails.Join(skillPostList,
                h => h.SkillPostId,
                p => p.SkillPostId,
                (h, p) => new {
                    SkillPostId = p.SkillPostId,
                    Title = p.Title,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName,
                    Content = p.Content,
                    Region = p.Region,
                    HashtagId = h.HashtagId,
                    HashtagName = h.Hashtag.HashtagName
                }).ToList();

            //參考資料: https://docs.microsoft.com/zh-tw/dotnet/csharp/linq/group-query-results
            //將文章分組備用
            //分完後的樣子會變成
            //postID_1:
            //      sqlRow1,sqlRow2,sqlRow3
            //postID_2:
            //      sqlRow1,sqlRow2
            var groupList = from skPost in skillPostJoin
                            group skPost by skPost.SkillPostId into newGroup
                            orderby newGroup.Key
                            select newGroup;

            //創建一個集合備用
            List<SkillPostViewModel> resultList = new();

            //將分組資料依序塞入陣列中
            //這裡的遍歷是根據
            foreach (var group in groupList) {
                List<SkillPostMessageViewModel> messageResult = null;
                var messageList = _db.skillPostMessages.Where(p => p.SkillPostId == group.Key);
                if (messageList.Count() != 0) {
                    messageResult = messageList.Select(p => new SkillPostMessageViewModel {
                        SkillPostId = p.SkillPostId,
                        UserId = p.UserId,
                        UserName = p.UserName,
                        UserHeadshot = p.UserHeadshot,
                        Content = p.Content
                    }).ToList();
                }


                resultList.Add(
                    new SkillPostViewModel {
                        SkillPostId = group.Key,
                        UserId = userId,
                        UserName = userName,
                        UserHeadshot = headShot,
                        Title = group.Select(x => x.Title).FirstOrDefault(),
                        CategoryId = group.Select(x => x.CategoryId).FirstOrDefault(),
                        CategoryName = group.Select(x => x.CategoryName).FirstOrDefault(),
                        Content = group.Select(x => x.Content).FirstOrDefault(),
                        Region = group.Select(x => x.Region).FirstOrDefault(),
                        HashtagId = group.Select(x => x.HashtagId).ToArray(),
                        HashtagName = group.Select(x => x.HashtagName).ToArray(),
                        Message = messageResult,
                        //讓前端用的欄位，後端不須使用
                        LeaveMsg = null
                    });
            }

            return resultList;
        }
        #endregion

        #region 留言

        //加入留言
        [HttpPost]
        [Route("SkPostMessage")]
        public void LiveSkillPostMessage([FromBody] SkillPostMessageViewModel skMessage) {
            //拿到當前使用者資訊後將留言寫入技能留言資料表中
            SkillPostMessage post = new SkillPostMessage() {
                SkillPostId = skMessage.SkillPostId,
                UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value),
                UserName = User.Claims.FirstOrDefault(u => u.Type == "UserName").Value,
                UserHeadshot = User.Claims?.FirstOrDefault(u => u.Type == "Headshot").Value,
                Content = skMessage.Content
            };
            _db.skillPostMessages.Add(post);
            _db.SaveChanges();
        }

        #endregion

    }
}
