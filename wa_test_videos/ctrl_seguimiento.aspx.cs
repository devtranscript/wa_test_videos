using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_transcript
{
    public partial class ctrl_seguimiento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    //inf_user();

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
        protected void gv_files_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnButton = (Button)e.Row.FindControl("cmd_ver");
                if (e.Row.Cells[8].Text == "ERROR")
                {

                    btnButton.Text = "Convertir";
                    btnButton.Enabled = true;
                }
                else if (e.Row.Cells[8].Text == "ACTIVO")
                {
                    btnButton.Text = "Ver";
                    btnButton.Enabled = true;
                }
                else if (e.Row.Cells[8].Text == "CONVIRTIENDO")
                {
                    btnButton.Text = "Esperar";
                    btnButton.Enabled = false;
                }
                else
                {
                    btnButton.Text = "";
        
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
                        div_panel.Visible = false;
                        UpdatePanel2.Update();
                        row.BackColor = Color.YellowGreen;
                    }
                    else
                    {
                        row.BackColor = Color.White;
                    }
                }
            }

        }
        protected void gv_files_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            System.Web.HttpBrowserCapabilities browser = Request.Browser;
            string s = browser.Browser;

            if (s == "Chrome")
            {
                div_panel_ie.Visible = false;
                div_panel.Visible = true;
            }
            else if (s == "Firefox")
            {
                div_panel_ie.Visible = false;
                div_panel.Visible = true;
            }
            else if (s == "InternetExplorer")
            {
                div_panel_ie.Visible = true;
            }



            foreach (GridViewRow row in gv_files.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[2].FindControl("chk_select") as CheckBox);
                    if (chkRow.Checked)
                    {

                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            Button btnButton = (Button)row.FindControl("cmd_ver");
                            if (row.Cells[8].Text == "INACTIVO")
                            {
                                div_panel.Visible = false;
                                UpdatePanel2.Update();

                            }
                            else
                            {
                                string str_namefile = @"videos\" + row.Cells[5].Text;
                                string str_session = row.Cells[1].Text;
                                string str_video = row.Cells[5].Text;

                                string d_pdf = "videos\\" + str_session + "\\ExtraFiles\\" + str_session + "_Report.pdf";
                                iframe_pdf.Visible = true;
                                iframe_pdf.Attributes["src"] = d_pdf;
                                UpdatePanel2.Update();


                                play_video.Visible = true;
                                play_video.Attributes["src"] = str_namefile;

                                div_panel.Visible = true;
                                UpdatePanel2.Update();

                            }
                        }

                        //Panel1.Visible = true;
                        //Panel1.Controls.Add(CrearControlVideo(str_namefile));
                    }
                }
            }



        }
        private Control CrearControlVideo(string str_namefile)
        {
            StringBuilder sa = new StringBuilder();
            // sa.Append("<center>");
            sa.Append("<OBJECT ID=\"Player\" Object Type=\"video/x-ms-wmv\" width=\"640\" height=\"480\" VIEWASTEXT > ");
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
        protected void img_pdf_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in gv_files.Rows)
            {
                CheckBox chkRow = (row.Cells[3].FindControl("chk_select") as CheckBox);
                if (chkRow.Checked)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if (row.Cells[8].Text == "INACTIVO")
                        {

                        }
                        else
                        {
                            string str_session = row.Cells[1].Text;
                            string str_video = row.Cells[5].Text;

                            string d_pdf = "videos\\" + str_session + "\\ExtraFiles\\" + str_session + "_Report.pdf";
                            iframe_pdf.Visible = true;
                            iframe_pdf.Attributes["src"] = d_pdf;
                            UpdatePanel2.Update();

                        }
                    }
                }
            }

        }
        protected void cmd_search_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_expedient.Text))
            {

                txt_expedient.BackColor = Color.Yellow;
            }
            else
            {
                txt_expedient.BackColor = Color.Transparent;
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
                        var two_user = new int?[] { 1, 4, 5 };
                        flist_user(two_user);
                    }
                }
            }


        }

        private void flist_user(int?[] two_user)
        {
            string str_expediente = txt_expedient.Text;
            DateTime str_fdateini = Convert.ToDateTime(txt_dateini.Text);
            DateTime str_fdatefin = Convert.ToDateTime(txt_datefin.Text);

            try
            {
                using (db_transcriptEntities data_user = new db_transcriptEntities())
                {
                    var inf_user = (from inf_m in data_user.inf_material
                                    join inf_em in data_user.fact_estatus_material on inf_m.id_estatus_material equals inf_em.id_estatus_material
                                    where two_user.Contains(inf_m.id_estatus_material)
                                    where inf_m.sesion == str_expediente
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

                    if (inf_user.Count == 0)
                    {
                        lblModalTitle.Text = "transcript";
                        lblModalBody.Text = "Expediente no existe, favor de reintentar";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                        upModal.Update();
                    }
                    else

                    {
                        gv_files.DataSource = inf_user;
                        gv_files.DataBind();
                        gv_files.Visible = true;
                    }

                 

                }
            }
            catch
            {

              
            }
        }

        private void filter_videos(int str_idload)
        {

        }

        protected void gv_files_RowDataBound1(object sender, GridViewRowEventArgs e)
        {

        }
    }
}