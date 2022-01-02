using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;

namespace RealEstateCRM
{
    public partial class CancellationDetails : System.Web.UI.Page
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
                    using (MySqlCommand cmd = new MySqlCommand("SELECT PassbookId,PassbookNo FROM Passbook where ProjectId='" + ddlProjects.SelectedValue + "'"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                ddlPassbook.Items.Clear();
                                sda.Fill(dt);
                                ddlPassbook.DataSource = dt;
                                ddlPassbook.DataTextField = "PassbookNo";
                                ddlPassbook.DataValueField = "PassbookId";
                                ddlPassbook.DataBind();
                                ddlPassbook.Items.Insert(0, "Please Select");
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
        protected void ddlPassbookNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        private void BindData()
        {
            string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("Select ps.*,pt.Status as PlotStatus,pt.PlotNo,pt.Facing,pt.Size as PlotSize,pr.ProjectName FROM Passbook ps, Plots pt,Projects pr where pr.ProjectId=ps.ProjectId and pt.PlotId=ps.PlotId and ps.PassbookId='" + ddlPassbook.SelectedValue + "'"))
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
                                hdnPlotId.Value = dt.Rows[0]["PlotId"].ToString();
                                txtPlotNo.Text = dt.Rows[0]["PlotNo"].ToString();
                                txtFacingCharges.Text = dt.Rows[0]["FacingCharges"].ToString();
                                txtPlotAmount.Text = dt.Rows[0]["PlotAmount"].ToString();
                                txtMaintainanceCharges.Text = dt.Rows[0]["Maintainance"].ToString();
                                txtName.Text = dt.Rows[0]["Name"].ToString();
                                txtMobile.Text = dt.Rows[0]["Mobile"].ToString();
                                txtDateOfJoin.Text = dt.Rows[0]["DateOfJoin"].ToString();
                                txtPendingAmount.Text= dt.Rows[0]["PendingAmount"].ToString();
                                txtTotalAmount.Text= dt.Rows[0]["TotalAmount"].ToString();
                                if (dt.Rows[0]["PendingAmount"].ToString() != null)
                                {
                                    txtPaidAmount.Text = (Convert.ToDouble(dt.Rows[0]["TotalAmount"].ToString()) - Convert.ToDouble(dt.Rows[0]["PendingAmount"].ToString())).ToString();
                                }
                                else
                                {
                                    txtPaidAmount.Text = "0";
                                }
                                BindPayments(dt.Rows[0]["PassbookId"].ToString(), dt.Rows[0]["ProjectId"].ToString());
                            }
                        }
                    }
                }
            }
        }
        private void BindPayments(string PassbookNo, string ProjectId)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                string htmldata = string.Empty;
                htmldata += "<table class='table table-bordered table-striped mt-3' id='commissionTable'>" +
                    "<thead>" +
                        "<tr>" +
                           "<th>Date</th>" +
                               "<th>ReceiptNo</th>" + "<th>Amount</th>" + "<th>PaymentMethod</th>" + "<th>PaymentDetails</th>" +
                        "</tr>" +
                    "</thead><tbody>";
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("Select * from PlotPayments where PassbookNo='" + PassbookNo + "' and ProjectId='" + ProjectId + "'"))
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
                                    htmldata += "<tr>" +
                                        "<td>" + dt.Rows[i]["PaymentDate"] + "</td>" +
                                        "<td>" + dt.Rows[i]["ReceiptNo"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["Amount"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["PaymentMethod"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["PaymentDetails"] + "</td>" +
                                    "</tr>";
                                }
                            }
                        }
                    }
                }
                htmldata += "</tbody></table>";
                htmlPDiv.InnerHtml = htmldata;
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
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Cancellations (PassbookId,CancellationDate,RefundAmount,PaymentMethod,PaymentDetails,ProjectId) " +
                        "VALUES (@PassbookId,@CancellationDate,@RefundAmount,@PaymentMethod,@PaymentDetails,@ProjectId)"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@PassbookId", ddlPassbook.SelectedValue);
                            cmd.Parameters.AddWithValue("@CancellationDate", DateTime.Now.ToString());
                            cmd.Parameters.AddWithValue("@RefundAmount", txtRefundAmount.Text);
                            cmd.Parameters.AddWithValue("@PaymentMethod", ddlPaymentMethod.SelectedValue);
                            cmd.Parameters.AddWithValue("@PaymentDetails", txtPaymentReference.Text);
                            cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            UpdatePlotData();
                            lblstatus.Text = "Passbook Cancelled Successfully";
                            lblstatus.ForeColor = Color.Green;
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
        private void UpdatePlotData()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("UPDATE Plots set Status=@Status where PlotId=@PlotId"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@Status", "A");
                            cmd.Parameters.AddWithValue("@PlotId", hdnPlotId.Value);
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
        protected void btnReset_Click(object sender, EventArgs e)
        {

        }
    }
}