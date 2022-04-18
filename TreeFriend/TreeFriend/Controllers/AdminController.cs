using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TreeFriend.Controllers {
    public class AdminController : Controller {
        [Authorize(Roles = "Admin")]
        //首頁
        public IActionResult AddCategory() {
            return View();
        }

        public IActionResult AddLecture()
        {
            return View();
        }
    }
}
