using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models
{
    public class FishCreatureModel : CreatureModel
    {
        public CreatureTypeModel CreatureType { get; private set; }

        public FishCreatureModel(string _name = null)
        {
            SetEnvironment(new EnvironmentModel { EnvironmentID = 1, EnvironmentName = "Aquatic Community" });
            SetName(_name);

            CreatedOn = DateTime.Now;
        }

        public FishCreatureModel()
        {

        }

        public void SetCreatureType(CreatureTypeModel _creatureType)
        {
            CreatureType = _creatureType;
        }

    }

    public enum AquaticFishSpecies
    {
        TETRA,
        SWORDTAIL,
        GLO_SHARK,
        GLO_TETRA,
        GLO_DANIO,
        UNKNOWN
    }
        
}
