using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Web;

namespace RealEstateCRM
{
    public partial class Index : System.Web.UI.Page
    {
        ErrorFile err = new ErrorFile();
        string ErrorPath = string.Empty;
        GlobalData gc = new GlobalData();
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPath = Server.MapPath("ErrorLog.txt");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("select usr.UserId, r.RoleName from Roles as r, Users as usr where r.RoleId=usr.RoleId and usr.UserName='" + txtUsername.Text.Trim() + "' and usr.Password='" + txtPassword.Text.Trim() + "' and usr.Status='1'"))
                    {
                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Session["UserId"] = reader["UserId"].ToString();
                            Session["RoleName"] = reader["RoleName"].ToString();
                            if (reader["RoleName"].ToString() == "SuperAdmin")
                            {
                                Response.Redirect("~/Dashboard.aspx", false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                            else if (reader["RoleName"].ToString() == "Admin")
                            {
                                Response.Redirect("~/AdminDashboard.aspx", false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                            else if (reader["RoleName"].ToString() == "Accountant")
                            {
                                Response.Redirect("~/AccountantDashboard.aspx", false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
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
    }
}