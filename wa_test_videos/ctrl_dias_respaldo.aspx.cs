﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_transcript
{
    public partial class ctrl_dias_respaldo : System.Web.UI.Page
    {
        static Guid guid_fidusuario, guid_fidcentro;
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
                var i_fecha_transf = (from c in edm_fecha_transf.inf_caducidad_videos
                                      select c).ToList();

                if (i_fecha_transf.Count != 0)
                {
                    rb_add_dayvideos.Visible = false;
                }
            }

        }
        protected void rb_add_dayvideos_CheckedChanged(object sender, EventArgs e)
        {
            rb_edit_dayvideos.Checked = false;
            txt_days.Text = "";
            div_infdayvideos.Visible = true;
        }
        protected void rb_edit_dayvideos_CheckedChanged(object sender, EventArgs e)
        {
            rb_add_dayvideos.Checked = false;

            div_infdayvideos.Visible = true;
            rb_add_dayvideos.Checked = false;
            gv_dayvideosf.Visible = false;
            txt_days.Text = "";



            using (db_transcriptEntities data_user = new db_transcriptEntities())
            {
                var inf_user = (from u in data_user.inf_caducidad_videos
                                select new
                                {
                                    u.id_caducidad_videos,
                                    u.dias_caducidad,
                                    u.fecha_registro

                                }).ToList();

                if (inf_user.Count == 0)
                {
                    rb_edit_dayvideos.Checked = false;


                    lblModalTitle.Text = "transcript";
                    lblModalBody.Text = "Sin registro, favor de agregar uno";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                    upModal.Update();
                }
                else
                {
                    gv_dayvideos.DataSource = inf_user;
                    gv_dayvideos.DataBind();
                    gv_dayvideos.Visible = true;
                }
            }
        }
        public int id_accion()
        {
            if (rb_add_dayvideos.Checked)
            {
                return 1;
            }
            else if (rb_edit_dayvideos.Checked)
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
        protected void cmd_save_days_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_days.Text))
            {

                txt_days.BackColor = Color.Yellow;
            }
            else
            {
                txt_days.BackColor = Color.Transparent;

                int str_ndias = Convert.ToInt32(txt_days.Text);

                int str_count;

                using (db_transcriptEntities edm_cadvideos = new db_transcriptEntities())
                {
                    var i_cadvideos = (from c in edm_cadvideos.inf_caducidad_videos

                                       select c).Count();
                    str_count = i_cadvideos;
                }

                if (str_count == 0)
                {
                    using (var edm_cadvideos = new db_transcriptEntities())
                    {
                        var i_cadvideos = new inf_caducidad_videos
                        {
                            dias_caducidad = str_ndias,
                            id_usuario = guid_fidusuario,
                            id_tribunal = guid_fidcentro,
                            fecha_registro = DateTime.Now

                        };
                        edm_cadvideos.inf_caducidad_videos.Add(i_cadvideos);
                        edm_cadvideos.SaveChanges();
                    }
                    using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
                    {
                        var ii_fecha_transf = (from u in edm_fecha_transf.inf_caducidad_videos
                                               select u).ToList();

                        if (ii_fecha_transf.Count == 0)
                        {

                        }
                        else
                        {
                            using (var insert_user = new db_transcriptEntities())
                            {
                                var items_user = new inf_caducidad_videos_dep
                                {
                                    id_usuario = guid_fidusuario,
                                    id_caducidad_videos = ii_fecha_transf[0].id_caducidad_videos,
                                    id_tipo_accion = id_accion(),
                                    fecha_registro = DateTime.Now,

                                };
                                insert_user.inf_caducidad_videos_dep.Add(items_user);
                                insert_user.SaveChanges();
                            }
                        }
                    }
                    txt_days.Text = "";
                    using (db_transcriptEntities edm_cadvideos = new db_transcriptEntities())
                    {
                        var i_cadvideos = (from u in edm_cadvideos.inf_caducidad_videos
                                           where u.dias_caducidad == str_ndias
                                           select new
                                           {

                                               u.id_caducidad_videos,
                                               u.dias_caducidad,
                                               u.fecha_registro

                                           }).ToList();

                        gv_dayvideosf.DataSource = i_cadvideos;
                        gv_dayvideosf.DataBind();
                        gv_dayvideosf.Visible = true;
                    }

                    rb_add_dayvideos.Visible = false;

                    lblModalTitle.Text = "transcript";
                    lblModalBody.Text = "Dias de respaldo, agregado con éxito";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                    upModal.Update();

                }
                else
                {
                    foreach (GridViewRow row in gv_dayvideos.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("chk_select") as CheckBox);
                            if (chkRow.Checked)
                            {
                                int str_code = Convert.ToInt32(row.Cells[1].Text);

                                using (var edm_cadvideos = new db_transcriptEntities())
                                {
                                    var i_cadvideos = (from c in edm_cadvideos.inf_caducidad_videos
                                                       where c.id_caducidad_videos == str_code
                                                       select c).FirstOrDefault();

                                    i_cadvideos.dias_caducidad = str_ndias;
                                    edm_cadvideos.SaveChanges();
                                }
                                using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
                                {
                                    var ii_fecha_transf = (from u in edm_fecha_transf.inf_caducidad_videos
                                                           select u).ToList();

                                    if (ii_fecha_transf.Count == 0)
                                    {

                                    }
                                    else
                                    {
                                        using (var insert_user = new db_transcriptEntities())
                                        {
                                            var items_user = new inf_caducidad_videos_dep
                                            {
                                                id_usuario = guid_fidusuario,
                                                id_caducidad_videos = ii_fecha_transf[0].id_caducidad_videos,
                                                id_tipo_accion = id_accion(),
                                                fecha_registro = DateTime.Now,

                                            };
                                            insert_user.inf_caducidad_videos_dep.Add(items_user);
                                            insert_user.SaveChanges();
                                        }
                                    }
                                }
                                txt_days.Text = ""; ;
                                lblModalTitle.Text = "transcript";
                                lblModalBody.Text = "Dias de respaldo, actualizado con éxito";
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                                upModal.Update();

                                using (db_transcriptEntities edm_cadvideos = new db_transcriptEntities())
                                {
                                    var i_cadvideos = (from u in edm_cadvideos.inf_caducidad_videos
                                                       where u.id_caducidad_videos == str_code
                                                       select new
                                                       {
                                                           u.id_caducidad_videos,
                                                           u.dias_caducidad,
                                                           u.fecha_registro

                                                       }).ToList();

                                    gv_dayvideos.DataSource = i_cadvideos;
                                    gv_dayvideos.DataBind();
                                    gv_dayvideos.Visible = true;
                                }
                            }
                        }
                    }


                }
            }

        }
        protected void chkselect_dayvideos(object sender, EventArgs e)
        {

            foreach (GridViewRow row in gv_dayvideos.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chk_select") as CheckBox);
                    if (chkRow.Checked)
                    {
                        row.BackColor = Color.YellowGreen;
                        int str_code = Convert.ToInt32(row.Cells[1].Text);

                        using (db_transcriptEntities edm_cadvideos = new db_transcriptEntities())
                        {
                            var i_cadvideos = (from u in edm_cadvideos.inf_caducidad_videos
                                               where u.id_caducidad_videos == str_code
                                               select new
                                               {
                                                   u.id_caducidad_videos,
                                                   u.dias_caducidad,
                                                   u.fecha_registro


                                               }).FirstOrDefault();

                            txt_days.Text = i_cadvideos.dias_caducidad.ToString();
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
}