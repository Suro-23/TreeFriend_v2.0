using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreeFriend.Models.Entity {
    public class HashtagDetail {
        [ForeignKey("SkillPost")]
        [Required]
        public int SkillPostId { get; set; }

        [ForeignKey("Hashtag")]
        [Required]
        public int HashtagId { get; set; }

        public virtual SkillPost SkillPost { get; set; }
        public virtual Hashtag Hashtag { get; set; }
    }
}
