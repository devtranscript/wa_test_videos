using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_transcript
{

    public partial class AsyncForm : System.Web.UI.Page
    {

        //IAsyncResult RecuperarDatos(object sender, EventArgs e, AsyncCallback cb, object state)
        //{
        //}
        //void CargarGrilla(IAsyncResult ar)
        //{
        //}

    }
    public partial class ctrl_conversion : System.Web.UI.Page
    {

        static string str_session, str_video;
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
                var i_fecha_transf = (from c in edm_fecha_transf.inf_fecha_transformacion
                                      select c).ToList();

                if (i_fecha_transf.Count != 0)
                {

                }
            }

        }
        protected void chk_OnCheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gv_files.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[3].FindControl("chk_select") as CheckBox);

                    if (chkRow.Checked)
                    {
                        row.BackColor = Color.YellowGreen;

                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                }
            }

        }
        protected void cmd_search_Click(object sender, EventArgs e)
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

                    using (var edm_materialf = new db_transcriptEntities())
                    {
                        var i_materialf = (from c in edm_materialf.inf_material
                                           select c).ToList();

                        if (i_materialf.Count == 0)
                        {
                            lblModalTitle.Text = "transcript";
                            lblModalBody.Text = "Sin videos por convertir, favor de reintentar o contactar con el administrador";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                            upModal.Update();

                        }
                        else
                        {
                            using (var edm_material = new db_transcriptEntities())
                            {
                                var i_material = (from c in edm_material.inf_material
                                                  where c.id_estatus_material == 6
                                                  select c).ToList();

                                if (i_material.Count == 0)
                                {
                                    lblModalTitle.Text = "transcript";
                                    lblModalBody.Text = "Sin videos por convertir";
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                                    upModal.Update();
                                    var two_user = new int?[] { 1, 4, 5 };
                                    flist_user(two_user);

                                }
                                else
                                {
                                    lblModalTitle.Text = "transcript";
                                    lblModalBody.Text = "Se estan convirtiendo videos, favor de esperar";
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                                    upModal.Update();
                                    var two_user = new int?[] { 1, 4, 5,6 };
                                    flist_user(two_user);


                                }

                            }
                        }

                    }


                }
            }

        }
        private void flist_user(int?[] str_idload)
        {
            CultureInfo en = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = en;
            string str_dateini = txt_dateini.Text;
            string str_datefin = txt_datefin.Text;

            DateTime str_fdateini = DateTime.Parse(str_dateini);
            DateTime str_fdatefin = DateTime.Parse(str_datefin);

            if (lbl_idprofileuser.Text == "4")
            {
                using (db_transcriptEntities edm_material = new db_transcriptEntities())
                {
                    var i_material = (from inf_m in edm_material.inf_material
                                      join inf_em in edm_material.fact_estatus_material on inf_m.id_estatus_material equals inf_em.id_estatus_material
                                      where str_idload.Contains(inf_m.id_estatus_material)
                                      where inf_m.fecha_registro >= str_fdateini && inf_m.fecha_registro <= str_fdatefin
                                      select new
                                      {
                                          inf_m.sesion,
                                          inf_m.titulo,
                                          inf_m.localizacion,
                                          inf_m.tipo,
                                          inf_m.archivo,
                                          inf_m.duracion,
                                          inf_m.fecha_registro,
                                          inf_em.desc_estatus_material,
                                          inf_m.id_control

                                      }).ToList();

                    gv_files.DataSource = i_material;
                    gv_files.DataBind();
                    gv_files.Visible = true;

                }
            }
            else
            {
                using (db_transcriptEntities edm_material = new db_transcriptEntities())
                {
                    var i_material = (from inf_m in edm_material.inf_material
                                      join inf_em in edm_material.fact_estatus_material on inf_m.id_estatus_material equals inf_em.id_estatus_material
									  join inf_rv in edm_material.inf_ruta_videos on inf_m.id_ruta_videos equals inf_rv.id_ruta_videos
									  join inf_s in edm_material.inf_salas on inf_rv.id_sala equals inf_s.id_sala
									  join inf_j in edm_material.inf_juzgados on inf_s.id_juzgado equals inf_j.id_juzgado
									  where str_idload.Contains(inf_m.id_estatus_material)
                                      where inf_m.fecha_registro >= str_fdateini && inf_m.fecha_registro <= str_fdatefin
                                      select new
                                      {
										  inf_j.localidad,
										  inf_j.numero,
										  inf_m.sesion,
                                          inf_m.titulo,
                                          inf_m.localizacion,
                                          inf_m.tipo,
                                          inf_m.archivo,
                                          inf_m.duracion,
                                          inf_m.fecha_registro,
                                          inf_em.desc_estatus_material,
                                          inf_m.id_control

                                      }).ToList();

                    gv_files.DataSource = i_material;
                    gv_files.DataBind();
                    gv_files.Visible = true;
                    //cmd_save.Visible = false;

                }
            }
        }
        protected void gv_files_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            foreach (GridViewRow row in gv_files.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[3].FindControl("chk_select") as CheckBox);
                    if (chkRow.Checked)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            Button btnButton = (Button)row.FindControl("cmd_action");
                            if (row.Cells[10].Text == "ERROR")
                            {
                                str_session = row.Cells[3].Text;
                                str_video = row.Cells[7].Text;

                                using (var edm_material = new db_transcriptEntities())
                                {
                                    var i_material = (from c in edm_material.inf_material
                                                      where c.sesion == str_session
                                                      select c).FirstOrDefault();

                                    i_material.id_estatus_material = 6;

                                    edm_material.SaveChanges();
                                }
                                var two_user = new int?[] { 1,4,5, 6 };
                                flist_user(two_user);

                                lblModalTitle.Text = "transcript";
                                lblModalBody.Text = "Comienza proceso de Conversión, favor de esperar.";
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                                upModal.Update();

                            }
                            else if (row.Cells[10].Text == "ACTIVO")
                            {
                                str_session = row.Cells[3].Text;
                                str_video = row.Cells[7].Text;
                                div_panel.Visible = true;
                                UpdatePanel2.Update();
                                using (var edm_material = new db_transcriptEntities())
                                {
                                    var i_material = new inf_log_videos
                                    {
                                        sesion = str_session,
                                        video = str_video,
                                        id_usuario = guid_fidusuario,
                                        id_tribunal = guid_fidcentro,
                                        fecha_registro = DateTime.Now,
                                        fecha_registro_alt = DateTime.Now
                                    };

                                    edm_material.inf_log_videos.Add(i_material);
                                    edm_material.SaveChanges();
                                }

                                str_session = row.Cells[3].Text;
                                str_video = row.Cells[7].Text;

                               

                                string d_pdf = "videos\\" + str_session + "\\ExtraFiles\\" + str_session + "_Report.pdf";
                                iframe_pdf.Visible = true;
                                iframe_pdf.Attributes["src"] = d_pdf;

                                string str_namefile = @"videos\" + row.Cells[7].Text;

                                play_video.Visible = true;
                                play_video.Attributes["src"] = str_namefile;
                            }
                        }
                    }
                }
            }
        }
        private static void DoWork(object sender, DoWorkEventArgs e)
        {
            // Long running background operation

            using (var edm_material = new db_transcriptEntities())
            {
                var i_material = (from c in edm_material.inf_material
                                  where c.id_estatus_material == 6
                                  select c).ToList();

                foreach (var item in i_material)
                {
                    string str_path_ini, str_path_fin;

                    using (db_transcriptEntities data_path = new db_transcriptEntities())
                    {
                        var count_path = (from c in data_path.inf_ruta_videos
                                          select c).FirstOrDefault();

                        str_path_fin = count_path.desc_ruta_fin;
                        str_path_ini = count_path.desc_ruta_fin + "\\" + str_video;
                    }

                    str_path_ini = str_path_fin + "\\" + item.archivo.ToString().Replace(".mp4", ".wmv");

                    string str_file_save = str_path_ini.ToString();
                    string str_save_file = str_path_ini.ToString().Replace(".wmv", ".mp4");
                    var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

                    var ffProbe = new NReco.VideoInfo.FFProbe();
                    var videoInfo = ffProbe.GetMediaInfo(str_file_save);

                    string str_duration_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

                    try
                    {

                        using (var data_mat = new db_transcriptEntities())
                        {
                            var items_mat = (from c in data_mat.inf_material
                                             where c.sesion == str_session
                                             select c).FirstOrDefault();

                            items_mat.id_estatus_material = 6;

                            data_mat.SaveChanges();
                        }

                        var two_user = new int?[] { 6 };

                        ffMpeg.ConvertMedia(str_file_save, str_save_file, Format.mp4);


                        using (var data_mat = new db_transcriptEntities())
                        {
                            var items_mat = (from c in data_mat.inf_material
                                             where c.sesion == str_session
                                             select c).FirstOrDefault();

                            items_mat.id_estatus_material = 1;

                            data_mat.SaveChanges();
                        }

                        two_user = new int?[] { 1 };



                    }
                    catch
                    {
                        using (var data_mat = new db_transcriptEntities())
                        {
                            var items_mat = (from c in data_mat.inf_material
                                             where c.sesion == str_session
                                             select c).FirstOrDefault();

                            items_mat.id_estatus_material = 5;

                            data_mat.SaveChanges();
                        }


                    }
                    var videoInfo_mp4 = ffProbe.GetMediaInfo(str_file_save);

                    string str_duration_mp4 = videoInfo_mp4.Duration.Hours + ":" + videoInfo_mp4.Duration.Minutes + ":" + videoInfo_mp4.Duration.Seconds;

                    if (str_duration_wmv == str_duration_mp4)
                    {
                        File.Delete(str_file_save);

                    }
                    else
                    {
                        using (var data_mat = new db_transcriptEntities())
                        {
                            var items_mat = (from c in data_mat.inf_material
                                             where c.sesion == str_session
                                             select c).FirstOrDefault();

                            items_mat.id_estatus_material = 5;
                            data_mat.SaveChanges();
                        }
                    }
                }
            }
        }
        private static void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // log when the worker is completed.
        }
        private Control CrearControlVideo(string str_namefile)
        {
            StringBuilder sa = new StringBuilder();
            // sa.Append("<center>");
            sa.Append("<OBJECT ID=\"Player\" Object Type=\"video/x-ms-wmv\" width=\"640\" height=\"490\" VIEWASTEXT > ");
            sa.Append("<PARAM name=\"autoStart\" value=\"false\">");
            sa.Append(string.Format("<PARAM name=\"SRC\" value=\"{0}\">", str_namefile));// IE needs this extra push when using MIME type not class id
            sa.Append(string.Format("<PARAM name=\"URL\" value=\"{0}\">", str_namefile));
            sa.Append("<PARAM name=\"AutoSize\" value=\"False\"");
            sa.Append("<PARAM name=\"rate\" value=\"1\">");
            sa.Append("<PARAM name=\"balance\" value=\"0\">");
            sa.Append("<PARAM name=\"enabled\" value=\"true\">");
            sa.Append("<PARAM name=\"enabledContextMenu\" value=\"true\">");
            sa.Append("<PARAM name=\"fullScreen\" value=\"false\">");
            sa.Append("<PARAM name=\"playCount\" value=\"1\">");
            sa.Append("<PARAM name=\"volume\" value=\"30\">  ");
            sa.Append("</OBJECT>");
            //  sa.Append("</center>");

            return new LiteralControl(sa.ToString());
        }
        protected void gv_files_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnButton = (Button)e.Row.FindControl("cmd_action");
                if (e.Row.Cells[10].Text == "ERROR")
                {

                    btnButton.Text = "Convertir";
                    btnButton.Enabled = true;
                }
                else if (e.Row.Cells[10].Text == "ACTIVO")
                {
                    btnButton.Text = "Ver";
                    btnButton.Enabled = true;
                }
                else if (e.Row.Cells[10].Text == "CONVIRTIENDO")
                {
                    btnButton.Text = "Esperar";
                    btnButton.Enabled = false;
                }
                else if (e.Row.Cells[10].Text == "INACTIVO")
                {
                    btnButton.Text = "";
                    btnButton.Enabled = false;
                }
            }
        }

        protected void rb_internos_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void rb_externos_CheckedChanged(object sender, EventArgs e)
        {

        }


        protected void Timer1_Tick(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gv_files.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (row.Cells[10].Text == "CONVIRTIENDO")
                    {
                        using (var edm_material = new db_transcriptEntities())
                        {
                            var i_material = (from c in edm_material.inf_material
                                              where c.id_estatus_material == 6
                                              select c).Count();

                            if (i_material != 0)
                            {
                                //BackgroundWorker worker = new BackgroundWorker();
                                //worker.DoWork += new DoWorkEventHandler(DoWork);
                                //worker.WorkerReportsProgress = false;

                                //worker.RunWorkerAsync();
                            }
                            else
                            {
                                var two_user = new int?[] { 1,4, 5 };
                                flist_user(two_user);
                            }

                        }
                    }


                }
            }

        }
        void worker_DoWork(ref int progress,
          ref object result, params object[] arguments)
        {
            // Get the value which passed to this operation.
            string input = string.Empty;
            if (arguments.Length > 0)
            {
                input = arguments[0].ToString();
            }

            // Need 10 seconds to complete this operation.
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);

                progress += 1;
            }

            // The operation is completed.
            progress = 100;
            result = "Operation is completed. The input is \"" + input + "\".";
        }
    }
}