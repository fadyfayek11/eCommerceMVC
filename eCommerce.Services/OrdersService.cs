using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Integrations.Model.Request;
using eCommerce.Integrations.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class OrdersService
    {
        #region Define as Singleton
        private static OrdersService _Instance;

        public static OrdersService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new OrdersService();
                }

                return (_Instance);
            }
        }

        private OrdersService()
        {
        }
        #endregion

        public bool SaveOrder(Order order)
        {
            var context = DataContextHelper.GetNewContext();

            context.Orders.Add(order);

            var isSaved = context.SaveChanges() > 0;

            if (isSaved)
            {
                SendToIntegration(order.OrderId);
            }

            return isSaved;
        }

        private void SendToIntegration(string OrderId)
        {
            Order order = GetOrderByOrderId(OrderId);

            if (order.OrderItems.ToList().Any(r => !string.IsNullOrEmpty(r.ProductType.SKU)))
            {
                var newRequset = new OrderRequest()
                {
                    id = order.ID.ToString(),
                    customer_id = 0,
                    order_date = DateTime.Now.ToString(),
                    order_state = "1",
                    reported = "0",
                    c_address = order.CustomerAddress,
                    c_city = order.CustomerCity,
                    c_country = order.CustomerCountry,
                    c_email = order.CustomerEmail,
                    c_name = order.CustomerName,
                    c_telephone = order.CustomerPhone,
                    total = order.FinalAmmount.ToString(),
                    vat_val = "0",
                    vat_per = "0",
                    emp_id = "0",
                    discount = "0",
                    paid = "0",
                    code_no = "0",
                    tax = "0",
                    user_id = "0",
                    is_trans = "0",
                    track_no = "0",
                    order_details = order.OrderItems.Where(r => !string.IsNullOrEmpty(r.ProductType.SKU)).Select(r => new OrderDetail()
                    {
                        item_amount = r.Quantity.ToString(),
                        item_price = r.ItemPrice.ToString(),
                        acc_id = r.ProductType.SKU.ToString(),
                        item_no = r.ProductType.ID.ToString(),
                        order_id = order.ID.ToString(),
                        x_vat = 0,
                        bonus = "0",
                        dis_count = "0",
                        id = r.ID.ToString(),

                    }).ToList(),



                };

                Task.Run(async () => await new ProductIntegrationService().SendOrder(newRequset));
            }
        }

        public Order GetOrderByID(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            return context.Orders.Include("OrderItems.Product.ProductRecords").FirstOrDefault(x => x.ID == ID);
        }
        public Order GetOrderByOrderId(string orderId)
        {
            var context = DataContextHelper.GetNewContext();

            return context.Orders.Include("OrderItems.ProductType.Product.ProductRecords").Include("OrderItems.ProductType.FeatureValue.Feature").FirstOrDefault(x => x.OrderId == orderId);
        }

        public List<Order> SearchOrders(string userID, string orderId, int? orderStatus, int? pageNo, int recordSize, out int count)
        {
            var context = DataContextHelper.GetNewContext();

            var orders = context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(userID))
            {
                orders = orders.Where(x => x.CustomerID.Equals(userID));
            }

            if (orderId != null && orderId.Length > 0)
            {
                orders = orders.Where(x => x.OrderId == orderId);
            }

            if (orderStatus.HasValue && orderStatus.Value > 0)
            {
                orders = orders.Where(x => x.OrderHistory.OrderByDescending(y => y.ModifiedOn).FirstOrDefault().OrderStatus == orderStatus);
            }

            count = orders.Count();

            pageNo = pageNo ?? 1;
            var skipCount = (pageNo.Value - 1) * recordSize;

            return orders.OrderByDescending(x => x.PlacedOn).Skip(skipCount).Take(recordSize).ToList();
        }

        public bool AddOrderHistory(OrderHistory orderHistory)
        {
            var context = DataContextHelper.GetNewContext();

            context.OrderHistories.Add(orderHistory);

            return context.SaveChanges() > 0;
        }
    }
}
