using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_transcript
{
    public partial class ctrl_juzgados_alt : System.Web.UI.Page
    {
        static Guid guid_fidusuario, guid_fidcentro, guid_ftribunal, guid_idjuzgado, guid_nsala, guid_njuzgado;
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
            ddl_colonia.Items.Insert(0, new ListItem("*Colonia", "0"));

        }
        public int id_accion_juzgado()
        {
            if (rb_agregar_juzgado.Checked)
            {
                return 1;
            }
            else if (rb_editar_juzgado.Checked)
            {
                return 2;
            }
            else if (rb_eliminar_juzgado.Checked)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        public int id_accion_salas()
        {
            if (rb_agregar_sala.Checked)
            {
                return 1;
            }
            else if (rb_editar_sala.Checked)
            {
                return 2;
            }
            else if (rb_eliminar_sala.Checked)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        protected void btn_guarda_sala_Click(object sender, EventArgs e)
        {
            if (rb_agregar_sala.Checked == false & rb_editar_sala.Checked == false & rb_eliminar_sala.Checked == false)
            {

                lblModalTitle.Text = "transcript";
                lblModalBody.Text = "Favor de seleccionar una acción";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                upModal.Update();

            }
            else
            {
                if (string.IsNullOrEmpty(txt_sala.Text))
                {

                    txt_sala.BackColor = Color.Yellow;
                }
                else
                {
                    txt_sala.BackColor = Color.Transparent;

                    if (string.IsNullOrEmpty(txt_ip.Text))
                    {

                        txt_ip.BackColor = Color.Yellow;
                    }
                    else
                    {
                        txt_ip.BackColor = Color.Transparent;

                        guarda_sala();
                    }
                }
            }
        }

        private void guarda_sala()
        {
            guid_nsala = Guid.NewGuid();
            string str_ip = txt_ip.Text;
            string str_nsala = txt_sala.Text;

            if (rb_agregar_sala.Checked)
            {
                foreach (GridViewRow row in gv_juzgado.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox fchkRow = (row.Cells[1].FindControl("chk_juzgado") as CheckBox);
                        if (fchkRow.Checked)
                        {
                            int int_codeuser = int.Parse(row.Cells[5].Text);
                            Guid codeuser;
                            using (var data_user = new db_transcriptEntities())
                            {
                                var items_user = (from c in data_user.inf_juzgados
                                                  where c.codigo_juzgado == int_codeuser
                                                  select c).FirstOrDefault();

                                codeuser = items_user.id_juzgado;

                            }
                            int int_especializa = Convert.ToInt32(ddl_especializa.SelectedValue);
                            string str_localidad = txt_localidad.Text.ToUpper();
                            string str_numero = txt_numero.Text;


                            using (var data_user = new db_transcriptEntities())
                            {
                                var items_user = (from c in data_user.inf_juzgados
                                                  join i_tu in data_user.inf_salas on c.id_juzgado equals i_tu.id_juzgado
                                                  where c.id_especializa == int_especializa
                                                  where c.localidad == str_localidad
                                                  where c.numero == str_numero
                                                  where i_tu.nombre == str_nsala
                                                  select c).ToList();

                                if (items_user.Count == 0)
                                {
                                    using (var edm_conexion = new db_transcriptEntities())
                                    {
                                        var ii_conexion = new inf_salas
                                        {
                                            id_sala = guid_nsala,
                                            id_estatus = 1,
                                            nombre = str_nsala,
                                            ip = str_ip,
                                            id_juzgado = codeuser,
                                            fecha_registro = DateTime.Now

                                        };
                                        edm_conexion.inf_salas.Add(ii_conexion);
                                        edm_conexion.SaveChanges();
                                    }
                                    using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
                                    {
                                        var ii_fecha_transf = (from u in edm_fecha_transf.inf_salas
                                                               where u.id_sala == guid_nsala
                                                               select u).ToList();

                                        if (ii_fecha_transf.Count == 0)
                                        {

                                        }
                                        else
                                        {
                                            using (var insert_userf = new db_transcriptEntities())
                                            {
                                                var items_userf = new inf_salas_dep
                                                {
                                                    id_usuario = guid_fidusuario,
                                                    id_sala = ii_fecha_transf[0].id_sala,
                                                    id_tipo_accion = id_accion_salas(),
                                                    fecha_registro = DateTime.Now,

                                                };
                                                insert_userf.inf_salas_dep.Add(items_userf);
                                                insert_userf.SaveChanges();
                                            }
                                        }
                                    }
                                    rb_agregar_sala.Checked = false;

                                    using (db_transcriptEntities edm_fjuzgado = new db_transcriptEntities())
                                    {
                                        var i_fjuzgado = (from i_u in edm_fjuzgado.inf_salas
                                                          where i_u.id_juzgado == guid_idjuzgado
                                                          where i_u.id_estatus == 1
                                                          select new
                                                          {
                                                              i_u.codigo_sala,
                                                              i_u.nombre,
                                                              i_u.ip,
                                                              i_u.fecha_registro,

                                                          }).ToList();

                                        gv_sala.DataSource = i_fjuzgado;
                                        gv_sala.DataBind();
                                        gv_sala.Visible = false;
                                    }

                                    foreach (GridViewRow row_s in gv_juzgado.Rows)
                                    {
                                        if (row_s.RowType == DataControlRowType.DataRow)
                                        {
                                            CheckBox chkRow = (row_s.Cells[0].FindControl("chk_juzgado") as CheckBox);
                                            if (chkRow.Checked)
                                            {
                                                row_s.BackColor = Color.White;
                                                chkRow.Checked = false;
                                            }
                                            else
                                            {
                                                row_s.BackColor = Color.White;
                                            }
                                        }
                                    }

                                    txt_sala.Text = "";
                                    txt_ip.Text = "";
                                    limpiar_textbox_juzgado();

                                    lblModalTitle.Text = "transcript";
                                    lblModalBody.Text = "Datos de sala agregada con éxito, seleccionar nuevamente juzgado para mostrar las salas";
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                                    upModal.Update();
                                }
                                else
                                {
                                    lblModalTitle.Text = "transcript";
                                    lblModalBody.Text = "Datos de sala ya existe, favor de agregar otra";
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                                    upModal.Update();
                                }
                            }


                        }
                    }
                }
            }

            else if (rb_editar_sala.Checked)
            {
                foreach (GridViewRow row in gv_sala.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox fchkRow = (row.Cells[0].FindControl("chk_sala") as CheckBox);
                        if (fchkRow.Checked)
                        {
                            row.BackColor = Color.YellowGreen;
                            int int_codeuser = int.Parse(row.Cells[4].Text);
                            Guid codeuser;
                            using (var data_user = new db_transcriptEntities())
                            {
                                var items_user = (from c in data_user.inf_salas
                                                  where c.codigo_sala == int_codeuser
                                                  select c).FirstOrDefault();

                                codeuser = items_user.id_sala;

                            }

                            using (var data_user = new db_transcriptEntities())
                            {
                                var items_user = (from c in data_user.inf_salas
                                                  where c.id_sala == codeuser
                                                  select c).FirstOrDefault();

                                items_user.nombre = str_nsala;
                                data_user.SaveChanges();
                            }
                            using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
                            {
                                var ii_fecha_transf = (from u in edm_fecha_transf.inf_salas
                                                       where u.id_sala == codeuser
                                                       select u).ToList();

                                if (ii_fecha_transf.Count == 0)
                                {

                                }
                                else
                                {
                                    using (var insert_userf = new db_transcriptEntities())
                                    {
                                        var items_userf = new inf_salas_dep
                                        {
                                            id_usuario = guid_fidusuario,
                                            id_sala = ii_fecha_transf[0].id_sala,
                                            id_tipo_accion = id_accion_salas(),
                                            fecha_registro = DateTime.Now,

                                        };
                                        insert_userf.inf_salas_dep.Add(items_userf);
                                        insert_userf.SaveChanges();
                                    }
                                }
                            }

                            rb_editar_sala.Checked = false;

                            using (db_transcriptEntities data_user = new db_transcriptEntities())
                            {
                                var inf_user = (from i_u in data_user.inf_salas
                                                where i_u.id_juzgado == guid_idjuzgado
                                                where i_u.id_estatus == 1
                                                select new
                                                {
                                                    i_u.codigo_sala,
                                                    i_u.nombre,
                                                    i_u.ip,
                                                    i_u.fecha_registro,

                                                }).ToList();

                                gv_sala.DataSource = inf_user;
                                gv_sala.DataBind();
                                gv_sala.Visible = true;
                            }

                            txt_sala.Text = "";
                            txt_ip.Text = "";


                            lblModalTitle.Text = "transcript";
                            lblModalBody.Text = "Datos de sala actualizados con éxito";
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
            else if (rb_eliminar_sala.Checked)
            {
                foreach (GridViewRow row in gv_sala.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox fchkRow = (row.Cells[0].FindControl("chk_sala") as CheckBox);
                        if (fchkRow.Checked)
                        {
                            row.BackColor = Color.YellowGreen;
                            int int_codeuser = int.Parse(row.Cells[4].Text);
                            Guid codeuser;

                            using (var data_user = new db_transcriptEntities())
                            {
                                var items_user = (from c in data_user.inf_salas
                                                  where c.codigo_sala == int_codeuser
                                                  select c).FirstOrDefault();

                                codeuser = items_user.id_sala;

                            }

                            var elimna_sala = new inf_salas { id_sala = codeuser };

    
                            using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
                            {
                                var ii_fecha_transf = (from u in edm_fecha_transf.inf_salas
                                                       where u.id_sala == codeuser
                                                       select u).ToList();

                                if (ii_fecha_transf.Count == 0)
                                {

                                }
                                else
                                {
                                    using (var insert_userf = new db_transcriptEntities())
                                    {
                                        var items_userf = new inf_salas_dep
                                        {
                                            id_usuario = guid_fidusuario,
                                            id_sala = ii_fecha_transf[0].id_sala,
                                            id_tipo_accion = id_accion_salas(),
                                            fecha_registro = DateTime.Now,

                                        };
                                        insert_userf.inf_salas_dep.Add(items_userf);
                                        insert_userf.SaveChanges();
                                    }
                                }
                            }
                            using (var data_user = new db_transcriptEntities())
                            {
                                var items_user = (from c in data_user.inf_salas
                                                  where c.id_sala == codeuser
                                                  select c).FirstOrDefault();

                                data_user.inf_salas.Remove(items_user);
                                data_user.SaveChanges();
                            }
                            using (db_transcriptEntities data_user = new db_transcriptEntities())
                            {
                                var inf_user = (from i_u in data_user.inf_salas
                                                where i_u.id_juzgado == guid_idjuzgado
                                                where i_u.id_estatus == 1
                                                select new
                                                {
                                                    i_u.codigo_sala,
                                                    i_u.nombre,
                                                    i_u.ip,
                                                    i_u.fecha_registro,

                                                }).ToList();

                                gv_sala.DataSource = inf_user;
                                gv_sala.DataBind();
                                gv_sala.Visible = true;
                            }

                            rb_eliminar_sala.Checked = false;
                            txt_sala.Text = "";
                            txt_ip.Text = "";

                            lblModalTitle.Text = "transcript";
                            lblModalBody.Text = "Datos de sala eliminado con éxito";
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

        private void guardar_juzgado()
        {
            guid_njuzgado = Guid.NewGuid();
            int int_especializa = Convert.ToInt32(ddl_especializa.SelectedValue);
            string str_localidad = txt_localidad.Text.ToUpper();
            string str_numero = txt_numero.Text;
            string str_callenum = txt_callenum.Text.ToUpper();
            string str_cp = txt_cp.Text;
            int int_colony = Convert.ToInt32(ddl_colonia.SelectedValue);
          


            if (rb_agregar_juzgado.Checked)
            {

                using (var data_user = new db_transcriptEntities())
                {
                    var items_user = (from c in data_user.inf_juzgados
                                      where c.id_especializa == int_especializa
                                      where c.localidad == str_localidad
                                      where c.numero == str_numero
                                      select c).ToList();

                    if (items_user.Count == 0)
                    {
                        using (var m_empresa = new db_transcriptEntities())
                        {
                            var i_empresa = new inf_juzgados
                            {
                                id_juzgado = guid_njuzgado,
                                id_especializa = int_especializa,
                                id_estatus = 1,
                                localidad = str_localidad,
                                numero = str_numero,
                                calle_num = str_callenum,
                                cp = str_cp,
                                id_asenta_cpcons = int_colony,
                                fecha_registro = DateTime.Now,
                                id_tribunal = guid_fidcentro
                            };

                            m_empresa.inf_juzgados.Add(i_empresa);
                            m_empresa.SaveChanges();
                        }

                        using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
                        {
                            var ii_fecha_transf = (from u in edm_fecha_transf.inf_juzgados
                                                   where u.id_juzgado == guid_njuzgado
                                                   select u).ToList();

                            if (ii_fecha_transf.Count == 0)
                            {

                            }
                            else
                            {
                                using (var insert_userf = new db_transcriptEntities())
                                {
                                    var items_userf = new inf_juzgados_dep
                                    {
                                        id_usuario = guid_fidusuario,
                                        id_juzgado = ii_fecha_transf[0].id_juzgado,
                                        id_tipo_accion = id_accion_juzgado(),
                                        fecha_registro = DateTime.Now,

                                    };
                                    insert_userf.inf_juzgados_dep.Add(items_userf);
                                    insert_userf.SaveChanges();
                                }
                            }
                        }


                        if (string.IsNullOrEmpty(txt_sala.Text) || string.IsNullOrEmpty(txt_ip.Text))
                        {


                        }
                        else
                        {
                            txt_sala.BackColor = Color.Transparent;
                            txt_ip.BackColor = Color.Transparent;

                            guid_nsala = Guid.NewGuid();
                            string str_ip = txt_ip.Text;
                            string str_nsala = txt_sala.Text;

                            using (var edm_conexion = new db_transcriptEntities())
                            {
                                var ii_conexion = new inf_salas
                                {
                                    id_sala = guid_nsala,
                                    id_estatus = 1,
                                    nombre = str_nsala,
                                    ip = str_ip,
                                    id_juzgado = guid_njuzgado,
                                    fecha_registro = DateTime.Now

                                };
                                edm_conexion.inf_salas.Add(ii_conexion);
                                edm_conexion.SaveChanges();
                            }
                            txt_sala.Text = "";
                            txt_ip.Text = "";
                        }

                        using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
                        {
                            var ii_fecha_transf = (from u in edm_fecha_transf.inf_salas
                                                   where u.id_sala == guid_nsala
                                                   select u).ToList();

                            if (ii_fecha_transf.Count == 0)
                            {

                            }
                            else
                            {
                                using (var insert_userf = new db_transcriptEntities())
                                {
                                    var items_userf = new inf_salas_dep
                                    {
                                        id_usuario = guid_fidusuario,
                                        id_sala = ii_fecha_transf[0].id_sala,
                                        id_tipo_accion = id_accion_juzgado(),
                                        fecha_registro = DateTime.Now,

                                    };
                                    insert_userf.inf_salas_dep.Add(items_userf);
                                    insert_userf.SaveChanges();
                                }
                            }
                        }

                        limpiar_textbox_juzgado();
                        rb_agregar_juzgado.Checked = false;

                        lblModalTitle.Text = "transcript";
                        lblModalBody.Text = "Datos de juzgado agregado con éxito";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                        upModal.Update();

                    }

                    else
                    {
                        lblModalTitle.Text = "transcript";
                        lblModalBody.Text = "Datos de juzgado ya existe, favor de agregar otro";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                        upModal.Update();
                    }

                }

            }
            else if (rb_editar_juzgado.Checked)
            {
                foreach (GridViewRow row in gv_juzgado.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chk_juzgado") as CheckBox);
                        if (chkRow.Checked)
                        {
                            row.BackColor = Color.YellowGreen;
                            int int_codeuser = int.Parse(row.Cells[5].Text);
                            Guid codeuser;

                            using (var data_user = new db_transcriptEntities())
                            {
                                var items_user = (from c in data_user.inf_juzgados
                                                  where c.codigo_juzgado == int_codeuser
                                                  select c).FirstOrDefault();

                                items_user.id_especializa = int_especializa;
                                items_user.localidad = str_localidad;
                                items_user.numero = str_numero;

                                items_user.calle_num = str_callenum;
                                items_user.cp = str_cp;
                                items_user.id_asenta_cpcons = int_colony;

                                data_user.SaveChanges();
                            }
                            using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
                            {
                                var ii_fecha_transf = (from u in edm_fecha_transf.inf_juzgados
                                                       where u.codigo_juzgado == int_codeuser
                                                       select u).ToList();

                                if (ii_fecha_transf.Count == 0)
                                {

                                }
                                else
                                {
                                    using (var insert_userf = new db_transcriptEntities())
                                    {
                                        var items_userf = new inf_juzgados_dep
                                        {
                                            id_usuario = guid_fidusuario,
                                            id_juzgado = ii_fecha_transf[0].id_juzgado,
                                            id_tipo_accion = id_accion_juzgado(),
                                            fecha_registro = DateTime.Now,

                                        };
                                        insert_userf.inf_juzgados_dep.Add(items_userf);
                                        insert_userf.SaveChanges();
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(txt_sala.Text) || string.IsNullOrEmpty(txt_ip.Text))
                            {


                            }
                            else
                            {
                                txt_sala.BackColor = Color.Transparent;
                                txt_ip.BackColor = Color.Transparent;
                                guarda_sala();
                            }

                            using (db_transcriptEntities data_user = new db_transcriptEntities())
                            {
                                var inf_user = (from i_u in data_user.inf_juzgados
                                                join i_e in data_user.fact_especializa on i_u.id_especializa equals i_e.id_especializa
                                                where i_u.id_tribunal == guid_fidcentro
                                                where i_u.id_estatus == 1
                                                select new
                                                {
                                                    i_u.codigo_juzgado,
                                                    i_e.desc_especializa,
                                                    i_u.localidad,
                                                    i_u.numero,
                                                    i_u.fecha_registro,

                                                }).ToList();

                                gv_juzgado.DataSource = inf_user;
                                gv_juzgado.DataBind();
                                gv_juzgado.Visible = true;
                            }

                            gv_sala.Visible = false;

                            limpiar_textbox_juzgado();
                            rb_editar_juzgado.Checked = false;

                            lblModalTitle.Text = "transcript";
                            lblModalBody.Text = "Datos de juzgado actualizados con éxito";
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
            else if (rb_eliminar_juzgado.Checked)
            {
                foreach (GridViewRow row in gv_juzgado.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chk_juzgado") as CheckBox);
                        if (chkRow.Checked)
                        {
                            row.BackColor = Color.YellowGreen;
                            int int_codeuser = int.Parse(row.Cells[5].Text);
                            Guid codeuser;

                            using (var data_user = new db_transcriptEntities())
                            {
                                var items_user = (from c in data_user.inf_juzgados
                                                  where c.codigo_juzgado == int_codeuser
                                                  select c).FirstOrDefault();

                                codeuser = items_user.id_juzgado;

                            }
                            using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
                            {
                                var ii_fecha_transf = (from u in edm_fecha_transf.inf_juzgados
                                                       where u.id_juzgado == codeuser
                                                       select u).ToList();

                                if (ii_fecha_transf.Count == 0)
                                {

                                }
                                else
                                {
                                    using (var insert_userf = new db_transcriptEntities())
                                    {
                                        var items_userf = new inf_juzgados_dep
                                        {
                                            id_usuario = guid_fidusuario,
                                            id_juzgado = ii_fecha_transf[0].id_juzgado,
                                            id_tipo_accion = id_accion_juzgado(),
                                            fecha_registro = DateTime.Now,

                                        };
                                        insert_userf.inf_juzgados_dep.Add(items_userf);
                                        insert_userf.SaveChanges();
                                    }
                                }
                            }
                            using (var data_user = new db_transcriptEntities())
                            {
                                var items_user = (from c in data_user.inf_salas
                                                  where c.id_juzgado == codeuser
                                                  select c).ToList();
                                items_user.ForEach(c => data_user.inf_salas.Remove(c));
                                data_user.SaveChanges();
                            }


                            using (var data_user = new db_transcriptEntities())
                            {
                                var items_user = (from c in data_user.inf_juzgados
                                                  where c.id_juzgado == codeuser
                                                  select c).FirstOrDefault();

                                data_user.inf_juzgados.Remove(items_user);
                                data_user.SaveChanges();
                            }
           
                            using (db_transcriptEntities data_user = new db_transcriptEntities())
                            {
                                var inf_user = (from i_u in data_user.inf_juzgados
                                                join i_e in data_user.fact_especializa on i_u.id_especializa equals i_e.id_especializa
                                                where i_u.id_tribunal == guid_fidcentro
                                                where i_u.id_estatus == 1
                                                select new
                                                {
                                                    i_u.codigo_juzgado,
                                                    i_e.desc_especializa,
                                                    i_u.localidad,
                                                    i_u.numero,
                                                    i_u.fecha_registro,

                                                }).ToList();

                                gv_juzgado.DataSource = inf_user;
                                gv_juzgado.DataBind();
                                gv_juzgado.Visible = true;
                            }

                            gv_sala.Visible = false;

                            limpiar_textbox_juzgado();
                            rb_eliminar_juzgado.Checked = false;

                            lblModalTitle.Text = "transcript";
                            lblModalBody.Text = "Datos de juzgado eliminado con éxito";
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

        protected void rb_agregar_juzgado_CheckedChanged(object sender, EventArgs e)
        {
            rb_editar_juzgado.Checked = false;
            rb_eliminar_juzgado.Checked = false;

            btn_guarda_sala.Visible = false;


            rb_agregar_sala.Visible = false;
            rb_editar_sala.Visible = false;
            rb_eliminar_sala.Visible = false;

            txt_buscar_juzgado.Visible = false;
            btn_buscar_juzgado.Visible = false;
            gv_juzgado.Visible = false;
            rb_agregar_sala.Checked = false;
            rb_editar_sala.Checked = false;
            rb_eliminar_sala.Checked = false;
            txt_sala.Text = "";
            txt_ip.Text = "";

            gv_sala.Visible = false;
        }

        private void limpiar_textbox_juzgado()
        {
            ddl_especializa.SelectedValue = "0";
            txt_localidad.Text = "";
            txt_numero.Text = "";
            txt_callenum.Text = "";
            txt_cp.Text = "";
            ddl_colonia.Items.Clear();
            ddl_colonia.Items.Insert(0, new ListItem("*Colonia", "0"));
            ddl_colonia.SelectedValue = "0";
            txt_municipio.Text = "";
            txt_estado.Text = "";

        }

        protected void rb_editar_juzgado_CheckedChanged(object sender, EventArgs e)
        {
            rb_agregar_juzgado.Checked = false;
            rb_eliminar_juzgado.Checked = false;

            limpiar_textbox_juzgado();
            txt_buscar_juzgado.Visible = false;
            btn_buscar_juzgado.Visible = false;

            rb_agregar_sala.Visible = true;
            rb_editar_sala.Visible = true;
            rb_eliminar_sala.Visible = true;

            using (db_transcriptEntities data_user = new db_transcriptEntities())
            {
                var inf_user = (from i_u in data_user.inf_juzgados
                                join i_e in data_user.fact_especializa on i_u.id_especializa equals i_e.id_especializa
                                where i_u.id_tribunal == guid_fidcentro
                                where i_u.id_estatus == 1
                                select new
                                {
                                    i_u.codigo_juzgado,
                                    i_e.desc_especializa,
                                    i_u.localidad,
                                    i_u.numero,
                                    i_u.fecha_registro,

                                }).ToList();

                gv_juzgado.DataSource = inf_user;
                gv_juzgado.DataBind();
                gv_juzgado.Visible = true;
            }
        }

        protected void rb_eliminar_juzgado_CheckedChanged(object sender, EventArgs e)
        {
            rb_agregar_juzgado.Checked = false;
            rb_editar_juzgado.Checked = false;
            txt_buscar_juzgado.Visible = false;
            btn_buscar_juzgado.Visible = false;


            rb_agregar_sala.Visible = true;
            rb_editar_sala.Visible = true;
            rb_eliminar_sala.Visible = true;

            limpiar_textbox_juzgado();

            using (db_transcriptEntities data_user = new db_transcriptEntities())
            {
                var inf_user = (from i_u in data_user.inf_juzgados
                                join i_e in data_user.fact_especializa on i_u.id_especializa equals i_e.id_especializa
                                where i_u.id_tribunal == guid_fidcentro
                                where i_u.id_estatus == 1
                                select new
                                {
                                    i_u.codigo_juzgado,
                                    i_e.desc_especializa,
                                    i_u.localidad,
                                    i_u.numero,
                                    i_u.fecha_registro,

                                }).ToList();

                gv_juzgado.DataSource = inf_user;
                gv_juzgado.DataBind();
                gv_juzgado.Visible = true;
            }
        }

        protected void chk_juzgado_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gv_juzgado.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chk_juzgado") as CheckBox);
                    if (chkRow.Checked)
                    {
                        row.BackColor = Color.YellowGreen;
                        int int_code = int.Parse(row.Cells[5].Text);


                        using (db_transcriptEntities m_tribunal = new db_transcriptEntities())
                        {
                            var i_tribunal = (from u in m_tribunal.inf_juzgados
                                              where u.codigo_juzgado == int_code
                                              select u).FirstOrDefault();

                            ddl_especializa.SelectedValue = i_tribunal.id_especializa.ToString();
                            txt_localidad.Text = i_tribunal.localidad;
                            txt_numero.Text = i_tribunal.numero;
                            txt_callenum.Text = i_tribunal.calle_num;
                            txt_cp.Text = i_tribunal.cp;
                            guid_idjuzgado = i_tribunal.id_juzgado;

                            using (db_transcriptEntities db_sepomex = new db_transcriptEntities())
                            {
                                var tbl_sepomex = (from c in db_sepomex.inf_sepomex
                                                   where c.d_codigo == i_tribunal.cp.ToString()
                                                   select c).ToList();

                                ddl_colonia.DataSource = tbl_sepomex;
                                ddl_colonia.DataTextField = "d_asenta";
                                ddl_colonia.DataValueField = "id_asenta_cpcons";
                                ddl_colonia.DataBind();

                                ddl_colonia.SelectedValue = i_tribunal.id_asenta_cpcons.ToString();
                                txt_municipio.Text = tbl_sepomex[0].D_mnpio;
                                txt_estado.Text = tbl_sepomex[0].d_estado;
                            }

                            using (db_transcriptEntities data_user = new db_transcriptEntities())
                            {
                                var inf_user = (from i_u in data_user.inf_salas
                                                where i_u.id_juzgado == guid_idjuzgado
                                                where i_u.id_estatus == 1
                                                select new
                                                {
                                                    i_u.codigo_sala,
                                                    i_u.nombre,
                                                    i_u.ip,
                                                    i_u.fecha_registro,

                                                }).ToList();

                                gv_sala.DataSource = inf_user;
                                gv_sala.DataBind();
                                gv_sala.Visible = true;
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

        protected void rb_agregar_sala_CheckedChanged(object sender, EventArgs e)
        {
            rb_editar_sala.Checked = false;
            rb_eliminar_sala.Checked = false;

            btn_guarda_sala.Visible = true;
            btn_guarda_sala.Text = "Guardar";
        }

        protected void rb_editar_sala_CheckedChanged(object sender, EventArgs e)
        {
            rb_agregar_sala.Checked = false;
            rb_eliminar_sala.Checked = false;

            btn_guarda_sala.Visible = true;
            btn_guarda_sala.Text = "Guardar";
        }

        protected void rb_eliminar_sala_CheckedChanged(object sender, EventArgs e)
        {
            rb_editar_sala.Checked = false;
            rb_agregar_sala.Checked = false;

            btn_guarda_sala.Visible = true;
            btn_guarda_sala.Text = "Guardar";
        }

        protected void btn_buscar_juzgado_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_buscar_juzgado.Text))
            {

                txt_buscar_juzgado.BackColor = Color.Yellow;
            }
            else
            {
                txt_buscar_juzgado.BackColor = Color.Transparent;
                string str_userb = txt_buscar_juzgado.Text;

                using (db_transcriptEntities data_user = new db_transcriptEntities())
                {
                    var inf_user = (from i_u in data_user.inf_juzgados
                                    join i_e in data_user.fact_especializa on i_u.id_especializa equals i_e.id_especializa
                                    where i_u.localidad.Contains(str_userb)
                                    where i_u.id_tribunal == guid_fidcentro
                                    where i_u.id_estatus == 1
                                    select new
                                    {
                                        i_u.codigo_juzgado,
                                        i_u.localidad,
                                        i_u.numero,
                                        i_e.desc_especializa,
                                        i_u.fecha_registro,

                                    }).ToList();

                    gv_juzgado.DataSource = inf_user;
                    gv_juzgado.DataBind();
                    gv_juzgado.Visible = true;
                }
            }
        }

        protected void chk_sala_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_editar_sala.Checked || rb_eliminar_sala.Checked)
            {

                foreach (GridViewRow row in gv_sala.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chk_sala") as CheckBox);
                        if (chkRow.Checked)
                        {
                            row.BackColor = Color.YellowGreen;

                            int int_code = int.Parse(row.Cells[4].Text);

                            using (db_transcriptEntities data_user = new db_transcriptEntities())
                            {
                                var inf_user = (from i_u in data_user.inf_salas
                                                where i_u.codigo_sala == int_code
                                                where i_u.id_estatus == 1
                                                select new
                                                {
                                                    i_u.id_sala,
                                                    i_u.nombre,
                                                    i_u.ip,
                                                    i_u.fecha_registro,

                                                }).FirstOrDefault();



                                txt_sala.Text = inf_user.nombre;
                                txt_ip.Text = inf_user.ip;
                            }
                        }
                        else
                        {
                            row.BackColor = Color.White;
                        }
                    }
                }
            }
        }

        protected void gv_juzgado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_juzgado.PageIndex = e.NewPageIndex;

            using (db_transcriptEntities data_user = new db_transcriptEntities())
            {
                var inf_user = (from i_u in data_user.inf_juzgados
                                join i_e in data_user.fact_especializa on i_u.id_especializa equals i_e.id_especializa
                                where i_u.id_tribunal == guid_fidcentro
                                where i_u.id_estatus == 1
                                select new
                                {
                                    i_u.codigo_juzgado,
                                    i_e.desc_especializa,
                                    i_u.localidad,
                                    i_u.numero,
                                    i_u.fecha_registro,

                                }).ToList();

                gv_juzgado.DataSource = inf_user;
                gv_juzgado.DataBind();
                gv_juzgado.Visible = true;
            }
        }

        protected void gv_sala_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_sala.PageIndex = e.NewPageIndex;
            foreach (GridViewRow row in gv_juzgado.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chk_juzgado") as CheckBox);
                    if (chkRow.Checked)
                    {
                        row.BackColor = Color.YellowGreen;
                        int int_code = int.Parse(row.Cells[5].Text);


                        using (db_transcriptEntities m_tribunal = new db_transcriptEntities())
                        {
                            var i_tribunal = (from u in m_tribunal.inf_juzgados
                                              where u.codigo_juzgado == int_code
                                              select u).FirstOrDefault();

                            ddl_especializa.SelectedValue = i_tribunal.id_especializa.ToString();
                            txt_localidad.Text = i_tribunal.localidad;
                            txt_numero.Text = i_tribunal.numero;
                            txt_callenum.Text = i_tribunal.calle_num;
                            txt_cp.Text = i_tribunal.cp;
                            guid_idjuzgado = i_tribunal.id_juzgado;

                            using (db_transcriptEntities db_sepomex = new db_transcriptEntities())
                            {
                                var tbl_sepomex = (from c in db_sepomex.inf_sepomex
                                                   where c.d_codigo == i_tribunal.cp.ToString()
                                                   select c).ToList();

                                ddl_colonia.DataSource = tbl_sepomex;
                                ddl_colonia.DataTextField = "d_asenta";
                                ddl_colonia.DataValueField = "id_asenta_cpcons";
                                ddl_colonia.DataBind();

                                ddl_colonia.SelectedValue = i_tribunal.id_asenta_cpcons.ToString();
                                txt_municipio.Text = tbl_sepomex[0].D_mnpio;
                                txt_estado.Text = tbl_sepomex[0].d_estado;
                            }

                            using (db_transcriptEntities data_user = new db_transcriptEntities())
                            {
                                var inf_user = (from i_u in data_user.inf_salas
                                                where i_u.id_juzgado == guid_idjuzgado
                                                where i_u.id_estatus == 1
                                                select new
                                                {
                                                    i_u.codigo_sala,
                                                    i_u.nombre,
                                                    i_u.ip,
                                                    i_u.fecha_registro,

                                                }).ToList();

                                gv_sala.DataSource = inf_user;
                                gv_sala.DataBind();
                                gv_sala.Visible = true;
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

        protected void btn_guardar_juzgado_Click(object sender, EventArgs e)
        {
            if (rb_agregar_juzgado.Checked == false & rb_editar_juzgado.Checked == false & rb_eliminar_juzgado.Checked == false)
            {

                lblModalTitle.Text = "transcript";
                lblModalBody.Text = "Favor de seleccionar una acción";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                upModal.Update();

            }
            else
            {
            }
            if (ddl_especializa.SelectedValue == "0")
            {

                ddl_especializa.BackColor = Color.Yellow;
            }
            else
            {
                ddl_especializa.BackColor = Color.Transparent;
                if (string.IsNullOrEmpty(txt_localidad.Text))
                {

                    txt_localidad.BackColor = Color.Yellow;
                }
                else
                {
                    txt_localidad.BackColor = Color.Transparent;
                    if (string.IsNullOrEmpty(txt_numero.Text))
                    {

                        txt_numero.BackColor = Color.Yellow;
                    }
                    else
                    {
                        txt_numero.BackColor = Color.Transparent;
                        if (string.IsNullOrEmpty(txt_callenum.Text))
                        {

                            txt_callenum.BackColor = Color.Yellow;
                        }
                        else
                        {
                            txt_callenum.BackColor = Color.Transparent;

                            if (string.IsNullOrEmpty(txt_cp.Text))
                            {

                                txt_cp.BackColor = Color.Yellow;
                            }
                            else
                            {
                                txt_cp.BackColor = Color.Transparent;

                                guardar_juzgado();
                            }
                        }
                    }
                }
            }
        }

        protected void btn_cp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_cp.Text))
            {

                txt_cp.BackColor = Color.Yellow;
            }
            else
            {
                txt_cp.BackColor = Color.Transparent;
                string str_codigo = txt_cp.Text;

                datos_sepomex(str_codigo);
            }
        }
        private void datos_sepomex(string str_codigo)
        {
            using (db_transcriptEntities db_sepomex = new db_transcriptEntities())
            {
                var tbl_sepomex = (from c in db_sepomex.inf_sepomex
                                   where c.d_codigo == str_codigo
                                   select c).ToList();

                ddl_colonia.DataSource = tbl_sepomex;
                ddl_colonia.DataTextField = "d_asenta";
                ddl_colonia.DataValueField = "id_asenta_cpcons";
                ddl_colonia.DataBind();

                if (tbl_sepomex.Count == 1)
                {

                    txt_cp.BackColor = Color.Transparent;
                    txt_municipio.Text = tbl_sepomex[0].D_mnpio;
                    txt_estado.Text = tbl_sepomex[0].d_estado;
                }
                if (tbl_sepomex.Count > 1)
                {
                    txt_cp.BackColor = Color.Transparent;
                    ddl_colonia.Items.Insert(0, new ListItem("*Colonia", "0"));

                    txt_municipio.Text = tbl_sepomex[0].D_mnpio;
                    txt_estado.Text = tbl_sepomex[0].d_estado;
                }
                else if (tbl_sepomex.Count == 0)
                {
                    txt_cp.BackColor = Color.Yellow;
                    ddl_colonia.Items.Clear();
                    ddl_colonia.Items.Insert(0, new ListItem("*Colonia", "0"));
                    txt_municipio.Text = "";
                    txt_estado.Text = "";
                }
            }
        }
    }
}