using EmailService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TreeFriend.Models;
using TreeFriend.Models.KallinAndYolan;

namespace TreeFriend.Controllers.Api
{
    public class SendMailController : Controller
    {

        private readonly IEmailSender _emailSender;
        private readonly TreeFriendDbContext _context;

        public SendMailController(IEmailSender emailSender, TreeFriendDbContext context)
        {
            _emailSender = emailSender;
            _context = context;
        }



        public IActionResult ForgetEmail()
        {
            return View();
        }

        //public void GetRdNum(string y) //產生隨機驗證碼
        //{
        //    var password = Guid.NewGuid().ToString("d").Substring(0, 7);
        //}

        string Newpassword = Guid.NewGuid().ToString("d").Substring(0, 7); //產生隨機驗證碼
        public void Get(string x) //寄信方法
        {

            var message = new Message(new string[] { $"{x}" }, "技能交換網站新密碼", "親愛的" + $"{ x }" + "先生/小姐" +
                "您好，您的新密碼為:" + $"{Newpassword}，請盡快登入修改您的密碼");
            _emailSender.SendEmail(message);
        }



        [HttpPost]

        public async Task<IActionResult> HasAccount([FromBody] UserLoginViewModel model)
        {

            var check = _context.users.Where(x => x.Email == model.Email)
                .FirstOrDefault();
            if (check == null)
            {
                return Content("查無此帳號");
            }
            else
            {
                //var Newpassword = Guid.NewGuid().ToString("d").Substring(0, 7);
                Get(model.Email); //寄送新密碼

                //加鹽雜湊
                byte[] salt = new byte[128 / 8];
                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetNonZeroBytes(salt);
                }
                byte[] copySalt = (byte[])salt.Clone();
                var pwdByte = Encoding.UTF8.GetBytes(Newpassword);
                int pwdByteLength = salt.Length;
                Array.Resize(ref copySalt, copySalt.Length + pwdByte.Length);
                for (int i = 0; i < pwdByte.Length; i++)
                {
                    copySalt[pwdByteLength + i] = pwdByte[i];
                }

                byte[] hashBytes = new SHA256Managed().ComputeHash(copySalt);
                string hashed = Convert.ToBase64String(hashBytes);

                check.Salt = salt;
                check.Password = hashed;


                _context.Attach(check);
                _context.Entry(check).Property(p => p.Password).IsModified = true;
                _context.Entry(check).Property(p => p.Salt).IsModified = true;


                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Register");

            }
        }
    }
}
