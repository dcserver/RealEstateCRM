using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace RealEstateCRM
{
    public partial class CancellationsList : System.Web.UI.Page
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
                htmldata += "<table class='table table-bordered table-striped mt-3' id='passbookList'>" +
                    "<thead>" +
                        "<tr>" +
                            "<th>PassbookNo</th>" +
                            "<th>PlotNo</th>" +
                            "<th>Project</th>" +
                            "<th>Date</th>" +
                            "<th>Refund Amount</th>" +
                            "<th>PaymentMethod</th>" +
                            "<th>Details</th>" +
                        "</tr>" +
                    "</thead><tbody>";
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.*,p.ProjectName,ps.PassbookNo,pt.PlotNo FROM Cancellations c, Projects p,Passbook ps,Plots pt where p.ProjectId=c.ProjectId and ps.PassbookId=c.PassbookId and ps.PlotId=pt.PlotId"))
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
                                    htmldata += "<tr> " +
                                                    "<td>" + dt.Rows[i]["PassbookNo"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["PlotNo"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["ProjectName"] + "</td>" +
                                                    "<td>" + Convert.ToDateTime(dt.Rows[i]["CancellationDate"]).ToString("dd/MM/yyyy") + "</td>" +
                                                    "<td>" + dt.Rows[i]["RefundAmount"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["PaymentMethod"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["PaymentDetails"] + "</td></tr>";
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