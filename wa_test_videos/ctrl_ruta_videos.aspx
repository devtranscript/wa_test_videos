<%@ Page Title="" Language="C#" MasterPageFile="~/master_transcript.Master" AutoEventWireup="true" CodeBehind="ctrl_ruta_videos.aspx.cs" Inherits="wa_transcript.ctrl_ruta_videos" %>

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
								<a href="ctrl_configuracion.aspx">
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
							<div class="col-md-12 text-left">
								<div class="panel-group" runat="server" id="pg_routevideos">
									<div class="panel panel-default">
										<div class="panel-heading">
											<h4 class="text-center">Ruta de Carpeta de Videos</h4>
											<asp:RadioButton CssClass="radio-inline text-right" ID="rb_add_routevideos" runat="server" Text="Agregar" AutoPostBack="True" OnCheckedChanged="rb_add_routevideos_CheckedChanged" />
											<asp:RadioButton CssClass="radio-inline text-right" ID="rb_edit_routevideos" runat="server" Text="Editar" AutoPostBack="True" OnCheckedChanged="rb_edit_routevideos_CheckedChanged" />
											<asp:RadioButton CssClass="radio-inline text-right" ID="rb_del_routevideos" runat="server" Text="Eliminar" AutoPostBack="True" OnCheckedChanged="rb_del_routevideos_CheckedChanged" />
										</div>
										<div class="panel-body">
											<div class="row" id="div_ruta_videos" runat="server" visible="false">
												<div class="col-md-12">
													<asp:GridView CssClass="table" ID="gv_routevideos" runat="server" AutoGenerateColumns="False" AllowPaging="true">
														<Columns>
															<asp:TemplateField HeaderText="Seleccionar">
																<ItemTemplate>
																	<asp:CheckBox ID="chk_routevideos" runat="server" onclick="CheckOne(this)" OnCheckedChanged="chkselect_routevideos" AutoPostBack="true" />
																</ItemTemplate>
															</asp:TemplateField>
															<asp:BoundField DataField="id_ruta_videos" HeaderText="ID" SortExpression="id_ruta_videos" Visible="true" />
															<asp:BoundField DataField="localidad" HeaderText="Localidad" SortExpression="localidad" />
															<asp:BoundField DataField="numero" HeaderText="Nombre y/o Número" SortExpression="numero" />
															<asp:BoundField DataField="nombre" HeaderText="Sala" SortExpression="nombre" />
															<asp:BoundField DataField="desc_ruta_ini" HeaderText="Directorio o ruta de videos" SortExpression="desc_ruta_ini" />
															<asp:BoundField DataField="fecha_registro" HeaderText="Fecha de Registro" SortExpression="fecha_registro" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />

														</Columns>
													</asp:GridView>
													<br />
												</div>
												<h4 class="text-center">Asignación a Juzgado y Sala</h4>
												<div class="col-md-3">

													<asp:DropDownList CssClass="form-control" ID="ddl_especializa" runat="server" ToolTip="*Tipo" OnSelectedIndexChanged="ddl_especializa_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
													<br />
												</div>
												<div class="col-md-3">
													<asp:DropDownList CssClass="form-control" ID="ddl_localidad" runat="server" ToolTip="*Localidad" OnSelectedIndexChanged="ddl_localidad_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
													<br />
												</div>
												<div class="col-md-3">
													<asp:DropDownList CssClass="form-control" ID="ddl_nomnum" runat="server" ToolTip="*Nombre y/o número" OnSelectedIndexChanged="ddl_nomnum_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
													<br />
												</div>
												<div class="col-md-3">
													<asp:DropDownList CssClass="form-control" ID="ddl_sala" runat="server" ToolTip="*Sala"></asp:DropDownList>
													<br />
												</div>
												<div class="col-md-2">
													<asp:TextBox CssClass="form-control" ID="txt_user" runat="server" placeholder="Usuario"></asp:TextBox>
													<br />
												</div>
												<div class="col-md-2">
													<asp:TextBox CssClass="form-control" ID="txt_pass" runat="server" placeholder="Contraseña" TextMode="Password"></asp:TextBox>
													<br />
												</div>
												<div class="col-md-4">
													<div class="form-group">

														<div class="input-group">
															<asp:TextBox CssClass="form-control" ID="txt_path_videos" runat="server" placeholder="Ruta de Carpeta"></asp:TextBox>
															<span class="input-group-btn">
																<asp:Button CssClass="btn btn-success" ID="cmd_test_path" runat="server" Text="Validar" OnClick="cmd_test_path_Click" />
															</span>
														</div>

													</div>
												</div>
												<div class="col-md-12 text-right">
													<asp:Button CssClass="btn btn-success" ID="cmd_save_path" runat="server" Text="Guardar" OnClick="cmd_save_path_Click" />
												</div>

											</div>
										</div>
									</div>
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
