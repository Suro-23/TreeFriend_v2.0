using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using TreeFriend.Models.Entity;
using TreeFriend.Models;
using TreeFriend.Models.KallinAndYolan;

namespace TreeFriend.Controllers {
    public class RegisterController : Controller {
        private readonly TreeFriendDbContext _context;

        public RegisterController(TreeFriendDbContext context) {
            _context = context;
        }


        // GET: Register
        public async Task<IActionResult> Index() {
            return View(await _context.users.ToListAsync());
        }

        // GET: Register/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var user = await _context.users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null) {
                return NotFound();
            }

            return View(user);
        }

        public bool SameEmail([FromBody] SameEmail sameEmail) {
            var check = _context.users.Where(x => x.Email == sameEmail.Email)
               .FirstOrDefault();
            if (check == null) {
                return true;
            } else {
                return false;
            }
        }

        // GET: Register/Create
        [AllowAnonymous]
        public IActionResult Create() {
            return View();
        }

        // POST: Register/Create
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user) {
            if (user.Email != "" && user.Password != "") {
                var register = _context.users.Where(x => x.Email == user.Email).FirstOrDefault();
                if (register == null) {
                    if (ModelState.IsValid) {
                        _context.Add(user);
                        await _context.SaveChangesAsync();

                        //YP : 註冊時順便寫入使用者基本資訊
                        //將使用者名稱預設為Email名稱
                        string[] name = user.Email.Split('@');
                        var _user = _context.users.Where(u => u.Email == user.Email).FirstOrDefault();
                        var UserDetail = new UserDetail() { UserId = _user.UserId, UserName = name[0] };
                        _context.Add(UserDetail);
                        await _context.SaveChangesAsync();

                        //YP : 註冊成功後轉跳至登入
                        return RedirectToAction(nameof(Create));
                        //return RedirectToAction(nameof(AfterRegister));
                    }
                    return View(user);
                } else {
                    return Content("成功");
                }
            } else {
                return Content("資料有誤");
            }
        }

        public IActionResult AfterRegister() {
            //YP :測試時修改過路徑
            return View("../Home/HomePage");
        }

        [HttpPost]
        //[Authorize(Roles="admin")]
        public async Task<IActionResult> Login([FromBody] UserLoginViewModel model) {
            var check = _context.users.Where(x => x.Email == model.Email && x.Password == model.Password)
                .FirstOrDefault();


            if (check == null) {
                return View("Create");
            } else {
                //YP : 登入時順便檢查身分，並獲得頭像
                var user = _context.usersDetail.Where(u => u.UserId == check.UserId).FirstOrDefault();

                var UserLevel = check.UserLevel == true ? "Admin" : "Member";
                var claims = new List<Claim>()
                {
                    //YP : 確認身分後cookie綁定權限
                    new Claim(ClaimTypes.Email,check.Email),
                    new Claim("UserId",check.UserId.ToString()),
                    new Claim(ClaimTypes.Role,UserLevel),
                    new Claim("Headshot",user.HeadshotPath),
                    new Claim("UserName",user.UserName)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);
                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                });

                //YP : 登入成功後轉跳回首頁
                return Json(Url.Action("HomePage", "Home"));
            }

        }

        //YP : 改成GET方法，沒有傳入參數
        [HttpGet]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Json(Url.Action("HomePage", "Home"));
        }



        // GET: Register/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var user = await _context.users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }
            return View(user);
        }

        // POST: Register/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Email,Password")] User user) {
            if (id != user.UserId) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!MembersExists(user.UserId)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Register/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var user = await _context.users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null) {
                return NotFound();
            }

            return View(user);
        }

        // POST: Register/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var user = await _context.users.FindAsync(id);
            _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembersExists(int id) {
            return _context.users.Any(e => e.UserId == id);
        }



    }
}
