using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreeFriend.Models.Bank
{
    /// <summary>執行結果基元</summary>
    public class Atom
    {
        /// <summary>是否成功</summary>
        public bool IsSuccess { get; set; }

        /// <summary>訊息</summary>
        public string Message { get; set; }
    }
}
