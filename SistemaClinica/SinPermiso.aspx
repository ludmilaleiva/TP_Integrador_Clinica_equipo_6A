<%@ Page Title="Sin permiso" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SinPermiso.aspx.cs" Inherits="SistemaClinica.SinPermiso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="alert alert-danger mt-4">
        <h4>Acceso denegado</h4>
        <p>No tenés permisos para acceder a esta página.</p>
    </div>
</asp:Content>