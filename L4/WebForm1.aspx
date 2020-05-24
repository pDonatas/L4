<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="L4.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="css.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="box">
        <asp:Label ID="Label1" runat="server" Text="Suma, kurios negalima viršyti"></asp:Label>
        
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        </div>
        <div class="box">
        <asp:Label ID="Label3" runat="server"></asp:Label>
        </div>
        <div class="box">
        <asp:Label ID="Label4" runat="server"></asp:Label>
        </div>
        <asp:Table ID="Table1" runat="server">
        </asp:Table>
        <div class="box">
        <asp:Label ID="Label6" runat="server"></asp:Label>
        </div>
        <asp:Table ID="Table2" runat="server">
        </asp:Table>
        <div class="box">
        <asp:Label ID="Label7" runat="server"></asp:Label>
        </div>
        <asp:Table ID="Table3" runat="server">
        </asp:Table>
        <br />
        <div class="box">
        <asp:Label ID="Label5" runat="server"></asp:Label>
        </div>
        <div class="error">
            <asp:Label ID="Label2" runat="server"></asp:Label>
        </div>
        
    </form>
</body>
</html>
