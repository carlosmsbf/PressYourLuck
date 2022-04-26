using System;
using System.ComponentModel.DataAnnotations;


namespace PressYourLuck.Models
{
    public class AuditType
    {

        [Key]
       public int AuditTypeId { get; set; }
       public string Name { get; set; }


    }
}
