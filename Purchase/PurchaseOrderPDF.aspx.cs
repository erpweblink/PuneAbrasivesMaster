using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Net.Mail;
using iTextSharp.text.pdf;
using System.Globalization;
using System.Threading.Tasks;
using iTextSharp.text;
using System.Collections;

public partial class Admin_PurchaseOrderPDF : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
    CommonCls objcls = new CommonCls();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserCode"] == null)
        {
            Response.Redirect("../Login.aspx");
        }
        else
        {
            if (!IsPostBack)
            {

                if (Request.QueryString["ID"] != null)
                {
                    ID = objcls.Decrypt(Request.QueryString["ID"].ToString());
                    // ID = Request.QueryString["Pono"].ToString();
                    Pdf(ID);
                }
            }
        }
    }

    private void Pdf(string ID)
    {

        SqlDataAdapter Da = new SqlDataAdapter("select * from vw_PurchaseOrder where Id = '" + ID + "'", con);

        Da.Fill(Dt);

        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());

        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);
        string DocName = (Dt.Rows[0]["PONo"].ToString() + " " + Dt.Rows[0]["SupplierName"].ToString() + "_PO.pdf").Replace("/", "_");
        lblcompany.Text = Dt.Rows[0]["SupplierName"].ToString();
        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/PDF_Files/") + "PurchaseOrder.pdf", FileMode.Create));

        doc.Open();

        string imageURL = Server.MapPath("~") + "/Content/Img/pune_abrassiv_logo.jpg";
        // string imageURLL = Server.MapPath("~") + "/image/Tirupati-Stamp_Final.png";
        System.Globalization.CultureInfo info = System.Globalization.CultureInfo.GetCultureInfo("en-IN");
        iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);
        // iTextSharp.text.Image pngg = iTextSharp.text.Image.GetInstance(imageURLL);

        //Resize image depend upon your need

        png.ScaleToFit(100, 70);
        // pngg.ScaleToFit(100, 50);

        //For Image Position
        png.SetAbsolutePosition(30, 740);
        //  pngg.SetAbsolutePosition(440, 160);
        //var document = new Document();

        //Give space before image
        //png.ScaleToFit(document.PageSize.Width - (document.RightMargin * 100), 50);
        png.SpacingBefore = 50f;
        // pngg.SpacingBefore = 50f;

        //Give some space after the image

        png.SpacingAfter = 1f;
        //  pngg.SpacingAfter = 1f;

        png.Alignment = Element.ALIGN_LEFT;
        //  pngg.Alignment = Element.ALIGN_LEFT;

        //paragraphimage.Add(png);                                                                                                                                              
        //doc.Add(paragraphimage);
        doc.Add(png);
        // doc.Add(pngg);

        PdfContentByte cb = writer.DirectContent;

        cb.Rectangle(17f, 735f, 560f, 80f);
        cb.Stroke();

        // Header 
        cb.BeginText();

        //cdd.SetColorFill (BaseColor.RED);

        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 25);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Pune Abrasive Pvt. Ltd.", 165, 790, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Plot No. 84, 2nd Floor D2 Block, MIDC Chinchwad, KSB Chowk,Near Shell Petrol Pump,", 130, 775, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " Pune 411019, Maharashtra,India", 200, 763, 0);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Website:http://www.puneabrasives.com/", 200, 753, 0);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "GST No.: 27ABCCS7002A1ZW   |   PAN No.: ABCCS7002A", 160, 740, 0); cb.EndText();

        //PdfContentByte cbb = writer.DirectContent;
        //cbb.Rectangle(17f, 710f, 560f, 25f);
        //cbb.Stroke();
        //// Header 
        //cbb.BeginText();
        //cbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        //cbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " girish.kulkarni@puneabrasives.com                                                                                 +91 9860441689, 9511712429 ", 30, 720, 0);
        //cbb.EndText();

        //PdfContentByte cd = writer.DirectContent;
        //cd.Rectangle(17f, 680f, 560f, 25f);
        //cd.Stroke();
        //// Header 
        //cd.BeginText();
        //cd.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 17);
        //cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PURCHASE ORDER", 245, 687, 0);
        //cd.EndText();

        //PdfContentByte cbb = writer.DirectContent;
        //cbb.Rectangle(17f, 710f, 560f, 25f);
        //cbb.Stroke();
        //// Header 
        //cbb.BeginText();
        //cbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        //cbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " CONTACT : 9225658662   Email ID : mktg@excelenclosures.com", 153, 722, 0);
        //cbb.EndText();

        PdfContentByte cbbb = writer.DirectContent;
        cbbb.Rectangle(17f, 710f, 560f, 25f);
        cbbb.Stroke();
        // Header 
        cbbb.BeginText();
        cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "GSTIN : 27ABCCS7002A1ZW", 48, 720, 0);
        cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PAN NO: ABCCS7002A", 170, 720, 0);
        cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "EMAIL :  girish.kulkarni@puneabrasives.com", 280, 720, 0);
        cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "CONTACT : +91 9860441689, 9511712429 ", 455, 720, 0);
        cbbb.EndText();

        PdfContentByte cd1 = writer.DirectContent;
        cd1.Rectangle(17f, 685f, 560f, 25f);
        cd1.Stroke();
        // Header 
        cd1.BeginText();
        cd1.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 14);
        cd1.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Purchase Order", 270, 693, 0);
        cd1.EndText();

        if (Dt.Rows.Count > 0)
        {
            var CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
            string PONo = Dt.Rows[0]["PONo"].ToString();
            string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
            string PODate = Convert.ToDateTime(Dt.Rows[0]["PODate"]).ToString("dd-MM-yyyy").TrimEnd("0:0".ToCharArray());
            string DeliveryDate = Convert.ToDateTime(Dt.Rows[0]["DeliveryDate"]).ToString("dd-MM-yyyy").TrimEnd("0:0".ToCharArray());
            string GrandTotal = Dt.Rows[0]["GrandTotat"].ToString();
            string SupplierName = Dt.Rows[0]["SupplierName"].ToString();
            string PaymentTerm = Dt.Rows[0]["PaymentTerm"].ToString();
            string PackingAndForwarding = Dt.Rows[0]["PackingAndForwarding"].ToString();
            string Transportation = Dt.Rows[0]["Transportation"].ToString();
            string Variation = Dt.Rows[0]["Variation"].ToString();
            string Delivery = Dt.Rows[0]["Delivery"].ToString();
            string TestCertificate = Dt.Rows[0]["TestCertificate"].ToString();
            string WeeklyOff = Dt.Rows[0]["WeeklyOff"].ToString();
            string Time = Dt.Rows[0]["Time"].ToString();
            string II = Dt.Rows[0]["II"].ToString();
            string Remarks = Dt.Rows[0]["Remarks"].ToString();
            string UOM = Dt.Rows[0]["UOM"].ToString();

            //17 march 2022
            string Transporattioncharges = Dt.Rows[0]["TotalCost"].ToString();
            string TransportationDescription = Dt.Rows[0]["TransportationDescription"].ToString();
            string TGST = Dt.Rows[0]["TCGSTPer"].ToString();
            string TIGST = Dt.Rows[0]["TIGSTPer"].ToString();
            string gstper = "0";
            if (TIGST == "0")
            {
                gstper = TGST.ToString();
            }
            else
            {
                gstper = TIGST.ToString();
            }

            string BillToAddress = "";
            string ShipToAddress = "";
            string StateName = "";
            string GSTNo = "";
            string PANNo = "";

            // SqlCommand cmdsum = new SqlCommand("select * from tblSupplierMaster where SupplierName", con);
            SqlDataAdapter ad = new SqlDataAdapter("select * from tbl_VendorMaster where VendorName='" + SupplierName + "'", con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                BillToAddress = dt.Rows[0]["Address"].ToString();
                ShipToAddress = dt.Rows[0]["Address"].ToString();
                StateName = dt.Rows[0]["State"].ToString();
                GSTNo = dt.Rows[0]["GSTNo"].ToString();
                PANNo = dt.Rows[0]["PANNo"].ToString();
            }

            Paragraph paragraphTable1 = new Paragraph();
            paragraphTable1.SpacingBefore = 120f;
            paragraphTable1.SpacingAfter = 0f;

            PdfPTable table = new PdfPTable(4);

            float[] widths2 = new float[] { 100, 180, 100, 180 };
            table.SetWidths(widths2);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            var date = DateTime.Now.ToString("yyyy-MM-dd");
            table.DefaultCell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;

            table.AddCell(new Phrase("PO No : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            table.AddCell(new Phrase(PONo, FontFactory.GetFont("Arial", 9, Font.NORMAL)));

            table.AddCell(new Phrase("PO Date :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            table.AddCell(new Phrase(PODate, FontFactory.GetFont("Arial", 9, Font.NORMAL)));

            table.AddCell(new Phrase("Supplier Name : \n", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            table.AddCell(new Phrase(SupplierName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Delivery Date :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            table.AddCell(new Phrase(DeliveryDate, FontFactory.GetFont("Arial", 9, Font.NORMAL)));

            table.AddCell(new Phrase("Address 1", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            table.AddCell(new Phrase(BillToAddress + "\n\n\n", FontFactory.GetFont("Arial", 9, Font.NORMAL)));

            table.AddCell(new Phrase("Address 2", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            table.AddCell(new Phrase(ShipToAddress + "\n\n\n", FontFactory.GetFont("Arial", 9, Font.NORMAL)));

            table.AddCell(new Phrase("GSTIN :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            table.AddCell(new Phrase(GSTNo, FontFactory.GetFont("Arial", 9, Font.NORMAL)));

            table.AddCell(new Phrase("State Name :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            table.AddCell(new Phrase(StateName, FontFactory.GetFont("Arial", 9, Font.NORMAL)));

            table.AddCell(new Phrase("Kind Att :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            table.AddCell(new Phrase(KindAtt, FontFactory.GetFont("Arial", 9, Font.NORMAL)));

            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 9, Font.NORMAL)));

            paragraphTable1.Add(table);
            doc.Add(paragraphTable1);

            PdfPCell tblcell = null;
            Paragraph paragraphTable2 = new Paragraph();
            paragraphTable2.SpacingAfter = 0f;

            if (Dt.Rows[0]["IGSTPer"].ToString() == "0")
            {
                table = new PdfPTable(12);
                float[] widths3 = new float[] { 4f, 40f, 13f, 12f, 10f, 8f, 15f, 8f, 12f, 8f, 12f, 16f };
                table.SetWidths(widths3);
            }
            else
            {
                table = new PdfPTable(10);
                float[] widths3 = new float[] { 4f, 40f, 13f, 12f, 10f, 8f, 12f, 8f, 9f, 13f };
                table.SetWidths(widths3);
            }

            double Ttotal_price = 0;
            double CGST_price = 0;
            double SGST_price = 0;
            double IGST_price = 0;
            if (Dt.Rows.Count > 0)
            {
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                //table.DefaultCell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                tblcell = new PdfPCell(new Phrase("SN.", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);
                tblcell = new PdfPCell(new Phrase("Name Of Particulars", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);
                tblcell = new PdfPCell(new Phrase("HSN Code", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);
                tblcell = new PdfPCell(new Phrase("Qty", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);
                tblcell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);
                tblcell = new PdfPCell(new Phrase("Disc (%)", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);
                tblcell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);
                if (Dt.Rows[0]["IGSTPer"].ToString() == "0")
                {
                    tblcell = new PdfPCell(new Phrase("CGST(%)", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase("CGST Amt", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase("SGST(%)", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase("SGST Amt", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                }
                else
                {
                    table.AddCell(new Phrase("IGST (%)", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                    table.AddCell(new Phrase("IGST Amt", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                }
                tblcell = new PdfPCell(new Phrase("Total", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);

                int rowid = 1;
                foreach (DataRow dr in Dt.Rows)
                {
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.DefaultCell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                    double Ftotal = Convert.ToDouble(dr["GrandTotal"].ToString());
                    string _ftotal = Ftotal.ToString("##.00");

                    string Qty = dr["Qty"].ToString();
                    string unit = dr["UOM"].ToString();

                    string description = dr["Particulars"].ToString().Replace("<br>", "") + "\n\n" + dr["Description"].ToString();

                    var ratef = Convert.ToDouble(dr["Rate"].ToString());
                    string Rate = ratef.ToString("N2", info);

                    var cgstf = Convert.ToDouble(dr["CGSTPer"].ToString());
                    string cgstper = cgstf.ToString("N2", info);

                    var sgstf = Convert.ToDouble(dr["SGSTPer"].ToString());
                    string sgstper = sgstf.ToString("N2", info);

                    var igstf = Convert.ToDouble(dr["IGSTPer"].ToString());
                    string igstper = igstf.ToString("N2", info);

                    var Amountf = Convert.ToDouble(dr["Amount"].ToString());
                    string Amount = Amountf.ToString("N2", info);

                    tblcell = new PdfPCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 8)));
                    tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase(description, FontFactory.GetFont("Arial", 8)));
                    tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                    tblcell.HorizontalAlignment = 0;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase(dr["HSN"].ToString(), FontFactory.GetFont("Arial", 8)));
                    tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase(Qty + " " + unit, FontFactory.GetFont("Arial", 8)));
                    tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase(Rate, FontFactory.GetFont("Arial", 8)));
                    tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase(dr["Discount"].ToString(), FontFactory.GetFont("Arial", 8)));
                    tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase(Amount, FontFactory.GetFont("Arial", 8)));
                    tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    if (Dt.Rows[0]["IGSTPer"].ToString() == "0")
                    {
                        tblcell = new PdfPCell(new Phrase(cgstper, FontFactory.GetFont("Arial", 8)));
                        tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                        tblcell.HorizontalAlignment = 1;
                        table.AddCell(tblcell);
                        tblcell = new PdfPCell(new Phrase(dr["CGSTAmt"].ToString(), FontFactory.GetFont("Arial", 8)));
                        tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                        tblcell.HorizontalAlignment = 1;
                        table.AddCell(tblcell);
                        tblcell = new PdfPCell(new Phrase(sgstper, FontFactory.GetFont("Arial", 8)));
                        tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                        tblcell.HorizontalAlignment = 1;
                        table.AddCell(tblcell);
                        tblcell = new PdfPCell(new Phrase(dr["SGSTAmt"].ToString(), FontFactory.GetFont("Arial", 8)));
                        tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                        tblcell.HorizontalAlignment = 1;
                        table.AddCell(tblcell);
                    }
                    else
                    {
                        tblcell = new PdfPCell(new Phrase(igstper, FontFactory.GetFont("Arial", 8)));
                        tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                        tblcell.HorizontalAlignment = 1;
                        table.AddCell(tblcell);
                        tblcell = new PdfPCell(new Phrase(dr["IGSTAmt"].ToString(), FontFactory.GetFont("Arial", 8)));
                        tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                        tblcell.HorizontalAlignment = 1;
                        table.AddCell(tblcell);
                    }
                    tblcell = new PdfPCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 8)));
                    tblcell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    rowid++;

                    Ttotal_price += Convert.ToDouble(dr["Amount"].ToString());
                    CGST_price += Convert.ToDouble(dr["CGSTAmt"].ToString());
                    SGST_price += Convert.ToDouble(dr["SGSTAmt"].ToString());
                    IGST_price += Convert.ToDouble(dr["IGSTAmt"].ToString());
                }

            }
            string amount = Ttotal_price.ToString();
            paragraphTable2.Add(table);
            doc.Add(paragraphTable2);

            //Space
            Paragraph paragraphTable3 = new Paragraph();

            string[] items = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

            Font font12 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font10 = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            Paragraph paragraph = new Paragraph("", font12);

            for (int i = 0; i < items.Length; i++)
            {
                paragraph.Add(new Phrase("", font10));
            }
            if (Dt.Rows[0]["IGSTPer"].ToString() == "0")
            {
                table = new PdfPTable(12);
                table.DefaultCell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                table.SetWidths(new float[] { 4f, 40f, 13f, 12f, 10f, 8f, 15f, 8f, 12f, 8f, 12f, 16f });
                table.AddCell(paragraph);

                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                if (Dt.Rows.Count == 1)
                {
                    table.AddCell(new Phrase("\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                }
                else if (Dt.Rows.Count == 2)
                {
                    table.AddCell(new Phrase("\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                }
                else if (Dt.Rows.Count == 3)
                {
                    table.AddCell(new Phrase("\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                }
                else if (Dt.Rows.Count == 4)
                {
                    table.AddCell(new Phrase("\n\n\n\n\n\n", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                }
                doc.Add(table);
            }
            else
            {
                table = new PdfPTable(10);
                table.DefaultCell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                table.SetWidths(new float[] { 4f, 40f, 13f, 12f, 10f, 8f, 12f, 8f, 9f, 13f });
                table.AddCell(paragraph);

                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                if (Dt.Rows.Count == 1)
                {
                    table.AddCell(new Phrase("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                }
                else if (Dt.Rows.Count == 2)
                {
                    table.AddCell(new Phrase("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                }
                else if (Dt.Rows.Count == 3)
                {
                    table.AddCell(new Phrase("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                }
                else if (Dt.Rows.Count == 4)
                {
                    table.AddCell(new Phrase("\n\n\n\n\n\n\n\n\n\n\n ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                }
                doc.Add(table);
            }
            //------------------------

            Paragraph paragraphTable55 = new Paragraph();

            string[] itemssszx = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

            Font font1311 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font1111 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Paragraph paragraphhhj = new Paragraph();
            //paragraphh.SpacingAfter = 10f;

            for (int i = 0; i < itemssszx.Length; i++)
            {
                paragraphhhj.Add(new Phrase());
            }


            if (Dt.Rows[0]["IGSTPer"].ToString() == "0")
            {
                table = new PdfPTable(12);
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                table.SetWidths(new float[] { 4f, 40f, 13f, 12f, 10f, 8f, 15f, 8f, 12f, 8f, 12f, 16f });
            }
            else
            {
                table = new PdfPTable(10);
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                table.SetWidths(new float[] { 4f, 40f, 13f, 12f, 10f, 8f, 12f, 8f, 9f, 13f });
            }

            table.AddCell(paragraphhhj);

            if (Dt.Rows.Count > 0)
            {
                tblcell = new PdfPCell(new Phrase("Transportation Charge", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);
                tblcell = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);
                tblcell = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);
                tblcell = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);
                tblcell = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);

                var TransportationCharges = Convert.ToDouble(Dt.Rows[0]["TransportationCharges"].ToString() == "" ? "0" : Dt.Rows[0]["TransportationCharges"].ToString());
                string TCharges = TransportationCharges.ToString("N2", info);

                var TCGSTPer = Convert.ToDouble(Dt.Rows[0]["TCGSTPer"].ToString() == "" ? "0" : Dt.Rows[0]["TCGSTPer"].ToString());
                string TCGSTPerf = TCGSTPer.ToString("N2", info);

                var TSGSTPer = Convert.ToDouble(Dt.Rows[0]["TSGSTPer"].ToString() == "" ? "0" : Dt.Rows[0]["TSGSTPer"].ToString());
                string TSGSTPerf = TSGSTPer.ToString("N2", info);

                var TIGSTPer = Convert.ToDouble(Dt.Rows[0]["TIGSTPer"].ToString() == "" ? "0" : Dt.Rows[0]["TIGSTPer"].ToString());
                string TIGSTPerf = TSGSTPer.ToString("N2", info);

                if (Dt.Rows[0]["TIGSTPer"].ToString() == "0")
                {
                    tblcell = new PdfPCell(new Phrase(TCharges + "\n\n" + TransportationDescription, FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase(TCGSTPerf, FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase(Dt.Rows[0]["TCGSTAmt"].ToString(), FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase(TSGSTPerf, FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase(Dt.Rows[0]["TSGSTAmt"].ToString(), FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                }
                else
                {
                    tblcell = new PdfPCell(new Phrase(TIGSTPerf, FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                    tblcell = new PdfPCell(new Phrase(Dt.Rows[0]["TIGSTAmt"].ToString(), FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    tblcell.HorizontalAlignment = 1;
                    table.AddCell(tblcell);
                }
                tblcell = new PdfPCell(new Phrase(Dt.Rows[0]["TotalCost"].ToString(), FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                tblcell.HorizontalAlignment = 1;
                table.AddCell(tblcell);

                //table.AddCell(new Phrase("\n        ", FontFactory.GetFont("Arial", 8, Font.BOLD)));
            }


            doc.Add(table);
            //--
            //Add Total Row start
            Paragraph paragraphTable5 = new Paragraph();

            string[] itemsss = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

            Font font13 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font11 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Paragraph paragraphh = new Paragraph("", font12);



            for (int i = 0; i < items.Length; i++)
            {
                paragraph.Add(new Phrase("", font10));
            }

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            paragraph.Alignment = Element.ALIGN_RIGHT;

            table.SetWidths(new float[] { 0f, 115f, 13f });
            table.AddCell(paragraph);
            PdfPCell cell = new PdfPCell(new Phrase("Value of Supply", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell);

            var totalval = Convert.ToDouble(Dt.Rows[0]["TransportationCharges"].ToString());

            var ValueAmountf = Convert.ToDouble(amount);
            var fttl = Convert.ToDecimal(totalval) + Convert.ToDecimal(ValueAmountf);
            string ValueAmount = fttl.ToString("N2", info);

            PdfPCell cell11 = new PdfPCell(new Phrase(ValueAmount, FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell11);
            doc.Add(table);



            //Grand total Row STart
            Paragraph paragraphTable17 = new Paragraph();
            paragraphTable5.SpacingAfter = 0f;

            string[] itemm = { "Goods once sold will not be taken back or exchange. \b",
                        "Interest at the rate of 18% will be charged if bill is'nt paid within 30 days.\b",
                        "Our risk and responsibility ceases the moment goods leaves out godown. \n",
                        };

            Font font16 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font font17 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Paragraph paragraphhhhh = new Paragraph("", font12);

            //paragraphh.SpacingAfter = 10f;

            for (int i = 0; i < items.Length; i++)
            {
                paragraph.Add(new Phrase("", font10));
            }

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            table.SetWidths(new float[] { 0f, 115f, 13f });
            table.AddCell(paragraph);

            double fnlTAX = 0;
            decimal Totalamt = 0;
            if (Dt.Rows[0]["IGSTPer"].ToString() == "0")
            {
                var CGSTPer = Convert.ToDouble(Dt.Rows[0]["CGSTPer"].ToString());
                var fnlCGST = Convert.ToDouble(fttl) * CGSTPer / 100;

                PdfPCell cell444 = new PdfPCell(new Phrase("Add CGST", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                cell444.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell444);
                PdfPCell cell555 = new PdfPCell(new Phrase(fnlCGST.ToString(), FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                cell555.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell555);


                PdfPCell cell4440 = new PdfPCell(new Phrase("Add CGST", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                cell444.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell4440);
                PdfPCell cell5550 = new PdfPCell(new Phrase(fnlCGST.ToString(), FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                cell555.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell5550);

                doc.Add(table);

                double Taxamot = CGST_price + SGST_price;

                Totalamt = Convert.ToDecimal(amount) + Convert.ToDecimal(Taxamot);

                table = new PdfPTable(3);
                table.TotalWidth = 560f;
                table.LockedWidth = true;

                paragraph.Alignment = Element.ALIGN_RIGHT;

                var SGSTPer = Convert.ToDouble(Dt.Rows[0]["SGSTPer"].ToString());
                var fnlSGST = Convert.ToDouble(fttl) * SGSTPer / 100;

                table.SetWidths(new float[] { 0f, 115f, 13f });
                table.AddCell(paragraph);
                PdfPCell cell4444 = new PdfPCell(new Phrase("Add SGST", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                cell4444.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell4444);

                PdfPCell cell5555 = new PdfPCell(new Phrase(fnlSGST.ToString(), FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                cell5555.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell5555);
                doc.Add(table);

                table = new PdfPTable(3);
                table.TotalWidth = 560f;
                table.LockedWidth = true;

                paragraph.Alignment = Element.ALIGN_RIGHT;

                table.SetWidths(new float[] { 0f, 115f, 13f });
                table.AddCell(paragraph);

                fnlTAX = fnlSGST + fnlCGST;
            }
            else
            {
                var IGSTPer = Convert.ToDouble(Dt.Rows[0]["IGSTPer"].ToString());
                var fnlIGST = Convert.ToDouble(fttl) * IGSTPer / 100;

                PdfPCell cell444 = new PdfPCell(new Phrase("Add IGST", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                cell444.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell444);
                PdfPCell cell555 = new PdfPCell(new Phrase(fnlIGST.ToString(), FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                cell555.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell555);

                fnlTAX = fnlIGST;
            }


            PdfPCell cellTaxAmount = new PdfPCell(new Phrase("Tax Amount", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            cellTaxAmount.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellTaxAmount);

            PdfPCell cellTaxAmount1 = new PdfPCell(new Phrase(fnlTAX.ToString(), FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            cellTaxAmount1.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellTaxAmount1);
            doc.Add(table);

            ////Transporation Charges
            //table = new PdfPTable(3);
            //table.TotalWidth = 560f;
            //table.LockedWidth = true;

            //paragraph.Alignment = Element.ALIGN_RIGHT;

            //table.SetWidths(new float[] { 0f, 115f, 13f });
            //table.AddCell(paragraph);
            //PdfPCell cellTCharges = new PdfPCell(new Phrase("Transportation Charges with " + gstper + " %", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            //cellTCharges.HorizontalAlignment = Element.ALIGN_RIGHT;
            //table.AddCell(cellTCharges);

            //PdfPCell cellTCharges1 = new PdfPCell(new Phrase(Transporattioncharges.ToString(), FontFactory.GetFont("Arial", 9, Font.NORMAL)));
            //cellTCharges1.HorizontalAlignment = Element.ALIGN_RIGHT;
            //table.AddCell(cellTCharges1);
            //doc.Add(table);

            ////////////////////////////////


            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            paragraph.Alignment = Element.ALIGN_RIGHT;

            table.SetWidths(new float[] { 0f, 115f, 13f });
            table.AddCell(paragraph);

            //var fnlTOTAL = fnlTAX + fttl;

            PdfPCell cell44 = new PdfPCell(new Phrase("Total Amount", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell44);

            var Totalamtf = Convert.ToDouble(fnlTAX) + Convert.ToDouble(fttl.ToString());
            var Totalamtfff = Math.Round(Totalamtf);
            string FinaleTotalamt = Totalamtfff.ToString("N2", info);

            PdfPCell cell440 = new PdfPCell(new Phrase(FinaleTotalamt, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            cell440.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell440);
            doc.Add(table);

            string Amtinword = ConvertNumbertoWords(Convert.ToInt32(Totalamtf));

            //Total amount In word
            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            paragraph.Alignment = Element.ALIGN_RIGHT;

            table.SetWidths(new float[] { 0f, 118f, 0f });
            table.AddCell(paragraph);

            PdfPCell cell4434 = new PdfPCell(new Phrase("Total Amount: " + Amtinword + " Only", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            cell4434.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell4434);

            PdfPCell cell44044 = new PdfPCell(new Phrase(Totalamt.ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
            cell44044.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell44044);
            doc.Add(table);
            //
            Font font14 = FontFactory.GetFont("Arial", 11);
            Font font15 = FontFactory.GetFont("Arial", 8, Font.NORMAL);

            string[] itemsstm = {
                "DELIVERY                                    :    "+Delivery +" \n",
                "TEST CERTIFICATE                    :    "+TestCertificate+" \n",
                "II                                                    :    "+II+" \n",
                        };

            Paragraph paragraphTable99 = new Paragraph(" Terms & Condition :\n\n", font15);

            for (int i = 0; i < itemsstm.Length; i++)
            {
                paragraphTable99.Add(new Phrase("\u2022 \u00a0" + itemsstm[i] + "\n", font15));
            }

            table = new PdfPTable(1);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.DefaultCell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
            table.SetWidths(new float[] { 560f });
            table.AddCell(paragraphTable99);
            doc.Add(table);

            //Puja Enterprises Sign
            string[] itemss = {
                //"REMARKS                                   :    "+Remarks+"\n",
                "PAYMENT TERM                         :    "+PaymentTerm+"\n",
                "PACKAING & FORWARDING     :    "+PackingAndForwarding+" \n",
                "TRANSPORTATION                    :    "+Transportation+" \n",
                "VARIATION                                  :    "+Variation+" \n",
                //"DELIVERY                                    :    "+Delivery +" \n",
               // "TEST CERTIFICATE                    :    "+TestCertificate+" \n",
                "WEEKLY OFF                              :    "+WeeklyOff+"\n",
                "TIME                                             :    "+Time+" \n",
                //"II                                                    :    "+II+" \n",
                        };


            Paragraph paragraphhh = new Paragraph("", font10);


            for (int i = 0; i < itemss.Length; i++)
            {
                paragraphhh.Add(new Phrase("\u2022 \u00a0" + itemss[i] + "\n", font15));
            }

            table = new PdfPTable(1);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 560f });
            table.DefaultCell.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
            table.AddCell(paragraphhh);
            //table.AddCell(new Phrase("Puja Enterprises \n\n\n\n         Sign", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //doc.Add(table);

            Paragraph paragraphTable10000 = new Paragraph();

            //Puja Enterprises Sign
            string[] itemss4 = {
                "Payment Term     ",

                        };

            Font font144 = FontFactory.GetFont("Arial", 11);
            Font font155 = FontFactory.GetFont("Arial", 8);
            Paragraph paragraphhhhhff = new Paragraph();


            //for (int i = 0; i < itemss4.Length; i++)
            //{
            //    paragraphhhhhff.Add(new Phrase("\u2022 \u00a0" + itemss4[i] + "\n", font155));
            //}

            table = new PdfPTable(2);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.DefaultCell.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
            table.SetWidths(new float[] { 300f, 100f });

            // Bind stamp Image
            //string imageStamp = Server.MapPath("~") + "/Content/img/Account.png";
            //iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(imageStamp);
            //image1.ScaleToFit(600, 120);
            //PdfPCell imageCell = new PdfPCell(image1);
            //imageCell.PaddingLeft = 10f;
            //imageCell.PaddingTop = 0f;
            /////////////////


            //table.AddCell(paragraphhhhhff);
            table.AddCell(paragraphhh);
            //table.AddCell(imageCell);
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            doc.Add(table);
            doc.Close();


            Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/PDF_Files/") + "PurchaseOrder.pdf");

            Font blackFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfReader reader = new PdfReader(FileBuffer);
                using (PdfStamper stamper = new PdfStamper(reader, stream))
                {
                    int pages = reader.NumberOfPages;
                    for (int i = 1; i <= pages; i++)
                    {
                        if (i == 1)
                        {

                        }
                        else
                        {
                            var pdfbyte = stamper.GetOverContent(i);
                            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageURL);
                            image.ScaleToFit(70, 100);
                            image.SetAbsolutePosition(40, 792);
                            image.SpacingBefore = 50f;
                            image.SpacingAfter = 1f;
                            image.Alignment = Element.ALIGN_LEFT;
                            pdfbyte.AddImage(image);
                        }
                        var PageName = "Page No. " + i.ToString();
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase(PageName, blackFont), 568f, 820f, 0);
                    }
                }
                FileBuffer = stream.ToArray();
            }


            //string empFilename = QuatationNumber + " " + PartyName + ".pdf";

            //if (FileBuffer != null)
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
            //    Response.BinaryWrite(FileBuffer);
            //    Response.AddHeader("Content-Disposition", "attachment;filename=" + empFilename);
            //}
            ifrRight6.Attributes["src"] = @"../PDF_Files/" + "PurchaseOrder.pdf";
        }
        doc.Close();
        //Session["PDFID"] = null;
    }


    public string ConvertNumbertoWords(int numbers)
    {
        Boolean paisaconversion = false;
        var pointindex = numbers.ToString().IndexOf(".");
        var paisaamt = 0;
        if (pointindex > 0)
            paisaamt = Convert.ToInt32(numbers.ToString().Substring(pointindex + 1, 2));

        int number = Convert.ToInt32(numbers);

        if (number == 0) return "Zero";
        if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";
        int[] num = new int[4];
        int first = 0;
        int u, h, t;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (number < 0)
        {
            sb.Append("Minus ");
            number = -number;
        }
        string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
        string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
        string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
        string[] words3 = { "Thousand ", "Lakh ", "Crore " };
        num[0] = number % 1000; // units
        num[1] = number / 1000;
        num[2] = number / 100000;
        num[1] = num[1] - 100 * num[2]; // thousands
        num[3] = number / 10000000; // crores
        num[2] = num[2] - 100 * num[3]; // lakhs
        for (int i = 3; i > 0; i--)
        {
            if (num[i] != 0)
            {
                first = i;
                break;
            }
        }
        for (int i = first; i >= 0; i--)
        {
            if (num[i] == 0) continue;
            u = num[i] % 10; // ones
            t = num[i] / 10;
            h = num[i] / 100; // hundreds
            t = t - 10 * h; // tens
            if (h > 0) sb.Append(words0[h] + "Hundred ");
            if (u > 0 || t > 0)
            {
                if (h > 0 || i == 0) sb.Append("and ");
                if (t == 0)
                    sb.Append(words0[u]);
                else if (t == 1)
                    sb.Append(words1[u]);
                else
                    sb.Append(words2[t - 2] + words0[u]);
            }
            if (i != 0) sb.Append(words3[i - 1]);
        }

        //string query1 = string.Empty;
        //query1 = @"select * from tbl_VendorMaster where VendorName='" + lblcompany.Text + "' ";
        //SqlDataAdapter ad = new SqlDataAdapter(query1, con);
        //DataTable dtt = new DataTable();
        //ad.Fill(dtt);
        //if (dtt.Rows.Count>0)
        //{
        //    string Currency = dtt.Rows[0]["Currency"].ToString();
        //    if (Currency == "INR")
        //    {
        //        if (paisaamt == 0 && paisaconversion == false)
        //        {
        //            sb.Append("Rupees ");
        //        }
        //        else if (paisaamt > 0)
        //        {
        //            var paisatext = ConvertNumbertoWords(paisaamt);
        //            sb.AppendFormat("Rupees {0} paise", paisatext);
        //        }
        //    }
        //    else if (Currency == "USD")
        //    {
        //        if (paisaamt == 0 && paisaconversion == false)
        //        {
        //            sb.Append("USD ");
        //        }
        //        else if (paisaamt > 0)
        //        {
        //            var paisatext = ConvertNumbertoWords(paisaamt);
        //            sb.AppendFormat(" {0} USD", paisatext);
        //        }
        //    }
        //if (paisaamt == 0 && paisaconversion == false)
        //{
        //    sb.Append(Currency);
        //}
        //else if (paisaamt > 0)
        //{
        //    var paisatext = ConvertNumbertoWords(paisaamt);
        //    sb.AppendFormat("'"+ Currency + "' {0} paise", paisatext);
        //}
        // }


        return sb.ToString().TrimEnd();
    }

}