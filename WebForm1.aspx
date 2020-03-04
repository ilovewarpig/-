<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication1.WebForm1" EnableSessionState="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 25px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <br />
        <p>
            &nbsp;</p>
        <table style="width:100%;">
            <tr>
                <td align="center">
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="测试图片识别" />
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="请上传图片用以识别"></asp:Label>
                    <asp:Label ID="alert" runat="server" Text="......"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center">
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="测试session" />
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="确认上传" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1" align="center">
                    <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="测试截图" />
                </td>
                <td class="auto-style1">
                    </td>
                <td class="auto-style1">
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
