using System;
using System.ComponentModel.DataAnnotations;


namespace PressYourLuck.Models
{
    public class Audit
    {

        [Key]
        public int AuditId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public double Amount { get; set; }  
        public int AuditTypeId { get; set; }
        public AuditType auditTypes { get; set; }


    }
}
