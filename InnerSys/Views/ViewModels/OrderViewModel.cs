using InnerSys.Db;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InnerSys.Views.ViewModels
{
    public class OrderViewModel
    {
        public int Desk { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}