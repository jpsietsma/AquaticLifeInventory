using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public abstract class CreatureModel
    {
        [Key]
        public int CreatureID { get; set; }
        public string Description { get; set; }
        public string Name { get; private set; }
        public EnvironmentModel Environment { get; private set; }
        public DateTime CreatedOn { get; set; }

        public void SetName(string _name)
        {
            Name = _name;
        }

        public void SetEnvironment(EnvironmentModel _ecosystem)
        {
            Environment = _ecosystem;
        }
    }

    public enum CreatureEcosystemType
    {
        DRY,
        AQUATIC,
        ANY
    }
}
