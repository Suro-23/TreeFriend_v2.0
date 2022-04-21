using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.ViewModel
{
    public class EditPostViewModel
    {
        public int PersonalPostId { get; set; }

        [Required]
        public string Content { get; set; }


        //使用getdate()
        public DateTime CreateDate { get; set; }
    }
}
