using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class TankNoteModel
    {
        [Key]
        public int TankNoteID { get; set; }
        public string NoteText { get; set; }
        public DateTime Added { get; set; }

        public int TankID { get; set; }
        public AquaticTankModel Tank { get; set; }
    }
}
