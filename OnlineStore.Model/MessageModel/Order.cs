using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Model.MessageModel
{
    public class CheckOutModelRequest
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public IEnumerable<OrderProductItem> Products { get; set; }
    }
    public class OrderProductItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }

    public class RequestCreateOrderModel
    {
        public decimal FeeShip { get; set; }
        public string OrderNote { get; set; }
        public string NameOfRecipient { get; set; }
        public string PhoneOfRecipient { get; set; }
        public string AddressOfRecipient { get; set; }
        public virtual ICollection<OrderProductItem> Products { get; set; }
    }


    public class UpdateOrderModel
    {
        [DisplayName("Order Id")]
        [Required]
        public int Id { get; set; }
        [DisplayName("Phí vận chuyển")]
        [Required]
        public decimal FeeShip { get; set; }
        [DisplayName("Trạng thái đơn hàng")]
        [Required]
        public int OrderStatus { get; set; }
        [DisplayName("Ghi chú")]
        public string OrderNote { get; set; }
        [DisplayName("Tên khách hàng")]
        [Required]
        public string NameOfRecipient { get; set; }
        [DisplayName("Số điện thoại")]
        [Required]
        public string PhoneOfRecipient { get; set; }
        [DisplayName("Địa chỉ")]
        [Required]
        public string AddressOfRecipient { get; set; }
        [DisplayName("Trạng thái")]
        [Required]
        public Nullable<int> Status { get; set; }

    }

}
