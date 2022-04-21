using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.Entity
{
    public class PersonalPostImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonalPostImageId { get; set; }
        [Required]
        public string PostPhotoPath { get; set; }
        public DateTime CreateDate { get; set; }
        [ForeignKey("PersonalPost")]
        [Required]
        public int PersonalPostId { get; set; }
        public virtual PersonalPost PersonalPost { get; set; }

    }
}
