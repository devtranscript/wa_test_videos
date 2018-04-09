using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_transcript
{
    public partial class ctrl_videos_consultados : System.Web.UI.Page
    {
        static Guid guid_fidusuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    inf_user();

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
            using (db_transcriptEntities data_user = new db_transcriptEntities())
            {
                var inf_user = (from i_u in data_user.inf_usuarios
                                join i_tu in data_user.fact_tipo_usuarios on i_u.id_tipo_usuario equals i_tu.id_tipo_usuario
                                join i_e in data_user.inf_tribunal on i_u.id_tribunal equals i_e.id_tribunal
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

                lbl_name.Text = inf_user.nombres + " " + inf_user.a_paterno + " " + inf_user.a_materno;
                lbl_profile_user.Text = inf_user.desc_tipo_usuario;
                lbl_id_profile_user.Text = inf_user.id_tipo_usuario.ToString();
                lbl_user_centerCP.Text = inf_user.nombre;
                lbl_id_centerCP.Text = inf_user.id_tribunal.ToString();


            }
        }
        protected void gv_usuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_files.PageIndex = e.NewPageIndex;
            DateTime str_fdateini = Convert.ToDateTime(txt_dateini.Text);
            DateTime str_fdatefin = Convert.ToDateTime(txt_datefin.Text);
            using (db_transcriptEntities data_user = new db_transcriptEntities())
            {
                var inf_user = (from inf_lv in data_user.inf_log_videos
                                join inf_u in data_user.inf_usuarios on inf_lv.id_usuario equals inf_u.id_usuario
                                where inf_lv.fecha_registro_alt >= str_fdateini && inf_lv.fecha_registro_alt <= str_fdatefin
                                select new
                                {
                                    inf_lv.id_log_videos,
                                    inf_lv.sesion,
                                    inf_lv.video,
                                    inf_u.nombres,
                                    inf_u.a_paterno,
                                    inf_u.a_materno,
                                    inf_lv.fecha_registro,
                                    inf_lv.fecha_registro_alt,

                                }).ToList();

                gv_files.DataSource = inf_user;
                gv_files.DataBind();
                gv_files.Visible = true;

            }

        }
        protected void cmd_search_Click(object sender, EventArgs e)
        {
            if (rb_internos.Checked == false & rb_externos.Checked == false )
            {

                lblModalTitle.Text = "transcript";
                lblModalBody.Text = "Favor de seleccionar una opción";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                upModal.Update();

            }
            else
            {


                if (string.IsNullOrEmpty(txt_dateini.Text))
                {

                    txt_dateini.BackColor = Color.Yellow;
                }
                else
                {
                    txt_dateini.BackColor = Color.Transparent;
                    if (string.IsNullOrEmpty(txt_datefin.Text))
                    {

                        txt_datefin.BackColor = Color.Yellow;
                    }
                    else
                    {
                        txt_datefin.BackColor = Color.Transparent;

                        DateTime str_fdateini = Convert.ToDateTime(txt_dateini.Text);
                        DateTime str_fdatefin = Convert.ToDateTime(txt_datefin.Text);

                        if (rb_internos.Checked)
                        {

                            using (db_transcriptEntities data_user = new db_transcriptEntities())
                            {
                                var inf_user = (from inf_lv in data_user.inf_log_videos
                                                join inf_u in data_user.inf_usuarios on inf_lv.id_usuario equals inf_u.id_usuario
                                                where inf_lv.fecha_registro_alt >= str_fdateini && inf_lv.fecha_registro_alt <= str_fdatefin
                                                select new
                                                {
                                                    inf_lv.id_log_videos,
                                                    inf_lv.sesion,
                                                    inf_lv.video,
                                                    inf_u.nombres,
                                                    inf_u.a_paterno,
                                                    inf_u.a_materno,
                                                    inf_lv.fecha_registro,
                                                    inf_lv.fecha_registro_alt,

                                                }).ToList();

                                gv_files.DataSource = inf_user;
                                gv_files.DataBind();
                                gv_files.Visible = true;

                            }
                        }
                        else if (rb_externos.Checked)
                        {
                            gv_files.Visible = false;
                        }
                    }
                }

            }
        }

        protected void rb_internos_CheckedChanged(object sender, EventArgs e)
        {
            rb_externos.Checked = false;
        }

        protected void rb_externos_CheckedChanged(object sender, EventArgs e)
        {
            rb_internos.Checked = false;
        }
    }
}