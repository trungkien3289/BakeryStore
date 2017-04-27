using OnlineStore.Model.Context;
using OnlineStore.Model.MessageModel;
using OnlineStore.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Service.Interfaces
{
    public interface IOrderManagementService:IDisposable
    {
        /// <summary>
        /// Create order
        /// </summary>
        /// <param name="order">order details</param>
        /// <returns>return true if add order success, if not return false</returns>
        bool CreateOrder(RequestCreateOrderModel order);

        /// <summary>
        /// Get list orders 
        /// </summary>
        /// <param name="pageNumber">current page</param>
        /// <param name="pageSize">number of order on each page</param>
        /// <param name="totalItems">number total items</param>
        /// <returns></returns>
        IEnumerable<ShortSummaryOrderModel> GetOrders(int pageNumber, int pageSize, Nullable<DateTime> startDate, Nullable<DateTime> endDate, out int totalItems);

        /// <summary>
        /// Delete Order
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>return true if delete order success, else return false</returns>
        bool Delete(int id);

        /// <summary>
        /// Get Order Details
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns></returns>
        ecom_Orders GetOrderById(int id);

        /// <summary>
        /// Get Order details
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        OrderSumaryModel GetOrderDetails(int p);

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        bool UpdateOrder(UpdateOrderModel orderRequest);

        /// <summary>
        /// Get order object by id for edit
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        UpdateOrderModel getOrderForEdit(int p);

        IList<SaleStatisticsModel> getSaleStatisticsAfterProduct(int page,int pageSize, Nullable<DateTime> fromDate, Nullable<DateTime> toDate,out int totalItems);
    }
}
