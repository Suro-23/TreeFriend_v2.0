using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.Enum
{
    public enum LectureStatus
    {
        [Display(Name = "未審核")]
        Unapproved,
        [Display(Name = "已上架")]
        Launched,
        [Display(Name = "未上架")]
        NotSold
    }
}
