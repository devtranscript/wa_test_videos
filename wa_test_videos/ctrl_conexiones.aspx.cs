using JAVS.Networking;
using JAVS.Networking.Services;
using JAVS.Publishing.JVL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_transcript
{
    public partial class ctrl_conexiones : System.Web.UI.Page
    {
        private WebDataProvider provider;
        private SystemConnection connection;
        private EventManagerSessionControl sessionControl;

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

            using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
            {
                var i_fecha_transf = (from c in edm_fecha_transf.inf_credenciales
                                      select c).ToList();

                if (i_fecha_transf.Count != 0)
                {
                    rb_add_credentials.Visible = false;
                }
            }

        }
        public int id_accion()
        {
            if (rb_add_credentials.Checked)
            {
                return 1;
            }
            else if (rb_edit_credentials.Checked)
            {
                return 2;
            }
            //else if (.Checked)
            //{
            //    return 3;
            //}
            else
            {
                return 4;
            }
        }
        protected void chkselect_credentials(object sender, EventArgs e)
        {

            foreach (GridViewRow row in gv_credentials.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chk_select") as CheckBox);
                    if (chkRow.Checked)
                    {
                        row.BackColor = Color.YellowGreen;
                        int str_code = Convert.ToInt32(row.Cells[1].Text);

                        using (db_transcriptEntities edm_conexion = new db_transcriptEntities())
                        {
                            var i_conexion = (from u in edm_conexion.inf_credenciales
                                              where u.id_credenciales == str_code
                                              select new
                                              {
                                                  u.id_credenciales,
                                                  u.ip,
                                                  u.usuario,
                                                  u.clave,
                                                  u.fecha_registro

                                              }).FirstOrDefault();

                            txt_ip.Text = i_conexion.ip;
                            txt_user.Text = i_conexion.usuario;
                            txt_pass.Text = i_conexion.clave;
                        }
                    }
                    else
                    {
                        row.BackColor = Color.White;
                        chkRow.Checked = false;
                    }
                }
            }
        }
        protected void cmd_save_credentials_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_ip.Text))
            {

                txt_ip.BackColor = Color.Yellow;
            }
            else
            {
                txt_ip.BackColor = Color.Transparent;
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

                        string str_ip = txt_ip.Text;
                        string str_user = txt_user.Text;
                        string str_pass = txt_pass.Text;

                        int str_count;


                        using (db_transcriptEntities edm_conexion = new db_transcriptEntities())
                        {
                            var i_conexion = (from c in edm_conexion.inf_credenciales
                                              select c).Count();

                            str_count = i_conexion;
                        }

                        if (str_count == 0)
                        {
                            using (var edm_conexion = new db_transcriptEntities())
                            {
                                var ii_conexion = new inf_credenciales
                                {
                                    ip = str_ip,
                                    usuario = str_user,
                                    clave = str_pass,
                                    fecha_registro = DateTime.Now

                                };
                                edm_conexion.inf_credenciales.Add(ii_conexion);
                                edm_conexion.SaveChanges();
                            }
                            using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
                            {
                                var ii_fecha_transf = (from u in edm_fecha_transf.inf_credenciales
                                                       select u).ToList();

                                if (ii_fecha_transf.Count == 0)
                                {

                                }
                                else
                                {
                                    using (var insert_user = new db_transcriptEntities())
                                    {
                                        var items_user = new inf_credenciales_dep
                                        {
                                            id_usuario = guid_fidusuario,
                                            id_credenciales = ii_fecha_transf[0].id_credenciales,
                                            id_tipo_accion = id_accion(),
                                            fecha_registro = DateTime.Now,

                                        };
                                        insert_user.inf_credenciales_dep.Add(items_user);
                                        insert_user.SaveChanges();
                                    }
                                }
                            }
                            clean_txt();
                            using (db_transcriptEntities edm_conexion = new db_transcriptEntities())
                            {
                                var ii_conexion = (from u in edm_conexion.inf_credenciales
                                                   where u.ip == str_ip
                                                   select new
                                                   {
                                                       u.id_credenciales,
                                                       u.ip,
                                                       u.usuario,
                                                       u.fecha_registro

                                                   }).ToList();

                                gv_credentialsf.DataSource = ii_conexion;
                                gv_credentialsf.DataBind();
                                gv_credentialsf.Visible = true;
                            }

                            rb_add_credentials.Visible = false;

                            lblModalTitle.Text = "transcript";
                            lblModalBody.Text = "Datos de conexión, guardados con éxito";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                            upModal.Update();

                        }
                        else if (rb_edit_credentials.Checked)
                        {
                            foreach (GridViewRow row in gv_credentials.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    CheckBox chkRow = (row.Cells[0].FindControl("chk_select") as CheckBox);
                                    if (chkRow.Checked)
                                    {
                                        int str_code = Convert.ToInt32(row.Cells[1].Text);

                                        using (var edm_conexion = new db_transcriptEntities())
                                        {
                                            var i_conexion = (from c in edm_conexion.inf_credenciales
                                                              where c.id_credenciales == str_code
                                                              select c).FirstOrDefault();

                                            i_conexion.ip = str_ip;
                                            i_conexion.usuario = str_user;
                                            i_conexion.clave = str_pass;
                                            edm_conexion.SaveChanges();
                                        }
                                        using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
                                        {
                                            var ii_fecha_transf = (from u in edm_fecha_transf.inf_credenciales
                                                                   select u).ToList();

                                            if (ii_fecha_transf.Count == 0)
                                            {

                                            }
                                            else
                                            {
                                                using (var insert_user = new db_transcriptEntities())
                                                {
                                                    var items_user = new inf_credenciales_dep
                                                    {
                                                        id_usuario = guid_fidusuario,
                                                        id_credenciales = ii_fecha_transf[0].id_credenciales,
                                                        id_tipo_accion = id_accion(),
                                                        fecha_registro = DateTime.Now,

                                                    };
                                                    insert_user.inf_credenciales_dep.Add(items_user);
                                                    insert_user.SaveChanges();
                                                }
                                            }
                                        }
                                        clean_txt();
                                        lblModalTitle.Text = "transcript";
                                        lblModalBody.Text = "Datos de conexión, actualizados con éxito";
                                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                                        upModal.Update();

                                        using (db_transcriptEntities edm_conexion = new db_transcriptEntities())
                                        {
                                            var i_conexion = (from u in edm_conexion.inf_credenciales
                                                              select new
                                                              {
                                                                  u.id_credenciales,
                                                                  u.ip,
                                                                  u.usuario,
                                                                  u.fecha_registro

                                                              }).ToList();

                                            gv_credentials.DataSource = i_conexion;
                                            gv_credentials.DataBind();
                                            gv_credentials.Visible = true;
                                        }
                                    }
                                }
                            }

                        }
                        //else if (rb_del_credentials.Checked)
                        //{
                        //    foreach (GridViewRow row in gv_credentials.Rows)
                        //    {
                        //        if (row.RowType == DataControlRowType.DataRow)
                        //        {
                        //            CheckBox chkRow = (row.Cells[0].FindControl("chk_select") as CheckBox);
                        //            if (chkRow.Checked)
                        //            {
                        //                int str_code = Convert.ToInt32(row.Cells[1].Text);


                        //                using (var data_user = new db_transcriptEntities())
                        //                {
                        //                    var items_user = (from c in data_user.inf_credenciales
                        //                                      where c.id_credenciales == str_code
                        //                                      select c).FirstOrDefault();

                        //                    data_user.inf_credenciales.Remove(items_user);
                        //                    data_user.SaveChanges();
                        //                }

                        //                clean_txt();
                        //                lblModalTitle.Text = "transcript";
                        //                lblModalBody.Text = "Datos de conexión, actualizado con éxito";
                        //                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                        //                upModal.Update();

                        //                using (db_transcriptEntities edm_conexion = new db_transcriptEntities())
                        //                {
                        //                    var i_conexion = (from u in edm_conexion.inf_credenciales
                        //                                      select new
                        //                                      {
                        //                                          u.id_credenciales,
                        //                                          u.ip,
                        //                                          u.usuario,
                        //                                          u.fecha_registro

                        //                                      }).ToList();

                        //                    gv_credentials.DataSource = i_conexion;
                        //                    gv_credentials.DataBind();
                        //                    gv_credentials.Visible = true;
                        //                }
                        //            }
                        //        }
                        //    }

                        //}
                    }
                }
            }


        }
        protected void gv_usuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void rb_add_credentials_CheckedChanged(object sender, EventArgs e)
        {
            rb_edit_credentials.Checked = false;
            //rb_del_credentials.Checked = false;
            clean_txt();
            div_infcredentials.Visible = true;
        }
        protected void rb_edit_credentials_CheckedChanged(object sender, EventArgs e)
        {
            rb_add_credentials.Checked = false;
            //rb_del_credentials.Checked = false;
            div_infcredentials.Visible = true;

            gv_credentialsf.Visible = false;
            clean_txt();

            using (db_transcriptEntities edm_conexion = new db_transcriptEntities())
            {
                var i_conexion = (from u in edm_conexion.inf_credenciales
                                  select new
                                  {
                                      u.id_credenciales,
                                      u.ip,
                                      u.usuario,
                                      u.clave,
                                      u.fecha_registro

                                  }).ToList();

                if (i_conexion.Count == 0)
                {
                    rb_edit_credentials.Checked = false;
                    lblModalTitle.Text = "transcript";
                    lblModalBody.Text = "Sin registro, favor de agregar uno";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                    upModal.Update();
                }
                else
                {
                    gv_credentials.DataSource = i_conexion;
                    gv_credentials.DataBind();
                    gv_credentials.Visible = true;
                }
            }
        }
        private void clean_txt()
        {
            txt_ip.Text = "";
            txt_user.Text = "";
            txt_pass.Text = "";
        }
        protected void gv_credentials_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            this.provider = new WebDataProvider();
            this.provider.Port = 80;
            this.provider.Username = "m";
            this.provider.Password = "m";

            provider.Host = txt_ip.Text;
            provider.Username = txt_user.Text;
            provider.Password = txt_pass.Text;

            GetLocations();

            ////First create a SystemConnection.
            //this.connection = new SystemConnection();

            ////To receive session notifications, subscribe to the session protocol service.
            //this.connection.ServicesSubscribed.Add(JAVS.Protocol.Session.Service.Number);

            ////Events may fire from a background thread. When handling events within a System.Windows.Forms GUI, 
            ////set the sync object to a control to avoid cross-threading issues.
            //this.connection.SyncObject = this;

            ////The Registered event will be triggered after a successful connection and the recorder
            ////is ready to handle commands.
            //this.connection.Registered += new EventHandler(connection_Registered);
            //this.connection.Disconnected += new EventHandler<DisconnectedEventArgs>(connection_Disconnected);

            ////Create an EventManagerSessionControl which provides session protocol commands and events.
            //this.sessionControl = new EventManagerSessionControl(this.connection);
            //this.sessionControl.SessionChanged += new EventHandler(sessionControl_SessionChanged);
            //this.sessionControl.RecordingChanged += new EventHandler(sessionControl_RecordingChanged);

            ////Connect to the recorder.
            //if (this.connection.Connect(this.textBoxIPAddress.Text, 6110))
            //{
            //    this.buttonDisconnect.Enabled = true;
            //}
            //else
            //{
            //    MessageBox.Show("Failed to establish connection");
            //}
        }

		//protected void rb_del_credentials_CheckedChanged(object sender, EventArgs e)
		//{
		//    rb_edit_credentials.Checked = false;
		//    rb_add_credentials.Checked = false;

		//    gv_credentialsf.Visible = false;
		//    clean_txt();

		//    using (db_transcriptEntities edm_conexion = new db_transcriptEntities())
		//    {
		//        var i_conexion = (from u in edm_conexion.inf_credenciales
		//                          select new
		//                          {
		//                              u.id_credenciales,
		//                              u.ip,
		//                              u.usuario,
		//                              u.clave,
		//                              u.fecha_registro

		//                          }).ToList();

		//        if (i_conexion.Count == 0)
		//        {
		//            rb_edit_credentials.Checked = false;

		//        }
		//        else
		//        {
		//            gv_credentials.DataSource = i_conexion;
		//            gv_credentials.DataBind();
		//            gv_credentials.Visible = true;
		//        }
		//    }
		//}
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
		private void GetLocations()
        {
            string str_ip = txt_ip.Text;
            string str_usuario = txt_user.Text;
            string str_pass = txt_pass.Text;

            try
            {
                Dns.GetHostEntry(str_ip); //using System.Net;
                lblModalTitle.Text = "transcript";
                lblModalBody.Text = "IP Activa";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                upModal.Update();
            }
            catch (Exception e)
            {
                lblModalTitle.Text = "transcript";
                lblModalBody.Text = "Falla de conexión, favor de reitentar o contactar al administrador";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                upModal.Update();
            }
        }
    }
}