using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Integrations.Model.Response
{
    public class ProductResponse
    {
        public String status;
        public List<List<String>> note;
        public List<Table> table;
    }

    public class ProductPrice
    {
        public String pprice;
        public String price_id;
    }



    public class Table
    {
        public String id;
        public String class_id;
        public String name;
        public String price;
        public String barcode;
        public String item_img;
        public String item_pdf;
        public List<ProductPrice> product_prices;
        public String unit;
        public String tax_class;
        public String company_id;
        public String trade_id;
        public String style_id;
        public String model_id;
        public String color_id;
        public String note;
        public String bonus_group;
        public String name_e;
        public int cost;
        public int amount;
        public List<Object> active_units;
        public List<Object> measure;
    }
}
