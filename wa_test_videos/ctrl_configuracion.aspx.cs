﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace wa_transcript
{
    public partial class ctrl_configuracion : System.Web.UI.Page
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

                int str_id_type_user = i_usuario.id_tipo_usuario;
                switch (str_id_type_user)
                {

                    case 1:


                        break;
                    case 2:
                        div_credentials.Visible = true;
                        div_routevideos.Visible = true;
                        div_transformation.Visible = true;
                        div_dayvideos.Visible = true;

                        break;
                    case 3:

                        div_credentials.Visible = true;
                        div_routevideos.Visible = true;
                        div_transformation.Visible = true;
                        div_dayvideos.Visible = true;
                        break;
                    case 4:
                        div_credentials.Visible = false;
                        div_routevideos.Visible = false;
                        div_transformation.Visible = false;
                        div_dayvideos.Visible = false;
                        div1.Visible = false;
                        div6.Visible = true;
                        break;
                }


            }

            using (db_transcriptEntities edm_fecha_transf = new db_transcriptEntities())
            {
                var i_fecha_transf = (from c in edm_fecha_transf.inf_fecha_transformacion
                                      select c).ToList();

                if (i_fecha_transf.Count != 0)
                {

                }
            }

        }


        protected void img_transformation_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ctrl_agenda_conversion.aspx");
        }

        protected void img_dayvideos_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ctrl_dias_respaldo.aspx");
        }

        protected void img_routevideos_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ctrl_ruta_videos.aspx");
        }

        protected void img_conversion_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ctrl_conversion.aspx");
        }

        protected void img_conexiones_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ctrl_conexiones.aspx");
        }
    }
}