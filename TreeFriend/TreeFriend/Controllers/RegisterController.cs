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
using System.Text;
using System.Security.Cryptography;
using System.IO;
using EmailService;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;

namespace TreeFriend.Controllers {
    public class RegisterController : Controller {
        private readonly TreeFriendDbContext _context;
        private readonly IEmailSender _emailSender;

        public RegisterController(TreeFriendDbContext context, IEmailSender emailSender) {
            _context = context;
            _emailSender = emailSender;

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
        #region 判斷email是否重複
        public bool SameEmail([FromBody] SameEmail sameEmail) {
            var check = _context.users.Where(x => x.Email == sameEmail.Email)
               .FirstOrDefault();
            if (check == null) {
                return true;
            } else {
                return false;
            }
        }
        #endregion
        // GET: Register/Create
        [AllowAnonymous]
        public IActionResult Create() {
            return View();
        }
        #region 加密解密
        static string encryptKey = "abcd";
        private string Encrypt(string str) //加密字符串
        {
            try {
                byte[] key = Encoding.Unicode.GetBytes(encryptKey);//密鑰
                byte[] data = Encoding.Unicode.GetBytes(str);//待加密字符串

                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();//加密、解密對象
                MemoryStream MStream = new MemoryStream();//內存流對象

                //用內存流實例化加密流對象
                CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
                CStream.Write(data, 0, data.Length);//向加密流中寫入數據
                CStream.FlushFinalBlock();//將數據壓入基礎流
                byte[] temp = MStream.ToArray();//從內存流中獲取字節序列
                CStream.Close();//關閉加密流
                MStream.Close();//關閉內存流

                return Convert.ToBase64String(temp);//返回加密後的字符串
            } catch {
                return str;
            }
        }
        private string Decrypt(string str) ////解密字符串
        {
            try {
                str = str.Replace(' ', '+');
                byte[] key = Encoding.Unicode.GetBytes(encryptKey);//密鑰
                byte[] data = Convert.FromBase64String(str);//待解密字符串

                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();//加密、解密對象
                MemoryStream MStream = new MemoryStream();//內存流對象

                //用內存流實例化解密流對象
                CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
                CStream.Write(data, 0, data.Length);//向加密流中寫入數據
                CStream.FlushFinalBlock();//將數據壓入基礎流
                byte[] temp = MStream.ToArray();//從內存流中獲取字節序列
                CStream.Close();//關閉加密流
                MStream.Close();//關閉內存流

                return Encoding.Unicode.GetString(temp);//返回解密後的字符串
            } catch {
                return str;
            }
        }
        #endregion

        #region 信箱開通 寄信
        public ActionResult GetNum(User members) //寄信方法
        {
            //string ranNum = randomNum;
            var secretcode = Encrypt(members.UserId.ToString() + "&y=true");
            //string g = Guid.NewGuid().ToString("d").Substring(0, 7);
            var message = new Message(new string[] { $"{members.Email}" }, "技能交換網站驗證碼", "親愛的" + $"{ members.Email }" + "先生/小姐" +
                "您好，請點擊以下網址註冊:" + "https://treefriends.azurewebsites.net/register/get?d=" + secretcode);
            _emailSender.SendEmail(message);

            return Content("信已寄出");

            //    //Console.WriteLine(randomNum);
            //    //return Content($"{randomNum}");
        }
        #endregion

        #region 信箱開通連結
        [HttpGetAttribute()]
        public async Task<IActionResult> Get([FromQuery] string d) {
            string Mid = Decrypt(d);
            //Mid.Replace("-","-");

            Console.WriteLine(Mid);
            string[] sArray = Mid.Split("&");

            string idString = sArray[0];
            var id = idString;
            var identity = _context.users.Where(x => x.UserId.ToString() == id)
               .FirstOrDefault();
            if (identity != null) {
                identity.UserStatus = true;
                _context.Attach(identity);
                _context.Entry(identity).Property(p => p.UserStatus).IsModified = true;
                await _context.SaveChangesAsync();
            }
            return Content("帳號已開通");
        }
        #endregion
        // POST: Register/Create
        //[ValidateAntiForgeryToken]
        #region 註冊
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user) {
            if (user.Email != "" && user.Password != "") {
                var register = _context.users.Where(x => x.Email == user.Email).FirstOrDefault();
                if (register == null) {
                    if (ModelState.IsValid) {
                        byte[] salt = new byte[128 / 8];
                        using (var rngCsp = new RNGCryptoServiceProvider()) {
                            rngCsp.GetNonZeroBytes(salt);
                        }
                        //Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

                        // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
                        //string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(user.Password + salt);
                        byte[] hashBytes = new SHA256Managed().ComputeHash(passwordAndSaltBytes);
                        string hashed = Convert.ToBase64String(hashBytes);


                        user.Salt = salt;
                        user.Password = hashed;
                        _context.Add(user);
                        await _context.SaveChangesAsync();
                        GetNum(user);

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
        #endregion

        #region 補加鹽用
        [AllowAnonymous]
        [HttpPost]
        public bool getSalt([FromBody] User user) {
            var _user = _context.users.FirstOrDefault(x => x.Email == user.Email);
            try {
                byte[] salt = new byte[128 / 8];
                using (var rngCsp = new RNGCryptoServiceProvider()) {
                    rngCsp.GetNonZeroBytes(salt);
                }
                byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(user.Password + salt);
                byte[] hashBytes = new SHA256Managed().ComputeHash(passwordAndSaltBytes);
                string hashed = Convert.ToBase64String(hashBytes);


                _user.Salt = salt;
                _user.Password = hashed;
                _context.SaveChanges();
                return true;
            } catch (Exception) {

                throw;
            }
        }
        #endregion

        public IActionResult AfterRegister() {
            //YP :測試時修改過路徑
            return View("../Home/HomePage");
        }

        #region 登入
        [HttpPost]
        //[Authorize(Roles="admin")]
        public async Task<IActionResult> Login([FromBody] UserLoginViewModel model) {
            var check = _context.users.Where(x => x.Email == model.Email)
                .FirstOrDefault();

            if (check == null && (model.Email == "" || model.Password == ""))
            {
                return View("帳號或密碼錯誤");
            }
            else
            {
                if (check.Salt != null)
                {
                    string salt = check.Salt.ToString();

                    byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(model.Password + salt);
                    byte[] hashBytes = new SHA256Managed().ComputeHash(passwordAndSaltBytes);
                    string hashString = Convert.ToBase64String(hashBytes);

                    if (hashString == check.Password)
                    {
                        if (check.UserStatus == true)
                        {
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
                            new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                            });
                            //YP : 登入成功後轉跳回首頁
                            return Json(Url.Action("HomePage", "Home"));
                        }
                        return Content("請至信箱開通帳號");


                    }
                    else
                    {
                        return Content("帳號或密碼錯誤");
                    }
                }
                return Content("帳號或密碼錯誤");
            }
        }
        #endregion

        #region 第三方登入
        public IActionResult FBLogin() {
            var p = new AuthenticationProperties() {
                RedirectUri = Url.Action("Response")
            };
            return Challenge(p, FacebookDefaults.AuthenticationScheme);

        }

        public async Task<IActionResult> ResponseAsync(User user) {
            var res = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //var data = res.Principal.Claims.Select(x => new
            //{
            //    x.Type,
            //    x.Value,
            //    x.Issuer,
            //    x.OriginalIssuer
            //});
            //var value= data.Select(x => x.Value);
            foreach (Claim claim in res.Principal.Claims) {
                var fbemail = "";
                //var fbId = "";
                if (claim.Type.Contains("emailaddress")) {
                    fbemail = claim.Value;
                    user.Email = fbemail;
                    user.Password = Guid.NewGuid().ToString("d").Substring(0, 7);
                    var sameEmail = _context.users.Where(x => x.Email == user.Email).FirstOrDefault();

                    if (sameEmail == null) { //第一次登入

                        _context.Add(user);
                        await _context.SaveChangesAsync();

                        var _user = _context.users.Where(x => x.Email == user.Email).FirstOrDefault();
                        //登入時存入使用者基本資料
                        string[] name = user.Email.Split('@');
                        var UserDetail = new UserDetail() { UserId = _user.UserId, UserName = name[0] };
                        _context.Add(UserDetail);
                        await _context.SaveChangesAsync();

                        var userFbG = _context.usersDetail.Where(u => u.UserId == _user.UserId).FirstOrDefault();
                        var UserLevel = _user.UserLevel == true ? "Admin" : "Member";
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Email, claim.Value),
                            new Claim("UserId", _user.UserId.ToString()),
                            new Claim(ClaimTypes.Role, UserLevel),
                            new Claim("Headshot", userFbG.HeadshotPath),
                            new Claim("UserName",userFbG.UserName)
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
                        return LocalRedirect("/Home/HomePage");
                    } else { //登入過
                        var userFbG = _context.usersDetail.Where(u => u.UserId == sameEmail.UserId).FirstOrDefault();
                        var UserLevel = sameEmail.UserLevel == true ? "Admin" : "Member";
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Email, claim.Value),
                            new Claim("UserId", sameEmail.UserId.ToString()),
                            new Claim(ClaimTypes.Role, UserLevel),
                            new Claim("Headshot", userFbG.HeadshotPath),
                            new Claim("UserName",userFbG.UserName)
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
                        return LocalRedirect("/Home/HomePage");
                    }
                }
                //Console.WriteLine("CLAIM TYPE: " + claim.Type + "; CLAIM VALUE: " + claim.Value + "</br>");
                //Console.WriteLine(fbemail);
            }


            return Content("1");
        }
        public IActionResult GoogleLogin() {
            var p = new AuthenticationProperties() {
                RedirectUri = Url.Action("Response")
            };
            return Challenge(p, GoogleDefaults.AuthenticationScheme);
        }
        //YP : 改成GET方法，沒有傳入參數
        [HttpGet]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Json(Url.Action("HomePage", "Home"));
        }
        #endregion


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
