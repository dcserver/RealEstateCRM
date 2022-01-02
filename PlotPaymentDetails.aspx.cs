using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using System.Web;

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
                    using (MySqlCommand cmd = new MySqlCommand("select * from PlotPayments pass where PassbookNo='" + ddlPassbook.SelectedValue + "' and ProjectId='"+ddlProjects.SelectedValue+"'"))
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
                                                    "<td>" + dt.Rows[i]["PaymentDate"] + "</td>" +
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
            //ddlPassbook.ClearSelection();
            ddlPaymentMethod.SelectedValue = "GooglePay";
            txtTotalPlotamount.Text = "0";
            //ddlProjects.ClearSelection();
            GenerateReceiptNo();
            CreateReceiptPDF(hdnpdfReceipt.Value, hdnpdfPaymentDate.Value, hdnpdfAmount.Value, hdnpdfPassbookId.Value, hdnpdfpdfPassbookNo.Value, hdnpdfPaymentMethod.Value, hdnpdfPaymentDetails.Value, hdnpdfProjectId.Value);
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
                            hdnpdfReceipt.Value = txtReceiptNo.Text;
                            hdnpdfPaymentDate.Value = DateTime.Now.Date.ToString("dd/MM/yyyy");
                            hdnpdfAmount.Value = txtAmount.Text;
                            hdnpdfPassbookId.Value = ddlPassbook.SelectedValue;
                            hdnpdfpdfPassbookNo.Value = ddlPassbook.SelectedItem.Text;
                            hdnpdfPaymentMethod.Value = ddlPaymentMethod.SelectedItem.Text;
                            hdnpdfPaymentDetails.Value = txtPaymentReference.Text;
                            hdnpdfProjectId.Value = ddlProjects.SelectedValue;
                            //gc.SendPaymentSMSToCustomer(txtMobile.Text,txtAmount.Text, hdnPlotNo.Value);
                            //gc.SendPaymentSMSToMD("9985340876", txtAmount.Text, hdnPlotNo.Value);                            
                            UpdatePlotData();
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
        private string GetProjectName(string ProjectId)
        {
            string ProjectName = "0";
            string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT ProjectName from Projects Where ProjectId='" + ProjectId + "'"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ProjectName = reader.GetValue(0).ToString();
                            }
                        }
                        con.Close();
                    }
                }
            }
            return ProjectName;
        }
        private string GetCustomerName(string PassbookId,string ProjectId)
        {
            string CustomerName = "0";
            string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT Name from Passbook Where PassbookId='" + PassbookId + "' and ProjectId='"+ ProjectId + "'"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CustomerName = reader.GetValue(0).ToString();
                            }
                        }
                        con.Close();
                    }
                }
            }
            return CustomerName;
        }
        public void CreateReceiptPDF(string ReceiptNo,string PaymentDate, string Amount,string PassbookId, string PassbookNo,string PaymentMethod,string PaymentDetails,string ProjectId)
        {
            string ProjectName = GetProjectName(ProjectId);
            string CustomerName = GetCustomerName(PassbookId, ProjectId);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
                Font NormalFont = FontFactory.GetFont("sans-serif", 12, Font.NORMAL, BaseColor.BLACK);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                PdfPTable HeaderTable = null;

                PdfPCell cell = null;
                BaseColor SkyBlue = WebColors.GetRGBColor("#3c8dbc");
                BaseColor Grey = WebColors.GetRGBColor("#696969");
                BaseColor Black = WebColors.GetRGBColor("#444");
                BaseColor Green = WebColors.GetRGBColor("#0b6623");
                BaseColor Orange = WebColors.GetRGBColor("#ed872d");

                document.Open();
                //A4 Page width = 595 height = 892
                HeaderTable = new PdfPTable(12);
                HeaderTable.TotalWidth = 590f;
                HeaderTable.LockedWidth = true;

                cell = new PdfPCell();
                cell.Colspan = 12;
                cell.BorderWidth = 0;
                cell.MinimumHeight = 10f;
                HeaderTable.AddCell(cell);

                Image logo = Image.GetInstance(Server.MapPath("~/assets/logo.png"));
                cell = new PdfPCell(logo);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Colspan = 6;
                cell.Rowspan = 6;
                cell.SetLeading(0f, 1.2f);
                cell.BorderWidth = 0;
                HeaderTable.AddCell(cell);

                //Image prlogo = Image.GetInstance(Server.MapPath("~/assets/logo.png"));
                //cell = new PdfPCell(prlogo);
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.Colspan = 6;
                //cell.Rowspan = 5;
                //cell.SetLeading(0f, 1.2f);
                //cell.BorderWidth = 0;
                //HeaderTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 12;
                cell.BorderWidth = 0;
                cell.MinimumHeight = 10f;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("VAISHNAVI DEVELOPERS", FontFactory.GetFont("sans-serif", 14, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 12;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("TRUST - INNOVATION - TRADITION", FontFactory.GetFont("sans-serif", 8, Font.BOLD, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 12;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("#5-5-789, Plot No.71, 2nd Floor, Agamalah Nagar Colony", FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 12;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Chintakunta, Beside KLM Shopping Mall, Mansoorabad,", FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 12;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Hyderabad - 500 074.", FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 12;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 12;
                cell.BorderWidth = 0;
                cell.MinimumHeight = 10f;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 12;
                cell.BorderWidth = 0;
                cell.SetLeading(0f, 3f);
                cell.MinimumHeight = 6f;

                cell = new PdfPCell(new Phrase("RECEIPT", FontFactory.GetFont("sans-serif", 18, Font.BOLD, Black)));
                cell.Colspan = 12;
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 6;
                cell.BorderWidth = 0;
                cell.MinimumHeight = 10f;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 6;
                cell.BorderWidth = 0;
                cell.SetLeading(0f, 3f);
                cell.MinimumHeight = 6f;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project : "+ ProjectName, FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Date : " + PaymentDate, FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Receipt No : " + ReceiptNo, FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("PassbookNo : " + PassbookNo, FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Customer Name : "+ CustomerName, FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Amount(INR) : " + Amount, FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Details : " + PaymentMethod + " " + PaymentDetails, FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Towards : Plot Payment", FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 6;
                cell.BorderWidth = 0;
                cell.MinimumHeight = 10f;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 6;
                cell.BorderWidth = 0;
                cell.SetLeading(0f, 3f);
                cell.MinimumHeight = 6f;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 6;
                cell.BorderWidth = 0;
                cell.SetLeading(0f, 3f);
                cell.MinimumHeight = 6f;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 6;
                cell.BorderWidth = 0;
                cell.SetLeading(0f, 3f);
                cell.MinimumHeight = 6f;
                HeaderTable.AddCell(cell);


                cell = new PdfPCell();
                cell.Colspan = 6;
                cell.BorderWidth = 0;
                cell.SetLeading(0f, 3f);
                cell.MinimumHeight = 6f;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Authorised Signature", FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 0;
                cell.Colspan = 12;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                document.Add(HeaderTable);
                document.Close();
                writer.Close();

                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename= ReceiptNo_" + ReceiptNo + "_PassbookNo_" + PassbookNo + "_" + ProjectName + ".pdf");
                HttpContext.Current.Response.OutputStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
                //HttpContext.Current.Response.End();
            }
        }
    }
}