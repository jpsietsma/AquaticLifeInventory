using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models
{
    public class FishCreatureModel : CreatureModel
    {
        public AquaticTankCreatureType CreatureType { get; private set; }
        public AquaticFishSpecies FishSpecies { get; private set; }

        public FishCreatureModel(string _name = null, AquaticFishSpecies _species = AquaticFishSpecies.UNKNOWN)
        {
            SetEcosystem(CreatureEcosystemType.AQUATIC);
            FishSpecies = _species;
            SetName(_name);

            CreatedOn = DateTime.Now;
        }

        public void SetCreatureType(AquaticTankCreatureType _creatureType)
        {
            CreatureType = _creatureType;
        }

        public void SetFishSpecies(AquaticFishSpecies _species)
        {
            FishSpecies = _species;
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
