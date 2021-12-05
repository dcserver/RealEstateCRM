using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace RealEstateCRM
{
    public partial class PlotPaymentDetails : System.Web.UI.Page
    {
        ErrorFile err = new ErrorFile();
        string ErrorPath = string.Empty;
        GlobalData gc = new GlobalData();
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
            BindPassbook();
        }
        private void BindPassbook()
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
        private void GenerateReceiptNo()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT ReceiptNo FROM PlotPayments Where ProjectId='" + ddlProjects.SelectedValue + "' ORDER BY 1 DESC LIMIT 1"))
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
                                    int pNo = Convert.ToInt32(dt.Rows[0]["ReceiptNo"]) + 1;
                                    txtReceiptNo.Text = pNo.ToString();
                                }
                                else
                                {
                                    txtReceiptNo.Text = "1";
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
        protected void ddlPassbook_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
            BindPlotAmounts();
            GenerateReceiptNo();
        }

        private void BindPlotAmounts()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT pass.TotalAmount,pass.PendingAmount,pass.FacingCharges,pass.Maintainance,pass.Name,pass.Mobile,p.Size,p.PlotNo FROM Passbook pass, Plots p where pass.PlotId=p.PlotId and pass.PassbookId=" + ddlPassbook.SelectedValue + " and pass.ProjectId='" + ddlProjects.SelectedValue + "'"))
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
                                    txtTotalPlotamount.Text = dt.Rows[0]["TotalAmount"].ToString();
                                    txtPendingAmount.Text = dt.Rows[0]["PendingAmount"].ToString();
                                    txtFacing.Text = dt.Rows[0]["FacingCharges"].ToString();
                                    txtMaintainance.Text = dt.Rows[0]["Maintainance"].ToString();
                                    txtCustomerName.Text = dt.Rows[0]["Name"].ToString();
                                    txtMobile.Text = dt.Rows[0]["Mobile"].ToString();
                                    txtSize.Text = dt.Rows[0]["Size"].ToString();
                                    hdnPlotNo.Value = dt.Rows[0]["PlotNo"].ToString();
                                    hdnPendingAmount.Value = dt.Rows[0]["PendingAmount"].ToString();
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
                htmldata += "<table class='table table-bordered table-striped mt-3' id='commissionTable'>" +
                    "<thead>" +
                        "<tr>" +
                           "<th>ReceiptNo</th>" + "<th>Date</th>" + "<th>Amount</th>" + "<th>PaymentMethod</th><th>Transaction</th>" +
                        "</tr>" +
                    "</thead><tbody>";
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("select * from PlotPayments pass where PassbookNo='" + ddlPassbook.SelectedValue + "' and ProjectId='"+ddlPassbook.SelectedItem.Text+"'"))
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
                                                    "<td>" + dt.Rows[i]["ReceiptNo"] + "</td>" +
                                                    "<td>" + Convert.ToDateTime(dt.Rows[i]["PaymentDate"]).ToString("dd/MM/yyyy") + "</td>" +
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
            txtAmount.Text = "0";
            txtFacing.Text = "0";
            txtMaintainance.Text = "0";
            txtPendingAmount.Text = "0";
            txtPaymentReference.Text = "";
            ddlPassbook.ClearSelection();
            ddlPaymentMethod.SelectedValue = "GooglePay";
            txtTotalPlotamount.Text = "0";
            GenerateReceiptNo();
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
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO PlotPayments (ReceiptNo,PassbookNo,Amount,PaymentDate," +
                        "PaymentMethod,PaymentDetails," +
                        "UserId,CreatedDate,UpdatedDate,ProjectId) VALUES (@ReceiptNo,@PassbookNo,@Amount,@PaymentDate," +
                        "@PaymentMethod,@PaymentDetails," +
                        "@UserId,@CreatedDate,@UpdatedDate,@ProjectId)"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@ReceiptNo", txtReceiptNo.Text);
                            cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbook.SelectedValue);
                            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                            cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now.Date);
                            cmd.Parameters.AddWithValue("@PaymentMethod", ddlPaymentMethod.SelectedValue);
                            cmd.Parameters.AddWithValue("@PaymentDetails", txtPaymentReference.Text);
                            cmd.Parameters.AddWithValue("@UserId", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            UpdatePlotData();
                            //gc.SendPaymentSMSToCustomer(txtMobile.Text,txtAmount.Text, hdnPlotNo.Value);
                            //gc.SendPaymentSMSToMD("9985340876", txtAmount.Text, hdnPlotNo.Value);
                            BindData();
                            Reset();
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
                double PendingAmount = Convert.ToDouble(hdnPendingAmount.Value) - Convert.ToDouble(txtAmount.Text);
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("UPDATE Passbook set PendingAmount=@PendingAmount,UpdatedDate=@UpdatedDate where PassbookNo=@PassbookNo and ProjectId=@ProjectId"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbook.SelectedItem.Text);
                            cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                            cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@PendingAmount", PendingAmount);
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
    }
}