<%@ Page Title="Asignar Turno" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AsignarTurno.aspx.cs" Inherits="SistemaClinica.AsignarTurno" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="row mb-4">
        <div class="col-12">
            <h2 class="text-primary"><i class="fa-solid fa-calendar-plus me-2"></i>Asignar Nuevo Turno Médicos</h2>
            <p class="text-muted">Gestión dinámica de agendas, sugerencia de horarios y reserva de turnos para pacientes.</p>
        </div>
    </div>

    <asp:Panel ID="pnlAlertaExito" runat="server" CssClass="alert alert-success alert-dismissible fade show shadow-sm d-none" role="alert">
        <strong><i class="fa-solid fa-circle-check me-2"></i>¡Turno Asignado!</strong> Se generó el comprobante y se envió el correo de confirmación.
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <div class="row g-4">
        
        <div class="col-lg-8">
            <div class="tarjeta-clinica shadow-sm mb-4">
                <h3 class="headline-sm mb-4 text-secondary"><i class="fa-solid fa-file-invoice me-2"></i>Datos del Turno</h3>
                
                <div class="row g-3">
                    
                    <div class="col-md-6">
                        <label class="form-label fw-bold text-secondary">Paciente *</label>
                        <asp:DropDownList ID="ddlPaciente" runat="server" CssClass="form-select form-control-clinica">
                            <asp:ListItem Text="Seleccione un paciente..." Value="" />
                            <%-- Se cargará desde la Base de Datos --%>
                            <asp:ListItem Text="Pérez, Juan Carlos (DNI: 35123456)" Value="1" />
                            <asp:ListItem Text="Gómez, María Belén (DNI: 38765432)" Value="2" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-bold text-secondary">Especialidad Requerida *</label>
                        <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select form-control-clinica" 
                            AutoEventWireup="true" AutoPostBack="true" OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged">
                            <asp:ListItem Text="Seleccione una especialidad..." Value="" />
                           
                        </asp:DropDownList>
                    </div>

                    <div class="col-12 my-2">
                        <hr style="border-top: 1px dashed var(--cp-borde-suave);" />
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-bold text-secondary">Médico Especialista *</label>
                        <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select form-control-clinica">
                            <asp:ListItem Text="Seleccione primero la especialidad..." Value="" />
                            <%-- Se filtrará dinámicamente según especialidad --%>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label fw-bold text-secondary">Fecha del Turno *</label>
                        <asp:TextBox ID="txtFechaTurno" runat="server" CssClass="form-control form-control-clinica" TextMode="Date"></asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label fw-bold text-secondary">Horarios Disponibles *</label>
                        <asp:DropDownList ID="ddlHorario" runat="server" CssClass="form-select form-control-clinica">
                            <asp:ListItem Text="Seleccione día..." Value="" />
                            <%-- Se cargarán los bloques de 1 hora libre del médico (ej: 10:00 a 11:00) --%>
                        </asp:DropDownList>
                    </div>

                    <div class="col-12">
                        <label class="form-label fw-bold text-secondary">Observaciones / Causa de la consulta *</label>
                        <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control form-control-clinica" TextMode="MultiLine" Rows="3" placeholder="Ej: Control anual, dolor agudo en rodilla, requiere orden médica..."></asp:TextBox>
                    </div>

                    <div class="col-12 text-end mt-4">
                        <hr class="mb-4" style="border-top: 1px solid var(--cp-borde-suave);" />
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar Formulario" CssClass="btn btn-clinica-secundario me-2 px-4" />
                        <asp:Button ID="btnConfirmarTurno" runat="server" Text="Confirmar y Reservar Turno" CssClass="btn btn-success px-4 fw-bold" />
                    </div>

                </div>
            </div>
        </div>

        <div class="col-lg-4">
            <div class="card border-primary shadow-sm" style="border-radius: 12px; overflow: hidden;">
                <div class="card-header bg-primary text-white py-3">
                    <h5 class="card-title mb-0 fw-bold"><!--i class="fa-solid fa-wand-magic-sparkles me-2"--></!--i>Horarios Sugeridos</h5>
                    <small class="text-white-50">Opciones rápidas basadas en la especialidad</small>
                </div>
                <div class="card-body bg-light">
                    <!--p class="text-muted small">Al elegir una especialidad, el sistema sugerirá las 3 opciones libres más próximas para ahorrar tiempo de carga.</!-->

                    <div class="d-grid gap-3">

                        <div class="p-3 border rounded bg-white shadow-xs position-relative hover-tarjeta" style="cursor: pointer; border-left: 4px solid var(--bs-success) !important;">
                            <span class="badge bg-success position-absolute top-0 end-0 m-2">Más próximo</span>
                            <h6 class="fw-bold mb-1 text-primary">Dr. Claudio Rossi</h6>
                            <p class="mb-0 small text-dark"><i class="fa-regular fa-calendar me-1"></i>Mañana, 15 de Junio</p>
                            <p class="mb-0 small text-muted"><i class="fa-regular fa-clock me-1"></i>10:00 a 11:00 hs</p>
                        </div>

                        <div class="p-3 border rounded bg-white shadow-xs hover-tarjeta" style="cursor: pointer; border-left: 4px solid var(--bs-primary) !important;">
                            <h6 class="fw-bold mb-1 text-primary">Dra. Natalia Soler</h6>
                            <p class="mb-0 small text-dark"><i class="fa-regular fa-calendar me-1"></i>Mañana, 15 de Junio</p>
                            <p class="mb-0 small text-muted"><i class="fa-regular fa-clock me-1"></i>14:00 a 15:00 hs</p>
                        </div>

                        <div class="p-3 border rounded bg-white shadow-xs hover-tarjeta" style="cursor: pointer; border-left: 4px solid var(--bs-primary) !important;">
                            <h6 class="fw-bold mb-1 text-primary">Dr. Claudio Rossi</h6>
                            <p class="mb-0 small text-dark"><i class="fa-regular fa-calendar me-1"></i>Miércoles, 17 de Junio</p>
                            <p class="mb-0 small text-muted"><i class="fa-regular fa-clock me-1"></i>11:00 a 12:00 hs</p>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </div>

    <style>
        .hover-tarjeta:hover {
            background-color: #f8f9fa !important;
            transform: translateY(-2px);
            box-shadow: 0 .125rem .25rem rgba(0,0,0,.075)!important;
            transition: all 0.2s ease-in-out;
        }
    </style>

</asp:Content>