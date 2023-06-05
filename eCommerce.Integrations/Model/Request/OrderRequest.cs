using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Integrations.Model.Request
{
    

        public class OrderDetail
        {
            public string id { get; set; }
            public string order_id { get; set; }
            public string item_no { get; set; }
            public string item_amount { get; set; }
            public string item_price { get; set; }
            public object note { get; set; }
            public object new_amount { get; set; }
            public string dis_count { get; set; }
            public string bonus { get; set; }
            public string color { get; set; }
            public string unit_id { get; set; }
            public string measure_id { get; set; }
            public string acc_id { get; set; }
            public object color_details { get; set; }
            public int x_vat { get; set; }
        }

        public class OrderRequest
        {
            public string id { get; set; }
            public int customer_id { get; set; }
            public string order_date { get; set; }
            public string order_state { get; set; }
            public string reported { get; set; }
            public string total { get; set; }
            public string new_total { get; set; }
            public string emp_id { get; set; }
            public string vat_per { get; set; }
            public string vat_val { get; set; }
            public object note_d { get; set; }
            public object d_date { get; set; }
            public object d_place { get; set; }
            public object d_note { get; set; }
            public string discount { get; set; }
            public string c_name { get; set; }
            public string c_telephone { get; set; }
            public object c_email { get; set; }
            public string c_address { get; set; }
            public string c_city { get; set; }
            public object c_country { get; set; }
            public string code_no { get; set; }
            public string paid { get; set; }
            public string tax { get; set; }
            public string user_id { get; set; }
            public string is_trans { get; set; }
            public string track_no { get; set; }
            public object shipment_track { get; set; }
            public List<OrderDetail> order_details { get; set; }
        }
    
}
