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
        public IEnumerable<ShortSummaryOrderModel> GetOrders(int pageNumber, int pageSize, out int totalItems)
        {
            IEnumerable<ecom_Orders> orders = orderRepository.GetAllOrders();
            // Convert to view model
            IEnumerable<ShortSummaryOrderModel> orderList = from o in orders
                                                            select new ShortSummaryOrderModel()
                                                            {
                                                                Id = o.Id,
                                                                NameOfRecipient = o.NameOfRecipient,
                                                                OrderStatus = EnumHelper.GetDescriptionFromEnum((OrderStatus)o.OrderStatus),
                                                                PhoneOfRecipient = o.PhoneOfRecipient,
                                                                AddressOfRecipient = o.AddressOfRecipient,
                                                                CreatedDate = o.CreatedDate !=null?((DateTime)o.CreatedDate).ToString("dd/MM/yyyy hh:mm:ss"):""
                                                            };
            totalItems = orders.Count();
            IEnumerable<ShortSummaryOrderModel> returnOrdersList = orderList.OrderBy(b => b.OrderStatus).OrderBy(b => b.CreatedDate)
                .Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();

            return returnOrdersList;
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
        public bool UpdateOrder(UpdateOrderModel orderRequest)
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
                    Status = order.Status
                };

                return retValue;
            }
            else
            {
                return null;
            }
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
