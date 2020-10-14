using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models
{
    public class WebsiteUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public List<AquaticTankModel> AquaticTanks { get; set; }

        public WebsiteUser()
        {
            AquaticTanks = new List<AquaticTankModel>();
        }

    }
}
