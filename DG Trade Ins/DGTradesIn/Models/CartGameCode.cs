//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DGTradesIn.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CartGameCode
    {
        public int CartGameCodeID { get; set; }
        public int GameCode { get; set; }
        public int CartID { get; set; }
    
        public virtual Cart Cart { get; set; }
        public virtual GameCode GameCode1 { get; set; }
    }
}
