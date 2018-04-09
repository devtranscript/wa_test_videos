using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wa_transcript
{
    public partial class ctrl_acceso : System.Web.UI.Page
    {
        static Guid str_id_user;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                inf_user();
            }
            else
            {

            }
        }

        private void inf_user()
        {

            try
            {
                using (db_transcriptEntities edm_usuario = new db_transcriptEntities())
                {
                    var i_usuario = (from u in edm_usuario.inf_usuarios
                                     where u.id_tipo_usuario == 1
                                     select u).ToList();

                    if (i_usuario.Count == 0)
                    {
                        lkb_registro.Visible = true;
                        lblModalTitle.Text = "transcript";
                        lblModalBody.Text = "No existe administrador ni tribunal en la aplicación, favor de registrarlos";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                        upModal.Update();
                    }
                    else
                    {
                        lkb_registro.Visible = false;

                    }
                }
            }
            catch
            {
                lblModalTitle.Text = "transcript";
                lblModalBody.Text = "Sin conexión a base de datos, contactar al administrador";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                upModal.Update();
            }

        }

        protected void cmd_login_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_code_user.Text))
            {

                txt_code_user.BackColor = Color.Yellow;
            }
            else
            {
                txt_code_user.BackColor = Color.Transparent;
                if (string.IsNullOrEmpty(txt_password.Text))
                {

                    txt_password.BackColor = Color.Yellow;
                }
                else
                {
                    txt_password.BackColor = Color.Transparent;

                    string str_codeuser = txt_code_user.Text;
                    string str_password = mdl_encrypta.Encrypt(txt_password.Text);
                    string str_password_V;
                    int? str_id_type_user, str_iduser_status;


                    try
                    {
                        using (db_transcriptEntities edm_usuario = new db_transcriptEntities())
                        {
                            var i_usuario = (from c in edm_usuario.inf_usuarios
                                             where c.codigo_usuario == str_codeuser
                                             select c).FirstOrDefault();

                            str_id_user = i_usuario.id_usuario;
                            str_password_V = i_usuario.clave;
                            str_id_type_user = i_usuario.id_tipo_usuario;
                            str_iduser_status = i_usuario.id_estatus;

                            if (str_password_V == str_password && str_iduser_status == 1)
                            {

                                using (var i_edmusuario = new db_transcriptEntities())
                                {
                                    var ii_usuario = new inf_sesion
                                    {
                                        fecha_registro = DateTime.Now,
                                        id_estatus_sesion = 1,
                                        id_aspx = 1,
                                        id_usuario = str_id_user
                                    };
                                    i_edmusuario.inf_sesion.Add(ii_usuario);
                                    i_edmusuario.SaveChanges();
                                }

                                Session["ss_id_user"] = mdl_user.code_user(str_codeuser);
                                Response.Redirect("ctrl_menu.aspx");
                            }
                            else
                            {
                                lblModalTitle.Text = "transcript";
                                lblModalBody.Text = "Contraseña incorrecta, favor de contactar al Administrador.";
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                                upModal.Update();
                            }
                        }
                    }
                    catch
                    {
                        lblModalTitle.Text = "transcript";
                        lblModalBody.Text = "Usuario incorrecto, favor de contactar al Administrador.";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                        upModal.Update();
                    }
                }
            }
        }

        protected void lkb_registro_Click(object sender, EventArgs e)
        {
            Response.Redirect("ctrl_registro_inicial.aspx");
        }
    }
}