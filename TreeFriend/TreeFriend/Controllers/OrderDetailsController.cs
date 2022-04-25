using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeFriend.Models;
using TreeFriend.Models.ViewModel;

namespace TreeFriend.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly TreeFriendDbContext _db;
        public OrderDetailsController(TreeFriendDbContext db)
        {
            _db = db;
        }

       public List<OrderHistoryViewModel> GetAllOrders()
        {
            var result = _db.OrderDetails.OrderByDescending(od => od.CreateDate).Select(od => new OrderHistoryViewModel {
                OrderDetailId = od.OrderDetailId,
                CreateDate = od.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                TotoalAmount = Convert.ToInt32(od.Price * od.Count),
                PayTime = od.PayTime.HasValue ? od.PayTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                PaymentStatus = od.PaymentStatus,
                OrderStatus = od.OrderStatus,
                Theme = od.Lecture.Theme,
                EventDate = od.Lecture.EventDate.ToString("yyyy-MM-dd"),
                EventTimeStart = od.Lecture.EventTimeStart.ToString("HH:mm"),
                EventTimeEnd = od.Lecture.EventTimeEnd.ToString("HH:mm"),
                Venue = od.Lecture.Venue,
                Price = od.Price,
                Count = od.Count,
                UserName=od.User.UserDetail.UserName,

            }).ToList();

            return result;
        }

        public List<OrderHistoryViewModel> GetOrderByDescendingOrders()
        {//.Where(od=>od.OrderStatus==true).
            var result = _db.OrderDetails.OrderByDescending(od=>od.Price*od.Count).Select(od => new OrderHistoryViewModel
            {
                OrderDetailId = od.OrderDetailId,
                CreateDate = od.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                TotoalAmount = Convert.ToInt32(od.Price * od.Count),
                PayTime = od.PayTime.HasValue ? od.PayTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                PaymentStatus = od.PaymentStatus,
                OrderStatus = od.OrderStatus,
                Theme = od.Lecture.Theme,
                EventDate = od.Lecture.EventDate.ToString("yyyy-MM-dd"),
                EventTimeStart = od.Lecture.EventTimeStart.ToString("HH:mm"),
                EventTimeEnd = od.Lecture.EventTimeEnd.ToString("HH:mm"),
                Venue = od.Lecture.Venue,
                Price = od.Price,
                Count = od.Count,
                UserName = od.User.UserDetail.UserName,

            }).ToList();
            return result;
        }

    }
}
