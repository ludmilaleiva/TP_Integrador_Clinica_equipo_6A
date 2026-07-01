<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SistemaClinica.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Login - Clínica</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>

<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container min-vh-100 d-flex justify-content-center align-items-center">
            <div class="card shadow-sm p-4" style="width: 380px;">
                <h3 class="text-center mb-4 text-primary">Sistema Clínica</h3>

                <div class="mb-3">
                    <label class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                </div>

                <div class="mb-3">
                    <label class="form-label">Contraseña</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                </div>

                <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mb-3" Visible="false" />

                <asp:Button 
                    ID="btnLogin" 
                    runat="server" 
                    Text="Ingresar" 
                    CssClass="btn btn-primary w-100"
                    OnClick="btnLogin_Click" />
            </div>
        </div>
    </form>
</body>
</html>