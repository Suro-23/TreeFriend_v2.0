using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.ViewModel
{
    public class PersonalPostViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonalPostId { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public List<IFormFile> Pic { get; set; }


        //使用getdate()
        public DateTime CreateDate { get; set; }
    }
}
