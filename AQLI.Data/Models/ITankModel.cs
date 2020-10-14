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
        Freshwater, 
        Saltwater,
        Brackish        
    }
    public enum AquariumTemporment
    {
        Agressive,
        Breeding,
        Community,
        None
    }
    public enum AquaticTankCreatureType
    {
        Fish,
        Turtle,
        Crab,
        Mixed_Community,
        Mixed_Agressive,
        Unknown
    }

    public enum DryTankCreatureType
    {
        Frog,
        Lizard,
        Gecko,
        Iquana,
        Rabbit,
        Mouse,
        Cricket,
        Unknown
    }
}
