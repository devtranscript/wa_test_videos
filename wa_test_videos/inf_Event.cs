//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace wa_transcript
{
    using System;
    using System.Collections.Generic;
    
    public partial class inf_Event
    {
        public System.Guid EventId { get; set; }
        public string Name { get; set; }
        public string TimeStamp { get; set; }
        public string Type { get; set; }
        public string TypeId { get; set; }
        public string TypeCategoryId { get; set; }
        public string IsSystemEvent { get; set; }
        public string IsPrivate { get; set; }
        public string Identifier { get; set; }
        public string EventNotes { get; set; }
        public System.Guid id_control { get; set; }
    
        public virtual inf_Master inf_Master { get; set; }
    }
}
