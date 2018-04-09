<%@ Page Title="" Language="C#" MasterPageFile="~/master_transcript.Master" AutoEventWireup="true" CodeBehind="ctrl_acceso.aspx.cs" Inherits="wa_transcript.ctrl_acceso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="section">
        <div class="container">

            <br>
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-4 text-center img-thumbnail">
                    <br />
                    <h3>Control de Acceso</h3>
                    <img src="img/business-and-office/png/301-browser-1.png" class="img-responsive center-block" width="64">
                    <br />
                    <div class="form-group">
                        <asp:TextBox CssClass="form-control" ID="txt_code_user" runat="server" TabIndex="1" placeholder="*Capturar Usuario"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:TextBox CssClass="form-control" ID="txt_password" runat="server" TabIndex="3" placeholder="*Capturar Contraseña" TextMode="Password"></asp:TextBox>
                    </div>
                    <br />
                    <asp:LinkButton CssClass="text-left" ID="lkb_registro" runat="server" Visible="false" Text="Registrar" OnClick="lkb_registro_Click"></asp:LinkButton>
                    <br />
                    <br />
                    <asp:Button CssClass="btn btn-block" ID="cmd_login" runat="server" Text="Entrar" TabIndex="4" OnClick="cmd_login_Click" />
                    <br />
                </div>
                <div class="col-md-4"></div>
            </div>

        </div>
    </div>

    <!-- Bootstrap Modal Dialog -->
    <div class="modal fade" id="myModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">
                                <asp:Label ID="lblModalTitle" runat="server" Text=""></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <asp:Label ID="lblModalBody" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-success" data-dismiss="modal" aria-hidden="true">Ok</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
