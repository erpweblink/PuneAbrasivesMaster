<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestImport.aspx.cs" Inherits="Inventory_TestImport" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Send Voucher to Tally</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btnSendToTally" runat="server" Text="Send Data to Tally" OnClick="btnSendToTally_Click" />
        <asp:Literal ID="ltResult" runat="server" />
    </form>
</body>
</html>
