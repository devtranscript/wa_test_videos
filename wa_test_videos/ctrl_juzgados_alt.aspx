<%@ Page Title="" Language="C#" MasterPageFile="~/master_transcript.Master" AutoEventWireup="true" CodeBehind="ctrl_juzgados_alt.aspx.cs" Inherits="wa_transcript.ctrl_juzgados_alt" %>

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
								<h2 class="text-center">
									<asp:Label ID="lbl_reg" runat="server" Text=""></asp:Label></h2>
							</div>
							<div class="col-md-6 text-right">
								<h3 class="text-center">Control de Juzgados</h3>
								<br />
								<asp:RadioButton CssClass="radio-inline" ID="rb_agregar_juzgado" runat="server" Text="Agregar" AutoPostBack="True" OnCheckedChanged="rb_agregar_juzgado_CheckedChanged" />
								<asp:RadioButton CssClass="radio-inline" ID="rb_editar_juzgado" runat="server" Text="Editar" AutoPostBack="True" OnCheckedChanged="rb_editar_juzgado_CheckedChanged" />
								<asp:RadioButton CssClass="radio-inline" ID="rb_eliminar_juzgado" runat="server" Text="Eliminar" AutoPostBack="True" OnCheckedChanged="rb_eliminar_juzgado_CheckedChanged" />
							</div>
							<div class="col-md-6 text-right">
								<h3 class="text-center">Control de Salas</h3>
								<br />
								<asp:RadioButton CssClass="radio-inline" ID="rb_agregar_sala" runat="server" Text="Agregar" AutoPostBack="True" OnCheckedChanged="rb_agregar_sala_CheckedChanged" Visible="false" />
								<asp:RadioButton CssClass="radio-inline" ID="rb_editar_sala" runat="server" Text="Editar" AutoPostBack="True" OnCheckedChanged="rb_editar_sala_CheckedChanged" Visible="false" />
								<asp:RadioButton CssClass="radio-inline" ID="rb_eliminar_sala" runat="server" Text="Eliminar" AutoPostBack="True" OnCheckedChanged="rb_eliminar_sala_CheckedChanged" Visible="false" />
							</div>
							<div class="col-md-6">
								<div class="form-group">
									<div class="input-group">
										<asp:TextBox CssClass="form-control" ID="txt_buscar_juzgado" runat="server" placeholder="Buscar" Visible="false"></asp:TextBox>
										<span class="input-group-btn">
											<asp:Button CssClass="btn btn-success" ID="btn_buscar_juzgado" runat="server" Text="Ir" Visible="false" OnClick="btn_buscar_juzgado_Click" />
										</span>
									</div>
								</div>
							</div>
						</div>
						<div class="row">
							<div class="col-md-6">
								<asp:GridView CssClass="table" ID="gv_juzgado" runat="server" AutoGenerateColumns="False" AllowPaging="true" OnPageIndexChanging="gv_juzgado_PageIndexChanging" PageSize="5">
									<Columns>
										<asp:TemplateField>
											<ItemTemplate>
												<asp:CheckBox ID="chk_juzgado" runat="server" onclick="CheckOne(this)" OnCheckedChanged="chk_juzgado_CheckedChanged" AutoPostBack="true" />
											</ItemTemplate>
										</asp:TemplateField>

										<asp:BoundField DataField="desc_especializa" HeaderText="Tipo" SortExpression="desc_especializa" />
										<asp:BoundField DataField="localidad" HeaderText="Localidad" SortExpression="localidad" />
										<asp:BoundField DataField="numero" HeaderText="Nombre y/o número" SortExpression="numero" />
										<asp:BoundField DataField="fecha_registro" HeaderText="Registro" SortExpression="fecha_registro" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" />
										<asp:BoundField DataField="codigo_juzgado" HeaderText="ID Base" SortExpression="codigo_juzgado" Visible="true" />
									</Columns>
									<PagerSettings Mode="NextPrevious" NextPageImageUrl="~/img/next_arrow.png" PreviousPageImageUrl="~/img/back_arrow.png" />
								</asp:GridView>
								<br />
							</div>
							<div class="col-md-6 ">
								<asp:GridView CssClass="table" ID="gv_sala" runat="server" AutoGenerateColumns="False" AllowPaging="true" OnPageIndexChanging="gv_sala_PageIndexChanging" PageSize="5">
									<Columns>
										<asp:TemplateField>
											<ItemTemplate>
												<asp:CheckBox ID="chk_sala" runat="server" onclick="CheckOne(this)" OnCheckedChanged="chk_sala_CheckedChanged" AutoPostBack="true" />
											</ItemTemplate>
										</asp:TemplateField>
										<asp:BoundField DataField="nombre" HeaderText="Nombre y/o número" SortExpression="nombre" />
										<asp:BoundField DataField="ip" HeaderText="IP" SortExpression="ip" />
										<asp:BoundField DataField="fecha_registro" HeaderText="Registro" SortExpression="fecha_registro" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" />
										<asp:BoundField DataField="codigo_sala" HeaderText="ID Base" SortExpression="codigo_sala" Visible="true" />
									</Columns>
									<PagerSettings Mode="NextPrevious" NextPageImageUrl="~/img/next_arrow.png" PreviousPageImageUrl="~/img/back_arrow.png" />
								</asp:GridView>
								<br />
							</div>

						</div>
						<div class="row">
				
							<div class="col-md-3">
								<asp:DropDownList CssClass="form-control" ID="ddl_especializa" runat="server" ToolTip="*Tipo"></asp:DropDownList>
								<br />
								<asp:TextBox CssClass="form-control" ID="txt_localidad" runat="server" placeholder="*Localidad"></asp:TextBox>
								<br />
								<asp:TextBox CssClass="form-control" ID="txt_numero" runat="server" placeholder="*Nombre y/o número"></asp:TextBox>
								<br />
								<asp:TextBox CssClass="form-control" ID="txt_callenum" runat="server" placeholder="*Calle y número"></asp:TextBox>
								<br />
							</div>
							<div class="col-md-3">
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
								<div class="col-md-15 text-right">
									<asp:Button CssClass="btn" ID="btn_guardar_juzgado" runat="server" Text="Guardar" OnClick="btn_guardar_juzgado_Click" />
								</div>
							</div>
							<div class="col-md-3">
								<asp:TextBox CssClass="form-control" ID="txt_sala" runat="server" placeholder="*Nombre y/o número"></asp:TextBox>
								<br />
							</div>
							<div class="col-md-2">
								<asp:TextBox CssClass="form-control" ID="txt_ip" runat="server" placeholder="*IP"></asp:TextBox>
								<asp:RegularExpressionValidator ID="IpValidator" ControlToValidate="txt_ip" runat="server" ValidationExpression="^((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){3}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})$"
									ErrorMessage="Formato de IP Invalido" CssClass="comments" Display="None"></asp:RegularExpressionValidator>
								<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqE" TargetControlID="IpValidator" HighlightCssClass="highlight" />
								<br />
							</div>
							<div class="col-md-1 text-right">
								<asp:Button CssClass="btn" ID="btn_guarda_sala" runat="server" OnClick="btn_guarda_sala_Click" Visible="false" />
							</div>
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
