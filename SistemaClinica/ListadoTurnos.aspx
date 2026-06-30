<%@ Page Title="Monitoreo de Turnos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListadoTurnos.aspx.cs" Inherits="SistemaClinica.ListadoTurnos" %>

<%-- 1. Este bloque maneja la cabecera (scripts/estilos).ESTO VACÍO por ahora --%>
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
            <asp:GridView ID="dgvTurnos" runat="server" DataKeyNames="Id" AutoGenerateColumns="false" OnRowCommand="dgvTurnos_RowCommand" GridLines="None" CssClass="table table-hover align-middle mb-0">
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

                    <%-- Columna de Estado con Badge Dinámico --%>
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <%# Eval("Estado.id").ToString() == "3" ? "<span class='badge bg-danger'>Cancelado</span>" :
                                Eval("Estado.id").ToString() == "2" ? "<span class='badge bg-warning text-dark'>Reprogramado</span>" :
                                Eval("Estado.id").ToString() == "4" ? "<span class='badge bg-secondary'>No Asistió</span>":
                                 Eval("Estado.id").ToString() == "1" ? "<span class='badge bg-success'>Nuevo</span>":
                                 "<span class='badge bg-success'>Sin estado</span>"
                                 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-end">
                        <ItemTemplate>
                          <%-- Botón Cancelar (Se oculta/deshabilita si ya está Cancelado o Cerrado) --%>
                        <asp:LinkButton ID="btnCancelar" runat="server" CommandName="Cancelar" 
                            CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-outline-danger"
                            Visible='<%# Eval("Estado.Nombre").ToString() != "Cancelado" && Eval("Estado.Nombre").ToString() != "Cerrado"  && Eval("Estado.id").ToString() != "4"  %>'>
                            <i class="bi bi-x-circle"></i> Cancelar
                        </asp:LinkButton>

                        <%-- Botón Reprogramar --%>
                        <asp:LinkButton ID="btnReprogramar" runat="server" CommandName="Reprogramar" 
                            CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-outline-warning"
                            Visible='<%# Eval("Estado.Nombre").ToString() != "Cancelado" && Eval("Estado.Nombre").ToString() != "Cerrado"  && Eval("Estado.id").ToString() != "4"  %>'>
                            <i class="bi bi-calendar-event"></i> Reprogramar
                        </asp:LinkButton>

                       <asp:LinkButton ID="btnNoAsistio" runat="server" CommandName="NoAsistio" 
                            CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-warning text-dark"
                            Visible='<%# Eval("Estado.Nombre").ToString() != "Cancelado"  && Eval("Estado.id").ToString() != "4"  && Eval("Estado.Nombre").ToString() != "Reprogramado" && Eval("Estado.Nombre").ToString() != "Cerrado" && Eval("Estado.Nombre").ToString() != "No Asistió" %>'>
                            <i class="bi bi-person-x"></i> No Asistió
                        </asp:LinkButton>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>