<%@ Page Title="Asignar Turno" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AsignarTurno.aspx.cs" Inherits="SistemaClinica.AsignarTurno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/select2-bootstrap-5-theme@1.3.0/dist/select2-bootstrap-5-theme.min.css" rel="stylesheet" />
    
    <div class="row mb-4">
        <div class="col-12">
            <h2 class="text-primary"><i class="fa-solid fa-calendar-plus me-2"></i>Asignar Nuevo Turno Médicos</h2>
            <p class="text-muted">Gestión dinámica de agendas, sugerencia de horarios y reserva de turnos para pacientes.</p>
        </div>
    </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:Panel ID="pnlAlertaExito" runat="server" CssClass="alert alert-success alert-dismissible fade show shadow-sm d-none" role="alert">
        <strong><i class="fa-solid fa-circle-check me-2"></i>¡Turno Asignado!</strong> Se generó el comprobante y se envió el correo de confirmación.
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

   <asp:UpdatePanel ID="upAsignarTurno" runat="server">
    <ContentTemplate>

      <div class="row g-4">
        
        <div class="col-lg-8">

            <div class="tarjeta-clinica shadow-sm mb-4">
                <h3 class="headline-sm mb-4 text-secondary"><i class="fa-solid fa-file-invoice me-2"></i>Datos del Turno</h3>
                
                <div class="row g-3">
                    
                    <div class="col-md-6">
                        <label class="form-label fw-bold text-secondary">Paciente *</label>
                        <asp:DropDownList ID="ddlPaciente" runat="server" CssClass="form-select form-control-clinica buscador-dinamico">
                            <asp:ListItem Text="Seleccione un paciente..." Value="" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-bold text-secondary">Especialidad Requerida *</label>
                        <asp:DropDownList 
                            ID="ddlEspecialidad" 
                            runat="server" 
                            CssClass="form-select form-control-clinica"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged">
                            <asp:ListItem Text="Seleccione una especialidad..." Value="" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-12 my-2">
                        <hr style="border-top: 1px dashed var(--cp-borde-suave);" />
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-bold text-secondary">Médico Especialista *</label>
                        <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select form-control-clinica" >
                 
                            <asp:ListItem Text="Seleccione primero la especialidad..." Value="" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label fw-bold text-secondary">Fecha del Turno *</label>
                        <asp:TextBox ID="txtFechaTurno" runat="server" CssClass="form-control form-control-clinica" TextMode="Date" AutoPostBack="true" OnTextChanged="txtFechaTurno_TextChanged" Enabled="false"></asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label fw-bold text-secondary">Horarios Disponibles *</label>
                        <asp:DropDownList ID="ddlHorario" runat="server" CssClass="form-select form-control-clinica" Enabled="false">
                            <asp:ListItem Text="Seleccione día..." Value="" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-12">
                        <label class="form-label fw-bold text-secondary">Observaciones / Causa de la consulta *</label>
                        <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control form-control-clinica" TextMode="MultiLine" Rows="3" placeholder="Ej: Control anual, dolor agudo en rodilla, requiere orden médica..."></asp:TextBox>
                    </div>

                    <div class="col-12 text-end mt-4">
                        <hr class="mb-4" style="border-top: 1px solid var(--cp-borde-suave);" />
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar Formulario" CssClass="btn btn-clinica-secundario me-2 px-4" OnClick="btnLimpiar_Click" />
                        <asp:Button ID="btnConfirmarTurno" runat="server" Text="Confirmar y Reservar Turno" CssClass="btn btn-success px-4 fw-bold" OnClick="btnConfirmarTurno_Click"/>
                    </div>

                </div>
            </div>
        </div>

        <div class="col-lg-4">
            <div class="card border-primary shadow-sm" style="border-radius: 12px; overflow: hidden;">
                <div class="card-header bg-primary text-white py-3">
                    <h5 class="card-title mb-0 fw-bold">Horarios Sugeridos</h5>
                    <small class="text-white-50">Opciones rápidas basadas en la especialidad</small>
                </div>
                <div class="card-body bg-light">
            <div class="d-grid gap-3">
                <%-- 🚀 REPEATER DINÁMICO: Reemplaza las 3 tarjetas fijas --%>
                <asp:Repeater ID="repHorariosSugeridos" runat="server" OnItemCommand="repHorariosSugeridos_ItemCommand">
                    <ItemTemplate>
                        <asp:LinkButton 
                            ID="btnSeleccionarSugerido" 
                            runat="server"
                            CommandName="SeleccionarTurno"
                            CommandArgument='<%# Eval("IdMedico") + "|" + Eval("FechaSql") + "|" + Eval("HoraInicio") %>'
                            CssClass="p-3 border rounded bg-white shadow-xs position-relative hover-tarjeta d-block"
                            Style="text-decoration: none; text-align: left; border-left: 4px solid var(--bs-primary) !important;">

                            <%# Container.ItemIndex == 0 ? "<span class='badge bg-success position-absolute top-0 end-0 m-2'>Más próximo</span>" : "" %>

                            <h6 class="fw-bold mb-1 text-primary">
                                Dr/a. <%# Eval("NombreMedico") %>
                            </h6>

                            <p class="mb-0 small text-dark">
                                <i class="fa-regular fa-calendar me-1"></i>
                                <%# Eval("FechaTexto") %>
                            </p>

                            <p class="mb-0 small text-muted">
                                <i class="fa-regular fa-clock me-1"></i>
                                <%# Eval("HoraTexto") %> hs
                            </p>

                        </asp:LinkButton>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%-- Mensaje por si no hay sugerencias disponibles --%>
                        <asp:Label ID="lblSinSugerencias" runat="server" Text="No hay sugerencias disponibles para esta especialidad." 
                            Visible='<%# repHorariosSugeridos.Items.Count == 0 %>' CssClass="text-muted small p-2 d-block text-center italic" />
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>

        </div>
      </ContentTemplate>
   </asp:UpdatePanel>

    <style>
        .hover-tarjeta:hover {
            background-color: #f8f9fa !important;
            transform: translateY(-2px);
            box-shadow: 0 .125rem .25rem rgba(0,0,0,.075)!important;
            transition: all 0.2s ease-in-out;
        }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>

    <script type="text/javascript">
        function initSelect2() {
            $('.buscador-dinamico').select2({
                theme: 'bootstrap-5',
                placeholder: 'Escriba nombre, apellido o DNI para buscar...',
                allowClear: true,
                language: {
                    noResults: function () { return "No se encontró ningún paciente"; }
                }
            });
        }

        // Carga inicial
        $(document).ready(function () {
            initSelect2();
        });

        // Carga post PostBacks de AJAX / Cambios de combos
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                initSelect2();
            });
        }
    </script>
</asp:Content>