<%@ Page Title="Mis Turnos - Médico" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MisTurnos.aspx.cs" Inherits="SistemaClinica.MisTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
function abrirModalObservacion(id, notaActual) {
    document.getElementById('<%= hfTurnoId.ClientID %>').value = id;
            document.getElementById('<%= txtModalObservacion.ClientID %>').value = notaActual;
            var myModal = new bootstrap.Modal(document.getElementById('modalObservacion'));
            myModal.show();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row mb-4">
        <div class="col-12">
            <h2 class="text-success"><i class="fa-solid fa-user-doctor me-2"></i>Agenda de Turnos - Panel Médico</h2>
            <p class="text-muted">Gestione la asistencia de sus pacientes y registre las observaciones médicas de cada consulta.</p>
        </div>
    </div>

    <div class="tarjeta-clinica shadow-sm">
        <div class="table-responsive">
            <asp:GridView ID="dgvTurnosMedico" runat="server" DataKeyNames="Id" AutoGenerateColumns="false" OnRowCommand="dgvTurnosMedico_RowCommand" GridLines="None" CssClass="table table-hover align-middle mb-0">
                <Columns>
                    <asp:BoundField HeaderText="Código" DataField="Numero" ItemStyle-CssClass="fw-bold text-success" />
                    
                    <asp:TemplateField HeaderText="Paciente">
                        <ItemTemplate>
                            <%# Eval("Paciente.Apellido") %>, <%# Eval("Paciente.Nombre") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Fecha / Hora">
                        <ItemTemplate>
                            <%# Eval("Fecha", "{0:dd/MM/yyyy}") %> - <%# Eval("HoraInicio", @"{0:hh\:mm}") %> hs
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <%# Convert.ToInt32(Eval("Estado.Id")) == 3 ? "<span class='badge bg-danger'>Cancelado</span>" :
                                Convert.ToInt32(Eval("Estado.Id")) == 2 ? "<span class='badge bg-warning text-dark'>Reprogramado</span>" :
                                Convert.ToInt32(Eval("Estado.Id")) == 4 ? "<span class='badge bg-secondary'>No Asistió</span>" :
                                "<span class='badge bg-success'>Nuevo</span>" %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Evolución / Nota Médica">
                        <ItemTemplate>
                            <small class="text-muted">
                                <%# string.IsNullOrEmpty(Eval("ObservacionesMedico") as string) ? "<em>Sin registrar</em>" : Eval("ObservacionesMedico") %>
                            </small>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-end">
                        <ItemTemplate>
                            <%-- Botón Observación --%>
                            <button type="button" class="btn btn-sm btn-outline-success" 
                                    onclick="abrirModalObservacion('<%# Eval("Id") %>', '<%# HttpUtility.JavaScriptStringEncode(Eval("ObservacionesMedico") as string ?? "") %>')">
                                <i class="bi bi-journal-text"></i> Observación
                            </button>

                            <%-- Botón No Asistió --%>
                            <asp:LinkButton ID="btnNoAsistio" runat="server" CommandName="NoAsistio" 
                                CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-warning text-dark ms-1"
                                Visible='<%# Convert.ToInt32(Eval("Estado.Id")) != 4 && Convert.ToInt32(Eval("Estado.Id")) != 3 %>'>
                                <i class="bi bi-person-x"></i> No Asistió
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="modal fade" id="modalObservacion" tabindex="-1" aria-labelledby="modalObservacionLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header bg-success text-white">
            <h5 class="modal-title" id="modalObservacionLabel"><i class="bi bi-journal-plus me-2"></i>Registrar Evolución Médica</h5>
            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
              <asp:HiddenField ID="hfTurnoId" runat="server" />
              <div class="mb-3">
                <label class="form-label fw-bold">Observaciones / Diagnóstico de la consulta</label>
                <asp:TextBox ID="txtModalObservacion" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control" placeholder="Escriba los detalles de la atención aquí..."></asp:TextBox>
              </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
            <asp:Button ID="btnGuardarNota" runat="server" Text="Guardar Evolución" CssClass="btn btn-success" OnClick="btnGuardarNota_Click" />
          </div>
        </div>
      </div>
    </div>
</asp:Content>