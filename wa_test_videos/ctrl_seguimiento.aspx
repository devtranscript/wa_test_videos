<%@ Page Title="" Language="C#" MasterPageFile="~/master_transcript.Master" AutoEventWireup="true" CodeBehind="ctrl_seguimiento.aspx.cs" Inherits="wa_transcript.ctrl_seguimiento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="section">
                <div class="container">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-md-1">
                                <a href="ctrl_menu.aspx">
                                    <img alt="" src="img/ico_back.png" /></a>
                            </div>
                            <div class="col-md-1">
                                <a href="ctrl_acceso.aspx">
                                    <img alt="" src="img/ico_exit.png" /></a>
                            </div>
                            <br />
                            <%--   <div class="col-md-10">
                                <p class="text-right">
                                    <asp:Label ID="lbl_welcome" runat="server" Text="Bienvenid@: "></asp:Label>
                                    <asp:Label ID="lbl_name" runat="server" Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="lbl_profilelbl" runat="server" Text="Perfil: "></asp:Label>
                                    <asp:Label ID="lbl_profile_user" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lbl_id_profile_user" runat="server" Text="" Visible="false"></asp:Label>
                                    <br />
                                    <asp:Label ID="lbl_user_centerP" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lbl_user_centerCP" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lbl_id_centerCP" runat="server" Text="" Visible="false"></asp:Label>
                                </p>
                            </div>--%>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <h2 class="text-center">
                                    <asp:Label ID="lbl_reg" runat="server" Text=""></asp:Label></h2>
                            </div>
                        </div>
                        <div class=" col-md-2">
                            <h5>
                                <asp:Label CssClass="control-label" ID="lbl_expedient" runat="server" Text="*Expediente"></asp:Label></h5>
                            <asp:TextBox CssClass="form-control" ID="txt_expedient" runat="server" placeholder="Capturar Expediente"></asp:TextBox>
                            <br />
                        </div>

                        <div class=" col-md-2">
                            <h5>
                                <asp:Label CssClass="control-label" ID="lbl_fechaini" runat="server" Text="*Fecha Inicial"></asp:Label></h5>
                            <asp:TextBox CssClass="form-control" ID="txt_dateini" runat="server" placeholder="Buscar fecha incial"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="ce_dateini" runat="server" BehaviorID="ce_dateini" TargetControlID="txt_dateini" Format="yyyy/MM/dd" />

                        </div>
                        <div class=" col-md-2">
                            <h5>
                                <asp:Label CssClass="control-label" ID="lbl_fechafin" runat="server" Text="*Fecha Final"></asp:Label></h5>
                            <asp:TextBox CssClass="form-control" ID="txt_datefin" runat="server" placeholder="Buscar fecha final"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="ce_datefin" runat="server" BehaviorID="ce_datefin" TargetControlID="txt_datefin" Format="yyyy/MM/dd" />

                        </div>
                        <div class="col-md-12">

                            <asp:Button CssClass="btn btn-success" ID="cmd_search" runat="server" Text="Buscar" OnClick="cmd_search_Click" />
                            <h5></h5>
                        </div>
                        <br />
                        <div class="col-md-12">
                            <asp:GridView CssClass="table" ID="gv_files" runat="server" AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="gv_files_RowCommand" OnRowDataBound="gv_files_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_select" runat="server" onclick="CheckOne(this)" OnCheckedChanged="chk_OnCheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="sesion" HeaderText="Expediente" SortExpression="sesion" />
                                    <asp:BoundField DataField="titulo" HeaderText="Titulo" SortExpression="titulo" />
                                    <asp:BoundField DataField="localizacion" HeaderText="Localización" SortExpression="localizacion" />
                                    <asp:BoundField DataField="tipo" HeaderText="Tipo" SortExpression="tipo" />
                                    <asp:BoundField DataField="archivo" HeaderText="Archivo" SortExpression="archivo" />
                                    <asp:BoundField DataField="duracion" HeaderText="Duración" SortExpression="duracion" DataFormatString="{00:00,0}" />
                                    <%--<asp:BoundField DataField="fecha_registro" HeaderText="Fecha Registro" SortExpression="fecha_registro" DataFormatString="{0:hh:mm tt}" HtmlEncode="false" />--%>
                                    <asp:BoundField DataField="fecha_registro" HeaderText="Fecha Registro" SortExpression="fecha_registro" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" />
                                    <asp:BoundField DataField="desc_estatus_material" HeaderText="Estatus" SortExpression="desc_estatus_material" />
                                    <%--<asp:BoundField DataField="id_control" HeaderText="ID Control" SortExpression="id_control" />--%>
                                    <asp:TemplateField HeaderText="Ver">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn btn-success" ID="cmd_ver" runat="server" Text="Ver" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />


                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row" id="div_panel_ie" runat="server">
                <div class="col-md-12 text-center">
                    <asp:Panel ID="Panel1" runat="server" CssClass=" img-thumbnail"></asp:Panel>
                </div>
            </div>
            <div class="row" id="div_panel" runat="server">
                <div class="col-md-12 text-center">
                    <video id="play_video" runat="server" visible="false" class="img-thumbnail" controls="controls">
                        <source src="demo" type='video/mp4;codecs="avc1.42E01E, mp4a.40.2"' />
                    </video>
                    <iframe id="iframe_pdf" src="" width="600" height="500" runat="server" visible="false"></iframe>
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

