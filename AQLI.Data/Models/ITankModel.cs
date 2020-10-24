using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models
{
    public interface ITankModel
    {
        public int TankId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Capacity { get; set; }
        public DateTime Added { get; set; }

        public TankEnvironment? Environment { get; set; }
        public TankSubEnvironment? SubEnvironment { get; set; }

        public List<TankInventoryRecordModel> InventoryRecords { get; set; }
        public WebsiteUser Owner { get; set; }

    }

    public enum TankEnvironment
    {
        Dry, 
        Aquatic,
        Brackish,
        Betta,
        Aquascape,
        Breeding,
        Palindarium
    }
    public enum TankSubEnvironment
    {
        Breeding,
        Agressive,
        Community
    }

    public enum AquariumWaterType
    {
        Freshwater = 1, 
        Saltwater = 2,
        Brackish = 3,
        No_Water = 0
    }
    public enum AquariumTemporment
    {
        Agressive,
        Breeding,
        Community,
        Quarantine
    }
    public enum AquaticTankCreatureType
    {
        Fish,
        Turtle,
        Crab,
        Mixed_Community,
        Mixed_Agressive,
        Plants_only,
        Other
    }

    public enum DryTankCreatureType
    {
        Unknown = 0,
        Frog = 1,
        Lizard = 2,
        Gecko = 3,
        Iquana = 4,
        Rabbit = 5,
        Mouse = 6,
        Cricket = 7
    }

    public enum TankType
    {
        Unknown = 0,
        Aquarium_freshwater_fish = 1,
        Aquarium_freshwater_crab = 2,
        Aquarium_freshwater_agressive = 3,
        Aquarium_freshwater_community = 4,
        Aquarium_freshwater_planted = 5,
        Aquarium_freshwater_MixedSpecies_community = 6,
        Aquarium_marine_fish = 7,
        Aquarium_marine_crab = 8,
        Aquarium_marine_agressive = 9,
        Aquarium_marine_community = 10,
        Aquarium_marine_planted = 11,
        Aquarium_marine_MixedSpecies_community = 12,
        Palindarium_Gecko = 13,
        Palindarium_Crab = 14,
        Palindarium_Amphibien = 15,
        Dry_Lizard = 16,
        Dry_Cricket = 17,
        Dry_Mouse = 18
    }
}
