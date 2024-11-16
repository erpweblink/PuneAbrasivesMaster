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
using System.Net.Mail;
using System.Net.Mime;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Globalization;

public partial class Purchase_PurchaseOrder : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
    DataTable dt = new DataTable();
    public static string sName = "";
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
                //fillddlpaymentterm();
                fillddlUnit();
                FillddlComponent();
                //BindParticular();
                //fillddlFooter();
                //txtPodate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                UpdateHistorymsg = string.Empty; regdate = string.Empty;
                if (Request.QueryString["ID"] != null)
                {
                    ViewState["RowNo"] = 0;
                    dt.Columns.AddRange(new DataColumn[16] { new DataColumn("id"),
                 new DataColumn("Particulars"), new DataColumn("HSN")
                , new DataColumn("Qty"), new DataColumn("Rate"),new DataColumn("Discount"), new DataColumn("Amount"),
                    new DataColumn("CGSTPer"),new DataColumn("CGSTAmt"),new DataColumn("SGSTPer")
                    ,new DataColumn("SGSTAmt"),new DataColumn("IGSTPer"),new DataColumn("IGSTAmt"),new DataColumn("TotalAmount"),new DataColumn("Description"),new DataColumn("UOM")
            });

                    ViewState["ParticularDetails"] = dt;

                    ViewState["UpdateRowId"] = Decrypt(Request.QueryString["ID"].ToString());
                    GetPurchaseOrderData(ViewState["UpdateRowId"].ToString());

                    sName = txtSupplierName.Text;
                }
                else
                {
                    ViewState["RowNo"] = 0;
                    dt.Columns.AddRange(new DataColumn[16] { new DataColumn("id"),
                 new DataColumn("Particulars"), new DataColumn("HSN")
                , new DataColumn("Qty"), new DataColumn("Rate"),new DataColumn("Discount"), new DataColumn("Amount"),
                    new DataColumn("CGSTPer"),new DataColumn("CGSTAmt"),new DataColumn("SGSTPer")
                    ,new DataColumn("SGSTAmt"),new DataColumn("IGSTPer"),new DataColumn("IGSTAmt"),new DataColumn("TotalAmount"),new DataColumn("Description"),new DataColumn("UOM")
            });
                    ViewState["ParticularDetails"] = dt;
                    //txtPONo.Text = GenerateComCode();
                    txtPONo.Text = Code();
                }
            }
        }
    }

    //private void fillddlpaymentterm()
    //{
    //    SqlDataAdapter adpt = new SqlDataAdapter("select distinct paymentterm from tbl_QuotationMainFooter", con);
    //    DataTable dtpt = new DataTable();
    //    adpt.Fill(dtpt);

    //    if (dtpt.Rows.Count > 0)
    //    {
    //        dtpt.Rows.Add("Specify");
    //        ddlPaymentTerm.DataSource = dtpt;
    //        ddlPaymentTerm.DataValueField = "paymentterm";
    //        ddlPaymentTerm.DataTextField = "paymentterm";
    //        ddlPaymentTerm.DataBind();
    //    }
    //    ddlPaymentTerm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Nill", "0"));
    //}
    private void fillddlUnit()
    {
        //SqlDataAdapter adpt = new SqlDataAdapter("select distinct Unit from tblUnit", con);
        //DataTable dtpt = new DataTable();
        //adpt.Fill(dtpt);

        //if (dtpt.Rows.Count > 0)
        //{
        //    txtUOM.DataSource = dtpt;
        //    txtUOM.DataValueField = "Unit";
        //    txtUOM.DataTextField = "Unit";
        //    txtUOM.DataBind();
        //}
        //txtUOM.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Nill", "0"));
    }
    //private void fillddlFooter()
    //{
    //    SqlDataAdapter adpt = new SqlDataAdapter("select PackingAndForwarding,Transportation,Variation,Delivery,TestCertificate,WeeklyOff,Time,II from tblPOFooter", con);
    //    DataTable dtpt = new DataTable();
    //    adpt.Fill(dtpt);

    //    if (dtpt.Rows.Count > 0)
    //    {
    //        ddlPackingAndForwarding.DataSource = dtpt;
    //        ddlPackingAndForwarding.DataValueField = "PackingAndForwarding";
    //        ddlPackingAndForwarding.DataTextField = "PackingAndForwarding";
    //        ddlPackingAndForwarding.DataBind();

    //        ddlTransportation.DataSource = dtpt;
    //        ddlTransportation.DataValueField = "Transportation";
    //        ddlTransportation.DataTextField = "Transportation";
    //        ddlTransportation.DataBind();

    //        ddlVariation.DataSource = dtpt;
    //        ddlVariation.DataValueField = "Variation";
    //        ddlVariation.DataTextField = "Variation";
    //        ddlVariation.DataBind();

    //        ddlDelivery.DataSource = dtpt;
    //        ddlDelivery.DataValueField = "Delivery";
    //        ddlDelivery.DataTextField = "Delivery";
    //        ddlDelivery.DataBind();

    //        ddlTestCertificate.DataSource = dtpt;
    //        ddlTestCertificate.DataValueField = "TestCertificate";
    //        ddlTestCertificate.DataTextField = "TestCertificate";
    //        ddlTestCertificate.DataBind();

    //        ddlWeeklyOff.DataSource = dtpt;
    //        ddlWeeklyOff.DataValueField = "WeeklyOff";
    //        ddlWeeklyOff.DataTextField = "WeeklyOff";
    //        ddlWeeklyOff.DataBind();

    //        ddlTime.DataSource = dtpt;
    //        ddlTime.DataValueField = "Time";
    //        ddlTime.DataTextField = "Time";
    //        ddlTime.DataBind();

    //        ddlII.DataSource = dtpt;
    //        ddlII.DataValueField = "II";
    //        ddlII.DataTextField = "II";
    //        ddlII.DataBind();
    //    }
    //    ddlPackingAndForwarding.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Nill", "0"));
    //    ddlTransportation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Nill", "0"));
    //    ddlVariation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Nill", "0"));
    //    ddlDelivery.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Nill", "0"));
    //    ddlTestCertificate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Nill", "0"));
    //    ddlWeeklyOff.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Nill", "0"));
    //    ddlTime.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Nill", "0"));
    //    ddlII.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Nill", "0"));
    //}

    static string regdate = string.Empty;
    protected void GetPurchaseOrderData(string id)
    {
        string query1 = string.Empty;
        query1 = @"select * from tblPurchaseOrderHdr where Id='" + id + "' ";
        SqlDataAdapter ad = new SqlDataAdapter(query1, con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            txtSupplierName.Text = dt.Rows[0]["SupplierName"].ToString();

            BindKindAtt();
            BindEmailId();

            ddlKindAtt.Text = dt.Rows[0]["KindAtt"].ToString();
            txtPONo.Text = dt.Rows[0]["PONo"].ToString();
            txtPodate.Text = dt.Rows[0]["PODate"].ToString();
            ddlMode.Text = dt.Rows[0]["Mode"].ToString();
            txtDeliverydate.Text = dt.Rows[0]["DeliveryDate"].ToString();
            txtReferQuotation.Text = dt.Rows[0]["ReferQuotation"].ToString();
            txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
            ddlOrderCloseMode.Text = dt.Rows[0]["OrderCloseMode"].ToString();
            hdnfileData.Value = dt.Rows[0]["RefDocuments"].ToString();
            //if (dt.Rows[0]["RefDocuments"].ToString() != "")
            //{
            //    spnFileUploadData.InnerText = "File Already Exsist, if you can update then update it.";
            //}
            //else
            //{
            //    spnFileUploadData.InnerText = "File Not Found";
            //}

            //ddlPaymentTerm.Text = dt.Rows[0]["PaymentTerm"].ToString();
            //ddlPackingAndForwarding.Text = dt.Rows[0]["PackingAndForwarding"].ToString();
            //ddlTransportation.Text = dt.Rows[0]["Transportation"].ToString();
            //ddlVariation.Text = dt.Rows[0]["Variation"].ToString();
            //ddlDelivery.Text = dt.Rows[0]["Delivery"].ToString();
            //ddlTestCertificate.Text = dt.Rows[0]["TestCertificate"].ToString();
            //ddlWeeklyOff.Text = dt.Rows[0]["WeeklyOff"].ToString();
            //ddlTime.Text = dt.Rows[0]["Time"].ToString();
            //ddlII.Text = dt.Rows[0]["II"].ToString();

            txtTCharge.Text = dt.Rows[0]["TransportationCharges"].ToString();
            txtTDescription.Text = dt.Rows[0]["TransportationDescription"].ToString();
            txtTCGSTPer.Text = dt.Rows[0]["TCGSTPer"].ToString();
            txtTCGSTamt.Text = dt.Rows[0]["TCGSTAmt"].ToString();
            txtTSGSTPer.Text = dt.Rows[0]["TSGSTPer"].ToString();
            txtTSGSTamt.Text = dt.Rows[0]["TSGSTAmt"].ToString();
            txtTIGSTPer.Text = dt.Rows[0]["TIGSTPer"].ToString();
            txtTIGSTamt.Text = dt.Rows[0]["TIGSTAmt"].ToString();
            txtTCost.Text = dt.Rows[0]["TotalCost"].ToString();
            getParticularsdts(id);

            btnadd.Text = "Update PO";
        }
    }

    protected void getParticularsdts(string id)
    {

        DataTable Dtproduct = new DataTable();
        SqlDataAdapter daa = new SqlDataAdapter("select * from tblPurchaseOrderDtls where HeaderID='" + id + "'", con);
        daa.Fill(Dtproduct);
        ViewState["RowNo"] = (int)ViewState["RowNo"] + 1;

        DataTable dt = ViewState["ParticularDetails"] as DataTable;

        if (Dtproduct.Rows.Count > 0)
        {
            for (int i = 0; i < Dtproduct.Rows.Count; i++)
            {
                dt.Rows.Add(ViewState["RowNo"], Dtproduct.Rows[i]["Particulars"].ToString(), Dtproduct.Rows[i]["HSN"].ToString(), Dtproduct.Rows[i]["Qty"].ToString(),
                    Dtproduct.Rows[i]["Rate"].ToString(), Dtproduct.Rows[i]["Discount"].ToString(), Dtproduct.Rows[i]["Amount"].ToString(), Dtproduct.Rows[i]["CGSTPer"].ToString(), Dtproduct.Rows[i]["CGSTAmt"].ToString(),
                    Dtproduct.Rows[i]["SGSTPer"].ToString(), Dtproduct.Rows[i]["SGSTAmt"].ToString(), Dtproduct.Rows[i]["IGSTPer"].ToString(), Dtproduct.Rows[i]["IGSTAmt"].ToString(),
                    Dtproduct.Rows[i]["GrandTotal"].ToString(), Dtproduct.Rows[i]["Description"].ToString(), Dtproduct.Rows[i]["UOM"].ToString());
                ViewState["ParticularDetails"] = dt;
            }
        }
        dgvParticularsDetails.DataSource = dt;
        dgvParticularsDetails.DataBind();
    }

    static string UpdateHistorymsg = string.Empty;

    public string Decrypt(string cipherText)
    {
        string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    protected string GenerateComCode()
    {

        string invoiceno;
        DateTime date = DateTime.Now;
        string currentyeaar = date.ToString();

        string FinYear = null;

        if (DateTime.Today.Month > 3)
        {
            //FinYear = DateTime.Today.Year.ToString();
            FinYear = DateTime.Today.AddYears(1).ToString("yy");
        }
        else
        {
            var finYear = DateTime.Today.AddYears(1).ToString("yy");
            FinYear = (Convert.ToInt32(finYear) - 1).ToString();
        }
        string previousyear = (Convert.ToDecimal(FinYear) - 1).ToString();

        SqlDataAdapter ad = new SqlDataAdapter("SELECT max([Id]) as maxid FROM [tblPurchaseOrderHdr]", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {

            int maxid = dt.Rows[0]["maxid"].ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[0]["maxid"].ToString());
            invoiceno = previousyear.ToString() + "-" + FinYear + "/" + (maxid + 1).ToString();
            if (maxid < 9)
            {
                invoiceno = previousyear.ToString() + "-" + FinYear + "/" + "000" + (maxid + 1).ToString();
            }
            else if (maxid <= 100)
            {
                invoiceno = previousyear.ToString() + "-" + FinYear + "/" + "00" + (maxid + 1).ToString();
            }
        }
        else
        {
            invoiceno = string.Empty;
        }
        return invoiceno;
    }

    protected string Code()
    {
        string FinYear = null;
        string FinFullYear = null;
        if (DateTime.Today.Month > 3)
        {
            FinYear = DateTime.Today.AddYears(1).ToString("yy");
            FinFullYear = DateTime.Today.AddYears(1).ToString("yyyy");
        }
        else
        {
            var finYear = DateTime.Today.AddYears(1).ToString("yy");
            FinYear = (Convert.ToInt32(finYear) - 1).ToString();

            var finfYear = DateTime.Today.AddYears(1).ToString("yyyy");
            FinFullYear = (Convert.ToInt32(finfYear) - 1).ToString();
        }
        string previousyear = (Convert.ToDecimal(FinFullYear) - 1).ToString();
        string strInvoiceNumber = "";
        string fY = previousyear.ToString() + "-" + FinYear;
        string strSelect = @"select ISNULL(MAX(PONo), '') AS maxno from tblPurchaseOrderHdr where PONo like '%" + fY + "%'";
       // string strSelect = @"SELECT TOP 1 ISNULL(MAX(PONo), '') AS maxno FROM tblPurchaseOrderHdr where PONo like '%" + fY + "%' ORDER BY ID DESC";
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strSelect;
        con.Open();
        string result = cmd.ExecuteScalar().ToString();
        // string result = "";
        con.Close();
        if (result != "")
        {
            int numbervalue = Convert.ToInt32(result.Substring(result.IndexOf("/") + 1, result.Length - (result.IndexOf("/") + 1)));
            numbervalue = numbervalue + 1;
            strInvoiceNumber = result.Substring(0, result.IndexOf("/") + 1) + "" + numbervalue.ToString("00");
        }
        else
        {
            strInvoiceNumber = previousyear.ToString() + "-" + FinYear + "/" + "01";
        }
        return strInvoiceNumber;
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        #region Insert
        if (btnadd.Text == "Add PO")
        {
            string PONo = Code();
            if (!string.IsNullOrEmpty(PONo))
            {
                //byte[] bytes = null;
                //if (UploadRefDocs.HasFile)
                //{
                //    string filename = Path.GetFileName(UploadRefDocs.PostedFile.FileName);
                //    string contentType = UploadRefDocs.PostedFile.ContentType;
                //    using (Stream fs = UploadRefDocs.PostedFile.InputStream)
                //    {
                //        using (BinaryReader br = new BinaryReader(fs))
                //        {
                //            bytes = br.ReadBytes((Int32)fs.Length);
                //        }
                //    }
                //}
                if (dgvParticularsDetails.Rows.Count > 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_PurchaseOrder", con);
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", "insert");
                    cmd.Parameters.AddWithValue("@SupplierName", txtSupplierName.Text.Trim());
                    cmd.Parameters.AddWithValue("@PONo", PONo);
                    DateTime PODate = Convert.ToDateTime(txtPodate.Text.ToString(), System.Globalization.CultureInfo.GetCultureInfo("ur-PK").DateTimeFormat);

                    txtPodate.Text = PODate.ToString("yyyy-MM-dd");
                    cmd.Parameters.AddWithValue("@PODate", txtPodate.Text);
                    cmd.Parameters.AddWithValue("@Mode", ddlMode.Text.Trim());
                    DateTime DeliveryDate = Convert.ToDateTime(txtDeliverydate.Text.ToString(), System.Globalization.CultureInfo.GetCultureInfo("ur-PK").DateTimeFormat);
                    txtDeliverydate.Text = DeliveryDate.ToString("yyyy-MM-dd");
                    cmd.Parameters.AddWithValue("@DeliveryDate", txtDeliverydate.Text.Trim());
                    cmd.Parameters.AddWithValue("@ReferQuotation", txtReferQuotation.Text.Trim());
                    cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());
                    cmd.Parameters.AddWithValue("@OrderCloseMode", ddlOrderCloseMode.Text.Trim());
                    cmd.Parameters.AddWithValue("@KindAtt", ddlKindAtt.Text.Trim());
                    //cmd.Parameters.AddWithValue("@RefDocuments", bytes);
                    //cmd.Parameters.AddWithValue("@PaymentTerm", ddlPaymentTerm.Text.Trim());
                    //cmd.Parameters.AddWithValue("@PackingAndForwarding", ddlPackingAndForwarding.Text.Trim());
                    //cmd.Parameters.AddWithValue("@Transportation", ddlTransportation.Text.Trim());
                    //cmd.Parameters.AddWithValue("@Variation", ddlVariation.Text.Trim());
                    //cmd.Parameters.AddWithValue("@Delivery", ddlDelivery.Text.Trim());
                    //cmd.Parameters.AddWithValue("@TestCertificate", ddlTestCertificate.Text.Trim());
                    //cmd.Parameters.AddWithValue("@WeeklyOff", ddlWeeklyOff.Text.Trim());
                    //cmd.Parameters.AddWithValue("@Time", ddlTime.Text.Trim());
                    //cmd.Parameters.AddWithValue("@II", ddlII.Text.Trim());
                    cmd.Parameters.AddWithValue("@GrandTotal", hdnGrandtotal.Value);
                    cmd.Parameters.AddWithValue("@CreatedBy", Session["Username"].ToString());

                    //17 March 2022
                    cmd.Parameters.AddWithValue("@TransportationCharges", txtTCharge.Text.Trim());
                    cmd.Parameters.AddWithValue("@TransportationDescription", txtTDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@TCGSTPer", txtTCGSTPer.Text.Trim());
                    cmd.Parameters.AddWithValue("@TCGSTAmt", txtTCGSTamt.Text.Trim());
                    cmd.Parameters.AddWithValue("@TSGSTPer", txtTSGSTPer.Text.Trim());
                    cmd.Parameters.AddWithValue("@TSGSTAmt", txtTSGSTamt.Text.Trim());
                    cmd.Parameters.AddWithValue("@TIGSTPer", txtTIGSTPer.Text.Trim());
                    cmd.Parameters.AddWithValue("@TIGSTAmt", txtTIGSTamt.Text.Trim());
                    cmd.Parameters.AddWithValue("@TotalCost", txtTCost.Text.Trim());
                    int a = 0;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    SqlCommand cmdmax = new SqlCommand("select MAX(Id) as MAxID from tblPurchaseOrderHdr", con);
                    con.Open();
                    Object mx = cmdmax.ExecuteScalar();
                    con.Close();
                    int MaxId = Convert.ToInt32(mx.ToString());


                    foreach (GridViewRow row in dgvParticularsDetails.Rows)
                    {
                        string Particulars = ((Label)row.FindControl("lblParticulars")).Text;
                        string HSN = ((Label)row.FindControl("lblHSN")).Text;
                        string Qty = ((Label)row.FindControl("lblQty")).Text;
                        string Rate = ((Label)row.FindControl("lblRate")).Text;
                        string Discount = ((Label)row.FindControl("lblDiscount")).Text;
                        string Amount = ((Label)row.FindControl("lblAmount")).Text;
                        string CGSTPer = ((Label)row.FindControl("lblCGSTPer")).Text;
                        string CGSTAmt = ((Label)row.FindControl("lblCGSTAmt")).Text;
                        string SGSTPer = ((Label)row.FindControl("lblSGSTPer")).Text;
                        string SGSTAmt = ((Label)row.FindControl("lblSGSTAmt")).Text;
                        string IGSTPer = ((Label)row.FindControl("lblIGSTPer")).Text;
                        string IGSTAmt = ((Label)row.FindControl("lblIGSTAmt")).Text;
                        string TotalAmount = ((Label)row.FindControl("lblTotalAmount")).Text;
                        string Description = ((Label)row.FindControl("lblDescription")).Text;
                        string UOM = ((Label)row.FindControl("lblUOM")).Text;

                        SqlCommand cmdParticulardata = new SqlCommand(@"INSERT INTO tblPurchaseOrderDtls([HeaderID],[Particulars],[HSN],[Qty],[Rate],[Amount],[CGSTPer],[CGSTAmt],[SGSTPer],[SGSTAmt],[IGSTPer],[IGSTAmt],[GrandTotal],[Discount],[Description],[UOM]) 
                        VALUES(" + MaxId + ",'" + Particulars + "','" + HSN + "','" + Qty + "'," +
                         "'" + Rate + "','" + Amount + "','" + CGSTPer + "','" + CGSTAmt + "'," +
                         "'" + SGSTPer + "','" + SGSTAmt + "','" + IGSTPer + "','" + IGSTAmt + "','" + TotalAmount + "','" + Discount + "','" + Description + "','" + UOM + "')", con);
                        con.Open();
                        cmdParticulardata.ExecuteNonQuery();
                        con.Close();
                    }


                    if (IsSedndMail.Checked == true)
                    {
                        string subject = "PO from Excel Enclosures";
                        Send_Mail(MaxId, subject);
                    }

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data Saved Sucessfully');window.location.href='PurchaseOrderList.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please Add Particulars Details !!');", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('PO no Generation Problem Please Try Again !!');", true);
            }
        }
        #endregion Insert

        #region Update
        if (btnadd.Text == "Update PO")
        {
            //byte[] bytes = null;
            //if (hdnfileData.Value == "")
            //{
            //    string filename = Path.GetFileName(UploadRefDocs.PostedFile.FileName);
            //    string contentType = UploadRefDocs.PostedFile.ContentType;
            //    using (Stream fs = UploadRefDocs.PostedFile.InputStream)
            //    {
            //        using (BinaryReader br = new BinaryReader(fs))
            //        {
            //            bytes = br.ReadBytes((Int32)fs.Length);
            //        }
            //    }
            //}
            //else
            //{
            //    if (UploadRefDocs.HasFile)
            //    {
            //        string filename = Path.GetFileName(UploadRefDocs.PostedFile.FileName);
            //        string contentType = UploadRefDocs.PostedFile.ContentType;
            //        using (Stream fs = UploadRefDocs.PostedFile.InputStream)
            //        {
            //            using (BinaryReader br = new BinaryReader(fs))
            //            {
            //                bytes = br.ReadBytes((Int32)fs.Length);
            //            }
            //        }
            //    }
            //}

            if (dgvParticularsDetails.Rows.Count > 0)
            {
                SqlCommand cmd = new SqlCommand("SP_PurchaseOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "update");
                cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(ViewState["UpdateRowId"].ToString()));
                cmd.Parameters.AddWithValue("@SupplierName", txtSupplierName.Text.Trim());
                cmd.Parameters.AddWithValue("@PONo", txtPONo.Text);
                DateTime PODate = Convert.ToDateTime(txtPodate.Text.ToString(), System.Globalization.CultureInfo.GetCultureInfo("ur-PK").DateTimeFormat);

                txtPodate.Text = PODate.ToString("yyyy-MM-dd");
                cmd.Parameters.AddWithValue("@PODate", txtPodate.Text);
                cmd.Parameters.AddWithValue("@Mode", ddlMode.Text.Trim());
                DateTime DeliveryDate = Convert.ToDateTime(txtDeliverydate.Text.ToString(), System.Globalization.CultureInfo.GetCultureInfo("ur-PK").DateTimeFormat);
                txtDeliverydate.Text = DeliveryDate.ToString("yyyy-MM-dd");
                cmd.Parameters.AddWithValue("@DeliveryDate", txtDeliverydate.Text.Trim());
                cmd.Parameters.AddWithValue("@ReferQuotation", txtReferQuotation.Text.Trim());
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());
                cmd.Parameters.AddWithValue("@OrderCloseMode", ddlOrderCloseMode.Text.Trim());
                cmd.Parameters.AddWithValue("@KindAtt", ddlKindAtt.Text.Trim());
                //if (hdnfileData.Value == "")
                //{

                //    cmd.Parameters.AddWithValue("@RefDocuments", bytes);

                //}
                //else
                //{
                //    if (UploadRefDocs.HasFile)
                //    {
                //        cmd.Parameters.AddWithValue("@RefDocuments", hdnfileData.Value);
                //    }
                //    else
                //    {
                //        cmd.Parameters.AddWithValue("@RefDocuments", bytes);
                //    }
                //}
                //cmd.Parameters.AddWithValue("@PaymentTerm", ddlPaymentTerm.Text.Trim());
                //cmd.Parameters.AddWithValue("@PackingAndForwarding", ddlPackingAndForwarding.Text.Trim());
                //cmd.Parameters.AddWithValue("@Transportation", ddlTransportation.Text.Trim());
                //cmd.Parameters.AddWithValue("@Variation", ddlVariation.Text.Trim());
                //cmd.Parameters.AddWithValue("@Delivery", ddlDelivery.Text.Trim());
                //cmd.Parameters.AddWithValue("@TestCertificate", ddlTestCertificate.Text.Trim());
                //cmd.Parameters.AddWithValue("@WeeklyOff", ddlWeeklyOff.Text.Trim());
                //cmd.Parameters.AddWithValue("@Time", ddlTime.Text.Trim());
                //cmd.Parameters.AddWithValue("@II", ddlII.Text.Trim());
                cmd.Parameters.AddWithValue("@GrandTotal", hdnGrandtotal.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", Session["Username"].ToString());

                //17 March 2022
                cmd.Parameters.AddWithValue("@TransportationCharges", txtTCharge.Text.Trim());
                cmd.Parameters.AddWithValue("@TransportationDescription", txtTDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@TCGSTPer", txtTCGSTPer.Text.Trim());
                cmd.Parameters.AddWithValue("@TCGSTAmt", txtTCGSTamt.Text.Trim());
                cmd.Parameters.AddWithValue("@TSGSTPer", txtTSGSTPer.Text.Trim());
                cmd.Parameters.AddWithValue("@TSGSTAmt", txtTSGSTamt.Text.Trim());
                cmd.Parameters.AddWithValue("@TIGSTPer", txtTIGSTPer.Text.Trim());
                cmd.Parameters.AddWithValue("@TIGSTAmt", txtTIGSTamt.Text.Trim());
                cmd.Parameters.AddWithValue("@TotalCost", txtTCost.Text.Trim());
                int a = 0;
                cmd.Connection.Open();
                a = cmd.ExecuteNonQuery();
                cmd.Connection.Close();


                SqlCommand cmddelete = new SqlCommand("delete from tblPurchaseOrderDtls where HeaderID='" + Convert.ToInt32(ViewState["UpdateRowId"].ToString()) + "'", con);
                con.Open();
                cmddelete.ExecuteNonQuery();
                con.Close();



                foreach (GridViewRow row in dgvParticularsDetails.Rows)
                {
                    string Particulars = ((Label)row.FindControl("lblParticulars")).Text;
                    string HSN = ((Label)row.FindControl("lblHSN")).Text;
                    string Qty = ((Label)row.FindControl("lblQty")).Text;
                    string Rate = ((Label)row.FindControl("lblRate")).Text;
                    string Discount = ((Label)row.FindControl("lblDiscount")).Text;
                    string Amount = ((Label)row.FindControl("lblAmount")).Text;
                    string CGSTPer = ((Label)row.FindControl("lblCGSTPer")).Text;
                    string CGSTAmt = ((Label)row.FindControl("lblCGSTAmt")).Text;
                    string SGSTPer = ((Label)row.FindControl("lblSGSTPer")).Text;
                    string SGSTAmt = ((Label)row.FindControl("lblSGSTAmt")).Text;
                    string IGSTPer = ((Label)row.FindControl("lblIGSTPer")).Text;
                    string IGSTAmt = ((Label)row.FindControl("lblIGSTAmt")).Text;
                    string TotalAmount = ((Label)row.FindControl("lblTotalAmount")).Text;
                    string Description = ((Label)row.FindControl("lblDescription")).Text;
                    string UOM = ((Label)row.FindControl("lblUOM")).Text;

                    SqlCommand cmdParticulardata = new SqlCommand(@"INSERT INTO tblPurchaseOrderDtls([HeaderID],[Particulars],[HSN],[Qty],[Rate],[Amount],[CGSTPer],[CGSTAmt],[SGSTPer],[SGSTAmt],[IGSTPer],[IGSTAmt],[GrandTotal],[Discount],[Description],[UOM]) 
                        VALUES(" + ViewState["UpdateRowId"].ToString() + ",'" + Particulars + "','" + HSN + "','" + Qty + "'," +
                     "'" + Rate + "','" + Amount + "','" + CGSTPer + "','" + CGSTAmt + "'," +
                     "'" + SGSTPer + "','" + SGSTAmt + "','" + IGSTPer + "','" + IGSTAmt + "','" + TotalAmount + "','" + Discount + "','" + Description + "','" + UOM + "')", con);
                    con.Open();
                    cmdParticulardata.ExecuteNonQuery();
                    con.Close();
                }


                if (IsSedndMail.Checked == true)
                {
                    int idd = Convert.ToInt32(ViewState["UpdateRowId"].ToString());
                    string subject = "Updated PO from Pune Abrasieves Pvt. Ltd.";
                    Send_Mail(idd, subject);
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data Updated Sucessfully');window.location.href='PurchaseOrderList.aspx';", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please Add Particulars Details !!');", true);
            }

        }
        #endregion Update
    }

    protected void btnreset_Click(object sender, EventArgs e)
    {
        Response.Redirect("PurchaseOrder.aspx");
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetSupplierList(string prefixText, int count)
    {
        return AutoFillSupplierName(prefixText);
    }

    public static List<string> AutoFillSupplierName(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "Select DISTINCT [Vendorname] from tbl_VendorMaster where " + "Vendorname like '%'+ @Search + '%'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> SupplierNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        SupplierNames.Add(sdr["Vendorname"].ToString());
                    }
                }
                con.Close();
                return SupplierNames;
            }
        }
    }

    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        BindKindAtt();

        BindEmailId();

        sName = txtSupplierName.Text;
    }

    protected void BindEmailId()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT [EmailID] FROM [tbl_VendorMaster] where Vendorname='" + txtSupplierName.Text.Trim() + "' ", con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            lblEmailID.Text = dt.Rows[0]["EmailID"].ToString() == "" ? "Email Id not found" : dt.Rows[0]["EmailID"].ToString();
        }
        else
        {
            lblEmailID.Text = "Email Id not found";
        }
    }

    //protected void BindParticular()
    //{
    //string com = "SELECT ItemName FROM tblItemMaster";
    //SqlDataAdapter adpt = new SqlDataAdapter(com, con);
    //DataTable dt = new DataTable();
    //adpt.Fill(dt);
    //ddlparticular.DataSource = dt;
    //ddlparticular.DataBind();
    //ddlparticular.DataTextField = "ItemName";
    //ddlparticular.DataValueField = "ItemName";
    //ddlparticular.DataBind();

    //ddlparticular.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Particular--", "0"));
    //}

    protected void BindKindAtt()
    {
        string com = "SELECT * FROM vwSupplierMaster where SupplierName='" + txtSupplierName.Text.Trim() + "'";
        SqlDataAdapter adpt = new SqlDataAdapter(com, con);
        DataTable dt = new DataTable();
        adpt.Fill(dt);
        ddlKindAtt.DataSource = dt;
        ddlKindAtt.DataBind();
        ddlKindAtt.DataTextField = "ContactName";
        ddlKindAtt.DataValueField = "ContactName";
        ddlKindAtt.DataBind();

        ddlKindAtt.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Kind. Att--", "0"));
    }

    protected void Insert(object sender, EventArgs e)
    {
        if (txtQty.Text == "" || ddlcomponent.SelectedItem.Text == "" || txtTotalamt.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please fill All Required fields !!!');", true);
        }
        else
        {
            Show_Grid();
        }
    }

    private void Show_Grid()
    {
        ViewState["RowNo"] = (int)ViewState["RowNo"] + 1;
        DataTable dt = (DataTable)ViewState["ParticularDetails"];

        dt.Rows.Add(ViewState["RowNo"], ddlcomponent.SelectedItem.Text, txtHSN.Text, txtQty.Text, txtRate.Text, txtDisc.Text, txtAmountt.Text, CGSTPer.Text, CGSTAmt.Text, SGSTPer.Text, SGSTAmt.Text, IGSTPer.Text, IGSTAmt.Text, txtTotalamt.Text, txtDescription.Text, txtUOM.Text);
        ViewState["ParticularDetails"] = dt;

        dgvParticularsDetails.DataSource = (DataTable)ViewState["ParticularDetails"];
        dgvParticularsDetails.DataBind();

        ddlcomponent.SelectedItem.Text = string.Empty;
        txtQty.Text = string.Empty;
        txtHSN.Text = string.Empty;
        txtRate.Text = string.Empty;
        txtDisc.Text = string.Empty;
        txtAmountt.Text = string.Empty;
        CGSTPer.Text = string.Empty;
        CGSTAmt.Text = string.Empty;
        SGSTPer.Text = string.Empty;
        SGSTAmt.Text = string.Empty;
        IGSTPer.Text = string.Empty;
        IGSTAmt.Text = string.Empty;
        txtTotalamt.Text = string.Empty;
        txtDescription.Text = string.Empty;
        //txtUOM.Text = string.Empty;
    }

    protected void dgvParticularsDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void dgvParticularsDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvParticularsDetails.EditIndex = e.NewEditIndex;
        dgvParticularsDetails.DataSource = (DataTable)ViewState["ParticularDetails"];
        dgvParticularsDetails.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void lnkbtnUpdate_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;

        string Particulars = ((Label)row.FindControl("lblParticulars")).Text;
        string HSN = ((Label)row.FindControl("lblHSN")).Text;
        string Qty = ((TextBox)row.FindControl("txtQty")).Text;
        string Rate = ((TextBox)row.FindControl("txtRate")).Text;

        string Discount = ((TextBox)row.FindControl("txtDiscount")).Text;

        string Amount = ((Label)row.FindControl("lblAmount")).Text;
        string CGSTPer = ((TextBox)row.FindControl("txtCGSTPer")).Text;
        string CGSTAmt = ((TextBox)row.FindControl("txtCGSTAmt")).Text;
        string SGSTPer = ((TextBox)row.FindControl("txtSGSTPer")).Text;
        string SGSTAmt = ((TextBox)row.FindControl("txtSGSTAmt")).Text;
        string IGSTPer = ((TextBox)row.FindControl("txtIGSTPer")).Text;
        string IGSTAmt = ((TextBox)row.FindControl("txtIGSTAmt")).Text;
        string TotalAmount = ((TextBox)row.FindControl("txtTotalAmount")).Text;
        string Description = ((TextBox)row.FindControl("txttblDescription")).Text;
        string UOM = ((TextBox)row.FindControl("txtUOM")).Text;

        DataTable Dt = ViewState["ParticularDetails"] as DataTable;

        Dt.Rows[row.RowIndex]["Particulars"] = Particulars;
        Dt.Rows[row.RowIndex]["HSN"] = HSN;
        Dt.Rows[row.RowIndex]["Qty"] = Qty;
        Dt.Rows[row.RowIndex]["Rate"] = Rate;
        Dt.Rows[row.RowIndex]["Discount"] = Discount;
        Dt.Rows[row.RowIndex]["Amount"] = Amount;
        Dt.Rows[row.RowIndex]["CGSTPer"] = CGSTPer;
        Dt.Rows[row.RowIndex]["CGSTAmt"] = CGSTAmt;
        Dt.Rows[row.RowIndex]["SGSTPer"] = SGSTPer;
        Dt.Rows[row.RowIndex]["SGSTAmt"] = SGSTAmt;
        Dt.Rows[row.RowIndex]["IGSTPer"] = IGSTPer;
        Dt.Rows[row.RowIndex]["IGSTAmt"] = IGSTAmt;
        Dt.Rows[row.RowIndex]["TotalAmount"] = TotalAmount;
        Dt.Rows[row.RowIndex]["Description"] = Description;
        Dt.Rows[row.RowIndex]["UOM"] = UOM;

        Dt.AcceptChanges();

        ViewState["ParticularDetails"] = Dt;
        dgvParticularsDetails.EditIndex = -1;

        dgvParticularsDetails.DataSource = (DataTable)ViewState["ParticularDetails"];
        dgvParticularsDetails.DataBind();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;

        DataTable dt = ViewState["ParticularDetails"] as DataTable;
        dt.Rows.Remove(dt.Rows[row.RowIndex]);
        ViewState["ParticularDetails"] = dt;
        dgvParticularsDetails.DataSource = (DataTable)ViewState["ParticularDetails"];
        dgvParticularsDetails.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Data Delete Succesfully !!!');", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);

    }

    protected void lnkCancel_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;

        DataTable Dt = ViewState["ParticularDetails"] as DataTable;
        dgvParticularsDetails.EditIndex = -1;

        ViewState["ParticularDetails"] = Dt;
        dgvParticularsDetails.EditIndex = -1;

        dgvParticularsDetails.DataSource = (DataTable)ViewState["ParticularDetails"];
        dgvParticularsDetails.DataBind();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "scrollToElement();", true);

    }

    private void FillddlComponent()
    {
        SqlDataAdapter ad = new SqlDataAdapter("SELECT DISTINCT[ComponentName] FROM [tbl_ComponentMaster]  where IsDeleted=0", Cls_Main.Conn);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            ddlcomponent.DataSource = dt;
            //ddlProduct.DataValueField = "ID";
            ddlcomponent.DataTextField = "ComponentName";
            ddlcomponent.DataBind();
            ddlcomponent.Items.Insert(0, "-- Select Component Name --");
        }
    }

    protected void ddlcomponent_TextChanged(object sender, EventArgs e)
    {
        SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM [tbl_ComponentMaster] WHERE ComponentName='" + ddlcomponent.SelectedItem.Text + "' ", Cls_Main.Conn);
        // SqlDataAdapter Da = new SqlDataAdapter("SELECT * FROM [tbl_MachineMaster] WHERE Productname='" + ddlProduct.SelectedItem.Text + "'", Cls_Main.Conn);
        DataTable Dt = new DataTable();
        Da.Fill(Dt);
        if (Dt.Rows.Count > 0)
        {

            txtHSN.Text = Dt.Rows[0]["HSN"].ToString() == "" ? "0" : Dt.Rows[0]["HSN"].ToString();
            txtRate.Text = Dt.Rows[0]["Price"].ToString() == "" ? "0" : Dt.Rows[0]["Price"].ToString();
            txtUOM.Text = Dt.Rows[0]["Unit"].ToString() == "" ? "0" : Dt.Rows[0]["Unit"].ToString();




        }
    }
    //[System.Web.Script.Services.ScriptMethod()]
    //[System.Web.Services.WebMethod]
    //public static List<string> GetItemList(string prefixText, int count)
    //{
    //    return AutoFilItem(prefixText);
    //}

    //public static List<string> AutoFilItem(string prefixText)
    //{
    //    using (SqlConnection con = new SqlConnection())
    //    {
    //        con.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    //        using (SqlCommand com = new SqlCommand())
    //        {
    //            com.CommandText = "select DISTINCT ComponentName from tbl_ComponentMaster where IsDeleted=0 AND " + "ComponentName like '%'+ @Search + '%'";

    //            com.Parameters.AddWithValue("@Search", prefixText);
    //            //com.Parameters.AddWithValue("@SName", sName);
    //            com.Connection = con;
    //            con.Open();
    //            List<string> Items = new List<string>();
    //            using (SqlDataReader sdr = com.ExecuteReader())
    //            {
    //                while (sdr.Read())
    //                {
    //                    Items.Add(sdr["ComponentName"].ToString());
    //                }
    //            }
    //            con.Close();
    //            return Items;
    //        }
    //    }
    //}

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetUOMList(string prefixText, int count)
    {
        return AutoFilUOM(prefixText);
    }

    public static List<string> AutoFilUOM(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "Select DISTINCT [Unit] from tbl_ProductMaster where " + "Unit like '%' + @Search + '%'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> StorageUnit = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        StorageUnit.Add(sdr["Unit"].ToString());
                    }
                }
                con.Close();
                return StorageUnit;
            }
        }
    }

    private void GST_Calculation()
    {
        var TotalAmt = Convert.ToDecimal(txtQty.Text.Trim()) * Convert.ToDecimal(txtRate.Text.Trim());

        decimal disc;
        if (string.IsNullOrEmpty(txtDisc.Text))
        {
            disc = 0;
            txtAmountt.Text = TotalAmt.ToString("0.00", CultureInfo.InvariantCulture);
        }
        else
        {
            decimal Val1 = Convert.ToDecimal(TotalAmt);
            decimal Val2 = Convert.ToDecimal(txtDisc.Text);
            disc = (Val1 * Val2 / 100);
            var result = Val1 - disc;
            txtAmountt.Text = result.ToString("0.00", CultureInfo.InvariantCulture);
        }



        decimal CGST;
        if (string.IsNullOrEmpty(CGSTPer.Text))
        {
            CGST = 0;
        }
        else
        {
            decimal Val1 = Convert.ToDecimal(txtAmountt.Text);
            decimal Val2 = Convert.ToDecimal(CGSTPer.Text);
            SGSTPer.Text = CGSTPer.Text;

            CGST = (Val1 * Val2 / 100);
        }
        CGSTAmt.Text = CGST.ToString("0.00", CultureInfo.InvariantCulture);

        decimal SGST;
        if (string.IsNullOrEmpty(SGSTPer.Text))
        {
            SGST = 0;
        }
        else
        {
            decimal Val1 = Convert.ToDecimal(txtAmountt.Text);
            decimal Val2 = Convert.ToDecimal(SGSTPer.Text);

            SGST = (Val1 * Val2 / 100);
        }
        SGSTAmt.Text = SGST.ToString("0.00", CultureInfo.InvariantCulture);


        decimal IGST;
        if (string.IsNullOrEmpty(IGSTPer.Text))
        {
            IGST = 0;
        }
        else
        {
            decimal Val1 = Convert.ToDecimal(txtAmountt.Text);
            decimal Val2 = Convert.ToDecimal(IGSTPer.Text);

            IGST = (Val1 * Val2 / 100);
        }
        IGSTAmt.Text = IGST.ToString("0.00", CultureInfo.InvariantCulture);

        var GSTTotal = CGST + SGST + IGST;

        var Finalresult = Convert.ToDecimal(txtAmountt.Text) + GSTTotal;

        txtTotalamt.Text = Finalresult.ToString("0.00", CultureInfo.InvariantCulture);
    }

    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        GST_Calculation();
    }

    protected void IGSTPer_TextChanged(object sender, EventArgs e)
    {
        GST_Calculation();

        if (IGSTPer.Text == "" || IGSTPer.Text == "0")
        {
            SGSTPer.Enabled = true;
            CGSTPer.Enabled = true;
            SGSTPer.Text = "0";
            CGSTPer.Text = "0";
        }
        else
        {
            SGSTPer.Enabled = false;
            CGSTPer.Enabled = false;
            SGSTPer.Text = "0";
            CGSTPer.Text = "0";
        }
    }

    protected void SGSTPer_TextChanged(object sender, EventArgs e)
    {
        GST_Calculation();

        if (SGSTPer.Text == "" || SGSTPer.Text == "0")
        {
            IGSTPer.Enabled = true;
            IGSTPer.Text = "0";
        }
        else
        {
            IGSTPer.Enabled = false;
            IGSTPer.Text = "0";
        }
    }

    protected void CGSTPer_TextChanged(object sender, EventArgs e)
    {
        GST_Calculation();

        if (CGSTPer.Text == "" || CGSTPer.Text == "0")
        {
            IGSTPer.Enabled = true;
            IGSTPer.Text = "0";
        }
        else
        {
            IGSTPer.Enabled = false;
            IGSTPer.Text = "0";
        }
    }

    private void GRID_GST_Calculation(GridViewRow row)
    {
        string Particulars = ((Label)row.FindControl("lblParticulars")).Text;
        string HSN = ((Label)row.FindControl("lblHSN")).Text;
        string Qty = ((TextBox)row.FindControl("txtQty")).Text;
        TextBox Rate = ((TextBox)row.FindControl("txtRate"));
        TextBox Discount = ((TextBox)row.FindControl("txtDiscount"));
        Label Amount = ((Label)row.FindControl("lblAmount"));
        string CGSTPer = ((TextBox)row.FindControl("txtCGSTPer")).Text;
        TextBox CGSTAmt = (TextBox)row.FindControl("txtCGSTAmt");
        string SGSTPer = ((TextBox)row.FindControl("txtSGSTPer")).Text;
        TextBox SGSTAmt = (TextBox)row.FindControl("txtSGSTAmt");
        string IGSTPer = ((TextBox)row.FindControl("txtIGSTPer")).Text;
        TextBox IGSTAmt = (TextBox)row.FindControl("txtIGSTAmt");
        TextBox TotalAmount = (TextBox)row.FindControl("txtTotalAmount");

        var totalamt = Convert.ToDecimal(Qty) * Convert.ToDecimal(Rate.Text);
        string Tot = "";

        decimal disc;
        if (string.IsNullOrEmpty(Discount.Text))
        {
            disc = 0;
            Amount.Text = totalamt.ToString("0.00", CultureInfo.InvariantCulture);
        }
        else
        {
            decimal val1 = Convert.ToDecimal(totalamt);
            decimal val2 = Convert.ToDecimal(Discount.Text);

            disc = (val1 * val2 / 100);
            var result = val1 - disc;
            Amount.Text = result.ToString("0.00", CultureInfo.InvariantCulture);
        }


        decimal Vcgst;
        if (string.IsNullOrEmpty(CGSTAmt.Text))
        {
            Vcgst = 0;
        }
        else
        {
            decimal val1 = Convert.ToDecimal(Amount.Text);
            decimal val2 = Convert.ToDecimal(CGSTPer);

            Vcgst = (val1 * val2 / 100);
        }
        CGSTAmt.Text = Vcgst.ToString("0.00", CultureInfo.InvariantCulture);

        decimal Vsgst;
        if (string.IsNullOrEmpty(SGSTAmt.Text))
        {
            Vsgst = 0;
        }
        else
        {
            decimal val1 = Convert.ToDecimal(Amount.Text);
            decimal val2 = Convert.ToDecimal(CGSTPer);

            Vsgst = (val1 * val2 / 100);
        }
        SGSTAmt.Text = Vsgst.ToString("0.00", CultureInfo.InvariantCulture);

        decimal Vigst;
        if (string.IsNullOrEmpty(IGSTAmt.Text))
        {
            Vigst = 0;
        }
        else
        {
            decimal val1 = Convert.ToDecimal(Amount.Text);
            decimal val2 = Convert.ToDecimal(IGSTPer);

            Vigst = (val1 * val2 / 100);
        }
        IGSTAmt.Text = Vigst.ToString("0.00", CultureInfo.InvariantCulture);

        var GSTTotal = Vcgst + Vsgst + Vigst;

        var taxamt = Convert.ToDecimal(Amount.Text) + GSTTotal;

        TotalAmount.Text = taxamt.ToString("0.00", CultureInfo.InvariantCulture);
    }

    protected void txtQty_TextChanged1(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;
        GRID_GST_Calculation(row);
    }

    protected void txtCGSTPer_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;
        GRID_GST_Calculation(row);
    }

    protected void txtSGSTPer_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;
        GRID_GST_Calculation(row);
    }

    protected void txtIGSTPer_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;
        GRID_GST_Calculation(row);
    }

    decimal Totalamt = 0;
    protected void dgvParticularsDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txts = (e.Row.FindControl("txtTotalAmount") as TextBox);

            if (txts == null)
            {
                Totalamt += Convert.ToDecimal((e.Row.FindControl("lblTotalAmount") as Label).Text);
                hdnGrandtotal.Value = Totalamt.ToString();
            }
            else
            {
                Totalamt += Convert.ToDecimal((e.Row.FindControl("txtTotalAmount") as TextBox).Text);
                hdnGrandtotal.Value = Totalamt.ToString();
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            (e.Row.FindControl("lbltotal") as Label).Text = Totalamt.ToString();
        }
    }

    //Send Mail
    /////pdf function
    protected void Send_Mail(int? Id, string Subject)
    {

        string strMessage = "Hello " + txtSupplierName.Text.Trim() + "<br/>" +


                        "Greetings From " + "<strong>Pune Abrasive Pvt. Ltd.<strong>" + "<br/>" +
                        "We sent you an Purchase Order Invoice." + txtPONo.Text.Trim() + "/" + txtPodate.Text.Trim() + ".pdf" + "<br/>" +

                         "We Look Foward to Conducting Future Business with you." + "<br/>" +

                        "Kind Regards," + "<br/>" +
                        "<strong>Pune Abrasive Pvt. Ltd.<strong>";
        string pdfname = "Purchase Order - " + txtPONo.Text.Trim() + "/" + txtPodate.Text.Trim() + ".pdf";

        MemoryStream file = new MemoryStream(PDF(Id).ToArray());

        file.Seek(0, SeekOrigin.Begin);
        Attachment data = new Attachment(file, pdfname, "application/pdf");
        ContentDisposition disposition = data.ContentDisposition;
        disposition.CreationDate = System.DateTime.Now;
        disposition.ModificationDate = System.DateTime.Now;
        disposition.DispositionType = DispositionTypeNames.Attachment;

        //msgendeaour.Attachments.Add(data);//Attach the file
        //msgenaccount.Attachments.Add(data);//Attach the file

        string fromMailID = Session["EmailID"].ToString().Trim().ToLower();
        string mailTo = lblEmailID.Text.Trim().ToLower();
        MailMessage mm = new MailMessage();

        mm.Attachments.Add(data);
        mm.Subject = "Purchase Order Invoice";
        mm.To.Add(mailTo);

        mm.CC.Add("girish.kulkarni@puneabrasives.com");
        //  mm.CC.Add("b.tikhe@puneabrasives.com");
        mm.CC.Add(Session["EmailID"].ToString().Trim().ToLower());

        mm.Body = strMessage;
        mm.IsBodyHtml = true;
        mm.From = new MailAddress("girish.kulkarni@puneabrasives.com", fromMailID);
        //message.Body = txtmessagebody.Text;
        SmtpClient SmtpMail = new SmtpClient();
        SmtpMail.Host = "us2.smtp.mailhostbox.com"; // Name or IP-Address of Host used for SMTP transactions  
        SmtpMail.Port = 25; // Port for sending the mail  
        SmtpMail.Credentials = new System.Net.NetworkCredential("girish.kulkarni@puneabrasives.com", "Qi#dKZN1"); // Username/password of network, if apply  
        SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
        SmtpMail.EnableSsl = false;

        SmtpMail.ServicePoint.MaxIdleTime = 0;
        SmtpMail.ServicePoint.SetTcpKeepAlive(true, 2000, 2000);
        mm.BodyEncoding = Encoding.Default;
        mm.Priority = MailPriority.High;
        SmtpMail.Send(mm);
    }

    public MemoryStream PDF(int? Id)
    {
        MemoryStream pdf = new MemoryStream();
        DataTable Dt = new DataTable();
        SqlDataAdapter Da = new SqlDataAdapter("select * from vw_PurchaseOrder where Id = '" + Id + "'", con);

        Da.Fill(Dt);

        StringWriter sw = new StringWriter();
        StringReader sr = new StringReader(sw.ToString());

        Document doc = new Document(PageSize.A4, 10f, 10f, 55f, 0f);
        PdfWriter pdfWriter = PdfWriter.GetInstance(doc, pdf);

        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/PDF_Files/") + "PurchaseOrder.pdf", FileMode.Create));
        XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);


        doc.Open();

        string imageURL = Server.MapPath("~") + "/Content/img/PAPL_logo.png";

        iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);

        //Resize image depend upon your need

        png.ScaleToFit(70, 100);

        //For Image Position
        png.SetAbsolutePosition(40, 718);
        //var document = new Document();

        //Give space before image
        //png.ScaleToFit(document.PageSize.Width - (document.RightMargin * 100), 50);
        png.SpacingBefore = 50f;

        //Give some space after the image

        png.SpacingAfter = 1f;

        png.Alignment = Element.ALIGN_LEFT;

        //paragraphimage.Add(png);
        //doc.Add(paragraphimage);
        doc.Add(png);


        PdfContentByte cb = pdfWriter.DirectContent;
        cb.Rectangle(17f, 710f, 560f, 60f);
        cb.Stroke();
        // Header 
        cb.BeginText();
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 25);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Pune Abrasive Pvt. Ltd.", 250, 745, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "GPlot No. 84, 2nd Floor D2 Block, MIDC Chinchwad, KSB Chowk, Near Shell Petrol Pump, Pune 411019", 145, 728, 0);
        cb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 11);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "", 227, 740, 0);
        cb.EndText();

        //PdfContentByte cbb = writer.DirectContent;
        //cbb.Rectangle(17f, 710f, 560f, 25f);
        //cbb.Stroke();
        //// Header 
        //cbb.BeginText();
        //cbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        //cbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " CONTACT : 9225658662   Email ID : mktg@excelenclosures.com", 153, 722, 0);
        //cbb.EndText();

        PdfContentByte cbbb = pdfWriter.DirectContent;
        cbbb.Rectangle(17f, 685f, 560f, 25f);
        cbbb.Stroke();
        // Header 
        cbbb.BeginText();
        cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "GSTIN : 27ABCCS7002A1ZW", 30, 695, 0);
        cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PAN NO: ATF*****J", 160, 695, 0);
        cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "EMAIL : girish.kulkarni@puneabrasives.com", 270, 695, 0);
        cbbb.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 10);
        cbbb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "CONTACT : +91 9860441689, 9511712429 ", 440, 695, 0);
        cbbb.EndText();

        PdfContentByte cd = pdfWriter.DirectContent;
        cd.Rectangle(17f, 660f, 560f, 25f);
        cd.Stroke();
        // Header 
        cd.BeginText();
        cd.SetFontAndSize(BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED), 14);
        cd.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Purchase Order", 260, 667, 0);
        cd.EndText();

        if (Dt.Rows.Count > 0)
        {
            var CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
            string PONo = Dt.Rows[0]["PONo"].ToString();
            string KindAtt = Dt.Rows[0]["KindAtt"].ToString();
            string PODate = Dt.Rows[0]["PODate"].ToString().TrimEnd("0:0".ToCharArray());
            string DeliveryDate = Dt.Rows[0]["DeliveryDate"].ToString().TrimEnd("0:0".ToCharArray());
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

            string BillToAddress = "";
            string ShipToAddress = "";
            string StateName = "";
            string GSTNo = "";
            string PANNo = "";

            // SqlCommand cmdsum = new SqlCommand("select * from tblSupplierMaster where SupplierName", con);
            SqlDataAdapter ad = new SqlDataAdapter("select * from tbl_VendorMaster where Vendorname='" + SupplierName + "'", con);
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
            paragraphTable1.SpacingAfter = 10f;

            PdfPTable table = new PdfPTable(4);

            float[] widths2 = new float[] { 100, 180, 100, 180 };
            table.SetWidths(widths2);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            var date = DateTime.Now.ToString("yyyy-MM-dd");


            table.AddCell(new Phrase("PO No : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(PONo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("PO Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(PODate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Supplier Name : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(SupplierName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Delivery Date :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(DeliveryDate, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Billing Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(BillToAddress, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Shipping Address", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(ShipToAddress, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Kind Att :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(KindAtt, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("GSTIN :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(GSTNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("Pan No", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(PANNo, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            table.AddCell(new Phrase("State Name :", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            table.AddCell(new Phrase(StateName, FontFactory.GetFont("Arial", 9, Font.BOLD)));

            paragraphTable1.Add(table);
            doc.Add(paragraphTable1);

            Paragraph paragraphTable2 = new Paragraph();
            paragraphTable2.SpacingAfter = 0f;
            table = new PdfPTable(12);
            float[] widths3 = new float[] { 4f, 40f, 11f, 6f, 10f, 8f, 11f, 8f, 10f, 8f, 10f, 16f };
            table.SetWidths(widths3);

            double Ttotal_price = 0;
            double CGST_price = 0;
            double SGST_price = 0;
            if (Dt.Rows.Count > 0)
            {
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                table.AddCell(new Phrase("SN.", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Name Of Particulars", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Hsn/Sac", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Qty", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Rate", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Disc (%)", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Amount", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("CGST(%)", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("CGST Amt", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("SGST(%)", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("SGST Amt", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Total", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                int rowid = 1;
                foreach (DataRow dr in Dt.Rows)
                {
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    string Qty = Dt.Rows[0]["Qty"].ToString();

                    double Ftotal = Convert.ToDouble(dr["GrandTotal"].ToString());
                    string _ftotal = Ftotal.ToString("##.00");
                    table.AddCell(new Phrase(rowid.ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Particulars"].ToString().Replace("<br>", "\n"), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["HSN"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(Qty + UOM, FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Rate"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Discount"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["Amount"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["CGSTPer"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["CGSTAmt"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["SGSTPer"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(dr["SGSTAmt"].ToString(), FontFactory.GetFont("Arial", 9)));
                    table.AddCell(new Phrase(_ftotal, FontFactory.GetFont("Arial", 9)));
                    rowid++;

                    Ttotal_price += Convert.ToDouble(dr["Amount"].ToString());
                    CGST_price += Convert.ToDouble(dr["CGSTAmt"].ToString());
                    SGST_price += Convert.ToDouble(dr["SGSTAmt"].ToString());
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
            Font font10 = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Paragraph paragraph = new Paragraph("", font12);

            for (int i = 0; i < items.Length; i++)
            {
                paragraph.Add(new Phrase("", font10));
            }

            table = new PdfPTable(12);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 4f, 40f, 11f, 6f, 10f, 8f, 11f, 8f, 10f, 8f, 10f, 16f });
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
            table.AddCell(new Phrase("\n\n\n\n\n\n\n\n ", FontFactory.GetFont("Arial", 10, Font.BOLD)));

            doc.Add(table);

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

            table.SetWidths(new float[] { 0f, 119f, 15f });
            table.AddCell(paragraph);
            PdfPCell cell = new PdfPCell(new Phrase("Value of Supply", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell);
            PdfPCell cell11 = new PdfPCell(new Phrase(amount, FontFactory.GetFont("Arial", 10, Font.BOLD)));
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

            table.SetWidths(new float[] { 0f, 119f, 15f });
            table.AddCell(paragraph);

            PdfPCell cell444 = new PdfPCell(new Phrase("Add CGST", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell444.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell444);
            PdfPCell cell555 = new PdfPCell(new Phrase(CGST_price.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell555.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell555);


            PdfPCell cell4440 = new PdfPCell(new Phrase("Add CGST", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell444.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell4440);
            PdfPCell cell5550 = new PdfPCell(new Phrase(CGST_price.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell555.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell5550);

            doc.Add(table);

            double Taxamot = CGST_price + SGST_price;

            var Totalamt = Convert.ToDecimal(amount) + Convert.ToDecimal(Taxamot);

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            paragraph.Alignment = Element.ALIGN_RIGHT;

            table.SetWidths(new float[] { 0f, 119f, 15f });
            table.AddCell(paragraph);
            PdfPCell cell4444 = new PdfPCell(new Phrase("Add SGST", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell4444.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell4444);

            PdfPCell cell5555 = new PdfPCell(new Phrase(SGST_price.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell5555.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell5555);
            doc.Add(table);

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            paragraph.Alignment = Element.ALIGN_RIGHT;

            table.SetWidths(new float[] { 0f, 119f, 15f });
            table.AddCell(paragraph);
            PdfPCell cellTaxAmount = new PdfPCell(new Phrase("Tax Amount", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cellTaxAmount.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellTaxAmount);

            PdfPCell cellTaxAmount1 = new PdfPCell(new Phrase(Taxamot.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cellTaxAmount1.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cellTaxAmount1);
            doc.Add(table);

            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            paragraph.Alignment = Element.ALIGN_RIGHT;

            table.SetWidths(new float[] { 0f, 119f, 15f });
            table.AddCell(paragraph);

            PdfPCell cell44 = new PdfPCell(new Phrase("Total Amount", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell44);

            PdfPCell cell440 = new PdfPCell(new Phrase(Totalamt.ToString(), FontFactory.GetFont("Arial", 10, Font.BOLD)));
            cell440.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell440);
            doc.Add(table);

            string Amtinword = ConvertNumbertoWords(Convert.ToInt32(Totalamt));

            //Total amount In word
            table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;

            paragraph.Alignment = Element.ALIGN_RIGHT;

            table.SetWidths(new float[] { 0f, 119f, 0f });
            table.AddCell(paragraph);

            PdfPCell cell4434 = new PdfPCell(new Phrase("Total Amount: " + Amtinword + "", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            cell4434.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell4434);

            PdfPCell cell44044 = new PdfPCell(new Phrase(Totalamt.ToString(), FontFactory.GetFont("Arial", 9, Font.BOLD)));
            cell44044.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell44044);
            doc.Add(table);

            Paragraph paragraphTable99 = new Paragraph(" Remarks :\n\n", font12);

            //Puja Enterprises Sign
            string[] itemss = {
                "REMARKS                                   :    "+Remarks+"\n",
                "PAYMENT TERM                         :    "+PaymentTerm+"\n",
                "PACKAING & FORWARDING     :    "+PackingAndForwarding+" \n",
                "TRANSPORTATION                    :    "+Transportation+" \n",
                "VARIATION                                  :    "+Variation+" \n",
                "DELIVERY                                    :    "+Delivery +" \n",
                "TEST CERTIFICATE                    :    "+TestCertificate+" \n",
                "WEEKLY OFF                              :    "+WeeklyOff+"\n",
                "TIME                                             :    "+Time+" \n",
                "II                                                    :    "+II+" \n",
                        };

            Font font14 = FontFactory.GetFont("Arial", 11);
            Font font15 = FontFactory.GetFont("Arial", 8);
            Paragraph paragraphhh = new Paragraph(" Terms & Condition :\n\n", font10);


            for (int i = 0; i < itemss.Length; i++)
            {
                paragraphhh.Add(new Phrase("\u2022 \u00a0" + itemss[i] + "\n", font15));
            }

            table = new PdfPTable(1);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 560f });

            table.AddCell(paragraphhh);
            //table.AddCell(new Phrase("Puja Enterprises \n\n\n\n         Sign", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            //table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            doc.Add(table);

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
            table.SetWidths(new float[] { 300f, 100f });

            //table.AddCell(paragraphhhhhff);
            table.AddCell(new Phrase(" ", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("       For Pune Abrasives Pvt. Ltd. \n\n\n\n\n\n         Authorised Signature", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            table.AddCell(new Phrase("", FontFactory.GetFont("Arial", 10, Font.BOLD)));
            doc.Add(table);
            doc.Close();


            //Byte[] FileBuffer = File.ReadAllBytes(Server.MapPath("~/files/") + "PurchaseOrder.pdf");

            //Font blackFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    PdfReader reader = new PdfReader(FileBuffer);
            //    using (PdfStamper stamper = new PdfStamper(reader, stream))
            //    {
            //        int pages = reader.NumberOfPages;
            //        for (int i = 1; i <= pages; i++)
            //        {
            //            if (i == 1)
            //            {

            //            }
            //            else
            //            {
            //                var pdfbyte = stamper.GetOverContent(i);
            //                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageURL);
            //                image.ScaleToFit(70, 100);
            //                image.SetAbsolutePosition(40, 792);
            //                image.SpacingBefore = 50f;
            //                image.SpacingAfter = 1f;
            //                image.Alignment = Element.ALIGN_LEFT;
            //                pdfbyte.AddImage(image);
            //            }
            //            var PageName = "Page No. " + i.ToString();
            //            ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase(PageName, blackFont), 568f, 820f, 0);
            //        }
            //    }
            //    FileBuffer = stream.ToArray();
            //}
            pdfWriter.CloseStream = true;
            //doc.Close();
            //pdf.Position = 0;
        }
        doc.Close();
        return pdf;
    }

    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "ZERO";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000000) + " MILLION ";
            number %= 1000000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "AND ";
            var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
            var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }


    protected void txtDisc_TextChanged(object sender, EventArgs e)
    {
        GST_Calculation();
    }

    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        GST_Calculation();
    }
    //17 march 2022
    private void Transportation_Calculation()
    {
        var TotalAmt = Convert.ToDecimal(txtTCharge.Text.Trim());

        decimal CGST;
        if (string.IsNullOrEmpty(txtTCGSTPer.Text))
        {
            CGST = 0;
        }
        else
        {
            decimal Val1 = Convert.ToDecimal(txtTCharge.Text.Trim());
            decimal Val2 = Convert.ToDecimal(txtTCGSTPer.Text);

            CGST = (Val1 * Val2 / 100);
        }
        txtTCGSTamt.Text = CGST.ToString("0.00", CultureInfo.InvariantCulture);

        decimal SGST;
        if (string.IsNullOrEmpty(txtTSGSTPer.Text))
        {
            SGST = 0;
        }
        else
        {
            decimal Val1 = Convert.ToDecimal(txtTCharge.Text);
            decimal Val2 = Convert.ToDecimal(txtTSGSTPer.Text);

            SGST = (Val1 * Val2 / 100);
        }
        txtTSGSTamt.Text = SGST.ToString("0.00", CultureInfo.InvariantCulture);


        decimal IGST;
        if (string.IsNullOrEmpty(txtTIGSTPer.Text))
        {
            IGST = 0;
        }
        else
        {
            decimal Val1 = Convert.ToDecimal(txtTCharge.Text);
            decimal Val2 = Convert.ToDecimal(txtTIGSTPer.Text);

            IGST = (Val1 * Val2 / 100);
        }
        txtTIGSTamt.Text = IGST.ToString("0.00", CultureInfo.InvariantCulture);

        var GSTTotal = CGST + SGST + IGST;

        var Finalresult = Convert.ToDecimal(txtTCharge.Text) + GSTTotal;

        txtTCost.Text = Finalresult.ToString("0.00", CultureInfo.InvariantCulture);


    }

    protected void txtTCharge_TextChanged(object sender, EventArgs e)
    {
        Transportation_Calculation();
    }

    protected void txtTCGSTPer_TextChanged(object sender, EventArgs e)
    {
        if (txtTCGSTPer.Text != "0")
        {
            txtTIGSTPer.Enabled = false;
            txtTSGSTPer.Text = txtTCGSTPer.Text;
        }
        else
        {
            txtTIGSTPer.Enabled = true;
            txtTSGSTPer.Text = "0";
        }


        Transportation_Calculation();
    }

    protected void txtTSGSTPer_TextChanged(object sender, EventArgs e)
    {
        if (txtTSGSTPer.Text != "0")
        {
            txtTIGSTPer.Enabled = false;
        }
        else
        {
            txtTIGSTPer.Enabled = true;
        }
        Transportation_Calculation();

        var TotalGrand = Convert.ToDouble(hdnGrandtotal.Value) + Convert.ToDouble(txtTCost.Text);
        hdnGrandtotal.Value = TotalGrand.ToString("0.00", CultureInfo.InvariantCulture);
    }

    protected void txtTIGSTPer_TextChanged(object sender, EventArgs e)
    {
        if (txtTIGSTPer.Text != "0")
        {
            txtTSGSTPer.Enabled = false;
            txtTCGSTPer.Enabled = false;
        }
        else
        {
            txtTSGSTPer.Enabled = true;
            txtTCGSTPer.Enabled = true;
        }
        Transportation_Calculation();

        var TotalGrand = Convert.ToDouble(hdnGrandtotal.Value) + Convert.ToDouble(txtTCost.Text);
        hdnGrandtotal.Value = TotalGrand.ToString("0.00", CultureInfo.InvariantCulture);
    }
    protected void txtDiscount_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;
        GRID_GST_Calculation(row);
    }
    protected void txtDeliverydate_TextChanged(object sender, EventArgs e)
    {
        //DateTime fromdate = DateTime.Parse(Convert.ToDateTime(txtBilldate.Text).ToShortDateString());
        DateTime PoDate = Convert.ToDateTime(txtPodate.Text);
        DateTime Ddate = Convert.ToDateTime(txtDeliverydate.Text);
        //DateTime todate = DateTime.Parse(Convert.ToDateTime(txtDOR.Text).ToShortDateString());
        if (PoDate > Ddate)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('PO Date is greater than Delivery Date...Please Choose Correct Date.');", true);
            btnadd.Enabled = false;
        }
        else
        {
            btnadd.Enabled = true;
        }
    }
    protected void txtRate_TextChanged1(object sender, EventArgs e)
    {
        GridViewRow row = (sender as TextBox).NamingContainer as GridViewRow;
        GRID_GST_Calculation(row);
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Purchase/PurchaseOrderList.aspx");
    }
}