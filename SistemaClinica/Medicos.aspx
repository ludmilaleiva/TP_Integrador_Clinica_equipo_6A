<%@ Page Title="Administración de Médicos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Medicos.aspx.cs" Inherits="SistemaClinica.Medicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="row mb-4">
        <div class="col-12">
            <h2 class="text-primary"><i class="fa-solid fa-user-doctor me-2"></i>Gestión de Médicos</h2>
            <p class="text-muted">Administrá el staff de profesionales médicos del establecimiento y asigná sus especialidades.</p>
        </div>
    </div>

    <div class="row">
        <div class="col-md-4 mb-4">
            <div class="card shadow-sm border-0">
                <div class="card-header bg-primary text-white fw-bold">
                    <i class="fa-solid fa-stethoscope me-1"></i> <asp:Literal ID="litTituloForm" runat="server" Text="Registrar / Modificar Médico"></asp:Literal>
                </div>
                <div class="card-body">
                    <asp:HiddenField ID="hfIdMedico" runat="server" />
                    
                    <div class="mb-3">
                        <label class="form-label small fw-bold text-secondary">Nombre</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej: María"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label small fw-bold text-secondary">Apellido</label>
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Ej: González"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label small fw-bold text-secondary">Matrícula</label>
                        <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" placeholder="Ej: MN-45678"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label small fw-bold text-secondary">Especialidad</label>
                        <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>

                    <div class="d-grid gap-2">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Médico" CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Limpiar / Nuevo" CssClass="btn btn-outline-secondary" OnClick="btnCancelar_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-8 mb-4">
            <div class="card shadow-sm border-0">
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView ID="dgvMedicos" runat="server" CssClass="table table-hover align-middle mb-0" 
                            AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="dgvMedicos_RowCommand" GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="ID" DataField="Id" ItemStyle-CssClass="fw-bold text-muted small" />
                                <asp:BoundField HeaderText="Apellido" DataField="Apellido" ItemStyle-CssClass="fw-bold" />
                                <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                                <asp:BoundField HeaderText="Matrícula" DataField="Matricula" />
                                <asp:BoundField HeaderText="Email" DataField="Email" />
                                <asp:TemplateField HeaderText="Especialidad">
                                <ItemTemplate>
                                <span class="badge bg-info text-dark">
                                <%# ((List<dominio.Especialidad>)Eval("Especialidades")).Count > 0 ? ((List<dominio.Especialidad>)Eval("Especialidades"))[0].Nombre : "Sin Asignar" %>
                                 </span>
                                    </ItemTemplate>
                                   </asp:TemplateField>
                               
                                
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-end pe-3">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEditar" runat="server" CssClass="btn btn-outline-primary btn-sm me-1"
                                            CommandName="EditarMedico" CommandArgument='<%# Eval("Id") %>'>
                                            <i class="fa-solid fa-pen-to-square"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>