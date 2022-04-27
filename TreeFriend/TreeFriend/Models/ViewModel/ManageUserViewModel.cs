using System;

namespace TreeFriend.Controllers.Api {
    public class ManageUserViewModel {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public bool Status { get; set; }
        public string Headshot { get; set; }
        public string Sex { get; set; }
        public DateTime Birthday { get; set; }
        public string SelfIntro { get; set; }
        public int PostCount { get; set; }
        public decimal TotalAmount { get; set; }
        public string TotalTime { get; set; }
    }
}