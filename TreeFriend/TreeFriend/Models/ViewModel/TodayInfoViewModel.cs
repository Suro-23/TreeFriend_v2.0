using System.Collections.Generic;

namespace TreeFriend.Models.ViewModel {
    public class TodayInfoViewModel {
        public decimal Sales { get; set; }
        public int PostCount { get; set; }
        public int MemberCount { get; set; }
        public List<PostInfo> CategoryPostCount { get; set; } = new List<PostInfo>();
    }

    public class PostInfo {
        public string Category { get; set; }
        public int Count { get; set; }
    }
}
