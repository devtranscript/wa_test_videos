//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace wa_transcript
{
    using System;
    using System.Collections.Generic;
    
    public partial class inf_ruta_videos
    {
        public int id_ruta_videos { get; set; }
        public string desc_ruta_fin { get; set; }
        public string ruta_user_ini { get; set; }
        public string ruta_pass_ini { get; set; }
        public string desc_ruta_ini { get; set; }
        public Nullable<System.Guid> id_sala { get; set; }
        public System.Guid id_usuario { get; set; }
        public System.Guid id_tribunal { get; set; }
        public Nullable<System.DateTime> fecha_registro { get; set; }
    }
}
