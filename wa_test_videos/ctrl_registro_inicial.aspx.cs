using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_transcript
{
    public partial class ctrl_registro_inicial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {

                    load_ddl();

                }
                else
                {

                }
            }
            catch
            {
                
            }
        }
        private void load_ddl()
        {
            ddl_colonia.Items.Clear();
            ddl_colonia.Items.Insert(0, new ListItem("*Colonia", "0"));

        }
        protected void btn_guardar_Click(object sender, EventArgs e)
        { 
                if (string.IsNullOrEmpty(txt_tribunal.Text))
                {

                    txt_tribunal.BackColor = Color.Yellow;
                }
                else
                {
                    txt_tribunal.BackColor = Color.Transparent;
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
                            if (ddl_colonia.SelectedValue == "0")
                            {

                                ddl_colonia.BackColor = Color.Yellow;
                            }
                            else
                            {
                                ddl_colonia.BackColor = Color.Transparent;


                                if (string.IsNullOrEmpty(txt_nombres.Text))
                                {

                                    txt_nombres.BackColor = Color.Yellow;
                                }
                                else
                                {
                                    txt_nombres.BackColor = Color.Transparent;
                                    if (string.IsNullOrEmpty(txt_apaterno.Text))
                                    {

                                        txt_apaterno.BackColor = Color.Yellow;
                                    }
                                    else
                                    {
                                        txt_apaterno.BackColor = Color.Transparent;
                                        if (string.IsNullOrEmpty(txt_amaterno.Text))
                                        {

                                            txt_amaterno.BackColor = Color.Yellow;
                                        }
                                        else
                                        {
                                            txt_amaterno.BackColor = Color.Transparent;

                                            if (string.IsNullOrEmpty(txt_usuario.Text))
                                            {

                                                txt_usuario.BackColor = Color.Yellow;
                                            }
                                            else
                                            {
                                                txt_usuario.BackColor = Color.Transparent;
                                                if (string.IsNullOrEmpty(txt_clave.Text))
                                                {

                                                    txt_clave.BackColor = Color.Yellow;
                                                }
                                                else
                                                {
                                                    txt_clave.BackColor = Color.Transparent;

                                                    guarda_registro();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            
        }
        private void guarda_registro()
        {
            Guid guid_fempresa = Guid.NewGuid();
            Guid id_fempresa = Guid.Parse("9A3C8442-2B53-45B7-9B5C-144BFA9C93BE");
          
            string str_empresa = txt_tribunal.Text.ToUpper();
            string str_telefono = txt_telefono.Text;
            string str_email = txt_email.Text;
            string str_callenum = txt_callenum.Text.ToUpper();
            string str_cp = txt_cp.Text;
            int int_colony = Convert.ToInt32(ddl_colonia.SelectedValue);

            Guid guid_nusuario = Guid.NewGuid();

            string str_nombres = txt_nombres.Text.ToUpper();
            string str_apaterno = txt_apaterno.Text.ToUpper();
            string str_amaterno = txt_amaterno.Text.ToUpper();

            string str_usuairo = txt_usuario.Text.ToLower();
            string str_password = mdl_encrypta.Encrypt(txt_clave.Text);


            using (var m_empresa = new db_transcriptEntities())
            {
                var i_empresa = new inf_tribunal
                {
                    id_tribunal = guid_fempresa,
                  
                    id_estatus = 1,
                    nombre = str_empresa,
                    telefono = str_telefono,
                    email = str_email,
                    calle_num = str_callenum,
                    cp = str_cp,
                    id_asenta_cpcons = int_colony,
                    fecha_registro = DateTime.Now,
                    id_empresa = id_fempresa
                };

                m_empresa.inf_tribunal.Add(i_empresa);
                m_empresa.SaveChanges();
            }

            using (var m_usuario = new db_transcriptEntities())
            {
                var i_usuario = new inf_usuarios
                {
                    id_usuario = guid_nusuario,
                    id_estatus = 1,
                    id_tipo_usuario = 1,
                    nombres = str_nombres,
                    a_paterno = str_apaterno,
                    a_materno = str_amaterno,
                    codigo_usuario = str_usuairo,
                    clave = str_password,
                    fecha_registro = DateTime.Now,
                    id_tribunal = guid_fempresa
                };
                m_usuario.inf_usuarios.Add(i_usuario);
                m_usuario.SaveChanges();
            }

            limpiar_textbox();

            lblModalTitle.Text = "transcript";
            lblModalBody.Text = "Datos de administrador y tribunal actualizados con éxito";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
            upModal.Update();

      
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
        private void limpiar_textbox()
        {

            txt_tribunal.Text = "";
            txt_telefono.Text = "";
            txt_email.Text = "";
            txt_callenum.Text = "";
            txt_cp.Text = "";
            ddl_colonia.Items.Clear();
            ddl_colonia.Items.Insert(0, new ListItem("*Colonia", "0"));
            ddl_colonia.SelectedValue = "0";
            txt_municipio.Text = "";
            txt_estado.Text = "";

            txt_nombres.Text = "";
            txt_apaterno.Text = "";
            txt_amaterno.Text = "";

            txt_usuario.Text = "";
            txt_clave.Text = "";
        }
    }
}