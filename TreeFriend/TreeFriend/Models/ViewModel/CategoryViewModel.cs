namespace TreeFriend.Models.ViewModel {
    public class CategoryViewModel {
        //判斷進來的為類別還是標籤
        //1:類別 2:標籤
        public int id { get; set; }

        //修改時會用到的ID
        public int cId { get; set; }
        public string input { get; set; }
    }
}
