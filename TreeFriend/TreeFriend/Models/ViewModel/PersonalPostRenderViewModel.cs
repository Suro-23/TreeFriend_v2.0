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
        public object Message { get; set; }
        public bool State { get; set; }





    }
}
