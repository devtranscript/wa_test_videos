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
    
    public partial class inf_material
    {
        public int id_material { get; set; }
        public string sesion { get; set; }
        public string titulo { get; set; }
        public string localizacion { get; set; }
        public string tipo { get; set; }
        public string archivo { get; set; }
        public string duracion { get; set; }
        public Nullable<System.DateTime> fecha_registro { get; set; }
        public Nullable<System.DateTime> fecha_registro_alt { get; set; }
        public Nullable<int> id_estatus_material { get; set; }
        public Nullable<int> id_estatus_qa { get; set; }
        public Nullable<int> id_ruta_videos { get; set; }
        public Nullable<System.Guid> id_usuario { get; set; }
        public Nullable<System.Guid> id_control { get; set; }
    }
}