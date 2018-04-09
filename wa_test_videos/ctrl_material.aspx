<%@ Page Title="" Language="C#" MasterPageFile="~/master_transcript.Master" AutoEventWireup="true" CodeBehind="ctrl_material.aspx.cs" Inherits="wa_transcript.ctrl_material" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="section">
        <div class="container">
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
                    <h1 class="text-center">Herramientas</h1>
                </div
            </div>
            <div class="row animated bounceInUp">
                <%-- <div class="col-md-4 text-center" id="div_videos_load" runat="server">
                        <h5>Subir</h5>
                        <asp:ImageButton ID="img_videos_load" runat="server" ImageUrl="~/img/iconos/videos@2x.png"  OnClick="img_videos_load_Click"  />
                    </div>--%>
                <div class="col-md-12 text-center" id="div_configuration" runat="server">
                    <h5>Configuración</h5>
                    <asp:ImageButton ID="img_configuration" runat="server" ImageUrl="~/img/iconos/control de centros@2x.png" OnClick="img_configuration_Click" />
                </div>
                <div class="col-md-12 text-center" id="div_conversion" runat="server">
                    <h5>Estatus Conversión</h5>
                    <asp:ImageButton ID="img_conversion" runat="server" ImageUrl="~/img/iconos/herramientas@2x.png" OnClick="img_conversion_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
