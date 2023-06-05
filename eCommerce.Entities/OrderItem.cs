using eCommerce.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderID { get; set; }

        [NotMapped]
        public string ProductName { get; set; }
        public int ProductTypeID { get; set; }
        public virtual ProductType ProductType { get; set; }

        /// <summary>
        /// Item Price is in Order Item because we can have a scenerio where we might charge less or greater than the Product Price.
        /// </summary>
        public decimal ItemPrice { get; set; }

        public int Quantity { get; set; }
    }
}
