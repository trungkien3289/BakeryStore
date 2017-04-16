using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Model.ViewModel
{

    public class OrderSumaryModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        [DisplayName("Phí vận chuyển")]
        public decimal FeeShip { get; set; }
        [DisplayName("Trạng thái đơn hàng")]
        public string OrderStatus { get; set; }
        [DisplayName("Ghi chú")]
        public string OrderNote { get; set; }
        [DisplayName("Tên khách hàng")]
        public string NameOfRecipient { get; set; }
         [DisplayName("Số điện thoại")]
        public string PhoneOfRecipient { get; set; }
         [DisplayName("Địa chỉ")]
        public string AddressOfRecipient { get; set; }
         [DisplayName("Ngày tạo")]
        public string CreatedDate { get; set; }

        public virtual IEnumerable<OrderDetailsSumaryModel> OrderDetailsList { get; set; }
    }

    public class OrderDetailsSumaryModel
    {
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }
        [DisplayName("Giá mỗi đơn vị")]
        public decimal PriceOfUnit { get; set; }
        [DisplayName("Discount")]
        public Nullable<decimal> TotalDiscount { get; set; }
        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }
        public string CoverImagePath { get; set; }
    }

    public class ShortSummaryOrderModel
    {
        public int Id { get; set; }
        [DisplayName("Delivery Status")]
        public string OrderStatus { get; set; }
        [DisplayName("Recipient Name")]
        public string NameOfRecipient { get; set; }
        [DisplayName("Recipient Phone")]
        public string PhoneOfRecipient { get; set; }
        [DisplayName("Recipient Address")]
        public string AddressOfRecipient { get; set; }
        [DisplayName("Created Date")]
        public string CreatedDate { get; set; }
    }

}
