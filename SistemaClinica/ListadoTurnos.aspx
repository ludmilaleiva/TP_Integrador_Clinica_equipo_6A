<%@ Page Title="Monitoreo de Turnos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListadoTurnos.aspx.cs" Inherits="SistemaClinica.ListadoTurnos" %>

<%-- 1. Este bloque maneja la cabecera (scripts/estilos). DEJA ESTO VACÍO por ahora --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<%-- 2. Este bloque es el CUERPO PRINCIPAL de la página. ACÁ VA LA GRILLA --%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="row mb-4">
        <div class="col-12">
            <h2 class="text-primary"><i class="fa-solid fa-clock me-2"></i>Monitorear Grilla de Turnos</h2>
            <p class="text-muted">Visualizá, filtrá y administrá las citas médicas programadas en tiempo real.</p>
        </div>
    </div>

    <div class="card shadow-sm mb-4 bg-light border-0">
        <div class="card-body">
            <div class="row g-3 align-items-end">
                <div class="col-md-4">
                    <label class="form-label small fw-bold text-secondary">Filtrar por Paciente / Médico</label>
                    <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control" placeholder="Escriba apellido o código de turno..." AutoPostBack="true" OnTextChanged="txtFiltro_TextChanged"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnBuscar" runat="server" Text="Filtrar" CssClass="btn btn-primary px-4" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiarFiltro" runat="server" Text="Resetear" CssClass="btn btn-outline-secondary ms-2" OnClick="btnLimpiarFiltro_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="tarjeta-clinica shadow-sm">
        <div class="table-responsive">
            <asp:GridView ID="dgvTurnos" runat="server" CssClass="table table-hover align-middle mb-0" 
                AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="dgvTurnos_RowCommand" GridLines="None">
                <Columns>
                    <asp:BoundField HeaderText="Código" DataField="Numero" ItemStyle-CssClass="fw-bold text-primary" />
                    
                    <asp:TemplateField HeaderText="Paciente">
                        <ItemTemplate>
                            <%# Eval("Paciente.Apellido") %>, <%# Eval("Paciente.Nombre") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Especialidad">
                        <ItemTemplate>
                            <span class="badge bg-light text-dark border"><%# Eval("Especialidad.Nombre") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Médico">
                        <ItemTemplate>
                            Dr/a. <%# Eval("Medico.Apellido") %>, <%# Eval("Medico.Nombre") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Fecha / Hora">
                        <ItemTemplate>
                            <i class="fa-regular fa-calendar text-muted me-1"></i>
                            <%# Eval("Fecha", "{0:dd/MM/yyyy}") %> 
                            <span class="text-muted ms-2"><i class="fa-regular fa-clock me-1"></i><%# Eval("HoraInicio", @"{0:hh\:mm}") %> hs</span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-end">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-outline-danger btn-sm" 
                                CommandName="CancelarTurno" CommandArgument='<%# Eval("Id") %>'
                                OnClientClick="return confirm('¿Está seguro de que desea cancelar este turno?');">
                                <i class="fa-solid fa-ban me-1"></i> Cancelar
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>