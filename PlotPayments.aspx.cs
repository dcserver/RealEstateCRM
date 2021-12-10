using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;

namespace RealEstateCRM
{
    public partial class PlotPayments : System.Web.UI.Page
    {
        ErrorFile err = new ErrorFile();
        string ErrorPath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPath = Server.MapPath("ErrorLog.txt");
            try
            {
                if (Session["UserId"] != null)
                {
                    if (!IsPostBack)
                    {
                        BindData();
                    }
                }
                else
                {
                    Response.Redirect("~/Index.aspx");
                }
            }
            catch (Exception ex)
            {
                err.LogError(ex, ErrorPath);
                Response.Redirect("Error.aspx");
            }
        }
        private void BindData()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                string htmldata = string.Empty;
                htmldata += "<table class='table table-bordered table-striped mt-3' id='commissionTable'>" +
                    "<thead>" +
                        "<tr>" +
                        "<th>ProjectName</th>" +
                        "<th>PassbookNo</th>" +
                           "<th>ReceiptNo</th>" + "<th>Amount</th>" + "<th>Date</th>" + "<th>PaymentMethod</th>" +
                        "</tr>" +
                    "</thead><tbody>";
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("select cp.ReceiptNo,ps.PassbookNo,cp.Amount,cp.PaymentDate,cp.PaymentMethod,pr.ProjectName from PlotPayments cp, Projects pr, Passbook ps where pr.ProjectId=cp.ProjectId and ps.PassbookId=cp.PassbookNo"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    int index = i + 1;
                                    htmldata += "<tr>" +
                                        "<td>" + dt.Rows[i]["ProjectName"] + "</td>" +
                                        "<td>" + dt.Rows[i]["PassbookNo"] + "</td>" +
                                        "<td>" + dt.Rows[i]["ReceiptNo"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["Amount"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["PaymentDate"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["PaymentMethod"] + "</td>" +
                                    "</tr>";
                                }
                            }
                        }
                    }
                }
                htmldata += "</tbody></table>";
                htmlDiv.InnerHtml = htmldata;
            }
            catch (Exception ex)
            {
                err.LogError(ex, ErrorPath);
                Response.Redirect("Error.aspx");
            }
        }
    }
}