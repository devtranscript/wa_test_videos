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
    
    public partial class inf_usuarios
    {
        public System.Guid id_usuario { get; set; }
        public string codigo_usuario { get; set; }
        public string nombres { get; set; }
        public string a_paterno { get; set; }
        public string a_materno { get; set; }
        public string clave { get; set; }
        public Nullable<int> id_genero { get; set; }
        public Nullable<int> id_estatus { get; set; }
        public Nullable<int> id_tipo_usuario { get; set; }
        public Nullable<System.DateTime> fecha_nacimiento { get; set; }
        public Nullable<System.DateTime> fecha_registro { get; set; }
        public Nullable<System.Guid> id_tribunal { get; set; }
    }
}
