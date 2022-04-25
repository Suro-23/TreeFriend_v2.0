using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.ViewModel
{
    public class PersonalPostRenderViewModel
    {
        public int PersonalPostId { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public List<string> PostPhotoPath { get; set; }
        public List<PersonalPostMessageViewModel> Message { get; set; }
        public bool State { get; set; }
        public string HeadshotPath { get; set; } 
        public string UserName { get; set; }
        public string SelfIntrodution { get; set; }
        public int UserId { get; set; }








    }
}
