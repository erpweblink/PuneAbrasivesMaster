using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Inventory_TestImport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSendToTally_Click(object sender, EventArgs e)
    {
        string tallyUrl = "http://127.0.0.1:9000"; // Tally default HTTP port

        string tallyXML = GenerateTallyXML();

        string response = SendRequestToTally(tallyUrl, tallyXML);

        ltResult.Text = "<pre>" + Server.HtmlEncode(response) + "</pre>";
    }

    private string GenerateTallyXML()
    {
        // Sample XML to create a ledger named "Test Ledger"
        return @"<ENVELOPE>
  <HEADER>
    <TALLYREQUEST>Import Data</TALLYREQUEST>
  </HEADER>
  <BODY>
    <IMPORTDATA>
      <REQUESTDESC>
        <REPORTNAME>All Masters</REPORTNAME>
      </REQUESTDESC>
      <REQUESTDATA>
        <TALLYMESSAGE xmlns:UDF=""TallyUDF"">
          <LEDGER NAME=""Test Ledger"" RESERVEDNAME="""">
            <PARENT>Indirect Expenses</PARENT>
            <ISBILLWISEON>No</ISBILLWISEON>
            <ISCOSTCENTRESON>No</ISCOSTCENTRESON>
            <ISINTERESTON>No</ISINTERESTON>
            <USEFORVAT>No</USEFORVAT>
            <ISACTIVE>No</ISACTIVE>
            <OPENINGBALANCE>0</OPENINGBALANCE>
          </LEDGER>
        </TALLYMESSAGE>
      </REQUESTDATA>
    </IMPORTDATA>
  </BODY>
</ENVELOPE>";
    }

    private string SendRequestToTally(string url, string xmlData)
    {
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(xmlData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = bytes.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }
}