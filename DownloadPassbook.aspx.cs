using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web;

namespace RealEstateCRM
{
    public partial class DownloadPassbook : System.Web.UI.Page
    {
        ErrorFile err = new ErrorFile();
        string ErrorPath = string.Empty;
        [Obsolete]
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
                //BindData("4333");
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
                using (MySqlCommand cmd = new MySqlCommand("Select ps.*,pt.Status as PlotStatus,pt.PlotNo,pt.Facing,pt.Size as PlotSize FROM Passbook ps, Plots pt where pt.PlotId=ps.PlotId and ps.PassbookId=" + PassBookId + ""))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dtPassbook = new DataTable())
                        {
                            sda.Fill(dtPassbook);
                            if (dtPassbook.Rows.Count > 0)
                            {
                                DataTable dtPayments = BindPayments(dtPassbook.Rows[0]["PassBookId"].ToString());
                                DataTable dtCommissions = BindCommissions(dtPassbook.Rows[0]["PassBookId"].ToString());
                                DataTable dtCommissionPayments = BindCommissionsPayments(dtPassbook.Rows[0]["PassBookId"].ToString());
                                CreatePassbookPDF(dtPassbook, dtPayments, dtCommissions, dtCommissionPayments);
                            }
                        }
                    }
                }
            }
        }
        private DataTable BindCommissionsPayments(string PassbookId)
        {
            DataTable dt = new DataTable();
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("select * from CommissionPayments where PassbookNo='" + PassbookId + "'"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                err.LogError(ex, ErrorPath);
                Response.Redirect("Error.aspx");
            }
            return dt;
        }
        private DataTable BindCommissions(string PassbookId)
        {
            DataTable dt = new DataTable();
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("Select * from CommissionEntry where PassbookNo='" + PassbookId + "'"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                err.LogError(ex, ErrorPath);
                Response.Redirect("Error.aspx");
            }
            return dt;
        }
        private DataTable BindPayments(string PassbookId)
        {
            DataTable dt = new DataTable();
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("Select * from PlotPayments where PassbookNo=" + PassbookId + ""))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                err.LogError(ex, ErrorPath);
                Response.Redirect("Error.aspx");
            }
            return dt;
        }
        public void CreatePassbookPDF(DataTable dtPassbook, DataTable dtPayments, DataTable dtCommissions, DataTable dtCommissionPayments)
        {
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
                Font NormalFont = FontFactory.GetFont("sans-serif", 12, Font.NORMAL, BaseColor.BLACK);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                PdfPTable HeaderTable = null;
                PdfPTable RoomTable = null;
                PdfPTable FooterTable = null;
                PdfPTable AmountTable = null;

                PdfPCell cell = null;
                BaseColor SkyBlue = WebColors.GetRGBColor("#3c8dbc");
                BaseColor Grey = WebColors.GetRGBColor("#696969");
                BaseColor Black = WebColors.GetRGBColor("#444");
                BaseColor Green = WebColors.GetRGBColor("#0b6623");
                BaseColor Orange = WebColors.GetRGBColor("#ed872d");

                document.Open();
                //A4 Page width = 595 height = 892
                HeaderTable = new PdfPTable(6);
                HeaderTable.TotalWidth = 590f;
                HeaderTable.LockedWidth = true;

                RoomTable = new PdfPTable(8);
                RoomTable.TotalWidth = 590f;
                RoomTable.LockedWidth = true;

                FooterTable = new PdfPTable(5);
                FooterTable.TotalWidth = 590f;
                FooterTable.LockedWidth = true;

                AmountTable = new PdfPTable(5);
                AmountTable.TotalWidth = 590f;
                AmountTable.LockedWidth = true;

                cell = new PdfPCell(new Phrase("Passbook Details", FontFactory.GetFont("sans-serif", 16, Font.BOLD, Black)));
                cell.Colspan = 6;
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 6;
                cell.BorderWidth = 0;
                cell.MinimumHeight = 10f;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Passbook No: ", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["Passbookno"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.NORMAL, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("PlotNo : ", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["PlotNo"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.NORMAL, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Customer Name : ", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black))); ;
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["Name"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.NORMAL, Grey)));
                cell.BorderWidth = 0;
                cell.SetLeading(0f, 1.5f);
                cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Facing : ", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["Facing"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.NORMAL, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Size(Sq Yards):", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["PlotSize"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.NORMAL, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Plot Cost :", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["PlotAmount"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.BOLD, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Maintainance :", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["Maintainance"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.BOLD, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Facing Charges:", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["FacingCharges"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.BOLD, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Total Cost : ", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["TotalAmount"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.BOLD, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Total Commission : ", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["Commission"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.BOLD, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("TDS(5%): ", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["TDS"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.BOLD, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Eligibility : ", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["Eligibility"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.BOLD, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("FinalCommission:", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(dtPassbook.Rows[0]["FinalComission"].ToString(), FontFactory.GetFont("sans-serif", 10, Font.BOLD, Grey)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
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
                //----------------------------------------------------------------------------- Header

                cell = new PdfPCell(new Phrase("Commission Entry", FontFactory.GetFont("sans-serif", 12, Font.BOLD, BaseColor.WHITE)));
                cell.Colspan = 8;
                cell.Padding = 7;
                cell.PaddingTop = 2;
                cell.BackgroundColor = SkyBlue;
                cell.BorderWidth = 0;
                RoomTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 8;
                cell.BorderWidth = 0;
                cell.SetLeading(0f, 3f);
                cell.MinimumHeight = 6f;
                RoomTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Marketer", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                RoomTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Date", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                RoomTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("%", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                RoomTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Total", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                RoomTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("TDS", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                RoomTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Eligibility", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                RoomTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Paid", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                RoomTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Pending", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                RoomTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 8;
                cell.BorderWidth = 0;
                cell.MinimumHeight = 3f;
                RoomTable.AddCell(cell);

                for (int i = 0; i < dtCommissions.Rows.Count; i++)
                {
                    cell = new PdfPCell(new Phrase(dtCommissions.Rows[i]["MarketerName"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.PaddingLeft = 8;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RoomTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Convert.ToDateTime(dtCommissions.Rows[i]["PaymentDate"]).ToString("dd/MM/yyyy"), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RoomTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtCommissions.Rows[i]["Percentage"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RoomTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtCommissions.Rows[i]["Total"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RoomTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtCommissions.Rows[i]["TDS"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RoomTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtCommissions.Rows[i]["Eligibility"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RoomTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtCommissions.Rows[i]["Paid"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RoomTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtCommissions.Rows[i]["Pending"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RoomTable.AddCell(cell);
                }
                cell = new PdfPCell();
                cell.Colspan = 8;
                cell.BorderWidth = 0;
                cell.MinimumHeight = 10f;
                RoomTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 8;
                cell.BorderWidth = 0;
                cell.SetLeading(0f, 3f);
                cell.MinimumHeight = 6f;
                RoomTable.AddCell(cell);
                //----------------------------------------------------------------------------- Commission Payments
                cell = new PdfPCell(new Phrase("Commission Payments", FontFactory.GetFont("sans-serif", 12, Font.BOLD, BaseColor.WHITE)));
                cell.Colspan = 5;
                cell.Padding = 7;
                cell.PaddingTop = 2;
                cell.BackgroundColor = SkyBlue;
                cell.BorderWidth = 0;
                FooterTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 5;
                cell.BorderWidth = 0;
                cell.SetLeading(0f, 3f);
                cell.MinimumHeight = 6f;
                FooterTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("VoucherNo", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                FooterTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Marketer", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                FooterTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Payment Date", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                FooterTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                FooterTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Method", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                FooterTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 5;
                cell.BorderWidth = 0;
                cell.MinimumHeight = 3f;
                FooterTable.AddCell(cell);

                for (int i = 0; i < dtCommissionPayments.Rows.Count; i++)
                {
                    cell = new PdfPCell(new Phrase(dtCommissionPayments.Rows[i]["VoucherNo"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.PaddingLeft = 8;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    FooterTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtCommissionPayments.Rows[i]["MarketerName"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    FooterTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtCommissionPayments.Rows[i]["PaymentDate"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    FooterTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtCommissionPayments.Rows[i]["Amount"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    FooterTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtCommissionPayments.Rows[i]["PaymentMethod"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    FooterTable.AddCell(cell);
                }
                cell = new PdfPCell();
                cell.Colspan = 5;
                cell.BorderWidth = 0;
                cell.MinimumHeight = 10f;
                FooterTable.AddCell(cell);


                //----------------------------------------------------------------------------- Plot Payments
                cell = new PdfPCell(new Phrase("Plot Payments", FontFactory.GetFont("sans-serif", 12, Font.BOLD, BaseColor.WHITE)));
                cell.Colspan = 5;
                cell.Padding = 7;
                cell.PaddingTop = 2;
                cell.BackgroundColor = SkyBlue;
                cell.BorderWidth = 0;
                FooterTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 5;
                cell.BorderWidth = 0;
                cell.SetLeading(0f, 3f);
                cell.MinimumHeight = 6f;
                FooterTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Date", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                AmountTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("ReceiptNo", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                AmountTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                AmountTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("PaymentMethod", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                AmountTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("PaymentDetails", FontFactory.GetFont("sans-serif", 10, Font.BOLD, Black)));
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                AmountTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Colspan = 5;
                cell.BorderWidth = 0;
                cell.MinimumHeight = 3f;
                AmountTable.AddCell(cell);

                for (int i = 0; i < dtCommissionPayments.Rows.Count; i++)
                {
                    cell = new PdfPCell(new Phrase(dtPayments.Rows[i]["PaymentDate"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.PaddingLeft = 8;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    AmountTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtPayments.Rows[i]["ReceiptNo"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    AmountTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtPayments.Rows[i]["Amount"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    AmountTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtPayments.Rows[i]["PaymentMethod"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    AmountTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(dtPayments.Rows[i]["PaymentDetails"].ToString(), FontFactory.GetFont("sans-serif", 8, Font.NORMAL, Grey)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    cell.SetLeading(0f, 1.5f);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    AmountTable.AddCell(cell);
                }
                cell = new PdfPCell();
                cell.Colspan = 5;
                cell.BorderWidth = 0;
                cell.MinimumHeight = 10f;
                AmountTable.AddCell(cell);

                document.Add(HeaderTable);
                document.Add(RoomTable);
                document.Add(FooterTable);
                document.Add(AmountTable);

                document.Close();
                writer.Close();

                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename= Passbook.pdf");
                HttpContext.Current.Response.OutputStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
            }
        }
    }
}