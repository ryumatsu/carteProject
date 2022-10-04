using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain
{
    public class Licenses
    {
        public LookUp LicenseId { get; set; }
        public LookUp LicenseStateId { get; set; }
        public LookUp LicenseTypeId { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime DateExpires { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsVerified { get; set; }
        public List<Licenses> License { get; set; }
        public Employees Employees { get; set; }
        public UserProfileBase UserProfile { get; set; }
        public string Email { get; set; }
    }
}
