using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;

namespace RealEstateCRM
{
    public partial class Plots : System.Web.UI.Page
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
                        if (Request.QueryString["Action"] == "Edit")
                        {
                            btnSubmit.Visible = false;
                            btnUpdate.Visible = true;
                            BindDataById(Request.QueryString["Id"]);
                        }
                        else if (Request.QueryString["Action"] == "Delete")
                        {
                            DeleteRecord(Request.QueryString["Id"]);
                        }
                        else
                        {
                            btnSubmit.Visible = true;
                            btnUpdate.Visible = false;
                            GeneratePlotNo();
                        }
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
        private void GeneratePlotNo()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT PlotNo FROM Plots WHERE PlotId=(SELECT MAX(PlotId) FROM Plots) and ProjectId='" + ddlProjects.SelectedValue + "'"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    int pNo = Convert.ToInt32(dt.Rows[0]["PlotNo"]) + 1;
                                    txtPlotNo.Text = pNo.ToString();
                                }
                                else
                                {
                                    txtPlotNo.Text = "1";
                                }
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
        private void BindProjects()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT ProjectName,ProjectId FROM Projects"))
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
                                ddlProjects.Items.Insert(0, "Please Select");
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
        private void DeleteRecord(string Id)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("DELETE FROM Plots WHERE PlotId = @PlotId"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@PlotId", Id);
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
        private void BindDataById(string Id)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM Plots where PlotId=@PlotId"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@PlotId", Id);
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    txtAmount.Text = dt.Rows[0]["Amount"].ToString();
                                    txtPlotNo.Text = dt.Rows[0]["PlotNo"].ToString();
                                    txtSize.Text = dt.Rows[0]["Size"].ToString();
                                    ddlStatus.SelectedValue = dt.Rows[0]["Status"].ToString();
                                    ddlProjects.SelectedValue = dt.Rows[0]["ProjectId"].ToString();
                                    txtMaintainance.Text = dt.Rows[0]["MaintainanceCharges"].ToString();
                                    txtFacingCharges.Text = dt.Rows[0]["FacingCharges"].ToString();
                                }
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
        private void BindData()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                string htmldata = string.Empty;
                htmldata += "<table class='table table-bordered table-striped mt-3' id='plotsList'>" +
                    "<thead>" +
                        "<tr>" +
                            "<th>Plot No</th>" +
                            "<th>Project</th>" +
                            "<th>Size</th>" +
                            "<th>Price</th>" +
                            "<th>MaintainanceCharges</th>" +
                            "<th>FacingCharges</th>" +
                            "<th>Status</th>" +
                            "<th class='align-middle text-center'>Edit</th>" +
                        "</tr>" +
                    "</thead><tbody>";
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("Select plo.*,pro.ProjectName as ProjectName from Plots AS plo INNER JOIN Projects AS pro ON plo.ProjectId = pro.ProjectId where plo.ProjectId='" + ddlProjects.SelectedValue + "'"))
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
                                                    "<td>" + dt.Rows[i]["PlotNo"] + "</td>";
                                    htmldata += "<td>" + dt.Rows[i]["ProjectName"] + "</td>";
                                    htmldata += "<td>" + dt.Rows[i]["Size"] + "</td>";
                                    htmldata += "<td>" + dt.Rows[i]["Amount"] + "</td>";
                                    htmldata += "<td>" + dt.Rows[i]["MaintainanceCharges"] + "</td>";
                                    htmldata += "<td>" + dt.Rows[i]["FacingCharges"] + "</td>";
                                    if (dt.Rows[i]["Status"].ToString() == "A")
                                    {
                                        htmldata += "<td>Available</td>";
                                    }
                                    else if (dt.Rows[i]["Status"].ToString() == "S")
                                    {
                                        htmldata += "<td>Sold</td>";
                                    }
                                    else if (dt.Rows[i]["Status"].ToString() == "P")
                                    {
                                        htmldata += "<td>Pending</td>";
                                    }
                                    else
                                    {
                                        htmldata += "<td>Registered</td>";
                                    }

                                    htmldata += "<td class='align-middle text-center'>" +
                                    "<a href=Plots.aspx?Id=" + dt.Rows[i]["PlotId"] + "&Action=Edit class='btn btn-link text-theme p-1'><i class='fa fa-pencil'></i></button>" +
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
        private void Reset()
        {
            txtPlotNo.Text = string.Empty;
            txtSize.Text = string.Empty;
            txtAmount.Text = string.Empty;
            ddlStatus.SelectedValue = "Available";
            ddlFacing.SelectedValue = "East";
            ddlProjects.SelectedValue = "0";
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Plots (ProjectId, Status,PlotNo,Size,Amount,Facing,MaintainanceCharges,FacingCharges) VALUES (@ProjectId, @Status,@PlotNo,@Size,@Amount,@Facing,@MaintainanceCharges,@FacingCharges)"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                            cmd.Parameters.AddWithValue("@PlotNo", txtPlotNo.Text.Trim());
                            cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                            cmd.Parameters.AddWithValue("@Size", txtSize.Text);
                            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                            cmd.Parameters.AddWithValue("@Facing", ddlFacing.SelectedValue);
                            cmd.Parameters.AddWithValue("@MaintainanceCharges", txtMaintainance.Text);
                            cmd.Parameters.AddWithValue("@FacingCharges", txtFacingCharges.Text);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            lblstatus.Text = "Plot Added Successfully";
                            lblstatus.ForeColor = Color.Green;
                        }
                    }
                }
                BindData();
                Reset();
            }
            catch (Exception ex)
            {
                err.LogError(ex, ErrorPath);
                Response.Redirect("Error.aspx");
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                string Name = txtPlotNo.Text.Trim();

                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("UPDATE Plots SET ProjectId=@ProjectId,Status=@Status,PlotNo=@PlotNo,Size=@Size,Amount=@Amount,Facing=@Facing,MaintainanceCharges=@MaintainanceCharges,FacingCharges=@FacingCharges WHERE PlotId=@PlotId"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                            cmd.Parameters.AddWithValue("@PlotNo", txtPlotNo.Text.Trim());
                            cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                            cmd.Parameters.AddWithValue("@Size", txtSize.Text);
                            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                            cmd.Parameters.AddWithValue("@Facing", ddlFacing.SelectedValue);
                            cmd.Parameters.AddWithValue("@MaintainanceCharges", txtMaintainance.Text);
                            cmd.Parameters.AddWithValue("@FacingCharges", txtFacingCharges.Text);
                            cmd.Parameters.AddWithValue("@PlotId", Request.QueryString["Id"]);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            lblstatus.Text = "Plot Updated Successfully";
                            lblstatus.ForeColor = Color.Green;
                            Response.Redirect("~/Plots.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
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

        protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
            GeneratePlotNo();
        }
    }
}