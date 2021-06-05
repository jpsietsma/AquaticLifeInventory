using AQLI.Data.Models.ListModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class UserFishModel
    {
        [Key]
        public int UserFishID { get; set; }

        [Display(Name = "Encumbrance Multiplier")]
        public decimal EncumbranceMultiplier { get; set; }

        [Display(Name = "Fish Friendly Name")]
        public string FishName { get; set; }

        public int FishTypeID { get; set; }
        public FishTypeModel FishType { get; set; }

        public int PurchaseID { get; set; }
        public PurchaseModel Purchase { get; set; }

        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }

        public int? FishTempormentID { get; set; }
        public TempormentModel FishTemporment { get; set; }

        public int? TankID { get; set; }
        public AquaticTankModel Tank { get; set; }

        public int? FishStatusID { get; set; }
        public FishStatusModel FishStatus { get; set; }

        public int? ParentFishID { get; set; }
        public UserFishModel ParentFish { get; set; }

        public List<UserFishModel> ChildrenFish { get; set; }

        public List<UserFish_MedicalRecordModel> MedicalRecords { get; set; }

        public string FishSex { get; set; }

        public DateTime? Added { get; set; }
        public int? AddedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
