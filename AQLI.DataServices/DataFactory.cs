using AQLI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace AQLI.DataServices
{
    public class DataFactory
    {
        private List<AquaticTankModel> _AllTanks;
        private List<WebsiteUser> _AllUsers;
        private List<FishCreatureModel> _AllFish;
        private List<TankInventoryRecordModel> _AllInventoryRecords;
        private List<MedicalRecord> _AllMedicalRecords;
        private List<AquariumWaterType> _AllWaterTypes;
        private List<AquariumTemporment> _AllAgressionLevels;


        public DataFactory()
        {
            _AllTanks = new List<AquaticTankModel>();
            _AllUsers = new List<WebsiteUser>();
            _AllFish = new List<FishCreatureModel>();

            _AllWaterTypes = new List<AquariumWaterType>
            {
                AquariumWaterType.Brackish,
                AquariumWaterType.Freshwater,
                AquariumWaterType.Saltwater
            };
            _AllAgressionLevels = new List<AquariumTemporment>
            {
                AquariumTemporment.Agressive,
                AquariumTemporment.Breeding,
                AquariumTemporment.Community,
                AquariumTemporment.Quarantine
            };

            PopulateLists();
            UpdateUserTanks();
        }

        public ITankModel CreateTankModel<T>()
        {
            var typeName = typeof(T).Name;

            switch (typeName)
            {
                case "AquaticTankModel":
                    {
                        return new AquaticTankModel();
                    }
                default:
                    return null;
            }
        }        

        public List<WebsiteUser> List_WebsiteUsers()
        {
            return _AllUsers;
        }

        public List<AquaticTankModel> List_Tanks()
        {            
            return _AllTanks;               
        }

        public List<FishCreatureModel> List_Fish()
        {
            return _AllFish;
        }

        public List<TankInventoryRecordModel> List_InventoryRecords()
        {
            return _AllInventoryRecords;
        }

        public List<MedicalRecord> List_MedicalRecords()
        {
            return _AllMedicalRecords;
        }

        public AquaticTankModel Find_TankDetails(int id)
        {
            return List_Tanks().Where(t => t.TankId == id).FirstOrDefault();
        }

        public List<AquaticTankModel> List_UserTanks(int id)
        {
            return _AllTanks.Where(t => t.Owner.UserId == id).ToList();
        }

        public List<TankType> List_TankTypes()
        {
            var finalList = new List<TankType>();

            finalList.Add(new TankType { TankTypeID = 1, TypeName = "Aquatic Community" });
            finalList.Add(new TankType { TankTypeID = 2, TypeName = "Aquatic Breeding" });
            finalList.Add(new TankType { TankTypeID = 3, TypeName = "Aquatic Agressive" });


            return finalList;
        }

        public List<AquariumWaterType> List_WaterTypes()
        {
            return _AllWaterTypes;
        }

        public List<AquariumTemporment> List_AgressionLevels()
        {
            return _AllAgressionLevels;
        }

        public WebsiteUser Find_UserDetails(int id)
        {
            WebsiteUser FinalUserDetails = List_WebsiteUsers().Where(u => u.UserId == id).FirstOrDefault();
            FinalUserDetails.AquaticTanks = List_Tanks().Where(o => o.Owner.UserId == id).ToList();
            
            return FinalUserDetails;
        }

        public AquaticTankModel Add_Tank(AquaticTankModel _dataModel)
        {
            _dataModel.TankId = _AllTanks.Select(t => t.TankId).Max() + 1;
            _AllTanks.Add(_dataModel);

            return _dataModel;
        }

        public AquaticTankModel Update_TankDetails(AquaticTankModel _dataModel)
        {
            var listEntry = _AllTanks.Where(t => t.TankId == _dataModel.TankId).FirstOrDefault();

            if (listEntry != null)
            {
                _AllTanks.Remove(listEntry);
            }

            _AllTanks.Add(_dataModel);

            return _dataModel;           
        }

        public void Remove_UserTank(int id)
        {            
            _AllTanks.Remove(_AllTanks.Where(t => t.TankId == id).First());            
        }

        #region private list population methods
            private void PopulateLists()
            {
                var user1 = new WebsiteUser
                {
                    UserId = 1,
                    UserName = "jpsietsma",
                    FirstName = "Jimmy",
                    LastName = "Sietsma",
                    EmailAddress = "jpsietsma@gmail.com"
                };

                var user2 = new WebsiteUser
                {
                    UserId = 2,
                    UserName = "admin",
                    FirstName = "Admin Jimmy",
                    LastName = "Sietsma",
                    EmailAddress = "admin_jpsietsma@gmail.com"
                };

                var user3 = new WebsiteUser
                {
                    UserId = 3,
                    UserName = "guser",
                    FirstName = "Guest",
                    LastName = "User",
                    EmailAddress = "guser@guest"
                };

                _AllUsers.AddRange(new List<WebsiteUser> { user1, user2, user3 });

                var aquaticTank1 = new AquaticTankModel(AquaticTankCreatureType.Fish, Find_UserDetails(1))
                {
                    TankId = 1,
                    Owner = List_WebsiteUsers().Where(u => u.UserId == 1).FirstOrDefault(),
                    Added = DateTime.Now.AddDays(-7),
                    Name = "Glo Special",
                    Capacity = 10.00,
                    Description = "10 Gallon special fish",
                    TankType = List_TankTypes().Where(t => t.TankTypeID == 1).First(),
                    SubEnvironment = TankSubEnvironment.Community,
                    Temporment = AquariumTemporment.Community,
                    WaterType = AquariumWaterType.Freshwater
                };
                var tankInventory1 = new List<TankInventoryRecordModel>()
                        {
                            new TankInventoryRecordModel { Id = 1, TankId = 1, Date = DateTime.Now, CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                            new TankInventoryRecordModel { Id = 2, TankId = 1, Date = DateTime.Now.AddDays(-7), CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                            new TankInventoryRecordModel { Id = 3, TankId = 1, Date = DateTime.Now.AddDays(365), CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                        };
                var tankPopulation1 = new List<FishCreatureModel>()
                    {
                        new FishCreatureModel("Speedy The Fish", AquaticFishSpecies.GLO_SHARK)
                        {
                            CreatureId = 1,
                            CreatedOn = DateTime.Now.AddDays(850)
                        },
                        new FishCreatureModel("Whitey The Unkillable", AquaticFishSpecies.TETRA)
                        {
                            CreatureId = 2,
                            CreatedOn = DateTime.Now.AddDays(-755)
                        },
                        new FishCreatureModel("Swordy McSword-Face", AquaticFishSpecies.SWORDTAIL)
                        {
                            CreatureId = 3,
                            CreatedOn = DateTime.Now.AddDays(-365)
                        },
                        new FishCreatureModel("Nitro", AquaticFishSpecies.GLO_DANIO)
                        {
                            CreatureId = 4,
                            CreatedOn = DateTime.Now.AddDays(-7)
                        }
                    };    

                aquaticTank1.InventoryRecords = tankInventory1;
                aquaticTank1.FishPopulation = tankPopulation1;

                var aquaticTank2 = new AquaticTankModel(AquaticTankCreatureType.Fish, Find_UserDetails(1))
                {
                    TankId = 2,
                    Owner = List_WebsiteUsers().Where(u => u.UserId == 1).FirstOrDefault(),
                    Added = DateTime.Now.AddDays(-3),
                    Name = "Work Tank",
                    Capacity = 15.00,
                    Description = "15 Gallon Work Tank",
                    TankType = List_TankTypes().Where(t => t.TankTypeID == 2).First(),
                    SubEnvironment = TankSubEnvironment.Community,
                    Temporment = AquariumTemporment.Community,
                    WaterType = AquariumWaterType.Freshwater
                };
                var tankInventory2 = new List<TankInventoryRecordModel>()
                        {
                            new TankInventoryRecordModel { Id = 4, TankId = 2, Date = DateTime.Now, CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                            new TankInventoryRecordModel { Id = 5, TankId = 2, Date = DateTime.Now.AddDays(-22), CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                            new TankInventoryRecordModel { Id = 6, TankId = 2, Date = DateTime.Now.AddDays(35), CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                        };
                var tankPopulation2 = new List<FishCreatureModel>()
                    {
                        new FishCreatureModel("Speedy The Fish", AquaticFishSpecies.GLO_SHARK)
                        {
                            CreatureId = 5,
                            CreatedOn = DateTime.Now.AddDays(850)
                        },
                        new FishCreatureModel("Whitey The Unkillable", AquaticFishSpecies.TETRA)
                        {
                            CreatureId = 6,
                            CreatedOn = DateTime.Now.AddDays(-755)
                        },
                        new FishCreatureModel("Swordy McSword-Face", AquaticFishSpecies.SWORDTAIL)
                        {
                            CreatureId = 7,
                            CreatedOn = DateTime.Now.AddDays(-365)
                        }
                    }; 

                aquaticTank2.InventoryRecords = tankInventory2;
                aquaticTank2.FishPopulation = tankPopulation2;

                var aquaticTank3 = new AquaticTankModel(AquaticTankCreatureType.Fish, Find_UserDetails(1))
                {
                    TankId = 3,
                    Owner = List_WebsiteUsers().Where(u => u.UserId == 1).FirstOrDefault(),
                    Added = DateTime.Now,
                    Name = "Home Natural Tank",
                    Capacity = 29.00,
                    Description = "29 Gallon Natural aquascaped tank.",
                    TankType = List_TankTypes().Where(t => t.TankTypeID == 3).First(),
                    SubEnvironment = TankSubEnvironment.Community,
                    Temporment = AquariumTemporment.Community,
                    WaterType = AquariumWaterType.Freshwater
                };
                var tankInventory3 = new List<TankInventoryRecordModel>()
                        {
                            new TankInventoryRecordModel { Id = 7, TankId = 3, Date = DateTime.Now, CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                            new TankInventoryRecordModel { Id = 8, TankId = 3, Date = DateTime.Now.AddDays(12), CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                            new TankInventoryRecordModel { Id = 9, TankId = 3, Date = DateTime.Now.AddDays(48), CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                        };
                var tankPopulation3 = new List<FishCreatureModel>()
                    {
                        new FishCreatureModel("Speedy The Fish", AquaticFishSpecies.GLO_SHARK)
                        {
                            CreatureId = 8,
                            CreatedOn = DateTime.Now.AddDays(850)
                        },
                        new FishCreatureModel("Whitey The Unkillable", AquaticFishSpecies.TETRA)
                        {
                            CreatureId = 9,
                            CreatedOn = DateTime.Now.AddDays(-755)
                        },
                        new FishCreatureModel("Swordy McSword-Face", AquaticFishSpecies.SWORDTAIL)
                        {
                            CreatureId = 10,
                            CreatedOn = DateTime.Now.AddDays(-365)
                        }
                    };

                aquaticTank3.FishPopulation = tankPopulation3;
                aquaticTank3.InventoryRecords = tankInventory3;

                var aquaticTank4 = new AquaticTankModel(AquaticTankCreatureType.Fish, Find_UserDetails(2))
                {
                    TankId = 4,
                    Owner = List_WebsiteUsers().Where(u => u.UserId == 2).FirstOrDefault(),
                    Added = DateTime.Now.AddDays(-7),
                    Name = "Glo Special",
                    Capacity = 10.00,
                    Description = "10 Gallon special fish",
                    TankType = List_TankTypes().Where(t => t.TankTypeID == 1).First(),
                    SubEnvironment = TankSubEnvironment.Community,
                    Temporment = AquariumTemporment.Community,
                    WaterType = AquariumWaterType.Freshwater
                };
                var tankInventory4 = new List<TankInventoryRecordModel>()
                        {
                            new TankInventoryRecordModel { Id = 10, TankId = 4, Date = DateTime.Now, CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                            new TankInventoryRecordModel { Id = 11, TankId = 4, Date = DateTime.Now.AddDays(-7), CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                            new TankInventoryRecordModel { Id = 12, TankId = 4, Date = DateTime.Now.AddDays(365), CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                        };
                var tankPopulation4 = new List<FishCreatureModel>()
                    {
                        new FishCreatureModel("Speedy The Fish", AquaticFishSpecies.GLO_SHARK)
                        {
                            CreatureId = 11,
                            CreatedOn = DateTime.Now.AddDays(850)
                        },
                        new FishCreatureModel("Whitey The Unkillable", AquaticFishSpecies.TETRA)
                        {
                            CreatureId = 12,
                            CreatedOn = DateTime.Now.AddDays(-755)
                        },
                        new FishCreatureModel("Swordy McSword-Face", AquaticFishSpecies.SWORDTAIL)
                        {
                            CreatureId = 13,
                            CreatedOn = DateTime.Now.AddDays(-365)
                        }
                    };

                aquaticTank4.FishPopulation = tankPopulation4;
                aquaticTank4.InventoryRecords = tankInventory4;

                var aquaticTank5 = new AquaticTankModel(AquaticTankCreatureType.Fish, Find_UserDetails(2))
                {
                    TankId = 5,
                    Owner = List_WebsiteUsers().Where(u => u.UserId == 2).FirstOrDefault(),
                    Added = DateTime.Now.AddDays(-3),
                    Name = "Work Tank",
                    Capacity = 15.00,
                    Description = "15 Gallon Work Tank",
                    TankType = List_TankTypes().Where(t => t.TankTypeID == 1).First(),
                    SubEnvironment = TankSubEnvironment.Community,
                    Temporment = AquariumTemporment.Community,
                    WaterType = AquariumWaterType.Freshwater
                };
                var tankInventory5 = new List<TankInventoryRecordModel>()
                        {
                            new TankInventoryRecordModel { Id = 13, TankId = 5, Date = DateTime.Now, CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                            new TankInventoryRecordModel { Id = 14, TankId = 5, Date = DateTime.Now.AddDays(-22), CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                            new TankInventoryRecordModel { Id = 15, TankId = 5, Date = DateTime.Now.AddDays(35), CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                        };
                var tankPopulation5 = new List<FishCreatureModel>()
                    {
                        new FishCreatureModel("Speedy The Fish", AquaticFishSpecies.GLO_SHARK)
                        {
                            CreatureId = 14,
                            CreatedOn = DateTime.Now.AddDays(850)
                        },
                        new FishCreatureModel("Whitey The Unkillable", AquaticFishSpecies.TETRA)
                        {
                            CreatureId = 15,
                            CreatedOn = DateTime.Now.AddDays(-755)
                        },
                        new FishCreatureModel("Swordy McSword-Face", AquaticFishSpecies.SWORDTAIL)
                        {
                            CreatureId = 16,
                            CreatedOn = DateTime.Now.AddDays(-365)
                        }
                    };

                aquaticTank5.FishPopulation = tankPopulation5;
                aquaticTank5.InventoryRecords = tankInventory5;

                var aquaticTank6 = new AquaticTankModel(AquaticTankCreatureType.Fish, Find_UserDetails(2))
                {
                    TankId = 6,
                    Owner = List_WebsiteUsers().Where(u => u.UserId == 2).FirstOrDefault(),
                    Added = DateTime.Now,
                    Name = "Home Natural Tank",
                    Capacity = 29.00,
                    Description = "29 Gallon Natural aquascaped tank.",
                    TankType = List_TankTypes().Where(t => t.TankTypeID == 1).First(),
                    SubEnvironment = TankSubEnvironment.Community,
                    Temporment = AquariumTemporment.Community,
                    WaterType = AquariumWaterType.Freshwater
                };
                var tankInventory6 = new List<TankInventoryRecordModel>()
                        {
                            new TankInventoryRecordModel { Id = 16, TankId = 6, Date = DateTime.Now, CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                            new TankInventoryRecordModel { Id = 17, TankId = 6, Date = DateTime.Now.AddDays(12), CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                            new TankInventoryRecordModel { Id = 18, TankId = 6, Date = DateTime.Now.AddDays(48), CreatureInventory = new List<FishCreatureModel>(), PerformedBy = 1 },
                        };
                var tankPopulation6 = new List<FishCreatureModel>()
                    {
                        new FishCreatureModel("Speedy The Fish", AquaticFishSpecies.GLO_SHARK)
                        {
                            CreatureId = 17,
                            CreatedOn = DateTime.Now.AddDays(850)
                        },
                        new FishCreatureModel("Whitey The Unkillable", AquaticFishSpecies.TETRA)
                        {
                            CreatureId = 18,
                            CreatedOn = DateTime.Now.AddDays(-755)
                        },
                        new FishCreatureModel("Swordy McSword-Face", AquaticFishSpecies.SWORDTAIL)
                        {
                            CreatureId = 19,
                            CreatedOn = DateTime.Now.AddDays(-365)
                        }
                    };

                aquaticTank6.FishPopulation = tankPopulation6;
                aquaticTank6.InventoryRecords = tankInventory6;

                _AllTanks.AddRange(new List<AquaticTankModel> { aquaticTank1, aquaticTank2, aquaticTank3, aquaticTank4, aquaticTank5, aquaticTank6 });

                UpdateUserTanks();
            }

            private void UpdateUserTanks()
            {
                foreach (var user in _AllUsers)
                {
                    user.AquaticTanks = _AllTanks.Where(t => t.Owner.UserId == user.UserId).ToList();
                }
            }

        #endregion
    }
}
