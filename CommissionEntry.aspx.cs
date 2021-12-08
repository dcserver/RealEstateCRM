using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;

namespace RealEstateCRM
{
    public partial class CommissionEntry : System.Web.UI.Page
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
            BindPassbook();
        }
        private void BindPassbook()
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT PassbookNo,PassbookId FROM Passbook where ProjectId='" + ddlProjects.SelectedValue + "'"))
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
                                ddlPassbook.Items.Insert(0, new ListItem("Please Select", "0"));
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
                bool CheckTCommission = CheckCommissionLimit();
                if (CheckTCommission)
                {
                    string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                    using (MySqlConnection con = new MySqlConnection(dbConnection))
                    {
                        using (MySqlCommand cmd = new MySqlCommand("INSERT INTO CommissionEntry (PassbookNo,CreatedDate,UpdatedDate," +
                            "PaymentDate,Percentage,Total,TDS,Eligibility,Advance,Pending,Paid,MarketerName,ProjectId) VALUES (@PassbookNo,@CreatedDate,@UpdatedDate," +
                            "@PaymentDate,@Percentage,@Total,@TDS,@Eligibility,@Advance,@Pending,@Paid,@MarketerName,@ProjectId)"))
                        {
                            using (MySqlDataAdapter sda = new MySqlDataAdapter())
                            {
                                cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbook.SelectedValue);
                                cmd.Parameters.AddWithValue("@MarketerName", txtDirectoryName.Text);
                                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@Percentage", txtDCommissionPercentage.Text);
                                cmd.Parameters.AddWithValue("@Total", txtDTCommission.Text);
                                cmd.Parameters.AddWithValue("@TDS", hdnDTDS.Value);
                                cmd.Parameters.AddWithValue("@Eligibility", hdnDECommission.Value);
                                cmd.Parameters.AddWithValue("@Advance", "0");
                                cmd.Parameters.AddWithValue("@Pending", hdnDECommission.Value);
                                cmd.Parameters.AddWithValue("@Paid", "0");
                                cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                                if (txtDDTCommission.Text != "0")
                                {
                                    InsertDeputyDirector(dbConnection);
                                }
                                if (txtCGMTCommission.Text != "0")
                                {
                                    InsertCheifGeneralManager(dbConnection);
                                }
                                if (txtGMTCommission.Text != "0")
                                {
                                    InsertGeneralManager(dbConnection);
                                }
                                if (txtDGMTCommission.Text != "0")
                                {
                                    InsertDeputyGeneralManager(dbConnection);
                                }
                                if (txtAGMTCommission.Text != "0")
                                {
                                    InsertAsstGeneralManager(dbConnection);
                                }
                                if (txtCMMTCommission.Text != "0")
                                {
                                    InsertCheifMarketingManager(dbConnection);
                                }
                                if (txtMMTCommission.Text != "0")
                                {
                                    InsertMarketingManager(dbConnection);
                                }
                                if (txtMOTCommission.Text != "0")
                                {
                                    InsertMarektingOfficer(dbConnection);
                                }
                                lblstatus.Text = "Entry Created Successfully";
                                lblstatus.ForeColor = Color.Green;                                
                                BindProjects();
                            }
                        }
                    }
                }
                else
                {
                    lblstatus.Text = "Total Commission(%) should not be greater than 30";
                    lblstatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                err.LogError(ex, ErrorPath);
                Response.Redirect("Error.aspx");
            }
        }
        private bool CheckCommissionLimit()
        {
            int Total = Convert.ToInt32(txtDCommissionPercentage.Text) +
                Convert.ToInt32(txtDDCommissionPercentage.Text) +
                Convert.ToInt32(txtCGMCommissionPercentage.Text) +
                Convert.ToInt32(txtGMCommissionPercentage.Text) +
                Convert.ToInt32(txtDGMCommissionPercentage.Text) +
                Convert.ToInt32(txtAGMCommissionPercentage.Text) +
                Convert.ToInt32(txtCMMCommissionPercentage.Text) +
                Convert.ToInt32(txtMMCommissionPercentage.Text) +
            Convert.ToInt32(txtMOCommissionPercentage.Text);
            txtGrandPercentage.Text = Total.ToString();
            if (Total > 30)
            {
                lblstatus.Text = "Total Commission(%) should not be greater than 30";
                lblstatus.ForeColor = Color.Red;
                return false;
            }
            else
            {
                lblstatus.Text = "";
                return true;
            }
        }
        private void InsertAsstGeneralManager(string dbConnection)
        {
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO CommissionEntry (PassbookNo,CreatedDate,UpdatedDate," +
                    "PaymentDate,Percentage,Total,TDS,Eligibility,Advance,Pending,Paid,MarketerName,ProjectId) VALUES (@PassbookNo,@CreatedDate,@UpdatedDate," +
                    "@PaymentDate,@Percentage,@Total,@TDS,@Eligibility,@Advance,@Pending,@Paid,@MarketerName,@ProjectId)"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbook.SelectedValue);
                        cmd.Parameters.AddWithValue("@MarketerName", txtAsstGeneralManager.Text);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Percentage", txtAGMCommissionPercentage.Text);
                        cmd.Parameters.AddWithValue("@Total", txtAGMTCommission.Text);
                        cmd.Parameters.AddWithValue("@TDS", hdnAGMTDS.Value);
                        cmd.Parameters.AddWithValue("@Eligibility", hdnAGMECommission.Value);
                        cmd.Parameters.AddWithValue("@Advance", "0");
                        cmd.Parameters.AddWithValue("@Pending", hdnAGMECommission.Value);
                        cmd.Parameters.AddWithValue("@Paid", "0");
                        cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }

        private void InsertMarektingOfficer(string dbConnection)
        {
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO CommissionEntry (PassbookNo,CreatedDate,UpdatedDate," +
                    "PaymentDate,Percentage,Total,TDS,Eligibility,Advance,Pending,Paid,MarketerName,ProjectId) VALUES (@PassbookNo,@CreatedDate,@UpdatedDate," +
                    "@PaymentDate,@Percentage,@Total,@TDS,@Eligibility,@Advance,@Pending,@Paid,@MarketerName,@ProjectId)"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbook.SelectedValue);
                        cmd.Parameters.AddWithValue("@MarketerName", txtMarketingOfficer.Text);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Percentage", txtMOCommissionPercentage.Text);
                        cmd.Parameters.AddWithValue("@Total", txtMOTCommission.Text);
                        cmd.Parameters.AddWithValue("@TDS", hdnMOTDS.Value);
                        cmd.Parameters.AddWithValue("@Eligibility", hdnMOECommission.Value);
                        cmd.Parameters.AddWithValue("@Advance", "0");
                        cmd.Parameters.AddWithValue("@Pending", hdnMOECommission.Value);
                        cmd.Parameters.AddWithValue("@Paid", "0");
                        cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }

        private void InsertMarketingManager(string dbConnection)
        {
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO CommissionEntry (PassbookNo,CreatedDate,UpdatedDate," +
                    "PaymentDate,Percentage,Total,TDS,Eligibility,Advance,Pending,Paid,MarketerName,ProjectId) VALUES (@PassbookNo,@CreatedDate,@UpdatedDate," +
                    "@PaymentDate,@Percentage,@Total,@TDS,@Eligibility,@Advance,@Pending,@Paid,@MarketerName,@ProjectId)"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbook.SelectedValue);
                        cmd.Parameters.AddWithValue("@MarketerName", txtMarketingManager.Text);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Percentage", txtMMCommissionPercentage.Text);
                        cmd.Parameters.AddWithValue("@Total", txtMMTCommission.Text);
                        cmd.Parameters.AddWithValue("@TDS", hdnMMTDS.Value);
                        cmd.Parameters.AddWithValue("@Eligibility", hdnMMECommission.Value);
                        cmd.Parameters.AddWithValue("@Advance", "0");
                        cmd.Parameters.AddWithValue("@Pending", hdnMMECommission.Value);
                        cmd.Parameters.AddWithValue("@Paid", "0");
                        cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }

        private void InsertCheifMarketingManager(string dbConnection)
        {
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO CommissionEntry (PassbookNo,CreatedDate,UpdatedDate," +
                    "PaymentDate,Percentage,Total,TDS,Eligibility,Advance,Pending,Paid,MarketerName,ProjectId) VALUES (@PassbookNo,@CreatedDate,@UpdatedDate," +
                    "@PaymentDate,@Percentage,@Total,@TDS,@Eligibility,@Advance,@Pending,@Paid,@MarketerName,@ProjectId)"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbook.SelectedValue);
                        cmd.Parameters.AddWithValue("@MarketerName", txtCheifMarketingManager.Text);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Percentage", txtCMMCommissionPercentage.Text);
                        cmd.Parameters.AddWithValue("@Total", txtCMMTCommission.Text);
                        cmd.Parameters.AddWithValue("@TDS", hdnCMMTDS.Value);
                        cmd.Parameters.AddWithValue("@Eligibility", hdnCMMECommission.Value);
                        cmd.Parameters.AddWithValue("@Advance", "0");
                        cmd.Parameters.AddWithValue("@Pending", hdnCMMECommission.Value);
                        cmd.Parameters.AddWithValue("@Paid", "0");
                        cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }

        private void InsertDeputyGeneralManager(string dbConnection)
        {
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO CommissionEntry (PassbookNo,CreatedDate,UpdatedDate," +
                    "PaymentDate,Percentage,Total,TDS,Eligibility,Advance,Pending,Paid,MarketerName,ProjectId) VALUES (@PassbookNo,@CreatedDate,@UpdatedDate," +
                    "@PaymentDate,@Percentage,@Total,@TDS,@Eligibility,@Advance,@Pending,@Paid,@MarketerName,@ProjectId)"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbook.SelectedValue);
                        cmd.Parameters.AddWithValue("@MarketerName", txtDeputyGeneralManager.Text);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Percentage", txtDGMCommissionPercentage.Text);
                        cmd.Parameters.AddWithValue("@Total", txtDGMTCommission.Text);
                        cmd.Parameters.AddWithValue("@TDS", hdnDGMTDS.Value);
                        cmd.Parameters.AddWithValue("@Eligibility", hdnDGMECommission.Value);
                        cmd.Parameters.AddWithValue("@Advance", "0");
                        cmd.Parameters.AddWithValue("@Pending", hdnDGMECommission.Value);
                        cmd.Parameters.AddWithValue("@Paid", "0");
                        cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }

        private void InsertGeneralManager(string dbConnection)
        {
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO CommissionEntry (PassbookNo,CreatedDate,UpdatedDate," +
                    "PaymentDate,Percentage,Total,TDS,Eligibility,Advance,Pending,Paid,MarketerName,ProjectId) VALUES (@PassbookNo,@CreatedDate,@UpdatedDate," +
                    "@PaymentDate,@Percentage,@Total,@TDS,@Eligibility,@Advance,@Pending,@Paid,@MarketerName,@ProjectId)"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbook.SelectedValue);
                        cmd.Parameters.AddWithValue("@MarketerName", txtGeneralManager.Text);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Percentage", txtGMCommissionPercentage.Text);
                        cmd.Parameters.AddWithValue("@Total", txtGMTCommission.Text);
                        cmd.Parameters.AddWithValue("@TDS", hdnGMTDS.Value);
                        cmd.Parameters.AddWithValue("@Eligibility", hdnGMECommission.Value);
                        cmd.Parameters.AddWithValue("@Advance", "0");
                        cmd.Parameters.AddWithValue("@Pending", hdnGMECommission.Value);
                        cmd.Parameters.AddWithValue("@Paid", "0");
                        cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }

        private void InsertCheifGeneralManager(string dbConnection)
        {
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO CommissionEntry (PassbookNo,CreatedDate,UpdatedDate," +
                    "PaymentDate,Percentage,Total,TDS,Eligibility,Advance,Pending,Paid,MarketerName,ProjectId) VALUES (@PassbookNo,@CreatedDate,@UpdatedDate," +
                    "@PaymentDate,@Percentage,@Total,@TDS,@Eligibility,@Advance,@Pending,@Paid,@MarketerName,@ProjectId)"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbook.SelectedValue);
                        cmd.Parameters.AddWithValue("@MarketerName", txtCheifGeneralManager.Text);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Percentage", txtCGMCommissionPercentage.Text);
                        cmd.Parameters.AddWithValue("@Total", txtCGMTCommission.Text);
                        cmd.Parameters.AddWithValue("@TDS", hdnCGMTDS.Value);
                        cmd.Parameters.AddWithValue("@Eligibility", hdnCGMECommission.Value);
                        cmd.Parameters.AddWithValue("@Advance", "0");
                        cmd.Parameters.AddWithValue("@Pending", hdnCGMECommission.Value);
                        cmd.Parameters.AddWithValue("@Paid", "0");
                        cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }

        private void InsertDeputyDirector(string dbConnection)
        {
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO CommissionEntry (PassbookNo,CreatedDate,UpdatedDate," +
                    "PaymentDate,Percentage,Total,TDS,Eligibility,Advance,Pending,Paid,MarketerName,ProjectId) VALUES (@PassbookNo,@CreatedDate,@UpdatedDate," +
                    "@PaymentDate,@Percentage,@Total,@TDS,@Eligibility,@Advance,@Pending,@Paid,@MarketerName,@ProjectId)"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@PassbookNo", ddlPassbook.SelectedValue);
                        cmd.Parameters.AddWithValue("@MarketerName", txtDeputyDirectory.Text);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Percentage", txtDDCommissionPercentage.Text);
                        cmd.Parameters.AddWithValue("@Total", txtDDTCommission.Text);
                        cmd.Parameters.AddWithValue("@TDS", hdnDDTDS.Value);
                        cmd.Parameters.AddWithValue("@Eligibility", hdnDDECommission.Value);
                        cmd.Parameters.AddWithValue("@Advance", "0");
                        cmd.Parameters.AddWithValue("@Pending", hdnDDECommission.Value);
                        cmd.Parameters.AddWithValue("@Paid", "0");
                        cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

        }

        protected void ddlPassbook_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT PlotAmount,FinalComission,Eligibility,Adjustment,TDS,Commission FROM Passbook where PassbookId='" + ddlPassbook.SelectedValue + "'"))
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
                                    txtPlotPrice.Text = dt.Rows[0]["PlotAmount"].ToString();
                                    txtTotalCommission.Text = dt.Rows[0]["Commission"].ToString();
                                    txtTDS.Text = dt.Rows[0]["TDS"].ToString();
                                    txtTotalEligibleCommission.Text = dt.Rows[0]["FinalComission"].ToString();
                                    //txtAdjustment.Text = dt.Rows[0]["Adjustment"].ToString();
                                    //txtFinalCommission.Text = dt.Rows[0]["FinalComission"].ToString();
                                    //BindCommissionData(dt.Rows[0]["EmployeeId"].ToString(), dt.Rows[0]["PlotAmount"].ToString());
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

        protected void txtDCommission_TextChanged(object sender, EventArgs e)
        {
            CheckCommissionLimit();
            double ECommission = (((double)Convert.ToDouble(txtDCommissionPercentage.Text) / 100) * (double)Convert.ToDouble(txtPlotPrice.Text));
            txtDTCommission.Text = ((int)ECommission).ToString();
            double TDS = (((double)5 / 100) * (double)Convert.ToDouble(txtDTCommission.Text));
            hdnDTDS.Value = ((int)TDS).ToString();
            hdnDECommission.Value = ((int)(Convert.ToDouble(txtDTCommission.Text) - TDS)).ToString();
        }

        protected void txtDDCommissionPercentage_TextChanged(object sender, EventArgs e)
        {
            CheckCommissionLimit();
            double ECommission = (((double)Convert.ToDouble(txtDDCommissionPercentage.Text) / 100) * (double)Convert.ToDouble(txtPlotPrice.Text));
            txtDDTCommission.Text = ((int)ECommission).ToString();
            double TDS = (((double)5 / 100) * (double)Convert.ToDouble(txtDDTCommission.Text));
            hdnDDTDS.Value = ((int)TDS).ToString();
            hdnDDECommission.Value = ((int)(Convert.ToDouble(txtDDTCommission.Text) - TDS)).ToString();
        }

        protected void txtCGMCommissionPercentage_TextChanged(object sender, EventArgs e)
        {
            CheckCommissionLimit();
            double ECommission = (((double)Convert.ToDouble(txtCGMCommissionPercentage.Text) / 100) * (double)Convert.ToDouble(txtPlotPrice.Text));
            txtCGMTCommission.Text = ((int)ECommission).ToString();
            double TDS = (((double)5 / 100) * (double)Convert.ToDouble(txtCGMTCommission.Text));
            hdnCGMTDS.Value = ((int)TDS).ToString();
            hdnCGMECommission.Value = ((int)(Convert.ToDouble(txtCGMTCommission.Text) - TDS)).ToString();
        }

        protected void txtGMCommissionPercentage_TextChanged(object sender, EventArgs e)
        {
            CheckCommissionLimit();
            double ECommission = (((double)Convert.ToDouble(txtGMCommissionPercentage.Text) / 100) * (double)Convert.ToDouble(txtPlotPrice.Text));
            txtGMTCommission.Text = ((int)ECommission).ToString();
            double TDS = (((double)5 / 100) * (double)Convert.ToDouble(txtGMTCommission.Text));
            hdnGMTDS.Value = ((int)TDS).ToString();
            hdnGMECommission.Value = ((int)(Convert.ToDouble(txtGMTCommission.Text) - TDS)).ToString();
        }

        protected void txtDGMCommissionPercentage_TextChanged(object sender, EventArgs e)
        {
            CheckCommissionLimit();
            double ECommission = (((double)Convert.ToDouble(txtDGMCommissionPercentage.Text) / 100) * (double)Convert.ToDouble(txtPlotPrice.Text));
            txtDGMTCommission.Text = ((int)ECommission).ToString();
            double TDS = (((double)5 / 100) * (double)Convert.ToDouble(txtDGMTCommission.Text));
            hdnDGMTDS.Value = ((int)TDS).ToString();
            hdnDGMECommission.Value = ((int)(Convert.ToDouble(txtDGMTCommission.Text) - TDS)).ToString();
        }

        protected void txtAGMCommissionPercentage_TextChanged(object sender, EventArgs e)
        {
            CheckCommissionLimit();
            double ECommission = (((double)Convert.ToDouble(txtAGMCommissionPercentage.Text) / 100) * (double)Convert.ToDouble(txtPlotPrice.Text));
            txtAGMTCommission.Text = ((int)ECommission).ToString();
            double TDS = (((double)5 / 100) * (double)Convert.ToDouble(txtAGMTCommission.Text));
            hdnAGMTDS.Value = ((int)TDS).ToString();
            hdnAGMECommission.Value = ((int)(Convert.ToDouble(txtAGMTCommission.Text) - TDS)).ToString();
        }

        protected void txtMMCommissionPercentage_TextChanged(object sender, EventArgs e)
        {
            CheckCommissionLimit();
            double ECommission = (((double)Convert.ToDouble(txtMMCommissionPercentage.Text) / 100) * (double)Convert.ToDouble(txtPlotPrice.Text));
            txtMMTCommission.Text = ((int)ECommission).ToString();
            double TDS = (((double)5 / 100) * (double)Convert.ToDouble(txtMMTCommission.Text));
            hdnMMTDS.Value = ((int)TDS).ToString();
            hdnMMECommission.Value = ((int)(Convert.ToDouble(txtMMTCommission.Text) - TDS)).ToString();
        }

        protected void txtMOCommissionPercentage_TextChanged(object sender, EventArgs e)
        {
            CheckCommissionLimit();
            double ECommission = (((double)Convert.ToDouble(txtMOCommissionPercentage.Text) / 100) * (double)Convert.ToDouble(txtPlotPrice.Text));
            txtMOTCommission.Text = ((int)ECommission).ToString();
            double TDS = (((double)5 / 100) * (double)Convert.ToDouble(txtMOTCommission.Text));
            hdnMOTDS.Value = ((int)TDS).ToString();
            hdnMOECommission.Value = ((int)(Convert.ToDouble(txtMOTCommission.Text) - TDS)).ToString();
        }

        protected void txtCMMCommissionPercentage_TextChanged(object sender, EventArgs e)
        {
            CheckCommissionLimit();
            double ECommission = (((double)Convert.ToDouble(txtCMMCommissionPercentage.Text) / 100) * (double)Convert.ToDouble(txtPlotPrice.Text));
            txtCMMTCommission.Text = ((int)ECommission).ToString();
            double TDS = (((double)5 / 100) * (double)Convert.ToDouble(txtCMMTCommission.Text));
            hdnCMMTDS.Value = ((int)TDS).ToString();
            hdnCMMECommission.Value = ((int)(Convert.ToDouble(txtCMMTCommission.Text) - TDS)).ToString();
        }
    }
}