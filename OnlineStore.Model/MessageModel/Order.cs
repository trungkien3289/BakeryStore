using System;
using System.Collections.Generic;
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
        public IEnumerable<CheckOutProductItem> Products { get; set; }
    }
    public class CheckOutProductItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
