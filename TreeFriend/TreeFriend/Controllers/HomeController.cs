using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TreeFriend.Models;

namespace TreeFriend.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        //首頁
        [AllowAnonymous]
        public IActionResult HomePage() {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AllSkillPostPage() {
            return View();
        }

        //講座
        [AllowAnonymous]
        public IActionResult ProductPage() {
            return View();
        }

        //發布技能貼文
        [Authorize]
        public IActionResult AddSkillPostPage() {
            return View();
        }

        //歷史技能貼文
        [Authorize]
        public IActionResult PersonalAllSkillPost() {
            return View();
        }

        //編輯個人資訊
        [Authorize]
        public IActionResult MemberInfo() {
            return View();
        }



        public IActionResult Index() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
