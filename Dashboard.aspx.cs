using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace RealEstateCRM
{
    public partial class Dashboard : System.Web.UI.Page
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
                    BindProjects();
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
        private void BindProjects()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                string htmldata = string.Empty;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("select * from Projects"))
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
                                    htmldata += "<div class='col-lg-6 col-md-4 col-sm-6 col-12 mb-3'>" +
                "<div class='bg-white border shadow'>" +
                    "<div class='media p-4'>" +
                        "<div class='align-self-center mr-3 rounded-circle notify-icon bg-theme'>" +
                            "<i class='fa fa-building'></i>" +
                        "</div>" +
                        "<div class=media-body pl-2'>" +
                            "<h3 class='mt-0 mb-0'><strong>" +
                            "<strong><a href=PassbookList.aspx>" + dt.Rows[i]["ProjectName"] + "</a></strong></h3>" +
                        "</div>" +
                    "</div>" +
                "</div>" +
            "</div>";
                                }
                            }
                        }
                    }
                }

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