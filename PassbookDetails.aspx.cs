using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;

namespace RealEstateCRM
{
    public partial class PassbookDetails : System.Web.UI.Page
    {
        ErrorFile err = new ErrorFile();
        GlobalData globalData = new GlobalData();
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
                        }
                        else
                        {
                            btnSubmit.Visible = true;
                            btnUpdate.Visible = false;
                        }
                        //GeneratePassBookNo();
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
        private void GeneratePassBookNo()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT pb.Passbookno FROM Passbook pb, Plots pt where pb.PlotId=pt.PlotId and pt.ProjectId='" + ddlProjects.SelectedValue + "' Order By pb.Passbookno Desc Limit 1"))
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
                                    int pNo = Convert.ToInt32(dt.Rows[0]["Passbookno"]) + 1;
                                    txtPassbookNo.Text = pNo.ToString();
                                }
                                else
                                {
                                    txtPassbookNo.Text = "1";
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
                    using (MySqlCommand cmd = new MySqlCommand("SELECT ProjectId,ProjectName FROM Projects"))
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
            GeneratePassBookNo();
            BindPlots();
        }
        private void BindPlots()
        {
            try
            {
                ddlPlots.Items.Clear();
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT PlotId,PlotNo FROM Plots where Status='A' and ProjectId='" + ddlProjects.SelectedValue + "'"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                ddlPlots.DataSource = dt;
                                ddlPlots.DataTextField = "PlotNo";
                                ddlPlots.DataValueField = "PlotId";
                                ddlPlots.DataBind();
                                ddlPlots.Items.Insert(0, "Please Select");
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
        private void Reset()
        {
            txtPassbookNo.Text = string.Empty;
            txtDateOfJoin.Text = string.Empty;
            txtPlotAmount.Text = string.Empty;
            txtMaintainanceCharges.Text = string.Empty;
            txtTotalAmount.Text = string.Empty;
            txtName.Text = string.Empty;
            txtMobile.Text = string.Empty;
            txtCommission.Text = string.Empty;
            txtTDS.Text = string.Empty;
            txtEligibility.Text = string.Empty;
            txtAdjustment.Text = string.Empty;
            txtFinalComission.Text = string.Empty;
            ddlPlots.ClearSelection();
            txtNominee.Text = "";
            ddlProjects.ClearSelection();
            txtAddress.Text = "";
            txtFacingCharges.Text = string.Empty;
            txtRegisterName.Text = string.Empty;
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
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Passbook (PlotId,PassbookNo,DateOfJoin,PaymentLastDate,Nominee," +
                        "TotalAmount,Commission,TDS,Eligibility,Adjustment,FinalComission,PlotAmount,Maintainance,PendingAmount,Name," +
                        "Mobile,Address,UserId,CreatedDate,UpdatedDate,FacingCharges,ProjectId) VALUES (@PlotId,@PassbookNo,@DateOfJoin,@PaymentLastDate,@Nominee," +
                        " @TotalAmount,@Commission,@TDS,@Eligibility,@Adjustment,@FinalComission,@PlotAmount,@Maintainance,@PendingAmount,@Name," +
                        "@Mobile,@Address,@UserId,@CreatedDate,@UpdatedDate,@FacingCharges,@ProjectId)"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@PlotId", ddlPlots.SelectedValue);
                            cmd.Parameters.AddWithValue("@PassbookNo", txtPassbookNo.Text.Trim());
                            DateTime startDate = DateTime.Now;
                            if (txtDateOfJoin.Text == "")
                            {
                                txtDateOfJoin.Text = DateTime.Now.ToString();
                            }
                            else
                            {
                                startDate = Convert.ToDateTime(txtDateOfJoin.Text);
                                txtDateOfJoin.Text = startDate.ToString();
                            }
                            cmd.Parameters.AddWithValue("@DateOfJoin", startDate);
                            DateTime endDate = Convert.ToDateTime(txtDateOfJoin.Text);
                            endDate = endDate.AddDays(45);
                            cmd.Parameters.AddWithValue("@PaymentLastDate", endDate);
                            cmd.Parameters.AddWithValue("@Nominee", txtNominee.Text);
                            cmd.Parameters.AddWithValue("@TotalAmount", txtTotalAmount.Text);
                            cmd.Parameters.AddWithValue("@Commission", txtCommission.Text);
                            cmd.Parameters.AddWithValue("@TDS", txtTDS.Text);
                            cmd.Parameters.AddWithValue("@Eligibility", txtEligibility.Text);
                            cmd.Parameters.AddWithValue("@Adjustment", txtAdjustment.Text);
                            cmd.Parameters.AddWithValue("@FinalComission", txtFinalComission.Text);
                            cmd.Parameters.AddWithValue("@PlotAmount", txtPlotAmount.Text);
                            cmd.Parameters.AddWithValue("@Maintainance", txtMaintainanceCharges.Text);
                            cmd.Parameters.AddWithValue("@PendingAmount", txtTotalAmount.Text);
                            cmd.Parameters.AddWithValue("@Name", txtName.Text);
                            cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@UserId", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@FacingCharges", txtFacingCharges.Text);
                            cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            UpdatePlotData(ddlPlots.SelectedValue);
                            lblstatus.Text = "Passbook Created Successfully";
                            lblstatus.ForeColor = Color.Green;
                            GeneratePassBookNo();
                            BindPlots();
                            //globalData.SendPlotBookingSMS(txtMobile.Text, ddlPlots.SelectedItem.Text);
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
        private void UpdatePlotData(string selectedValue)
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
                            cmd.Parameters.AddWithValue("@Status", "P");
                            cmd.Parameters.AddWithValue("@PlotId", selectedValue);
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
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("UPDATE Passbook set PlotId=@PlotId,EmployeeId=@EmployeeId,FacingCharges=@FacingChargesPassbookNo=@PassbookNo,DateOfJoin=@DateOfJoin,PaymentLastDate=@PaymentLastDate,Nominee=@Nominee,TotalAmount=@TotalAmount,Commission=@Commission,TDS=@TDS,Eligibility=@Eligibility,Adjustment=@Adjustment,FinalComission=@FinalComission,PendingAmount=@PendingAmount,Name=@Name,Mobile=@Mobile,Address=@Address,UserId=@UserId,CreatedDate=@CreatedDate,UpdatedDate=@UpdatedDate,ProjectId=@ProjectId where PassbookId=@PassbookId"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@PassbookId", Request.QueryString["Id"]);
                            cmd.Parameters.AddWithValue("@PlotId", ddlPlots.SelectedValue);
                            cmd.Parameters.AddWithValue("@PassbookNo", txtPassbookNo.Text.Trim());
                            cmd.Parameters.AddWithValue("@DateOfJoin", txtDateOfJoin.Text);
                            DateTime endDate = Convert.ToDateTime(txtDateOfJoin.Text);
                            endDate = endDate.AddDays(45);
                            cmd.Parameters.AddWithValue("@PaymentLastDate", endDate);
                            cmd.Parameters.AddWithValue("@Nominee", txtNominee.Text);
                            cmd.Parameters.AddWithValue("@TotalAmount", txtTotalAmount.Text);
                            cmd.Parameters.AddWithValue("@Commission", txtCommission.Text);
                            cmd.Parameters.AddWithValue("@TDS", txtTDS.Text);
                            cmd.Parameters.AddWithValue("@Eligibility", txtEligibility.Text);
                            cmd.Parameters.AddWithValue("@Adjustment", txtAdjustment.Text);
                            cmd.Parameters.AddWithValue("@FinalComission", txtFinalComission.Text);
                            cmd.Parameters.AddWithValue("@PlotAmount", txtPlotAmount.Text);
                            cmd.Parameters.AddWithValue("@Maintainance", txtMaintainanceCharges.Text);
                            cmd.Parameters.AddWithValue("@FacingCharges", txtFacingCharges.Text);
                            cmd.Parameters.AddWithValue("@PendingAmount", txtTotalAmount.Text);
                            cmd.Parameters.AddWithValue("@Name", txtName.Text);
                            cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@UserId", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
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
        protected void ddlPlots_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT Amount,MaintainanceCharges,Size,FacingCharges FROM Plots where PlotId=" + ddlPlots.SelectedValue + ""))
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
                                    double MAmount = Convert.ToDouble(dt.Rows[0]["MaintainanceCharges"]) * Convert.ToDouble(dt.Rows[0]["Size"]);
                                    txtMaintainanceCharges.Text = MAmount.ToString();
                                    double FAmount = Convert.ToDouble(dt.Rows[0]["FacingCharges"]) * Convert.ToDouble(dt.Rows[0]["Size"]);
                                    txtPlotAmount.Text = dt.Rows[0]["Amount"].ToString();
                                    txtFacingCharges.Text = FAmount.ToString();
                                    double TAmount = Convert.ToDouble(dt.Rows[0]["Amount"]) + MAmount + FAmount;
                                    txtTotalAmount.Text = TAmount.ToString();
                                    double CalculateCommission = ((double)30 / 100) * Convert.ToDouble(dt.Rows[0]["Amount"]);
                                    txtCommission.Text = ((int)CalculateCommission).ToString();
                                    double TDS = (((double)5 / 100) * (double)CalculateCommission);
                                    txtTDS.Text = ((int)TDS).ToString();
                                    txtEligibility.Text = ((int)(CalculateCommission - TDS)).ToString();
                                    txtAdjustment.Text = "0";
                                    txtFinalComission.Text = (Convert.ToDouble(txtEligibility.Text) - Convert.ToDouble(txtAdjustment.Text)).ToString();
                                    lblSize.Text = dt.Rows[0]["Size"].ToString() + "Sq. Yards";
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
        protected void txtAdjustment_TextChanged(object sender, EventArgs e)
        {
            if (txtAdjustment.Text != null)
            {
                txtFinalComission.Text = (Convert.ToDouble(txtEligibility.Text) - Convert.ToDouble(txtAdjustment.Text)).ToString();
            }
        }
    }
}