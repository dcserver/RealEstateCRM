using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace RealEstateCRM
{
    public partial class PassbookList : System.Web.UI.Page
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
                        if (Request.QueryString["Action"] == "Delete")
                        {
                            DeleteRecord(Request.QueryString["Id"]);
                        }
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
        private void DeleteRecord(string Id)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("DELETE FROM Passbook WHERE PassbookId = @PassbookId"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@PassbookId", Id);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
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
                            "<th>Project</th>" +
                            "<th>DateOfJoin</th>" +
                            "<th>LastDate</th>" +
                            "<th>PlotPrice</th>" +
                            "<th>TotalAmount</th>" +
                            "<th>Pending(INR)</th>" +
                            "<th class='align-middle text-center'>View</th>" +
                        "</tr>" +
                    "</thead><tbody>";
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("Select ps.*,pt.Status as PlotStatus,pr.ProjectName as ProjectName FROM Passbook ps, Plots pt,Projects pr where pt.PlotId=ps.PlotId and pt.ProjectId=pr.ProjectId  order by DateOfJoin desc"))
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
                                                    "<td>" + dt.Rows[i]["ProjectName"] + "</td>" +
                                                    "<td>" + Convert.ToDateTime(dt.Rows[i]["DateOfJoin"]).ToString("dd/MM/yyyy") + "</td>" +
                                                    "<td>" + Convert.ToDateTime(dt.Rows[i]["PaymentLastDate"]).ToString("dd/MM/yyyy") + "</td>" +
                                                    "<td>" + dt.Rows[i]["PlotAmount"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["TotalAmount"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["PendingAmount"] + "</td>" +
                                                    "<td class='align-middle text-center'>" +
                                    "<a href=PassbookView.aspx?Id=" + dt.Rows[i]["PassbookId"] + " class='btn btn-link text-theme p-1'><i class='fa fa-eye'></i></button>" +
                                "</td></tr>";
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