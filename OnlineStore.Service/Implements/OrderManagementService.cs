using OnlineStore.Model.Context;
using OnlineStore.Model.MessageModel;
using OnlineStore.Model.Repository;
using OnlineStore.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineStore.Infractructure.Utility;
using OnlineStore.Model.ViewModel;
using OnlineStore.Infractructure.Helper;
using LinqKit;

namespace OnlineStore.Service.Implements
{
    public class OrderManagementService : IOrderManagementService
    {
        #region Properties

        private static OnlineStoreMVCEntities context;
        private OrderRepository orderRepository;
        private ProductRepository productRepository;

        #endregion

        #region Constructor
        public OrderManagementService()
        {
            context = new OnlineStoreMVCEntities();
            orderRepository = new OrderRepository(context);
            productRepository = new ProductRepository(context);
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        public bool CreateOrder(RequestCreateOrderModel orderRequest)
        {
            try
            {
                ecom_Orders order = new ecom_Orders()
                {
                    OrderCode = new Guid().ToString(),
                    FeeShip = 0,
                    OrderStatus = (int)OrderStatus.NotDeliver,
                    OrderNote = orderRequest.OrderNote,
                    NameOfRecipient = orderRequest.NameOfRecipient,
                    PhoneOfRecipient = orderRequest.PhoneOfRecipient,
                    AddressOfRecipient = orderRequest.AddressOfRecipient,
                    Status = (int)OnlineStore.Infractructure.Utility.Define.Status.Active,
                    CreatedDate = DateTime.Now,
                    ecom_OrderDetails = (from product in orderRequest.Products
                                         select new ecom_OrderDetails()
                                         {
                                             ProductID = product.Id,
                                             Quantity = product.Quantity,
                                             PriceOfUnit = productRepository.GetByID(product.Id).Price,
                                             Status = (int)OnlineStore.Infractructure.Utility.Define.Status.Active,
                                             CreatedDate = DateTime.Now
                                         }).ToList()
                };

                orderRepository.Insert(order);
                orderRepository.Save();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Get orders list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ShortSummaryOrderModel> GetOrders(int pageNumber, int pageSize, Nullable<DateTime> startDate, Nullable<DateTime> endDate, out int totalItems)
        {
            var searchQuery = PredicateBuilder.True<ecom_Orders>();

            if (startDate != null)
            {
                searchQuery = searchQuery.And(p => p.CreatedDate >= (DateTime)startDate);
            }
            if (endDate != null)
            {
                searchQuery = searchQuery.And(p => p.CreatedDate <= (DateTime)endDate);
            }

            IEnumerable<ecom_Orders> ordersMatchingRefinement = orderRepository.Get(
                filter: searchQuery, includeProperties: "");
                 totalItems = ordersMatchingRefinement.Count();
                 ordersMatchingRefinement = ordersMatchingRefinement.OrderBy(o => o.CreatedDate).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();

            // Convert to view model
            IEnumerable<ShortSummaryOrderModel> orderList = from o in ordersMatchingRefinement
                                                            select new ShortSummaryOrderModel()
                                                            {
                                                                Id = o.Id,
                                                                NameOfRecipient = o.NameOfRecipient,
                                                                OrderStatus = EnumHelper.GetDescriptionFromEnum((OrderStatus)o.OrderStatus),
                                                                PhoneOfRecipient = o.PhoneOfRecipient,
                                                                AddressOfRecipient = o.AddressOfRecipient,
                                                                CreatedDate = o.CreatedDate != null ? ((DateTime)o.CreatedDate).ToString("dd/MM/yyyy hh:mm:ss") : "",
                                                                DeliveryDateString = o.DeliveryDate != null ? ((DateTime)o.DeliveryDate).ToString("dd/MM/yyyy") : "",
                                                                DeliveryDate = o.DeliveryDate
                                                            };
           

            return orderList;
        }

        /// <summary>
        /// Delete Order
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>return true if delete order success, else return false</returns>
        public bool Delete(int id)
        {
            try
            {
                var selectedOrder = orderRepository.GetByID(id);
                if (selectedOrder == null)
                {
                    return false;
                }
                else
                {
                    selectedOrder.Status = (int)Define.Status.Delete;
                    orderRepository.Update(selectedOrder);
                    orderRepository.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Get Order Details
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns></returns>
        public ecom_Orders GetOrderById(int id)
        {
            var order = orderRepository.GetByID(id);

            return order;
        }

        /// <summary>
        /// Get Order Details
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public OrderSumaryModel GetOrderDetails(int id)
        {

            ecom_Orders order = orderRepository.GetByID(id);

            OrderSumaryModel retValue = new OrderSumaryModel()
            {
                Id = order.Id,
                NameOfRecipient = order.NameOfRecipient,
                OrderCode = order.OrderCode,
                PhoneOfRecipient = order.PhoneOfRecipient,
                AddressOfRecipient = order.AddressOfRecipient,
                FeeShip = order.FeeShip,
                OrderNote = order.OrderNote,
                OrderStatus = EnumHelper.GetDescriptionFromEnum((OrderStatus)order.OrderStatus),
                CreatedDate = order.CreatedDate.ToString(),
                OrderDetailsList = from orderDetails in order.ecom_OrderDetails
                                   select new OrderDetailsSumaryModel()
                                   {
                                       ProductName = orderDetails.ecom_Products.Name,
                                       PriceOfUnit = orderDetails.PriceOfUnit,
                                       Quantity = orderDetails.Quantity,
                                       TotalDiscount = orderDetails.TotalDiscount,
                                       CoverImagePath = orderDetails.ecom_Products.CoverImage.ImagePath
                                   }
            };

            return retValue;
            
        }

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        public bool UpdateOrder(ecom_Orders orderRequest)
        {
            try
            {
                ecom_Orders order = orderRepository.GetByID(orderRequest.Id);
                if (order != null)
                {
                    order.FeeShip = orderRequest.FeeShip;
                    order.AddressOfRecipient = orderRequest.AddressOfRecipient;
                    order.NameOfRecipient = orderRequest.NameOfRecipient;
                    order.OrderNote = orderRequest.OrderNote;
                    order.PhoneOfRecipient = orderRequest.PhoneOfRecipient;
                    order.Status = orderRequest.Status;
                    order.OrderStatus = orderRequest.OrderStatus;
                    order.DeliveryDate = orderRequest.DeliveryDate;
                    orderRepository.Update(order);
                    orderRepository.Save();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public UpdateOrderModel getOrderForEdit(int id)
        {
            UpdateOrderModel retValue;
            ecom_Orders order = orderRepository.GetByID(id);
            if (order != null)
            {
                retValue = new UpdateOrderModel()
                {
                    Id = order.Id,
                    AddressOfRecipient = order.AddressOfRecipient,
                    FeeShip = order.FeeShip,
                    NameOfRecipient = order.NameOfRecipient,
                    OrderNote = order.OrderNote,
                    OrderStatus = order.OrderStatus,
                    PhoneOfRecipient = order.PhoneOfRecipient,
                    Status = order.Status,
                    DeliveryDate = order.DeliveryDate!=null?((DateTime)order.DeliveryDate).ToString("dd/MM/yyyy"):""
                };

                return retValue;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get sale statistics after products
        /// </summary>
        /// <param name="fromDate">filter from date</param>
        /// <param name="toDate">filter to date</param>
        /// <returns></returns>
        public IList<SaleStatisticsModel> getSaleStatisticsAfterProduct(int pageNumber,int pageSize, Nullable<DateTime> fromDate, Nullable<DateTime> toDate, out int totalItems)
        {
            var searchQuery = PredicateBuilder.True<ecom_Orders>();

            if (fromDate != null)
            {
                searchQuery = searchQuery.And(p => p.CreatedDate >= (DateTime)fromDate);
            }
            if (toDate != null)
            {
                searchQuery = searchQuery.And(p => p.CreatedDate < (DateTime)toDate);
            }

            IList<SaleStatisticsModel> productList;

            using (var db = new OnlineStoreMVCEntities())
            {
                productList = db.ecom_Orders.Include("ecom_OrderDetails").AsExpandable().Where(searchQuery)
                   .SelectMany(o => o.ecom_OrderDetails)
                   .GroupBy(od => new {od.ProductID,
                                    od.PriceOfUnit})
                   .Select(gr => new SaleStatisticsModel() { 
                    ProductId = gr.Key.ProductID,
                    UnitPrice = gr.Key.PriceOfUnit,
                    Quantity = gr.Count()
                   }).ToList();

                foreach (var item in productList)
                {
                    var productDetails = productRepository.GetByID(item.ProductId);
                    item.ProductName = productDetails.Name;
                    item.ImagePath = productDetails.CoverImage.ImagePath;
                }
            }
            totalItems = productList.Count();
            return productList.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
        }

        #endregion

        #region Release resources

        /// <summary>
        /// Dispose database connection using in repositories, which used in this service
        /// </summary>
        public void Dispose()
        {
            orderRepository.Dispose();
            productRepository.Dispose();
        }

        #endregion

        
    }
}
