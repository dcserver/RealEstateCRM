using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace RealEstateCRM
{
    public partial class PassbookView : System.Web.UI.Page
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
                        if (Request.QueryString["Id"] != null)
                        {
                            BindData(Request.QueryString["Id"]);
                        }
                        else
                        {
                            Response.Redirect("~/PassbookList.aspx");
                        }
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
        private void BindData(string PassBookId)
        {
            string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("Select ps.*,pt.Status as PlotStatus,pt.PlotNo,pt.Facing,pt.Size as PlotSize,pr.ProjectName FROM Passbook ps, Plots pt,Projects pr where pr.ProjectId=pt.ProjectId and pt.PlotId=ps.PlotId and ps.PassbookId='" + PassBookId + "'"))
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
                                lblProjectName.Text= dt.Rows[0]["ProjectName"].ToString();
                                lblPassBookNo.Text = dt.Rows[0]["Passbookno"].ToString();
                                lblPlotNo.Text = dt.Rows[0]["PlotNo"].ToString();
                                lblFacing.Text = dt.Rows[0]["Facing"].ToString();
                                lblSize.Text = dt.Rows[0]["PlotSize"].ToString();
                                lblPlotCost.Text = dt.Rows[0]["PlotAmount"].ToString();
                                lblMaintainance.Text = dt.Rows[0]["Maintainance"].ToString();
                                lblFacingCharges.Text = dt.Rows[0]["FacingCharges"].ToString();
                                lblTotalCost.Text = dt.Rows[0]["TotalAmount"].ToString();
                                lblTotalCommission.Text = dt.Rows[0]["Commission"].ToString();
                                lblTDS.Text = dt.Rows[0]["TDS"].ToString();
                                lblEligibility.Text = dt.Rows[0]["Eligibility"].ToString();
                                //lblAdjustment.Text = dt.Rows[0]["Adjustment"].ToString();
                                lblFinalCommission.Text = dt.Rows[0]["FinalComission"].ToString();
                                lblCustomerName.Text = dt.Rows[0]["Name"].ToString();
                                BindCommissions(dt.Rows[0]["PassbookId"].ToString(), dt.Rows[0]["ProjectId"].ToString());
                                BindPayments(dt.Rows[0]["PassbookId"].ToString(), dt.Rows[0]["ProjectId"].ToString());
                                //BindPassbookDocuents(dt.Rows[0]["Passbookno"].ToString(), dt.Rows[0]["Passbookno"].ToString());
                                BindCommissionsPayments(dt.Rows[0]["PassbookId"].ToString(), dt.Rows[0]["ProjectId"].ToString());
                            }
                        }
                    }
                }
            }
        }

        private void BindCommissionsPayments(string PassbookNo,string ProjectId)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                string htmldata = string.Empty;
                double total = 0;
                htmldata += "<table class='table table-bordered table-striped mt-3' id='commissionTabl'>" +
                    "<thead>" +
                        "<tr>" +
                               "<th>VoucherNo</th>" + "<th>Marketer</th>" + "<th>Payment Date</th>" + "<th>Amount</th>" + "<th>PaymentMethod</th>" +
                        "</tr>" +
                    "</thead><tbody>";
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("select VoucherNo,MarketerName,Amount,PaymentDate,PaymentMethod from CommissionPayments where PassbookNo='" + PassbookNo + "' and ProjectId='" + ProjectId + "'"))
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
                                    total = total + Convert.ToDouble(dt.Rows[i]["Amount"]);
                                    htmldata += "<tr>" +
                                                    "<td>" + dt.Rows[i]["VoucherNo"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["MarketerName"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["PaymentDate"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["Amount"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["PaymentMethod"] + "</td>" +
                                    "</tr>";
                                }
                            }
                        }
                    }
                }
                htmldata += "</tbody><tfoot><tr><td></td><td></td><td></td><td>" + total + "</td><td></td></tr></tfoot></table>";
                htmCPDiv.InnerHtml = htmldata;
            }
            catch (Exception ex)
            {
                err.LogError(ex, ErrorPath);
                Response.Redirect("Error.aspx");
            }
        }
        private void BindCommissions(string PassbookNo, string ProjectId)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                string htmldata = string.Empty;
                double totalEligibility = 0;
                htmldata += "<table class='table table-bordered table-striped mt-3' id='commissionTabl'>" +
                    "<thead>" +
                        "<tr>" +
                               "<th>Marketer</th>" + "<th>Payment Date</th>" + "<th>%</th>" + "<th>Total</th>" + "<th>TDS</th>" + "<th>Eligibility</th>" + "<th>Paid</th>" + "<th>Pending</th>" +
                        "</tr>" +
                    "</thead><tbody>";
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("Select * from CommissionEntry where PassbookNo='" + PassbookNo + "' and ProjectId='" + ProjectId + "'"))
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
                                    totalEligibility = totalEligibility + Convert.ToDouble(dt.Rows[i]["Eligibility"]);
                                    htmldata += "<tr>" +
                                                    "<td>" + dt.Rows[i]["MarketerName"] + "</td>" +
                                                    "<td>" + Convert.ToDateTime(dt.Rows[i]["PaymentDate"]).ToString("dd/MM/yyyy") + "</td>" +
                                                    "<td>" + dt.Rows[i]["Percentage"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["Total"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["TDS"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["Eligibility"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["Paid"] + "</td>" +
                                                    "<td>" + dt.Rows[i]["Pending"] + "</td>" +
                                    "</tr>";
                                }
                            }
                        }
                    }
                }
                htmldata += "</tbody><tfoot><tr><td></td><td></td><td></td><td></td><td></td><td>" + totalEligibility + "</td><td></td><td></td></tr></tfoot></table>";
                htmlCDiv.InnerHtml = htmldata;
            }
            catch (Exception ex)
            {
                err.LogError(ex, ErrorPath);
                Response.Redirect("Error.aspx");
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
                    using (MySqlCommand cmd = new MySqlCommand("Select * from PlotPayments where PassbookNo='" + PassbookNo + "' and ProjectId='"+ProjectId+"'"))
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
                                        "<td>" + Convert.ToDateTime(dt.Rows[i]["PaymentDate"]).ToString("dd/MM/yyyy") + "</td>" +
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
        private void BindPassbookDocuents(string PassbookNo, string ProjectId)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                string htmldata = string.Empty;
                htmldata += "<table class='table table-bordered table-striped mt-3' id='commissionTable'>" +
                    "<thead>" +
                        "<tr>" +
                           "<th>No</th>" +
                               "<th>DocumentName</th>" + "<th>Action</th>" +
                        "</tr>" +
                    "</thead><tbody>";
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("Select * from PassbookDocuments where PassbookNo='" + PassbookNo + "' and ProjectId='" + ProjectId + "'"))
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
                htmlDDiv.InnerHtml = htmldata;
            }
            catch (Exception ex)
            {
                err.LogError(ex, ErrorPath);
                Response.Redirect("Error.aspx");
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DownloadPassbook.aspx?Id=" + Request.QueryString["Id"]);
        }
    }
}