<%@ Page Title="" Language="C#" MasterPageFile="~/master_transcript.Master" AutoEventWireup="true" CodeBehind="ctrl_tribunal.aspx.cs" Inherits="wa_transcript.ctrl_tribunal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="section">
                <div class="container">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-md-1">
                                <a href="ctrl_menu_tribunal.aspx">
                                    <img alt="" src="img/ico_back.png" /></a>
                            </div>
                            <div class="col-md-1">
                                <a href="ctrl_acceso.aspx">
                                    <img alt="" src="img/ico_exit.png" /></a>
                            </div>
                            <br />
                            <div class="col-md-10">
                                <p class="text-right">
                                    <asp:Label ID="lbl_welcome" runat="server" Text="Bienvenid@: "></asp:Label>
                                    <asp:Label ID="lbl_fuser" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lbl_idfuser" runat="server" Text="" Visible="false"></asp:Label>
                                    <br />
                                    <asp:Label ID="lbl_profile" runat="server" Text="Perfil: "></asp:Label>
                                    <asp:Label ID="lbl_profileuser" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lbl_idprofileuser" runat="server" Text="" Visible="false"></asp:Label>
                                    <br />
                                    <asp:Label ID="lbl_center" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lbl_centername" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lbl_idcenter" runat="server" Text="" Visible="false"></asp:Label>
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <h3 class="text-center">Actualizar datos de Tribunal</h3>
                                <br />
                            </div>
                            <div class="col-md-12 text-right">
                                <asp:CheckBox CssClass="checkbox-inline" ID="chkb_editar" runat="server" AutoPostBack="true" Text="Editar" OnCheckedChanged="chkb_editar_CheckedChanged" />
                            </div>
                            <div class="col-md-4">
                                <asp:Image CssClass="center-block img-responsive" ID="Image1" runat="server" ImageUrl="~/img/tribunal.png" Width="64" Height="64" />
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox CssClass="form-control" ID="txt_tribunal" runat="server" placeholder="*Nombre de Tribunal"></asp:TextBox>
                                <br />
                                <asp:TextBox CssClass="form-control" ID="txt_telefono" runat="server" placeholder="Teléfono"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="mee_telefono" runat="server" TargetControlID="txt_telefono" Mask="(52) 999.99.99.99.99.99 ext 99999" />
                                <br />
                                <asp:TextBox CssClass="form-control" ID="txt_email" runat="server" placeholder="e-mail"></asp:TextBox>
                                <br />
                                <asp:TextBox CssClass="form-control" ID="txt_callenum" runat="server" placeholder="*Calle y número"></asp:TextBox>
                                <br />
                            </div>
                            <div class="col-md-4">
                                <div class="input-group">
                                    <asp:TextBox CssClass="form-control" ID="txt_cp" runat="server" placeholder="*Código Postal" MaxLength="5"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="mee_cp" runat="server" TargetControlID="txt_cp" Mask="99999" />
                                    <span class="input-group-btn">
                                        <asp:Button CssClass="btn" ID="btn_cp" runat="server" Text="validar" OnClick="btn_cp_Click" />
                                    </span>
                                </div>
                                <br />
                                <asp:DropDownList CssClass="form-control" ID="ddl_colonia" runat="server" ToolTip="*Colonia"></asp:DropDownList>
                                <br />
                                <asp:TextBox CssClass="form-control" ID="txt_municipio" runat="server" placeholder="*Municipio" Enabled="false"></asp:TextBox>
                                <br />
                                <asp:TextBox CssClass="form-control" ID="txt_estado" runat="server" placeholder="*Estado" Enabled="false"></asp:TextBox>
                                <br />
                            </div>
                            <div class="col-md-12 text-right">
                                <asp:Button CssClass="btn" ID="btn_guardar" runat="server" Text="Guardar" OnClick="btn_guardar_Click" />
                            </div>
                        </div>
                        <hr />
                    </div>
                </div>
            </div>
            </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
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
