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
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;

public partial class Account_CreditDebitList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            Response.Redirect("../Login.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                Gvbind();
            }
        }
    }

    private void Gvbind()
    {
        string query = string.Empty;
        query = @"select * from tblCreditDebitNoteHdr where NoteFor='Sale' order by Id desc";
        SqlDataAdapter ad = new SqlDataAdapter(query, con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            GvCreditDebit.DataSource = dt;
            GvCreditDebit.DataBind();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GvCreditDebit.ClientID + "', 450, 1020 , 40 ,true); </script>", false);
        }
        else
        {
            GvCreditDebit.DataSource = null;
            GvCreditDebit.DataBind();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GvCreditDebit.ClientID + "', 450, 1020 , 40 ,true); </script>", false);
        }
    }

    protected void GvCreditDebit_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ViewState["CompRowId"] = e.CommandArgument.ToString();
        if (e.CommandName == "RowEdit")
        {
            ViewState["id"] = e.CommandArgument.ToString();
            Response.Redirect("CreditDebitNote_Sale.aspx?ID=" + encrypt(e.CommandArgument.ToString()));
        }

        if (e.CommandName == "DownloadPDF")
        {
            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                Session["PDFID"] = e.CommandArgument.ToString();
                Response.Redirect("CDNotePDF.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + " ");
                //Response.Write("<script>window.open('CDNotePDF.aspx?Id=" + encrypt(e.CommandArgument.ToString()) + "','_blank');</script>");
            }
        }

        if (e.CommandName == "RowDelete")
        {
            con.Open();

            //// Update ID in [tblCreditDebitNote_ID]
            SqlCommand cmdNoteType = new SqlCommand("SELECT NoteType FROM [tblCreditDebitNoteHdr] with (nolock) where Id='" + Convert.ToInt32(e.CommandArgument.ToString()) + "'", con);
            Object mxNoteType = cmdNoteType.ExecuteScalar();
            string NoteType = mxNoteType.ToString();

            SqlCommand cmdmaxx = new SqlCommand("SELECT ID_Max as maxid FROM [tblCreditDebitNote_ID] with (nolock) where NoteType='" + NoteType + "'", con);
            Object mxx = cmdmaxx.ExecuteScalar();
            int MaxIdd = Convert.ToInt32(mxx.ToString()) - 1;

            SqlCommand CmdUpdtID = new SqlCommand("Update tblCreditDebitNote_ID Set ID_Max=@ID_Max where NoteType='" + NoteType + "'", con);
            CmdUpdtID.Parameters.AddWithValue("@ID_Max", MaxIdd);
            CmdUpdtID.ExecuteNonQuery();

            SqlCommand Cmd = new SqlCommand("delete from tblCreditDebitNoteHdr WHERE Id=@Id", con);
            Cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            Cmd.ExecuteNonQuery();

            SqlCommand Cmddtl = new SqlCommand("delete from tblCreditDebitNoteDtls WHERE HeaderID=@Id", con);
            Cmddtl.Parameters.AddWithValue("@Id", Convert.ToInt32(e.CommandArgument.ToString()));
            Cmddtl.ExecuteNonQuery();

            con.Close();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data Deleted Sucessfully');window.location.href='CreditDebitNoteSaleList.aspx';", true);

        }
    }


    public string encrypt(string encryptString)
    {
        string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                encryptString = Convert.ToBase64String(ms.ToArray());
            }
        }
        return encryptString;
    }

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

    #region Filter

    protected void txtcnamefilter_TextChanged(object sender, EventArgs e)
    {
        string query = string.Empty;
        if (!string.IsNullOrEmpty(txtcnamefilter.Text.Trim()))
        {
            query = "SELECT * FROM tblCreditDebitNoteHdr where SupplierName like '" + txtcnamefilter.Text.Trim() + "%' order by Id desc";
        }
        else
        {
            query = "SELECT * FROM tblCreditDebitNoteHdr where order by Id desc";
        }

        SqlDataAdapter ad = new SqlDataAdapter(query, con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            GvCreditDebit.DataSource = dt;
            GvCreditDebit.DataBind();
        }
        else
        {
            GvCreditDebit.DataSource = null;
            GvCreditDebit.DataBind();
        }
    }
    #endregion Filter

    protected void btnresetfilter_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreditDebitNoteSaleList.aspx");
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
                //com.CommandText = "Select DISTINCT [SupplierName] from [tblSupplierMaster] where " + "SupplierName like @Search + '%'";
                com.CommandText = "Select DISTINCT [Companyname] from [tbl_CompanyMaster] where " + "Companyname like @Search + '%'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["Companyname"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetSupplierOwnerList(string prefixText, int count)
    {
        return AutoFillSupplierOwnerName(prefixText);
    }

    public static List<string> AutoFillSupplierOwnerName(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "Select DISTINCT Ownername from tbl_VendorMaster where Ownername like @Search + '%' and status=0 and [isdeleted]=0";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["Ownername"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }

    private string GetMailIdOfEmpl(string Empcode)
    {
        string query1 = "SELECT [email] FROM [employees] where [empcode]='" + Empcode + "' ";
        SqlDataAdapter ad = new SqlDataAdapter(query1, con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        string email = string.Empty;
        if (dt.Rows.Count > 0)
        {
            email = dt.Rows[0]["email"].ToString();
        }
        return email;
    }

    protected void ddlsalesMainfilter_TextChanged(object sender, EventArgs e)
    {
        Gvbind();
    }

    protected void btnAddEnq_Click(object sender, EventArgs e)
    {
        string Cname = ((sender as Button).CommandArgument).ToString();
        Response.Redirect("Addenquiry.aspx?Cname=" + encrypt(Cname));
        //Page.ClientScript.RegisterStartupScript(GetType(), "", "window.open('EnquiryFile.aspx?Fileid=" + id + "','','width=700px,height=600px');", true);
    }
    protected void GvCreditDebit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblNoteType = (e.Row.FindControl("lblNoteType") as Label);
                Label txtbillno = (e.Row.FindControl("lblBillNumber") as Label);
                LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                LinkButton btnDelete = e.Row.FindControl("btnDelete") as LinkButton;
                LinkButton btnPDF = e.Row.FindControl("btnPDF") as LinkButton;
                string empcode = Session["UserCode"].ToString();
                DataTable Dt = new DataTable();
                SqlDataAdapter Sd = new SqlDataAdapter("Select ID from tbl_UserMaster where UserCode='" + empcode + "'", con);
                Sd.Fill(Dt);
                if (Dt.Rows.Count > 0)
                {
                    string id = Dt.Rows[0]["ID"].ToString();
                    DataTable Dtt = new DataTable();
                    SqlDataAdapter Sdd = new SqlDataAdapter("Select * FROM tblUserRoleAuthorization where UserID = '" + id + "' AND PageName = 'CustomerTaxInvoiceList.aspx' AND PagesView = '1'", con);
                    Sdd.Fill(Dtt);
                    if (Dtt.Rows.Count > 0)
                    {
                        Button1.Visible = false;
                        btnEdit.Visible = false;
                        btnDelete.Visible = false;
                        btnPDF.Visible = true;
                    }
                }
             

                if (lblNoteType.Text == "Debit_Sale Note")
                {
                    lblNoteType.Text = "Debit";
                }
                else if (lblNoteType.Text == "Credit_Sale Note")
                {
                    lblNoteType.Text = "Credit";
                }

                if (txtbillno.Text == "--Select Invoice Number--")
                {
                    txtbillno.Text = "";
                }
                else if (txtbillno.Text != "")
                {
                    //SqlCommand cmd = new SqlCommand("select SupplierBillNo from tblPurchaseBillHdr where BillNo='" + txtbillno.Text + "'", con);
                    SqlCommand cmd = new SqlCommand("select InvoiceNo from tblTaxInvoiceHdr where InvoiceNo='" + txtbillno.Text + "'", con);
                    con.Open();

                    //string Supplierbill = cmd.ExecuteScalar().ToString() == "--Select Invoice Number--" ? "" : cmd.ExecuteScalar().ToString();
                    string Supplierbill = cmd.ExecuteScalar().ToString();
                    txtbillno.Text = Supplierbill;

                    con.Close();
                }
                else
                {

                }

                // Check whether the E-Invoice is Created or not
                con.Open();
                int IDD = Convert.ToInt32(GvCreditDebit.DataKeys[e.Row.RowIndex].Values[0]);
                SqlCommand cmdIgstVal = new SqlCommand("select Irn from tblCreditDebitNoteHdr where Id='" + IDD + "'", con);
                Object F_IgstVal = cmdIgstVal.ExecuteScalar();
                string IsCreatedIRN = F_IgstVal.ToString();
                if (IsCreatedIRN == "")
                {
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;

                }
                else
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
                con.Close();

            }


        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreditDebitNote_Sale.aspx");
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("EInv_CrDbNote.aspx");
    }

    protected void GvInvoiceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCreditDebit.PageIndex = e.NewPageIndex;
        Gvbind();
    }
}