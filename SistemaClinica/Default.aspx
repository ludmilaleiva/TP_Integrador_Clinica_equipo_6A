<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SistemaClinica.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <div class="row">
        <div class="col-12">
            <div class="tarjeta-clinica shadow-sm mb-4">
                <div class="py-4 text-center">
                    <h1 class="text-primary mb-3">Sistema de Gestión Médica</h1>
                    <p class="col-md-9 mx-auto text-muted" style="font-size: 16px;">
                        Bienvenido al sistema de gestión de turnos de la clínica BioClinic. Seleccioná una opción para comenzar a operar
                    </p>
                 
                    <hr class="my-4 mx-auto" style="max-width: 300px; border-top: 1px solid var(--cp-borde-suave);" />
                    
                    <div class="d-grid gap-2 d-sm-flex justify-content-sm-center">
                        <a href="TurnosPacientes.aspx" class="btn btn-clinica-primario px-4 fs-6">
                            <i class="fa-solid fa-calendar-check me-2"></i>Asignar Nuevo Turno
                        </a>
                        <a href="ListadoTurnos.aspx" class="btn btn-clinica-secundario px-4 fs-6">
                            <i class="fa-solid fa-clock me-2"></i>Monitorear Grilla
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
