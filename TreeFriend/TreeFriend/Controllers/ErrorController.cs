using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Quantity(string whatever)
        {
            ViewBag.message = whatever;
            return View();
        }
    }
}
