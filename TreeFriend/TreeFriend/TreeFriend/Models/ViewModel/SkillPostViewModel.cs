using System.Collections.Generic;

namespace TreeFriend.Models.ViewModel {
    public class SkillPostViewModel {
        public int SkillPostId { get; set; }

        public int UserId { get; set; }
        //放使用者名稱，供前端使用
        public string UserName { get; set; }

        public string UserHeadshot { get; set; }
        public string Title { get; set; }

        public int CategoryId { get; set; }

        //存放Join後的分類名稱
        public string CategoryName { get; set; }

        public string Region { get; set; }

        public string Content { get; set; }

        //新增文章時使用到的標籤ID
        public int[] HashtagId { get; set; }

        //存放後端JOIN後的標籤名稱
        public string[] HashtagName { get; set; }

        public List<SkillPostMessageViewModel> Message { get; set; }

        //讓前端用的欄位，後端不需要使用
        public string LeaveMsg { get; set; }
    }
}
