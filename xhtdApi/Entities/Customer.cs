using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace xhtdApi.Entities
{
    public class Customer : BaseClass
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250)]
        public string FirstName { get; set; } = "";
        [MaxLength(250)]
        public string LastName { get; set; } = "";
        [MaxLength(11)]
        public string PhoneNumber { get; set; } = "";
    }
}