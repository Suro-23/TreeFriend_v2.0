using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.ViewModel
{
    public class PersonalPostMessageViewModel
    {

        public int PersonalPostId { get; set; }
        public string UserMessage { get; set; }
        public string HeadshotPath { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
