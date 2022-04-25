using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.ViewModel
{
    public class SystemDetailViewModel
    {
        public int ArticleID { get; set; }
        public String Title { get; set; }
        public String CreateDate { get; set; }
        public String Description { get; set; }
        public String PicPath { get; set; }  //如果傳的是檔案型別 要用 IFromFile 
                                             //用List<> 來裝 因為不知道會送多少張照片進來
        public string UserId { get; internal set; }
    }
}
