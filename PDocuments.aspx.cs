using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.IO;

namespace RealEstateCRM
{
    public partial class PDocuments : System.Web.UI.Page
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
                        BindProjects();
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
        private void BindProjects()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM Projects"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ddlProjects.DataSource = dt;
                                ddlProjects.DataTextField = "ProjectName";
                                ddlProjects.DataValueField = "ProjectId";
                                ddlProjects.DataBind();
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
        protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPassbookNo();
        }
        private void BindPassbookNo()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT PassbookId,PassbookNo FROM Passbook where ProjectId='"+ddlProjects.SelectedValue+"'"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ddlPassbookNo.DataSource = dt;
                                ddlPassbookNo.DataTextField = "PassbookNo";
                                ddlPassbookNo.DataValueField = "PassbookId";
                                ddlPassbookNo.DataBind();
                                ddlPassbookNo.Items.Insert(0, "Please Select");
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = Path.GetFileName(FUPDocument.PostedFile.FileName);
                FileInfo fi = new FileInfo(filename);
                string extn = fi.Extension;
                string dynamicdocumentname = DateTime.Now.ToString("hhmmssffffff") + extn;
                FUPDocument.SaveAs(Server.MapPath("PDocuments/" + dynamicdocumentname));
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO PassbookDocuments (PassbookNo, DocumentName,DocumentUrl,ProjectId) VALUES (@PassbookNo,@DocumentName,@DocumentUrl,@ProjectId)"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbookNo.SelectedValue);
                            cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                            cmd.Parameters.AddWithValue("@DocumentName", txtDocumentName.Text);
                            cmd.Parameters.AddWithValue("@DocumentUrl", dynamicdocumentname);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                BindData();
            }
            catch (Exception ex)
            {
                err.LogError(ex, ErrorPath);
                Response.Redirect("Error.aspx");
            }
        }
        protected void ddlPassbookNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
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
                           "<th>No</th>" +
                               "<th>DocumentName</th>" + "<th>View</th>" +
                        "</tr>" +
                    "</thead><tbody>";
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("Select * from PassbookDocuments where PassbookNo = '" + ddlPassbookNo.SelectedValue + "'"))
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
                                                    "<td>" + index + "</td>" +
                                                    "<td>" + dt.Rows[i]["DocumentName"] + "</td>" +
                                                    "<td><a href='PDocuments/" + dt.Rows[i]["DocumentUrl"] + "' target='_blank' class='btn btn-link text-theme p-1'><i class='fa fa-download'></i></a></td>" +
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