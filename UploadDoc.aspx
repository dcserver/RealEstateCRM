<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadDoc.aspx.cs" Inherits="RealEstateCRM.UploadDoc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnPlots" runat="server" Text="Upload Plots" OnClick="btnPlots_Click" Visible="false" />
        </div>
        <div>
            <asp:Button ID="btnPassbook" runat="server" Text="Upload Passbook" OnClick="btnPassbook_Click" Visible="false" />
        </div>
        <div>
            <asp:Button ID="btnReceipts" runat="server" Text="Upload Receipts" OnClick="btnReceipts_Click" Visible="false" />
        </div>
        <div>
            <asp:Button ID="btnCommissions" runat="server" Text="Upload Commissions" OnClick="btnCommissions_Click" Visible="false" />
        </div>
        <div>
            <asp:Button ID="btnCPayment" runat="server" Text="Upload Commission Payment" OnClick="btnCPayment_Click" Visible="false" />
        </div>
    </form>
</body>
</html>
