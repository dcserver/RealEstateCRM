using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace RealEstateCRM
{
    public partial class CommissionPaymentDetails : System.Web.UI.Page
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
        private void BindEmployees()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT MarketerName,CommissionEntryId from CommissionEntry where PassbookNo='" + ddlPassbook.SelectedValue + "'"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ddlEmployees.Items.Clear();
                                ddlEmployees.DataSource = dt;
                                ddlEmployees.DataTextField = "MarketerName";
                                ddlEmployees.DataValueField = "CommissionEntryId";
                                ddlEmployees.DataBind();
                                ddlEmployees.Items.Insert(0, "Please Select");
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
                    using (MySqlCommand cmd = new MySqlCommand("SELECT VoucherNo FROM CommissionPayments Where ProjectId='" + ddlProjects.SelectedValue + "' ORDER BY 1 DESC LIMIT 1"))
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
                                    int pNo = Convert.ToInt32(dt.Rows[0]["VoucherNo"]) + 1;
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
        private void BindData()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                string htmldata = string.Empty;
                htmldata += "<table class='table table-bordered table-striped mt-3' id='commissionTable'>" +
                    "<thead>" +
                        "<tr>" +
                           "<th>VoucherNo</th>" +
                               "<th>Marketer</th>" + "<th>PassbookNo</th>" + "<th>Amount</th>" + "<th>Date</th>" + "<th>PaymentMethod</th>" +
                        "</tr>" +
                    "</thead><tbody>";
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("select * from CommissionPayments where ProjectId='" + ddlProjects.SelectedValue + "' and PassbookNo='" + ddlPassbook.SelectedValue + "'"))
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
                                                    "<td>" + dt.Rows[i]["VoucherNo"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["MarketerName"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["PassbookNo"] + "</td>" +
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
        private void Reset()
        {
            ddlPassbook.ClearSelection();
            txtAmount.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtPaymentReference.Text = string.Empty;
            ddlEmployees.ClearSelection();
            ddlPaymentMethod.SelectedValue = "GooglePay";
            txtPendingAmount.Text = "0";
            txtCommission.Text = "0";
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
                double Pending = 0;
                double Paid = 0;
                double Advance = Convert.ToDouble(txtAdvance.Text);
                if (ddlPaymentType.SelectedValue == "Advance")
                {
                    Advance = Advance + Convert.ToDouble(txtAmount.Text);
                }
                Pending = Convert.ToDouble(txtPendingAmount.Text) - Convert.ToDouble(txtAmount.Text);
                Paid = Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble(txtAmount.Text);
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("Update CommissionEntry set Pending=@Pending,Paid=@Paid,UpdatedDate=@UpdatedDate where CommissionEntryId=@CommissionEntryId and ProjectId=@ProjectId"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@Pending", Pending);
                            cmd.Parameters.AddWithValue("@Paid", Paid);
                            cmd.Parameters.AddWithValue("@CommissionEntryId", ddlEmployees.SelectedValue);
                            cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
                            cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Advance", Advance);
                            cmd.Parameters.AddWithValue("@UserId", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            InsertPaymentsTable();
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

        private void InsertPaymentsTable()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO CommissionPayments (VoucherNo,PassbookNo,ProjectId,Amount,PaymentDate," +
                        "PaymentMethod,PaymentDetails," +
                        "UserId,CreatedDate,UpdatedDate,MarketerName) VALUES (@VoucherNo,@PassbookNo,@ProjectId,@Amount,@PaymentDate," +
                        "@PaymentMethod,@PaymentDetails," +
                        "@UserId,@CreatedDate,@UpdatedDate,@MarketerName)"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@VoucherNo", txtReceiptNo.Text);
                            cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbook.SelectedValue);
                            cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                            cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now.Date);
                            cmd.Parameters.AddWithValue("@PaymentMethod", ddlPaymentMethod.SelectedValue);
                            cmd.Parameters.AddWithValue("@PaymentDetails", txtPaymentReference.Text);
                            cmd.Parameters.AddWithValue("@UserId", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@MarketerName", ddlEmployees.SelectedItem.Text);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
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
        protected void ddlPassbookNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindEmployees();
            BindData();
            GenerateReceiptNo();
        }
        protected void ddlEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("Select * from CommissionEntry where CommissionEntryId='" + ddlEmployees.SelectedValue + "'"))
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
                                    txtAdvance.Text = dt.Rows[0]["Advance"].ToString();
                                    txtPaidAmount.Text = dt.Rows[0]["Paid"].ToString();
                                    txtPendingAmount.Text = dt.Rows[0]["Pending"].ToString();
                                    txtCommission.Text = dt.Rows[0]["Eligibility"].ToString();
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
    }
}