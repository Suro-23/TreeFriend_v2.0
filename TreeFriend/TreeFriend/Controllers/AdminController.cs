using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TreeFriend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //首頁
        public IActionResult AddCategory()
        {
            return View();
        }

        //講座頁面
        public IActionResult AddLecture()
        {
            return View();
        }

        //訂單明細
        public IActionResult OrderDetails()
        {
            return View();
        }

        //圖表頁面
        public IActionResult ChartPage()
        {
            return View();
        }

        //系統發文
        public IActionResult AddSystem()
        {
            return View();
        }

        //管理使用者
        public IActionResult ManageUser() {
            return View();
        }

    }
}
