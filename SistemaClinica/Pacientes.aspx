<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pacientes.aspx.cs" Inherits="SistemaClinica.Pacientes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row mb-4">
        <div class="col-12">
            <h2 class="text-primary"><i class="fa-solid fa-address-book me-2"></i>Administración de Pacientes</h2>
            <p class="text-muted">Panel centralizado para el registro, edición y consulta del padrón de pacientes de la clínica.</p>
        </div>
    </div>

    <div class="row mb-4">
        <div class="col-12">
            <div class="tarjeta-clinica shadow-sm">
                <h3 class="headline-sm mb-3 text-secondary">Pacientes Registrados</h3>
                
                <div class="table-responsive">
                    <asp:GridView ID="dgvPacientes" runat="server" CssClass="tabla-clinica" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="DNI" DataField="DNI" />
                            <asp:BoundField HeaderText="Apellido" DataField="Apellido" />
                            <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                            <asp:BoundField HeaderText="Email" DataField="Email" />
                            <asp:BoundField HeaderText="Teléfono" DataField="Telefono" />
                            <asp:BoundField HeaderText="Obra Social" DataField="ObraSocial" />
                            
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:Button ID="btnModificar" runat="server" Text="Editar" CssClass="btn btn-sm btn-outline-primary py-0 px-2" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-12">
            <div class="tarjeta-clinica shadow-sm">
                <h3 class="headline-sm mb-4 text-secondary"><i class="fa-solid fa-user-plus me-2"></i>Registrar / Modificar Paciente</h3>
                
                <div class="row g-3">
                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">Nombre *</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control form-control-clinica" placeholder="Ej: Juan Carlos"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">Apellido *</label>
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control form-control-clinica" placeholder="Ej: Pérez"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">DNI *</label>
                        <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control form-control-clinica" placeholder="Sin puntos, ej: 35123456"></asp:TextBox>
                    </div>

                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">Fecha de Nacimiento *</label>
                        <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control form-control-clinica" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">Sexo *</label>
                        <asp:DropDownList ID="ddlSexo" runat="server" CssClass="form-select form-control-clinica">
                            <asp:ListItem Text="Seleccionar..." Value="" />
                            <asp:ListItem Text="Masculino" Value="M" />
                            <asp:ListItem Text="Femenino" Value="F" />
                            <asp:ListItem Text="Otro" Value="O" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">Dirección</label>
                        <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control form-control-clinica" placeholder="Calle, Número, Localidad"></asp:TextBox>
                    </div>

                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">Correo Electrónico</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-clinica" TextMode="Email" placeholder="ejemplo@correo.com"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">Teléfono de Contacto</label>
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control form-control-clinica" placeholder="Ej: 1123456789"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="form-label fw-bold text-secondary">Obra Social</label>
                        <asp:TextBox ID="txtObraSocial" runat="server" CssClass="form-control form-control-clinica" placeholder="Ej: OSDE"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="form-label fw-bold text-secondary">Nro Afiliado</label>
                        <asp:TextBox ID="txtNroAfiliado" runat="server" CssClass="form-control form-control-clinica" placeholder="Ej: 1-12345-0"></asp:TextBox>
                    </div>

                    <div class="col-12 text-end mt-4">
                        <hr class="mb-4" style="border-top: 1px solid var(--cp-borde-suave);" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar Operación" CssClass="btn btn-clinica-secundario me-2 px-4" />
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Paciente" CssClass="btn btn-clinica-primario px-4" />
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>

