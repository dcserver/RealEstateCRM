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
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateReceiptPDF();
        }
        public void CreateReceiptPDF()
        {
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
                cell.Rowspan = 5;
                cell.SetLeading(0f, 1.2f);
                cell.BorderWidth = 0;
                HeaderTable.AddCell(cell);

                Image prlogo = Image.GetInstance(Server.MapPath("~/assets/logo.png"));
                cell = new PdfPCell(prlogo);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Colspan = 6;
                cell.Rowspan = 5;
                cell.SetLeading(0f, 1.2f);
                cell.BorderWidth = 0;
                HeaderTable.AddCell(cell);

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

                cell = new PdfPCell(new Phrase("Project : SRI SAI HARITHAVANAM GOLD", FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Date : 10/12/2021", FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Receipt No : 145", FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("PassbookNo : 150", FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Customer Name : SAYYAD TANISHA", FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Amount(INR) : 150000", FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 1;
                cell.Colspan = 6;
                cell.PaddingLeft = 8;
                cell.SetLeading(0f, 1.5f);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeaderTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("In Words : Fifteen Thousand Rupees only", FontFactory.GetFont("sans-serif", 10, Font.NORMAL, BaseColor.BLACK)));
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
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename= Receipt.pdf");
                HttpContext.Current.Response.OutputStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
            }
        }
    }
}