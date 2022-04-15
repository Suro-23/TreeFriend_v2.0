using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TreeFriend.Models;
using TreeFriend.Models.Bank;
using TreeFriend.Models.Bank.Util;
using TreeFriend.Models.ViewModel;

namespace TreeFriend.Controllers
{
    [Authorize]
    public class BankController : Controller
    {
        private readonly TreeFriendDbContext _db;

        public BankController(TreeFriendDbContext db)
        {
            _db = db;
        }


        /// <summary>
        /// 金流基本資料(可再移到Web.config或資料庫設定)
        /// </summary>
        private BankInfoModel _bankInfoModel = new BankInfoModel
        {   //已改為TreeFriend
            MerchantID = "MS134173586",
            HashKey = "DIQL4I5DZ6sG6aVcBnQ6sFgkzmxUHSdP",
            HashIV = "CNnhZ4oa0qTNVUMP",
            ReturnURL = "http://yourWebsitUrl/Bank/SpgatewayReturn", //ngrok網址要改
            NotifyURL = "https://0d54-1-164-234-176.ngrok.io/Bank/SpgatewayReturn",
            CustomerURL = "http://yourWebsitUrl/Bank/SpgatewayCustomer",
            AuthUrl = "https://ccore.spgateway.com/MPG/mpg_gateway",
            CloseUrl = "https://core.newebpay.com/API/CreditCard/Close"
        };

        


        

         /// <summary>
         /// 付款頁面
         /// </summary>
         /// <returns></returns>
         //public ActionResult PayBill()
         //{
         //    return View();
         //}

         /// <summary>
         /// [智付通支付]金流介接
         /// </summary>
         /// <param name="ordernumber">訂單單號</param>
         /// <param name="amount">訂單金額</param>
         /// <param name="payType">請款類型</param>
         /// <returns></returns>
         /// 



         //金流只在意付款方式、價格、訂單編號
         [HttpPost]
        public async Task SpgatewayPayBillAsync(int Buyercount, int InputlectureId)
        {
            var UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value);
           
            
            _db.OrderDetails.Add(new Models.Entity.OrderDetail()
            {
                CreateDate = DateTime.Now,
                Price = _db.Lectures.Where(x => x.LectureId == InputlectureId).Select(y => y.Price).SingleOrDefault(),
                Count=Buyercount,
                UserId=UserId,
                LectureId=InputlectureId

            }) ;
            _db.SaveChanges();


            var i=_db.OrderDetails.Where(x => x.LectureId == InputlectureId && x.UserId == UserId).OrderBy(x=>x.OrderDetailId).LastOrDefault().OrderDetailId.ToString();
            Console.WriteLine(i);
            string PayMethod = "";
            string version = "2.0";
            string ordernumber = i;
            var price = _db.Lectures.Where(x => x.LectureId == InputlectureId).Select(y => y.Price).SingleOrDefault();
            Console.WriteLine(price);
            int amount = Convert.ToInt32(Buyercount * price);
            Console.WriteLine(amount);
           

            

            TradeInfo tradeInfo = new TradeInfo()
            {
                // * 商店代號
                MerchantID = _bankInfoModel.MerchantID,
                // * 回傳格式
                RespondType = "String",
                // * TimeStamp
                // 目前時間轉換 +08:00, 防止傳入時間或Server時間時區不同造成錯誤
                TimeStamp = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0)).ToUnixTimeSeconds().ToString(),
                // * 串接程式版本
                Version = version,
                // * 商店訂單編號
                //MerchantOrderNo = $"T{DateTime.Now.ToString("yyyyMMddHHmm")}",
                MerchantOrderNo = ordernumber,
                // * 訂單金額
                Amt = amount,
                // * 商品資訊
                ItemDesc = "商品資訊(自行修改)",
                // 繳費有效期限(適用於非即時交易)
                ExpireDate = null,
                // 支付完成 返回商店網址
                //ReturnURL = _bankInfoModel.ReturnURL,
                ReturnURL = null,
                // 支付通知網址
                NotifyURL = _bankInfoModel.NotifyURL,
                // 商店取號網址
                CustomerURL = _bankInfoModel.CustomerURL,
                // 支付取消 返回商店網址
                ClientBackURL = null,
                // * 付款人電子信箱
                Email = string.Empty,
                // 付款人電子信箱 是否開放修改(1=可修改 0=不可修改)
                EmailModify = 0,
                // 商店備註
                OrderComment = null,
                // 信用卡 一次付清啟用(1=啟用、0或者未有此參數=不啟用)
                CREDIT = null,
                // WEBATM啟用(1=啟用、0或者未有此參數，即代表不開啟)
                WEBATM = null,
                // ATM 轉帳啟用(1=啟用、0或者未有此參數，即代表不開啟)
                VACC = null,
                // 超商代碼繳費啟用(1=啟用、0或者未有此參數，即代表不開啟)(當該筆訂單金額小於 30 元或超過 2 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。)
                CVS = null,
                // 超商條碼繳費啟用(1=啟用、0或者未有此參數，即代表不開啟)(當該筆訂單金額小於 20 元或超過 4 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。)
                BARCODE = null,
                //LINEPAY = null
            };

            if (PayMethod == "creditcard")
            {
                tradeInfo.CREDIT = 1;
            }
            //else if (PayMethod == "linePay")
            //{
            //    tradeInfo.LINEPAY = 1;
            //}

            Atom<string> result = new Atom<string>()
            {
                IsSuccess = true
            };

            var inputModel = new SpgatewayInputModel
            {
                MerchantID = _bankInfoModel.MerchantID,
                Version = version
            };

            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            List<KeyValuePair<string, string>> tradeData = LambdaUtil.ModelToKeyValuePairList<TradeInfo>(tradeInfo);
            // 將List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
            var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
            //tradeQueryPara = tradeQueryPara + "&SAMSUNGPAY=1&ANDROIDPAY=1";
            // AES 加密
            inputModel.TradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara, _bankInfoModel.HashKey, _bankInfoModel.HashIV);
            // SHA256 加密
            inputModel.TradeSha = CryptoUtil.EncryptSHA256($"HashKey={_bankInfoModel.HashKey}&{inputModel.TradeInfo}&HashIV={_bankInfoModel.HashIV}");

            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            List<KeyValuePair<string, string>> postData = LambdaUtil.ModelToKeyValuePairList<SpgatewayInputModel>(inputModel);

            StringBuilder s = new StringBuilder();
            s.Append("<html>");
            s.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
            s.AppendFormat("<form name='form' action='{0}' method='post'>", _bankInfoModel.AuthUrl);
            foreach (KeyValuePair<string, string> item in postData)
            {
                s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", item.Key, item.Value);
            }
            s.Append("</form></body></html>");


            Response.ContentType = "text/html";
            //using (var sw = new StreamWriter(Response.Body))
            //{
            //   await sw.WriteAsync(s.ToString());
            //}
            var bytes = Encoding.UTF8.GetBytes(s.ToString());
            await Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// [智付通]金流介接(結果: 支付完成 返回商店網址)
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SpgatewayReturn()
        {
            //Request.LogFormData("SpgatewayReturn(支付完成)");

            // Status 回傳狀態 
            // MerchantID 回傳訊息
            // TradeInfo 交易資料AES 加密
            // TradeSha 交易資料SHA256 加密
            // Version 串接程式版本
            var collection = Request.Form;

            if (collection["MerchantID"].Count >0 && string.Equals(collection["MerchantID"], _bankInfoModel.MerchantID) &&
                collection["TradeInfo"] .Count>0 && string.Equals(collection["TradeSha"], CryptoUtil.EncryptSHA256($"HashKey={_bankInfoModel.HashKey}&{collection["TradeInfo"]}&HashIV={_bankInfoModel.HashIV}")))
            {
                var decryptTradeInfo = CryptoUtil.DecryptAESHex(collection["TradeInfo"], _bankInfoModel.HashKey, _bankInfoModel.HashIV);

                // 取得回傳參數(ex:key1=value1&key2=value2),儲存為NameValueCollection
                NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(decryptTradeInfo);
                SpgatewayOutputDataModel convertModel = LambdaUtil.DictionaryToObject<SpgatewayOutputDataModel>(decryptTradeCollection.AllKeys.ToDictionary(k => k, k => decryptTradeCollection[k]));

                
                if (convertModel.Status== "SUCCESS") {
                    var od = _db.OrderDetails.FirstOrDefault(x => x.OrderDetailId == Convert.ToInt32(convertModel.MerchantOrderNo));
                    od.PaymentStatus = true;
                    od.PayTime = Convert.ToDateTime(convertModel.PayTime);
                    var lecture = _db.Lectures.FirstOrDefault(x => x.LectureId == od.LectureId);
                    lecture.Count -= od.Count;
                    _db.SaveChanges();
                }

                
                return Content(JsonConvert.SerializeObject(convertModel));
            }

            //return Content(string.Empty);
            return RedirectToAction("Index", "Home");

        }

        /// <summary>
        /// [智付通]金流介接(結果: 支付通知網址)
        /// </summary>
        //[HttpPost]
        //public ActionResult SpgatewayNotify()
        //{
        //    // 取法同SpgatewayResult

        //    Request.LogFormData("SpgatewayNotify(支付通知)");
        //    return Content(string.Empty);
        //}


        /// <summary>
        /// 銀行API測試
        /// </summary>
        /// <returns></returns>
        public ActionResult RefundTest()
        {
            return View();
        }

    }
}
