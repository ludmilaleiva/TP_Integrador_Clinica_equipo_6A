<%@ Page Title="Gestión de Turnos de Trabajo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TurnosTrabajo.aspx.cs" Inherits="SistemaClinica.TurnosTrabajo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-md-4">
                <div class="card shadow-sm">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0"><asp:Literal ID="litTituloForm" runat="server" Text="Nuevo Turno de Trabajo" /></h5>
                    </div>
                    <div class="card-body">
                        <asp:HiddenField ID="hfIdTurnoTrabajo" runat="server" />
                        
                        <div class="mb-3">
                            <label class="form-label font-weight-bold">Nombre del Turno</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" Placeholder="Ej: Mañana, Tarde, Noche" Required="true" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label font-weight-bold">Hora Entrada</label>
                            <asp:TextBox ID="txtHoraEntrada" runat="server" TextMode="Time" CssClass="form-control" Required="true" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label font-weight-bold">Hora Salida</label>
                            <asp:TextBox ID="txtHoraSalida" runat="server" TextMode="Time" CssClass="form-control" Required="true" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label font-weight-bold">Descripción / Notas</label>
                            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" Placeholder="Opcional..." />
                        </div>
                        
                        <div class="d-grid gap-2 d-md-flex justify-content-md-end mt-4">
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary me-md-2" OnClick="btnCancelar_Click" FormNoValidate="true" />
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="card shadow-sm">
                    <div class="card-header bg-dark text-white">
                        <h5 class="mb-0">Formatos de Turnos de Trabajo</h5>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="dgvTurnosTrabajo" runat="server" CssClass="table table-hover table-bordered m-0" AutoGenerateColumns="false" OnRowCommand="dgvTurnosTrabajo_RowCommand">
                            <Columns>
                                <asp:BoundField HeaderText="ID" DataField="Id" HeaderStyle-CssClass="bg-light" />
                                <asp:BoundField HeaderText="Turno" DataField="Nombre" HeaderStyle-CssClass="bg-light" />
                                <asp:BoundField HeaderText="H. Entrada" DataField="HoraEntrada" DataFormatString="{0:hh\:mm}" HeaderStyle-CssClass="bg-light" />
                                <asp:BoundField HeaderText="H. Salida" DataField="HoraSalida" DataFormatString="{0:hh\:mm}" HeaderStyle-CssClass="bg-light" />
                                <asp:BoundField HeaderText="Descripción" DataField="Descripcion" HeaderStyle-CssClass="bg-light" />
                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-CssClass="bg-light text-center" ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEditar" runat="server" CommandName="EditarTurno" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-outline-warning">
                                            <i class="fa-solid fa-pen"></i> Editar
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