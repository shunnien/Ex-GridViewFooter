using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace GridViewFooter
{
    public partial class GridViewFooter1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GVBind();
            }
        }

        //資料連結
        protected void GVBind()
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString))
            {
                string str = "SELECT TOP 10 OrderID,CustomerID,Freight,ShipName from Orders";
                using (DbDataAdapter da = new SqlDataAdapter(str, conn.ConnectionString))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    GridView1.DataSource = dt;
                    GridView1.ShowFooter = true;
                    GridView1.DataBind();
                }
            }
        }

        decimal _totalFreight = 0m;
        int _totalCount = 0;
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //選擇列時變色
            if (e.Row.RowIndex > -1)
            {
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#e0e0ff'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c;");
            }
            
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    e.Row.Cells[0].Text = "訂單編號";
                    e.Row.Cells[1].Text = "客戶編號";
                    e.Row.Cells[2].Text = "貨物";
                    e.Row.Cells[3].Text = "收貨人";
                    break;
                case DataControlRowType.DataRow:
                    _totalFreight += decimal.Parse(e.Row.Cells[2].Text);
                    _totalCount++;
                    break;
                case DataControlRowType.Footer:
                    // 合計值的呈現
                    e.Row.Cells[1].Text = "合計";
                    e.Row.Cells[2].Text = _totalFreight.ToString();
                    _totalFreight = 0;
                    _totalCount = 0;
                    break;
            }

        }
    }
}