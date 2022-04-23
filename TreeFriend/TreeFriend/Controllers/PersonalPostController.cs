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

namespace TreeFriend.Controllers.Api
{


    public class PersonalPostController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly TreeFriendDbContext _context;
        public PersonalPostController(IWebHostEnvironment environment, TreeFriendDbContext context)
        {
            _environment = environment;
            _context = context;
        }
        public IActionResult Post(int? id)
        {
            return View();
        }
        public IActionResult Postbackstage()
        {
            return View();

        }
        #region 新增個人動態
        //新增個人動態
        [HttpPost]
        //[Route("AddPersonalPost")]
        public bool Create(PersonalPostViewModel post)
        {
            var path = _environment.WebRootPath + "/PostPicture";
            var pic = post.Pic.ToArray(); //取出照片的陣列
            var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);

            if (pic != null)
            {
                var fileName = "";
                //var sum = "";

                try
                {
                    _context.personalPosts.Add(new PersonalPost()
                    {
                        UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value),
                        Content = post.Content,
                        //PostPhotoPath = sum
                    });


                    //先將資料更新貼文，才能取得當前貼文ID
                    _context.SaveChanges();

                    var imgpost = _context.personalPosts.Where(x => x.UserId == UserId).OrderByDescending(x => x.PersonalPostId).FirstOrDefault();
                    for (var i = 0; i < pic.Length; i++)
                    {
                        fileName = DateTime.Now.Ticks.ToString() + pic[i].FileName;
                        using (var fs = System.IO.File.Create($"{path}/{fileName}"))  //create一個路徑
                        {
                            pic[i].CopyTo(fs); //至根目錄
                        }
                        //sum += fileName; //存進資料庫

                        _context.PersonalPostImages.Add(new PersonalPostImage()
                        {
                            PersonalPostId = Convert.ToInt32(imgpost.PersonalPostId),
                            PostPhotoPath = fileName,

                        });
                        _context.SaveChanges();
                    }

                    //拿到該使用者的最後一筆新增貼文的ID後
                    //存入標籤細節表，對應的標籤&貼文
                    //TODO: 多對多

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region 渲染
        //public string GetId(int id)
        //{
        //    var userid = "https://localhost:44341/personalpost/post?d=" + id;
        //    GetAllContent(userid);
        //    return userid;
        //}
        //渲染個人動態到前端  渲染的資料不能抓cookie的使用者，id因為這個頁面是給其他人看的
        //[AllowAnonymous]
        [HttpGet]
        [Route("[controller]/[action]/{UserId}")]
        public List<PersonalPostRenderViewModel> GetUser([FromRoute] int UserId)  //id要等於下面的Userid
        {
            var user = _context.usersDetail.Where(n => n.UserId == UserId)
            .Select(x => new PersonalPostRenderViewModel()
            {
                HeadshotPath = x.HeadshotPath,
                UserName = x.UserName,
                SelfIntrodution = x.SelfIntrodution

            }).ToList();
        return user;
        }

            [HttpGet]
       [Route("[controller]/[action]/{UserId}")]
        public List<PersonalPostRenderViewModel> GetAllContent([FromRoute] int UserId)  //id要等於下面的Userid
        {

            //var UserId = _context.users.Where(n=>n.UserId);  //這裡要更改
            var p = _context.personalPosts.Where(x => x.UserId == UserId).OrderByDescending(y => y.PersonalPostId).Take(6).Select(z => z.PersonalPostId); //抓登入userId的所有貼文 反序列personalPosts表 取前三筆PersonalPostId
            var L = new List<PersonalPostRenderViewModel>();
              
            foreach (var F in p.ToList())
            {

                //取出跟6筆貼文PersonalPostId一樣的PersonalPostMessages
                var head = _context.PersonalPostMessages.FirstOrDefault(x => x.PersonalPostId == F);

                if (head != null)
                {
                    //選出跟usersDetail一樣的userid 取出頭像路徑
                    var h = _context.usersDetail.FirstOrDefault(n => n.UserId == head.UserId);
                    //建立一個留言區物件
                    var mes = _context.PersonalPostMessages.Where(x => x.PersonalPostId == F)
                    .Select(y => new PersonalPostMessageViewModel()
                    {
                        PersonalPostId = F,
                        UserMessage = y.Message,
                        HeadshotPath = h.HeadshotPath,
                        CreateDate = y.CreateDate
                        //TODO 加暱稱
                    }).ToList();
                    //建立貼文內容物件渲染
                    var a = _context.personalPosts.Where(x => x.PersonalPostId == F).OrderByDescending(x => x.CreateDate).Take(6)
                    .Select(y => new PersonalPostRenderViewModel()
                    {
                        PersonalPostId = F,
                        Content = y.Content,
                        PostPhotoPath = _context.PersonalPostImages.Where(w => w.PersonalPostId == F).Select(s => s.PostPhotoPath).ToList(),
                        Message = mes
                    }).ToList();
                    

                    L.AddRange(a);
                }
                else
                {
                    //如果貼文內沒有留言，留言為空值
                    List<PersonalPostMessageViewModel> mes = null;
                    var a = _context.personalPosts.Where(x => x.PersonalPostId == F).OrderByDescending(x => x.CreateDate).Take(6)
                    .Select(y => new PersonalPostRenderViewModel()
                    {
                        PersonalPostId = F,
                        Content = y.Content,
                        PostPhotoPath = _context.PersonalPostImages.Where(w => w.PersonalPostId == F).Select(s => s.PostPhotoPath).ToList(),
                        Message = mes,


                    }).ToList();

                    L.AddRange(a);
                }
            }
            Console.WriteLine(L);
            return L;

        }
        #endregion


        #region 使用者編輯渲染個人動態頁面
        //編輯個人動態
        public IActionResult EditPost()
        {
            return View();
        }

        public List<PersonalPostRenderViewModel> GetUserContent()  //id要等於下面的Userid
        {
            var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);

            //var UserId = _context.users.Where(n=>n.UserId);  //這裡要更改
            var p = _context.personalPosts.Where(x => x.UserId == UserId).OrderByDescending(y => y.PersonalPostId).Take(6).Select(z => z.PersonalPostId); //抓登入userId的所有貼文 反序列personalPosts表 取前三筆PersonalPostId
            var L = new List<PersonalPostRenderViewModel>();

            foreach (var F in p.ToList())
            {

                //取出跟6筆貼文PersonalPostId一樣的PersonalPostMessages
                var head = _context.PersonalPostMessages.FirstOrDefault(x => x.PersonalPostId == F);

                if (head != null)
                {
                    //選出跟usersDetail一樣的userid 取出頭像路徑
                    var h = _context.usersDetail.FirstOrDefault(n => n.UserId == head.UserId);
                    //建立一個留言區物件
                    var mes = _context.PersonalPostMessages.Where(x => x.PersonalPostId == F)
                    .Select(y => new PersonalPostMessageViewModel()
                    {
                        PersonalPostId = F,
                        UserMessage = y.Message,
                        HeadshotPath = h.HeadshotPath,
                        CreateDate = y.CreateDate
                        //TODO 加暱稱
                    }).ToList();
                    //建立貼文內容物件渲染
                    var a = _context.personalPosts.Where(x => x.PersonalPostId == F).OrderByDescending(x => x.CreateDate).Take(6)
                    .Select(y => new PersonalPostRenderViewModel()
                    {
                        PersonalPostId = F,
                        Content = y.Content,
                        PostPhotoPath = _context.PersonalPostImages.Where(w => w.PersonalPostId == F).Select(s => s.PostPhotoPath).ToList(),
                        Message = mes
                    }).ToList();


                    L.AddRange(a);
                }
                else
                {
                    //如果貼文內沒有留言，留言為空值
                    List<PersonalPostMessageViewModel> mes = null;
                    var a = _context.personalPosts.Where(x => x.PersonalPostId == F).OrderByDescending(x => x.CreateDate).Take(6)
                    .Select(y => new PersonalPostRenderViewModel()
                    {
                        PersonalPostId = F,
                        Content = y.Content,
                        PostPhotoPath = _context.PersonalPostImages.Where(w => w.PersonalPostId == F).Select(s => s.PostPhotoPath).ToList(),
                        Message = mes,


                    }).ToList();

                    L.AddRange(a);
                }
            }
            Console.WriteLine(L);
            return L;

        }

        //渲染個人動態到編輯的前端
        public List<PersonalPostRenderViewModel> GetAllContentOnePic()
        {
            var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);

            var p = _context.personalPosts.Where(x => x.UserId == UserId).OrderByDescending(y => y.PersonalPostId).Take(6).Select(z => z.PersonalPostId); //抓登入userId的所有貼文 反序列personalPosts表 取前三筆PersonalPostId

            var L = new List<PersonalPostRenderViewModel>();

            foreach (var F in p.ToList())
            {
                var mes = _context.PersonalPostMessages.Where(x => x.PersonalPostId == F)
                .Select(y => new PersonalPostMessageViewModel()
                {
                    PersonalPostId = F,
                    UserMessage = y.Message,
                    //HeadshotPath = h.HeadshotPath,
                    CreateDate = y.CreateDate
                    //TODO 加暱稱
                }).ToList();
                        var a = _context.personalPosts.Where(x => x.PersonalPostId == F).OrderByDescending(x => x.CreateDate).Take(6)
                .Select(y => new PersonalPostRenderViewModel()
                {
                    PersonalPostId = F,
                    Content = y.Content,
                    PostPhotoPath = _context.PersonalPostImages.Where(w => w.PersonalPostId == F).Select(s => s.PostPhotoPath).Take(1).ToList(),
                    Message = mes,
                    State = false
                }).ToList();

                L.AddRange(a);
            }
            Console.WriteLine(L);
            return L;

        }
        #endregion

        #region 編輯個人動態
        [HttpPost]
        public bool EditPost(EditPostViewModel update)
        {
            var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);

            var updatepost = _context.personalPosts.Where(x => x.PersonalPostId == update.PersonalPostId).FirstOrDefault();
            if (updatepost != null)
            {
                updatepost.Content = update.Content;
                updatepost.CreateDate = update.CreateDate;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 刪除個人動態
        [HttpPost]
        public bool DeletePost(int postId)
        {
            try
            {
                var result = _context.personalPosts.Where(x => x.PersonalPostId == postId).SingleOrDefault();
                // var img = _context.PersonalPostImages.Where(x => x.PersonalPostId == postId).Select(y => y.PersonalPostId);
                _context.Remove(result);
                //_context.personalPosts.Update(result);
                _context.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 建立留言
        public bool CreateMessage(PersonalPostMessage mes)
        {
            if (mes != null)
            {
                var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
                try
                {
                    _context.PersonalPostMessages.Add(new PersonalPostMessage()
                    {
                        UserId = UserId,
                        PersonalPostId = mes.PersonalPostId,
                        Message = mes.Message,
                    });
                    _context.SaveChanges();

                    return true;
                }

                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region 留言即時渲染
        public object PCreateMessage(PersonalPostMessageViewModel mes)
        {
            //var list = new List<PersonalPostMessageViewModel>();
            var personalp = _context.PersonalPostMessages.Where(x => x.PersonalPostId == mes.PersonalPostId).OrderBy(x => x.CreateDate)
                                    .LastOrDefault();
            var post = new PersonalPostMessageViewModel()
            {
                PersonalPostId = mes.PersonalPostId,
                UserMessage = personalp.Message,
                // HeadshotPath = n.HeadshotPath,
                CreateDate = personalp.CreateDate
                //TODO 加暱稱
            };
            //list.AddRange(personalp);

            return post;
        }
        #endregion
    }
}
