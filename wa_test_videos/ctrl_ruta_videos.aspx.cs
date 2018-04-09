using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_transcript
{
	public partial class ctrl_ruta_videos : System.Web.UI.Page
	{
		static Guid guid_fidusuario, guid_fidcentro;
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{

				if (!IsPostBack)
				{
					inf_user();
					load_ddl();
				}
				else
				{

				}
			}
			catch
			{
				Response.Redirect("ctrl_acceso.aspx");
			}
		}
		private void load_ddl()
		{
			using (db_transcriptEntities m_especializa = new db_transcriptEntities())
			{
				var i_especializa = (from c in m_especializa.fact_especializa
									 select c).ToList();

				ddl_especializa.DataSource = i_especializa;
				ddl_especializa.DataTextField = "desc_especializa";
				ddl_especializa.DataValueField = "id_especializa";
				ddl_especializa.DataBind();
			}
			ddl_especializa.Items.Insert(0, new ListItem("*Tipo", "0"));
			ddl_localidad.Items.Insert(0, new ListItem("*Localidad", "0"));
			ddl_nomnum.Items.Insert(0, new ListItem("*Nombre y/o número", "0"));
			ddl_sala.Items.Insert(0, new ListItem("*Sala", "0"));

		}

		protected void ddl_especializa_SelectedIndexChanged(object sender, EventArgs e)
		{
			int int_idespecializa = int.Parse(ddl_especializa.SelectedValue);


			ddl_localidad.Items.Clear();
			using (db_transcriptEntities m_especializa = new db_transcriptEntities())
			{
				var i_especializa = (from c in m_especializa.inf_juzgados
									 where c.id_especializa == int_idespecializa
									 select c).Distinct().ToList();

				ddl_localidad.DataSource = i_especializa;
				ddl_localidad.DataTextField = "localidad";
				ddl_localidad.DataValueField = "localidad";
				ddl_localidad.DataBind();
			}

			ddl_localidad.Items.Insert(0, new ListItem("*Localidad", "0"));
			ddl_nomnum.Items.Clear();
			ddl_nomnum.Items.Insert(0, new ListItem("*Nombre y/o número", "0"));
			ddl_sala.Items.Clear();
			ddl_sala.Items.Insert(0, new ListItem("*Sala", "0"));
		}

		protected void ddl_localidad_SelectedIndexChanged(object sender, EventArgs e)
		{
			string str_localidad = ddl_localidad.SelectedValue;
			ddl_nomnum.Items.Clear();
			using (db_transcriptEntities m_especializa = new db_transcriptEntities())
			{
				var i_especializa = (from c in m_especializa.inf_juzgados
									 where c.localidad == str_localidad
									 select c).Distinct().ToList();

				ddl_nomnum.DataSource = i_especializa;
				ddl_nomnum.DataTextField = "numero";
				ddl_nomnum.DataValueField = "numero";
				ddl_nomnum.DataBind();
			}


			ddl_nomnum.Items.Insert(0, new ListItem("*Nombre y/o número", "0"));
			ddl_sala.Items.Clear();
			ddl_sala.Items.Insert(0, new ListItem("*Sala", "0"));
		}

		protected void ddl_nomnum_SelectedIndexChanged(object sender, EventArgs e)
		{
			int int_idespecializa = int.Parse(ddl_especializa.SelectedValue);
			string str_localidad = ddl_localidad.SelectedValue;
			string str_numnum = ddl_nomnum.SelectedValue;
			Guid guid_idjusgado;


			using (db_transcriptEntities m_especializa = new db_transcriptEntities())
			{
				var i_especializa = (from c in m_especializa.inf_juzgados
									 join i_tu in m_especializa.inf_salas on c.id_juzgado equals i_tu.id_juzgado
									 where c.id_especializa == int_idespecializa
									 where c.localidad == str_localidad
									 where c.numero == str_numnum
									 select c).FirstOrDefault();

				guid_idjusgado = i_especializa.id_juzgado;


			}

			ddl_sala.Items.Clear();

			using (db_transcriptEntities m_especializa = new db_transcriptEntities())
			{
				var i_especializa = (from c in m_especializa.inf_salas
									 where c.id_juzgado == guid_idjusgado
									 select c).Distinct().ToList();

				ddl_sala.DataSource = i_especializa;
				ddl_sala.DataTextField = "nombre";
				ddl_sala.DataValueField = "id_sala";
				ddl_sala.DataBind();
			}
			ddl_sala.Items.Insert(0, new ListItem("*Sala", "0"));
		}

		private void inf_user()
		{
			guid_fidusuario = (Guid)(Session["ss_id_user"]);

			using (db_transcriptEntities edm_usuario = new db_transcriptEntities())
			{
				var i_usuario = (from i_u in edm_usuario.inf_usuarios
								 join i_tu in edm_usuario.fact_tipo_usuarios on i_u.id_tipo_usuario equals i_tu.id_tipo_usuario
								 join i_e in edm_usuario.inf_tribunal on i_u.id_tribunal equals i_e.id_tribunal
								 where i_u.id_usuario == guid_fidusuario
								 select new
								 {
									 i_u.nombres,
									 i_u.a_paterno,
									 i_u.a_materno,
									 i_tu.desc_tipo_usuario,
									 i_tu.id_tipo_usuario,
									 i_e.nombre,
									 i_e.id_tribunal

								 }).FirstOrDefault();

				lbl_fuser.Text = i_usuario.nombres + " " + i_usuario.a_paterno + " " + i_usuario.a_materno;
				lbl_profileuser.Text = i_usuario.desc_tipo_usuario;
				lbl_idprofileuser.Text = i_usuario.id_tipo_usuario.ToString();
				lbl_centername.Text = i_usuario.nombre;
				guid_fidcentro = i_usuario.id_tribunal;

			}

		}
		public int id_accion()
		{
			if (rb_add_routevideos.Checked)
			{
				return 1;
			}
			else if (rb_edit_routevideos.Checked)
			{
				return 2;
			}
			else if (rb_del_routevideos.Checked)
			{
				return 3;
			}
			else
			{
				return 4;
			}
		}
		protected void cmd_save_path_Click(object sender, EventArgs e)
		{

			if (ddl_especializa.SelectedValue == "0")
			{
				ddl_especializa.BackColor = Color.Yellow;
			}
			else
			{
				ddl_especializa.BackColor = Color.Transparent;
				if (ddl_localidad.SelectedValue == "0")
				{
					ddl_localidad.BackColor = Color.Yellow;
				}
				else
				{
					ddl_localidad.BackColor = Color.Transparent;
					if (ddl_nomnum.SelectedValue == "0")
					{
						ddl_nomnum.BackColor = Color.Yellow;
					}
					else
					{
						ddl_nomnum.BackColor = Color.Transparent;
						if (ddl_sala.SelectedValue == "0")
						{
							ddl_sala.BackColor = Color.Yellow;
						}
						else
						{
							ddl_sala.BackColor = Color.Transparent;
							if (string.IsNullOrEmpty(txt_user.Text))
							{

								txt_user.BackColor = Color.Yellow;
							}
							else
							{
								txt_user.BackColor = Color.Transparent;
								if (string.IsNullOrEmpty(txt_pass.Text))
								{

									txt_pass.BackColor = Color.Yellow;
								}
								else
								{
									txt_pass.BackColor = Color.Transparent;
									if (string.IsNullOrEmpty(txt_path_videos.Text))
									{

										txt_path_videos.BackColor = Color.Yellow;
									}
									else
									{
										txt_path_videos.BackColor = Color.Transparent;

										int str_count, str_fpath;

										string str_user = txt_user.Text;
										string str_pass = txt_pass.Text;
										var networkPath = txt_path_videos.Text;
										Guid guid_idsala = Guid.Parse(ddl_sala.SelectedValue);

										if (rb_add_routevideos.Checked == false & rb_edit_routevideos.Checked == false & rb_del_routevideos.Checked == false)
										{

											lblModalTitle.Text = "transcript";
											lblModalBody.Text = "Favor de seleccionar una acción";
											ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
											upModal.Update();

										}
										else
										{

											if (rb_add_routevideos.Checked == true)
											{
												using (db_transcriptEntities edm_rv = new db_transcriptEntities())
												{
													var i_rv = (from c in edm_rv.inf_ruta_videos
																where c.desc_ruta_ini == networkPath
																select c).ToList();

													if (i_rv.Count == 0)
													{
														try
														{
															using (new NetworkConnection(networkPath, new NetworkCredential(str_user, str_pass)))
															{

																using (db_transcriptEntities data_user = new db_transcriptEntities())
																{
																	var items_user = (from c in data_user.inf_ruta_videos
																					  where c.desc_ruta_ini == networkPath
																					  select c).Count();

																	str_count = items_user;
																}

																if (str_count == 0)
																{
																	using (var insert_fiscal = new db_transcriptEntities())
																	{
																		var items_fiscal = new inf_ruta_videos
																		{
																			desc_ruta_fin = @"C:\inetpub\wwwroot\videos",
																			ruta_user_ini = str_user,
																			ruta_pass_ini = str_pass,
																			desc_ruta_ini = networkPath,
																			id_usuario = guid_fidusuario,
																			id_sala = guid_idsala,
																			id_tribunal = guid_fidcentro,
																			fecha_registro = DateTime.Now

																		};
																		insert_fiscal.inf_ruta_videos.Add(items_fiscal);
																		insert_fiscal.SaveChanges();
																	}
																	using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
																	{
																		var ii_fecha_transf = (from u in edm_fecha_transf.inf_ruta_videos
																							   where u.desc_ruta_ini == networkPath
																							   select u).ToList();

																		if (ii_fecha_transf.Count == 0)
																		{

																		}
																		else
																		{
																			using (var insert_userf = new db_transcriptEntities())
																			{
																				var items_userf = new inf_ruta_videos_dep
																				{
																					id_usuario = guid_fidusuario,
																					id_ruta_videos = ii_fecha_transf[0].id_ruta_videos,
																					id_tipo_accion = id_accion(),
																					fecha_registro = DateTime.Now,

																				};
																				insert_userf.inf_ruta_videos_dep.Add(items_userf);
																				insert_userf.SaveChanges();
																			}
																		}

																	}

																	limpia_txt();




																	lblModalTitle.Text = "transcript";
																	lblModalBody.Text = "Ruta de videos guardada con éxito";
																	ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
																	upModal.Update();

																}

															}
														}
														catch
														{

															lblModalTitle.Text = "transcript";
															lblModalBody.Text = lblModalBody.Text = "Ruta y credenciales incorrectas,favor de verificar o contactar al Administrador";
															ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
															upModal.Update();
														}
													}
													else
													{
														txt_path_videos.Text = "";

														lblModalTitle.Text = "transcript";
														lblModalBody.Text = "Ruta ya existe en la base, favor de reintentar";
														ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
														upModal.Update();
													}
												}
											}
											else if (rb_edit_routevideos.Checked == true)
											{
												try
												{
													using (new NetworkConnection(networkPath, new NetworkCredential(str_user, str_pass)))
													{

														foreach (GridViewRow row in gv_routevideos.Rows)
														{
															if (row.RowType == DataControlRowType.DataRow)
															{
																CheckBox chkRow = (row.Cells[0].FindControl("chk_routevideos") as CheckBox);
																if (chkRow.Checked)
																{
																	row.BackColor = Color.YellowGreen;

																	int str_code = Convert.ToInt32(row.Cells[1].Text);
																	string str_fruta = row.Cells[2].Text;

																	using (var data_address = new db_transcriptEntities())
																	{
																		var items_address = (from c in data_address.inf_ruta_videos
																							 where c.id_ruta_videos == str_code
																							 select c).FirstOrDefault();

																		items_address.ruta_user_ini = str_user;
																		items_address.ruta_pass_ini = str_pass;
																		items_address.id_sala = guid_idsala;
																		items_address.desc_ruta_ini = networkPath;
																		data_address.SaveChanges();
																	}
																	using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
																	{
																		var ii_fecha_transf = (from u in edm_fecha_transf.inf_ruta_videos
																							   select u).ToList();

																		if (ii_fecha_transf.Count == 0)
																		{

																		}
																		else
																		{
																			using (var insert_userf = new db_transcriptEntities())
																			{
																				var items_userf = new inf_ruta_videos_dep
																				{
																					id_usuario = guid_fidusuario,
																					id_ruta_videos = ii_fecha_transf[0].id_ruta_videos,
																					id_tipo_accion = id_accion(),
																					fecha_registro = DateTime.Now,

																				};
																				insert_userf.inf_ruta_videos_dep.Add(items_userf);
																				insert_userf.SaveChanges();
																			}
																		}
																	}

																	using (db_transcriptEntities data_user = new db_transcriptEntities())
																	{
																		var inf_user = (from u in data_user.inf_ruta_videos
																						join i_s in data_user.inf_salas on u.id_sala equals i_s.id_sala
																						join i_j in data_user.inf_juzgados on i_s.id_juzgado equals i_j.id_juzgado
																						select new
																						{
																							i_j.localidad,
																							i_j.numero,
																							i_s.nombre,
																							u.id_ruta_videos,
																							u.desc_ruta_ini,
																							u.fecha_registro

																						}).ToList();

																		gv_routevideos.DataSource = inf_user;
																		gv_routevideos.DataBind();
																		gv_routevideos.Visible = true;
																	}

																	limpia_txt();

																	lblModalTitle.Text = "transcript";
																	lblModalBody.Text = "Ruta de de videos actualizada con éxito";
																	ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
																	upModal.Update();

																}
																else
																{
																	row.BackColor = Color.White;
																}
															}
														}
													}
												}
												catch
												{

													lblModalTitle.Text = "transcript";
													lblModalBody.Text = lblModalBody.Text = "Ruta y credenciales incorrectas,favor de verificar o contactar al Administrador";
													ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
													upModal.Update();
												}
											}
											else if (rb_del_routevideos.Checked == true)
											{

												foreach (GridViewRow row in gv_routevideos.Rows)
												{
													if (row.RowType == DataControlRowType.DataRow)
													{
														CheckBox chkRow = (row.Cells[0].FindControl("chk_routevideos") as CheckBox);
														if (chkRow.Checked)
														{
															row.BackColor = Color.YellowGreen;

															int str_code = Convert.ToInt32(row.Cells[1].Text);
															string str_fruta = row.Cells[2].Text;

															

															using (var data_user = new db_transcriptEntities())
															{
																var items_user = (from c in data_user.inf_ruta_videos
																				  where c.id_ruta_videos == str_code
																				  select c).FirstOrDefault();

																data_user.inf_ruta_videos.Remove(items_user);
																data_user.SaveChanges();
															}

															using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
															{
																var ii_fecha_transf = (from u in edm_fecha_transf.inf_ruta_videos
																					   select u).ToList();

																if (ii_fecha_transf.Count == 0)
																{

																}
																else
																{
																	using (var insert_userf = new db_transcriptEntities())
																	{
																		var items_userf = new inf_ruta_videos_dep
																		{
																			id_usuario = guid_fidusuario,
																			id_ruta_videos = ii_fecha_transf[0].id_ruta_videos,
																			id_tipo_accion = id_accion(),
																			fecha_registro = DateTime.Now,

																		};
																		insert_userf.inf_ruta_videos_dep.Add(items_userf);
																		insert_userf.SaveChanges();
																	}
																}
															}

															using (db_transcriptEntities data_user = new db_transcriptEntities())
															{
																var inf_user = (from u in data_user.inf_ruta_videos
																				join i_s in data_user.inf_salas on u.id_sala equals i_s.id_sala
																				join i_j in data_user.inf_juzgados on i_s.id_juzgado equals i_j.id_juzgado
																				select new
																				{
																					i_j.localidad,
																					i_j.numero,
																					i_s.nombre,
																					u.id_ruta_videos,
																					u.desc_ruta_ini,
																					u.fecha_registro

																				}).ToList();

																gv_routevideos.DataSource = inf_user;
																gv_routevideos.DataBind();
																gv_routevideos.Visible = true;
															}

															limpia_txt();

															lblModalTitle.Text = "transcript";
															lblModalBody.Text = "Ruta de de videos eliminada con éxito";
															ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
															upModal.Update();

														}
														else
														{
															row.BackColor = Color.White;
														}
													}
												}

											}
											else
											{ }
										}





									}
								}
							}
						}
					}
				}
			}
		}

		private void limpia_txt()
		{

			using (db_transcriptEntities m_especializa = new db_transcriptEntities())
			{
				var i_especializa = (from c in m_especializa.fact_especializa
									 select c).ToList();

				ddl_especializa.DataSource = i_especializa;
				ddl_especializa.DataTextField = "desc_especializa";
				ddl_especializa.DataValueField = "id_especializa";
				ddl_especializa.DataBind();
			}
			ddl_especializa.Items.Insert(0, new ListItem("*Tipo", "0"));
			ddl_localidad.Items.Clear();
			ddl_localidad.Items.Insert(0, new ListItem("*Localidad", "0"));
			ddl_nomnum.Items.Clear();
			ddl_nomnum.Items.Insert(0, new ListItem("*Nombre y/o número", "0"));
			ddl_sala.Items.Clear();
			ddl_sala.Items.Insert(0, new ListItem("*Sala", "0"));

			txt_user.Text = "";
			txt_pass.Text = "";
			txt_path_videos.Text = "";
		}

		protected void rb_add_routevideos_CheckedChanged(object sender, EventArgs e)
		{
			rb_del_routevideos.Checked = false;
			rb_edit_routevideos.Checked = false;
			div_ruta_videos.Visible = true;

			gv_routevideos.Visible = false;
			limpia_txt();
		}

		protected void rb_edit_routevideos_CheckedChanged(object sender, EventArgs e)
		{
			rb_add_routevideos.Checked = false;
			rb_del_routevideos.Checked = false;
			div_ruta_videos.Visible = true;

			limpia_txt();

			using (db_transcriptEntities data_user = new db_transcriptEntities())
			{
				var inf_user = (from u in data_user.inf_ruta_videos
								join i_s in data_user.inf_salas on u.id_sala equals i_s.id_sala
								join i_j in data_user.inf_juzgados on i_s.id_juzgado equals i_j.id_juzgado
								select new
								{
									i_j.localidad,
									i_j.numero,
									i_s.nombre,
									u.id_ruta_videos,
									u.desc_ruta_ini,
									u.fecha_registro

								}).ToList();

				if (inf_user.Count == 0)
				{
					rb_edit_routevideos.Checked = false;
					div_ruta_videos.Visible = false;
					txt_path_videos.Text = "";

					lblModalTitle.Text = "transcript";
					lblModalBody.Text = "Sin registro, favor de agregar uno";
					ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
					upModal.Update();
				}
				else
				{

					gv_routevideos.DataSource = inf_user;
					gv_routevideos.DataBind();
					gv_routevideos.Visible = true;
				}
			}
		}
		protected void rb_del_routevideos_CheckedChanged(object sender, EventArgs e)
		{
			rb_add_routevideos.Checked = false;
			rb_edit_routevideos.Checked = false;
			div_ruta_videos.Visible = true;

			limpia_txt();

			using (db_transcriptEntities data_user = new db_transcriptEntities())
			{
				var inf_user = (from u in data_user.inf_ruta_videos
								join i_s in data_user.inf_salas on u.id_sala equals i_s.id_sala
								join i_j in data_user.inf_juzgados on i_s.id_juzgado equals i_j.id_juzgado
								select new
								{
									i_j.localidad,
									i_j.numero,
									i_s.nombre,
									u.id_ruta_videos,
									u.desc_ruta_ini,
									u.fecha_registro

								}).ToList();

				if (inf_user.Count == 0)
				{
					rb_edit_routevideos.Checked = false;
					div_ruta_videos.Visible = false;
					txt_path_videos.Text = "";

					lblModalTitle.Text = "transcript";
					lblModalBody.Text = "Sin registro, favor de agregar uno";
					ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
					upModal.Update();
				}
				else
				{

					gv_routevideos.DataSource = inf_user;
					gv_routevideos.DataBind();
					gv_routevideos.Visible = true;
				}
			}
		}
		protected void chkselect_routevideos(object sender, EventArgs e)
		{

			foreach (GridViewRow row in gv_routevideos.Rows)
			{
				if (row.RowType == DataControlRowType.DataRow)
				{
					CheckBox chkRow = (row.Cells[0].FindControl("chk_routevideos") as CheckBox);
					if (chkRow.Checked)
					{
						row.BackColor = Color.YellowGreen;
						int str_code = Convert.ToInt32(row.Cells[1].Text);

						using (db_transcriptEntities data_user = new db_transcriptEntities())
						{
							var inf_user = (from u in data_user.inf_ruta_videos
											select new
											{
												u.id_ruta_videos,
												u.ruta_user_ini,
												u.id_sala,
												u.desc_ruta_ini,
												u.fecha_registro

											}).FirstOrDefault();
							txt_user.Text = inf_user.ruta_user_ini;
							txt_path_videos.Text = inf_user.desc_ruta_ini;


							using (db_transcriptEntities edm_juzgados = new db_transcriptEntities())
							{
								var i_juzgados = (from u in edm_juzgados.inf_salas
												  join i_tu in edm_juzgados.inf_juzgados on u.id_juzgado equals i_tu.id_juzgado
												  join i_e in edm_juzgados.fact_especializa on i_tu.id_especializa equals i_e.id_especializa
												  select new
												  {
													  u.id_sala,
													  i_tu.id_juzgado,
													  u.nombre,
													  i_tu.numero,
													  i_tu.localidad,
													  i_e.id_especializa,
													  i_e.desc_especializa


												  }).FirstOrDefault();
								txt_user.Text = inf_user.ruta_user_ini;
								txt_path_videos.Text = inf_user.desc_ruta_ini;

								ddl_especializa.SelectedValue = i_juzgados.id_especializa.ToString();

								int int_idespecializa = int.Parse(ddl_especializa.SelectedValue);


								ddl_localidad.Items.Clear();
								using (db_transcriptEntities m_especializa = new db_transcriptEntities())
								{
									var i_especializa = (from c in m_especializa.inf_juzgados
														 where c.id_especializa == int_idespecializa
														 select c).Distinct().ToList();

									ddl_localidad.DataSource = i_especializa;
									ddl_localidad.DataTextField = "localidad";
									ddl_localidad.DataValueField = "localidad";
									ddl_localidad.DataBind();
								}

								ddl_localidad.SelectedValue = i_juzgados.localidad.ToString();

								string str_localidad = ddl_localidad.SelectedValue;
								ddl_nomnum.Items.Clear();
								using (db_transcriptEntities m_especializa = new db_transcriptEntities())
								{
									var i_especializa = (from c in m_especializa.inf_juzgados
														 where c.localidad == str_localidad
														 select c).Distinct().ToList();

									ddl_nomnum.DataSource = i_especializa;
									ddl_nomnum.DataTextField = "numero";
									ddl_nomnum.DataValueField = "numero";
									ddl_nomnum.DataBind();
								}
								ddl_nomnum.SelectedValue = i_juzgados.numero.ToString();


								using (db_transcriptEntities m_especializa = new db_transcriptEntities())
								{
									var i_especializa = (from c in m_especializa.inf_salas
														 where c.id_juzgado == i_juzgados.id_juzgado
														 where c.id_sala == i_juzgados.id_sala
														 select c).Distinct().ToList();

									ddl_sala.DataSource = i_especializa;
									ddl_sala.DataTextField = "nombre";
									ddl_sala.DataValueField = "id_sala";
									ddl_sala.DataBind();
								}


							}

						}



					}
					else
					{
						row.BackColor = Color.White;
					}
				}
			}
		}



		protected void cmd_test_path_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(txt_user.Text))
			{

				txt_user.BackColor = Color.Yellow;
			}
			else
			{
				txt_user.BackColor = Color.Transparent;
				if (string.IsNullOrEmpty(txt_pass.Text))
				{

					txt_pass.BackColor = Color.Yellow;
				}
				else
				{
					txt_pass.BackColor = Color.Transparent;
					if (string.IsNullOrEmpty(txt_path_videos.Text))
					{

						txt_path_videos.BackColor = Color.Yellow;
					}
					else
					{
						txt_path_videos.BackColor = Color.Transparent;
						string str_user = txt_user.Text;
						string str_pass = txt_pass.Text;
						var networkPath = txt_path_videos.Text;

						try
						{
							using (new NetworkConnection(networkPath, new NetworkCredential(str_user, str_pass)))
							{

								lblModalTitle.Text = "transcript";
								lblModalBody.Text = "Ruta y credenciales correctas";
								ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
								upModal.Update();
							}
						}
						catch
						{

							lblModalTitle.Text = "transcript";
							lblModalBody.Text = lblModalBody.Text = "Ruta y credenciales incorrectas,favor de verificar o contactar al Administrador";
							ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
							upModal.Update();
						}
					}
				}
			}
		}

		public class NetworkConnection : IDisposable
		{
			#region Variables

			/// <summary>
			/// The full path of the directory.
			/// </summary>
			private readonly string _networkName;

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="NetworkConnection"/> class.
			/// </summary>
			/// <param name="networkName">
			/// The full path of the network share.
			/// </param>
			/// <param name="credentials">
			/// The credentials to use when connecting to the network share.
			/// </param>
			public NetworkConnection(string networkName, NetworkCredential credentials)
			{
				_networkName = networkName;

				var netResource = new NetResource
				{
					Scope = ResourceScope.GlobalNetwork,
					ResourceType = ResourceType.Disk,
					DisplayType = ResourceDisplaytype.Share,
					RemoteName = networkName.TrimEnd('\\')
				};

				var result = WNetAddConnection2(
					netResource, credentials.Password, credentials.UserName, 0);

				if (result != 0)
				{

					throw new Win32Exception(result);
				}
			}

			#endregion

			#region Events

			/// <summary>
			/// Occurs when this instance has been disposed.
			/// </summary>
			public event EventHandler<EventArgs> Disposed;

			#endregion

			#region Public methods

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			#endregion

			#region Protected methods

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
			protected virtual void Dispose(bool disposing)
			{
				if (disposing)
				{
					var handler = Disposed;
					if (handler != null)
						handler(this, EventArgs.Empty);
				}

				WNetCancelConnection2(_networkName, 0, true);
			}

			#endregion

			#region Private static methods

			/// <summary>
			///The WNetAddConnection2 function makes a connection to a network resource. The function can redirect a local device to the network resource.
			/// </summary>
			/// <param name="netResource">A <see cref="NetResource"/> structure that specifies details of the proposed connection, such as information about the network resource, the local device, and the network resource provider.</param>
			/// <param name="password">The password to use when connecting to the network resource.</param>
			/// <param name="username">The username to use when connecting to the network resource.</param>
			/// <param name="flags">The flags. See http://msdn.microsoft.com/en-us/library/aa385413%28VS.85%29.aspx for more information.</param>
			/// <returns></returns>
			[DllImport("mpr.dll")]
			private static extern int WNetAddConnection2(NetResource netResource,
														 string password,
														 string username,
														 int flags);

			/// <summary>
			/// The WNetCancelConnection2 function cancels an existing network connection. You can also call the function to remove remembered network connections that are not currently connected.
			/// </summary>
			/// <param name="name">Specifies the name of either the redirected local device or the remote network resource to disconnect from.</param>
			/// <param name="flags">Connection type. The following values are defined:
			/// 0: The system does not update information about the connection. If the connection was marked as persistent in the registry, the system continues to restore the connection at the next logon. If the connection was not marked as persistent, the function ignores the setting of the CONNECT_UPDATE_PROFILE flag.
			/// CONNECT_UPDATE_PROFILE: The system updates the user profile with the information that the connection is no longer a persistent one. The system will not restore this connection during subsequent logon operations. (Disconnecting resources using remote names has no effect on persistent connections.)
			/// </param>
			/// <param name="force">Specifies whether the disconnection should occur if there are open files or jobs on the connection. If this parameter is FALSE, the function fails if there are open files or jobs.</param>
			/// <returns></returns>
			[DllImport("mpr.dll")]
			private static extern int WNetCancelConnection2(string name, int flags, bool force);

			#endregion

			/// <summary>
			/// Finalizes an instance of the <see cref="NetworkConnection"/> class.
			/// Allows an <see cref="System.Object"></see> to attempt to free resources and perform other cleanup operations before the <see cref="System.Object"></see> is reclaimed by garbage collection.
			/// </summary>
			~NetworkConnection()
			{
				Dispose(false);
			}
		}

		#region Objects needed for the Win32 functions
#pragma warning disable 1591

		/// <summary>
		/// The net resource.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public class NetResource
		{
			public ResourceScope Scope;
			public ResourceType ResourceType;
			public ResourceDisplaytype DisplayType;
			public int Usage;
			public string LocalName;
			public string RemoteName;
			public string Comment;
			public string Provider;
		}

		/// <summary>
		/// The resource scope.
		/// </summary>
		public enum ResourceScope
		{
			Connected = 1,
			GlobalNetwork,
			Remembered,
			Recent,
			Context
		};

		/// <summary>
		/// The resource type.
		/// </summary>
		public enum ResourceType
		{
			Any = 0,
			Disk = 1,
			Print = 2,
			Reserved = 8,
		}





		/// <summary>
		/// The resource displaytype.
		/// </summary>
		public enum ResourceDisplaytype
		{
			Generic = 0x0,
			Domain = 0x01,
			Server = 0x02,
			Share = 0x03,
			File = 0x04,
			Group = 0x05,
			Network = 0x06,
			Root = 0x07,
			Shareadmin = 0x08,
			Directory = 0x09,
			Tree = 0x0a,
			Ndscontainer = 0x0b
		}
#pragma warning restore 1591
		#endregion
	}
}