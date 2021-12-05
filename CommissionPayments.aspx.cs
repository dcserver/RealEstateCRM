using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace RealEstateCRM
{
    public partial class CommissionPayments : System.Web.UI.Page
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
                           "<th>VoucherNo</th>" +
                               "<th>Marketer</th>" +"<th>Amount</th>" + "<th>Date</th>" + "<th>PaymentMethod</th>" +
                        "</tr>" +
                    "</thead><tbody>";
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("select cp.VoucherNo,cp.MarketerName,cp.PassbookNo,cp.Amount,cp.PaymentDate,cp.PaymentMethod,pr.ProjectName from CommissionPayments cp, Projects pr where pr.ProjectId=cp.ProjectId"))
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
                                        "<td>" + dt.Rows[i]["VoucherNo"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["MarketerName"] + "</td>" +                                                    
                                                    "<td>" + dt.Rows[i]["Amount"] + "</td>" +
                                                    "<td>" + Convert.ToDateTime(dt.Rows[i]["PaymentDate"]).ToString("dd/MM/yyyy") + "</td>" +
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