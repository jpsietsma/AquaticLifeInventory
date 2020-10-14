using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models
{
    public abstract class CreatureModel
    {
        public int CreatureId { get; set; }
        public string Description { get; set; }
        public string CreatureName { get; private set; }
        public CreatureEcosystemType EcosystemType { get; private set; }
        public DateTime CreatedOn { get; set; }

        public void SetName(string _name)
        {
            CreatureName = _name;
        }

        public void SetEcosystem(CreatureEcosystemType _ecosystem)
        {
            EcosystemType = _ecosystem;
        }
    }

    public enum CreatureEcosystemType
    {
        DRY,
        AQUATIC,
        ANY
    }
}
