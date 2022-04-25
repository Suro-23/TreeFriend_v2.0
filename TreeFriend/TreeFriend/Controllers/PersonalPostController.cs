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

                try
                {
                    _context.personalPosts.Add(new PersonalPost()
                    {
                        UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value),
                        Content = post.Content,
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

        //post的user
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
            var p = _context.personalPosts.Where(x => x.UserId == UserId).OrderByDescending(y => y.PersonalPostId).Select(z => z.PersonalPostId); //抓登入userId的所有貼文 反序列personalPosts表 取前三筆PersonalPostId
            var L = new List<PersonalPostRenderViewModel>();
              
            foreach (var F in p.ToList())
            {

                //取出跟每一筆貼文PersonalPostId一樣的PersonalPostMessages
                var head = _context.PersonalPostMessages.FirstOrDefault(x => x.PersonalPostId == F);

                if (head != null)
                {
                    //建立一個留言區物件
                    var mes = _context.PersonalPostMessages.Where(x => x.PersonalPostId == F)
                    .Select(y => new PersonalPostMessageViewModel()
                    {
                        PersonalPostId = F,
                        UserMessage = y.Message,
                        HeadshotPath =_context.usersDetail.Where(x=>x.UserId==y.UserId).FirstOrDefault().HeadshotPath,
                        CreateDate = y.CreateDate
                        //TODO 加暱稱
                    }).ToList();
                    //建立貼文內容物件渲染
                    var a = _context.personalPosts.Where(x => x.PersonalPostId == F).OrderByDescending(x => x.CreateDate)
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
                    var a = _context.personalPosts.Where(x => x.PersonalPostId == F).OrderByDescending(x => x.CreateDate)
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
        //edit的user
        [HttpGet]
        public List<PersonalPostRenderViewModel> GetUserEdit()  
        {
            var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);

            var user = _context.usersDetail.Where(n => n.UserId == UserId)
            .Select(x => new PersonalPostRenderViewModel()
            {
                HeadshotPath = x.HeadshotPath,
                UserName = x.UserName,
                SelfIntrodution = x.SelfIntrodution,
                UserId=x.UserId

            }).ToList();
            return user;
        }
        //edit 的貼文
        public List<PersonalPostRenderViewModel> GetUserContent()  
        {
            var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
            //抓登入userId的所有貼文 反序列personalPosts表 取前六筆PersonalPostId
            var p = _context.personalPosts.Where(x => x.UserId == UserId).OrderByDescending(y => y.PersonalPostId).Take(6).Select(z => z.PersonalPostId);
            //建立一個List後面可以接資料
            var L = new List<PersonalPostRenderViewModel>();
            //將取出的貼文逐一帶入
            foreach (var F in p.ToList())
            {

                //取出跟每一筆貼文PersonalPostId一樣的PersonalPostMessages  一篇貼文可能有多篇不同使用者的留言
                var head = _context.PersonalPostMessages.FirstOrDefault(x => x.PersonalPostId == F);

                if (head != null)
                {
                    //建立一個留言區物件
                    //從留言表內選取所要取出的內容(與上面找出的PersonalPostId一樣)
                    var mes = _context.PersonalPostMessages.Where(x => x.PersonalPostId == F)
                    .Select(y => new PersonalPostMessageViewModel()
                    {
                        PersonalPostId = F,
                        UserMessage = y.Message,
                        HeadshotPath = _context.usersDetail.Where(x => x.UserId == y.UserId).FirstOrDefault().HeadshotPath,
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
                _context.Remove(result);
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
                    Console.WriteLine(ex);
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region 留言即時渲染
        public PersonalPostMessageViewModel PCreateMessage(PersonalPostMessageViewModel mes)
        {
            //從cookie找出使用者的userId
            var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
            //找出cookie與userDetail相符的userId
            var headshot = _context.usersDetail.Where(x => x.UserId == UserId).FirstOrDefault();
            //從留言資料表內找出最後一筆留言
            var personalp = _context.PersonalPostMessages.Where(x => x.PersonalPostId == mes.PersonalPostId).OrderBy(x => x.CreateDate)
                                    .LastOrDefault();
            //new一個viewmodel把資料塞進去
            var post = new PersonalPostMessageViewModel()
            {
                PersonalPostId = mes.PersonalPostId,
                UserMessage = personalp.Message,
                HeadshotPath = headshot.HeadshotPath,
                CreateDate = personalp.CreateDate
                //TODO 加暱稱
            };

            return post;
        }
        #endregion
    }
}
