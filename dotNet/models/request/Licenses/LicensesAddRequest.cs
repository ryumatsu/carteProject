using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Licenses
{
    public class LicensesAddRequest 
    {
        [Required]
        [Range(1, 51)]
        public int LicenseStateId { get; set; }

        [Required]
        [Range(1,2)]
        public int LicenseType { get; set; }

        [Required]
        [StringLength(20)]
        public string LicenseNumber { get; set; }

        [Required]
        public DateTime DateExpires { get; set; }

        [Required]
        public bool IsVerified { get; set; }
    }
}
