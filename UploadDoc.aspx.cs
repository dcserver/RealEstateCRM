using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RealEstateCRM
{
    public partial class UploadDoc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnPlots_Click(object sender, EventArgs e)
        {
            DataTable dtPlots = ImportPlots();
            if (dtPlots.Columns.Count > 0)
            {
                InsertPlots(dtPlots);
            }
        }
        private DataTable ImportPlots()
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(@"D:\VCPlots.tsv"))
            {
                string[] headers = sr.ReadLine().Split('\t');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split('\t');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }
        private void InsertPlots(DataTable dtPlots)
        {
            string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            for (int i = 0; i < dtPlots.Rows.Count; i++)
            {
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Plots (ProjectId, Status,PlotNo,Size,Amount,Facing,MaintainanceCharges,FacingCharges) VALUES (@ProjectId, @Status,@PlotNo,@Size,@Amount,@Facing,@MaintainanceCharges,@FacingCharges)"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@ProjectId", dtPlots.Rows[i]["PROJECT ID"]);
                            cmd.Parameters.AddWithValue("@PlotNo", dtPlots.Rows[i]["PLOT NO"]);
                            cmd.Parameters.AddWithValue("@Status", dtPlots.Rows[i]["STATUS"]);
                            cmd.Parameters.AddWithValue("@Size", dtPlots.Rows[i]["SIZE"]);
                            cmd.Parameters.AddWithValue("@Amount", dtPlots.Rows[i]["Plot Cost"]);
                            cmd.Parameters.AddWithValue("@Facing", dtPlots.Rows[i]["Facing"]);
                            cmd.Parameters.AddWithValue("@MaintainanceCharges", dtPlots.Rows[i]["MAINTAINANCE"]);
                            cmd.Parameters.AddWithValue("@FacingCharges", dtPlots.Rows[i]["Facing Charges"]);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
        }

        protected void btnPassbook_Click(object sender, EventArgs e)
        {
            DataTable dt = ImportPassbook();
            if (dt.Columns.Count > 0)
            {
                InsertPassbook(dt);
            }
        }
        private DataTable ImportPassbook()
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(@"D:\GoldPassBook.tsv"))
            {
                string[] headers = sr.ReadLine().Split('\t');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split('\t');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }
        private void InsertPassbook(DataTable dtPlots)
        {
            string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            for (int i = 0; i < dtPlots.Rows.Count; i++)
            {
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
                            string PlotId = GetPlotId(dtPlots.Rows[i]["Plot No"].ToString());
                            cmd.Parameters.AddWithValue("@PlotId", PlotId);
                            cmd.Parameters.AddWithValue("@PassbookNo", dtPlots.Rows[i]["PassbookNo"]);
                            cmd.Parameters.AddWithValue("@DateOfJoin", dtPlots.Rows[i]["Date of Joining"]);
                            DateTime endDate = Convert.ToDateTime(dtPlots.Rows[i]["Date of Joining"]);
                            endDate = endDate.AddDays(45);
                            cmd.Parameters.AddWithValue("@PaymentLastDate", endDate);
                            cmd.Parameters.AddWithValue("@Nominee", "");
                            cmd.Parameters.AddWithValue("@TotalAmount", dtPlots.Rows[i]["Total Price"]);
                            cmd.Parameters.AddWithValue("@Commission", dtPlots.Rows[i]["Commission"]);
                            cmd.Parameters.AddWithValue("@TDS", dtPlots.Rows[i]["Tds"]);
                            cmd.Parameters.AddWithValue("@Eligibility", dtPlots.Rows[i]["Eligibility"]);
                            cmd.Parameters.AddWithValue("@Adjustment", "0");
                            cmd.Parameters.AddWithValue("@FinalComission", dtPlots.Rows[i]["Eligibility"]);
                            cmd.Parameters.AddWithValue("@PlotAmount", dtPlots.Rows[i]["Plot Cost"]);
                            cmd.Parameters.AddWithValue("@Maintainance", dtPlots.Rows[i]["Maintainance"]);
                            cmd.Parameters.AddWithValue("@PendingAmount", dtPlots.Rows[i]["Balnce Amount"]);
                            cmd.Parameters.AddWithValue("@Name", dtPlots.Rows[i]["Customer Name"]);
                            cmd.Parameters.AddWithValue("@Mobile", "");
                            cmd.Parameters.AddWithValue("@Address", "");
                            cmd.Parameters.AddWithValue("@UserId", "1");
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@FacingCharges", dtPlots.Rows[i]["Total Facing Charges"]);
                            cmd.Parameters.AddWithValue("@ProjectId", "3");
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            UpdatePlotData(PlotId);
                            //lblstatus.Text = "Passbook Created Successfully";
                            //lblstatus.ForeColor = Color.Green;
                            //GeneratePassBookNo();
                            //BindPlots();
                            //globalData.SendPlotBookingSMS(txtMobile.Text, ddlPlots.SelectedItem.Text);
                            //Reset();
                        }
                    }
                }
            }
        }
        private void UpdatePlotData(string selectedValue)
        {
            try
            {
                string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("UPDATE Plots set Status=@Status where PlotId=@PlotId and ProjectId=ProjectId"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@Status", "P");
                            cmd.Parameters.AddWithValue("@PlotId", selectedValue);
                            cmd.Parameters.AddWithValue("@ProjectId", '3');
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
                Response.Redirect("Error.aspx");
            }
        }
        private string GetPlotId(string PlotNo)
        {
            string PlotId = "0";
            string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT PlotId from Plots Where PlotNo='" + PlotNo + "' and ProjectId='3'"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                PlotId = reader.GetValue(0).ToString();
                            }
                        }
                        con.Close();
                    }
                }
            }
            return PlotId;
        }

        protected void btnReceipts_Click(object sender, EventArgs e)
        {
            DataTable dt = ImportReceipt();
            if (dt.Columns.Count > 0)
            {
                InsertReceipt(dt);
            }
        }
        private DataTable ImportReceipt()
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(@"D:\b1.tsv"))
            {
                string[] headers = sr.ReadLine().Split('\t');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split('\t');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }
        private void InsertReceipt(DataTable dtPlots)
        {
            string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            for (int i = 0; i < dtPlots.Rows.Count; i++)
            {
                string PassbookId = GetPassbookId(dtPlots.Rows[i]["PassbookNo"].ToString());
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
                            cmd.Parameters.AddWithValue("@ReceiptNo", PassbookId);
                            cmd.Parameters.AddWithValue("@PassbookNo", dtPlots.Rows[i]["PassbookNo"]);
                            cmd.Parameters.AddWithValue("@Amount", dtPlots.Rows[i]["AmountReceived"]);
                            cmd.Parameters.AddWithValue("@PaymentDate", dtPlots.Rows[i]["Date"]);
                            cmd.Parameters.AddWithValue("@PaymentMethod", dtPlots.Rows[i]["Method"]);
                            cmd.Parameters.AddWithValue("@PaymentDetails", "");
                            cmd.Parameters.AddWithValue("@UserId", "1");
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@ProjectId", "3");
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            //UpdatePlotPayment(dtPlots.Rows[i]);
                        }
                    }
                }
            }
        }

        protected void btnCommissions_Click(object sender, EventArgs e)
        {
            DataTable dtPlots = ImportCommissions();
            if (dtPlots.Columns.Count > 0)
            {
                InsertCommission(dtPlots);
            }
        }
        private DataTable ImportCommissions()
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(@"D:\cp.tsv"))
            {
                string[] headers = sr.ReadLine().Split('\t');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split('\t');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }

        private void InsertCommission(DataTable dt)
        {
            string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string PassbookId = GetPassbookId(dt.Rows[i]["PassbookNo"].ToString());
                using (MySqlConnection con = new MySqlConnection(dbConnection))
                {
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO CommissionEntry (PassbookNo,CreatedDate,UpdatedDate," +
                        "PaymentDate,Percentage,Total,TDS,Eligibility,Advance,Adjustment,Pending,Paid,MarketerName,ProjectId) VALUES (@PassbookNo,@CreatedDate,@UpdatedDate," +
                        "@PaymentDate,@Percentage,@Total,@TDS,@Eligibility,@Advance,@Adjustment,@Pending,@Paid,@MarketerName,@ProjectId)"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@PassbookNo", PassbookId);
                            cmd.Parameters.AddWithValue("@MarketerName", dt.Rows[i]["MarketerName"]);
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Percentage", dt.Rows[i]["Percentage"]);
                            cmd.Parameters.AddWithValue("@Total", dt.Rows[i]["Total"]);
                            cmd.Parameters.AddWithValue("@TDS", dt.Rows[i]["TDS"]);
                            cmd.Parameters.AddWithValue("@Eligibility", dt.Rows[i]["Eligibility"]);
                            cmd.Parameters.AddWithValue("@Advance", dt.Rows[i]["Advance"]);
                            cmd.Parameters.AddWithValue("@Adjustment", dt.Rows[i]["Adjustment"]);
                            cmd.Parameters.AddWithValue("@Pending", dt.Rows[i]["Pending"]);
                            cmd.Parameters.AddWithValue("@Paid", dt.Rows[i]["Paid"]);
                            cmd.Parameters.AddWithValue("@ProjectId", '3');
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
        }
        private string GetPassbookId(string PassbookNo)
        {
            string PassbookId = "0";
            string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(dbConnection))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT PassbookId from Passbook Where PassbookNo='" + PassbookNo + "' and ProjectId='3'"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                PassbookId = reader.GetValue(0).ToString();
                            }
                        }
                        con.Close();
                    }
                }
            }
            return PassbookId;
        }
    }
}