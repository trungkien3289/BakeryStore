using OnlineStore.Infractructure.Utility;
using OnlineStore.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Model.Repository
{
    public class OrderRepository : Repository<ecom_Orders>
    {
        public OrderRepository(OnlineStoreMVCEntities context)
            : base(context)
        {

        }

        public IEnumerable<ecom_Orders> GetAllOrders()
        {
            return dbSet.Where(o => o.Status == (int)Define.Status.Active).ToList();
        }

    }
}
