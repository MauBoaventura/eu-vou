using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EuVou.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the EuVouUser class
    public class EuVouUser : IdentityUser
    {
        [PersonalData]
        public string CPF { get; set; }
       
        [PersonalData]
        public string Name { get; set; }
        
        [PersonalData]
        public Boolean IsADM { get; set; }
    }
}
