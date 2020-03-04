<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="showResult.aspx.cs" Inherits="WebApplication1.showResult" EnableSessionState="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 20px;
        }
        .auto-style2 {
            width: 349px;
        }
        .auto-style3 {
            height: 20px;
            width: 349px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <table style="width:100%;">
            <tr>
                <td class="auto-style2">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="重新上传" />
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3" align="center" valign="middle">
                    <asp:Label ID="im3" runat="server" Text="im3"></asp:Label>
                    <br />
                    <asp:ImageButton ID="ib3" runat="server" OnClick="ib3_Click" />
                </td>
                <td class="auto-style1" align="center" valign="middle">
                    <asp:Label ID="im1" runat="server" Text="Label"></asp:Label>
                    <br />
                    <asp:ImageButton ID="ib1" runat="server" OnClick="ib1_Click" />
                </td>
                <td class="auto-style1" align="center" valign="middle">
                    <asp:Label ID="im2" runat="server" Text="Label"></asp:Label>
                    <br />
                    <asp:ImageButton ID="ib2" runat="server" OnClick="ib2_Click" />
                </td>
            </tr>
            <tr>
                <td class="auto-style2" align="center" valign="middle">
                    <asp:Label ID="lb3" runat="server" Text="Label"></asp:Label>
                </td>
                <td align="center" valign="middle">
                    <asp:Label ID="lb1" runat="server" Text="Label"></asp:Label>
                </td>
                <td align="center" valign="middle">
                    <asp:Label ID="lb2" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
