//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MyWcModel.Abstract;

namespace MyWcModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Word:WcBase
    {
        public Word()
        {
            this.colword_collocations = new HashSet<Collocation>();
            this.word_collocations = new HashSet<Collocation>();
        }
    
        public new long Id { get; set; }
        //public string Entry { get; set; }
        //public string EntryZht { get; set; }
        //public string EntryZhs { get; set; }
        //public string EntryJap { get; set; }
        public short posId { get; set; }
        //public System.DateTime RowVersion { get; set; }
        //public Nullable<bool> CanDel { get; set; }
    
        public virtual ICollection<Collocation> colword_collocations { get; set; }
        public virtual ICollection<Collocation> word_collocations { get; set; }
        public virtual Pos pos { get; set; }
    }
}
